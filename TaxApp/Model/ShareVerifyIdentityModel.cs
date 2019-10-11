using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ShareVerifyIdentityModel
    {
        [Required]
        public int OTP { get; set; }
        [Required]
        public int ID { get; set; }
        public char type { get; set; }
        public string userName { get; set; }
    }
}
