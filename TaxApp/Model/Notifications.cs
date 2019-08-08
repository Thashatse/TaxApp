using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Notifications
    {
        public int notificationID { get; set; }
        public int ProfileID { get; set; }
        public DateTime date { get; set; }
        public string timeSince { get; set; }
        public string Details { get; set; }
        public string Link { get; set; }
    }
}
