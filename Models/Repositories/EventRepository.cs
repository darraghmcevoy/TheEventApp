using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheEventApp.Models.Repositories
{
    public class EventRepository : Repository<Event>
    {
        public List<Event> GetByTitle(String Title)
        {
            return DbSet.Where(a => a.Title.Contains(Title)).ToList();
        }
    }
}