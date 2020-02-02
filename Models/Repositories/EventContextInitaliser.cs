using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Text;

namespace TheEventApp.Models.Repositories
{
    public class EventContextInitaliser : DropCreateDatabaseAlways<EventContext>
    {
        protected override void Seed(EventContext context)
        {
            base.Seed(context);
        }
    }
}