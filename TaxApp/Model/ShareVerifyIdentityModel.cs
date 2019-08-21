using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ShareVerifyIdentityModel
    {
        public int OTP { get; set; }
        public int ID { get; set; }
        public char type { get; set; }
        public string userName { get; set; }
    }
}
