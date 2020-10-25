using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TheEventApp.Models;

namespace TheEventApp.Controllers
{
    public class EventsController : BaseController
    {
        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            var attendees = db.EventAttendees.Include("User").Include("Event").Where(p => p.Event.Id == @event.Id).ToList();
            EventDetailModel model = new EventDetailModel
            {
                Event = @event,
                EventAttendees = attendees
            };


            return View(model);
        }

        [Authorize(Roles = "Admin,Eventor")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Eventor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventModel @event)
        {
            if (ModelState.IsValid)
            {
                @event.CreatedBy = db.Users.FirstOrDefault(x => x.Id == GetApplicationUserId);
                db.Events.Add(@event);
                db.SaveChanges();
                if (@event.ImageFile != null && @event.ImageFile.FileName != null)
                {
                    string newName = @event.Id + Path.GetExtension(@event.ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images"), newName);
                    @event.ImageFile.SaveAs(path);
                    @event.EventImagePath = "~/Images/" + newName;
                    db.Events.AddOrUpdate(@event);
                    db.SaveChanges();
                    try
                    {
                        EmailHelper.SendEmail(@event, GetApplicationUser);
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        [Authorize(Roles = "Admin,Eventor")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }


            if (@event.CreatedBy != GetApplicationUser && !IsAdmin)
            {
                ModelState.AddModelError("Edit", "Cannot edit, you are not creator of this event!");
            }

            return View(@event);
        }

        [Authorize(Roles = "Admin,Eventor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventModel @event)
        {
            if (ModelState.IsValid)
            {
                @event.CreatedBy = @event.CreatedBy = db.Users.FirstOrDefault(x => x.Id == GetApplicationUserId);
                if (@event.CreatedBy != GetApplicationUser && !IsAdmin)
                {
                    ModelState.AddModelError("Edit", "Cannot edit, you are not creator of this event!");
                }
                else
                {
                    db.Entry(@event).State = EntityState.Modified;
                    db.SaveChanges();
                    if (@event.ImageFile != null && @event.ImageFile.FileName != null)
                    {
                        string newName = @event.Id + Path.GetExtension(@event.ImageFile.FileName);
                        string path = Path.Combine(Server.MapPath("~/Images"), newName);
                        @event.ImageFile.SaveAs(path);
                        @event.EventImagePath = "~/Images/" + newName;
                        db.Events.AddOrUpdate(@event);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(@event);
        }

        [Authorize(Roles = "Admin,Eventor")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            if (@event.CreatedBy != GetApplicationUser && !IsAdmin)
            {
                ModelState.AddModelError("Edit", "Cannot delete, you are not creator of this event!");
            }
            return View(@event);
        }

        [Authorize(Roles = "Admin,Eventor")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            if (@event.CreatedBy != GetApplicationUser && !IsAdmin)
            {
                ModelState.AddModelError("Edit", "Cannot delete, you are not creator of this event!");
            }
            else
            {
                db.Events.Remove(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }


        #region Attendee
        //[Authorize(Roles = "Admin,Eventor")]
        [HttpPost]
        public ActionResult AttendEvent(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { Message = "Please register first and then signup for an event!" });
                }
                Event @event = db.Events.Find(id);
                if (@event == null)
                {
                    return Json(new { Message = "Event not found!!" });
                }
                else
                {
                    var existing = db.EventAttendees.Where(p => p.Event.Id == @event.Id && p.User.Id == GetApplicationUser.Id).FirstOrDefault();
                    if (existing != null)
                    {
                        //update
                        db.Database.ExecuteSqlCommand($"update EventAttendees set Accepted = {1} where Event_Id = {@event.Id} and User_id = '{GetApplicationUser.Id}'");
                        db.SaveChanges();
                        return Json(new { Message = "Successfully Saved!" });
                    }
                    else
                    {
                        //insert
                        db.Database.ExecuteSqlCommand($"insert into EventAttendees values ({@event.Id}, '{GetApplicationUser.Id}', {1})");
                        db.SaveChanges();
                        return Json(new { Message = "Successfully Saved!" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult RejectEvent(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { Message = "Please register first and then signup for an event!" });
                }
                Event @event = db.Events.Find(id);
                if (@event == null)
                {
                    return Json(new { Message = "Event not found!!" });
                }
                else
                {
                    var existing = db.EventAttendees.Where(p => p.Event.Id == @event.Id && p.User.Id == GetApplicationUser.Id).FirstOrDefault();
                    if (existing != null)
                    {
                        //update
                        db.Database.ExecuteSqlCommand($"update EventAttendees set Accepted = {0} where Event_Id = {@event.Id} and User_id = '{GetApplicationUser.Id}'");
                        db.SaveChanges();
                        return Json(new { Message = "Successfully Saved!" });
                    }
                    else
                    {
                        //insert
                        db.Database.ExecuteSqlCommand($"insert into EventAttendees values ({@event.Id}, '{GetApplicationUser.Id}', {0})");
                        db.SaveChanges();
                        return Json(new { Message = "Successfully Saved!" });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message });
            }
        }

        public ActionResult AcceptInvitation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            var attendees = db.EventAttendees.Include("User").Include("Event").Where(p => p.Event.Id == @event.Id).ToList();
            EventDetailModel model = new EventDetailModel
            {
                Event = @event,
                EventAttendees = attendees
            };

            return View(model);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
