using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logged_In_Users.Models
{
    public class Login
    {
        public int user_id { get; set; }
        public string user_email { get; set; }
        public string user_name { get; set; }
        public string user_status { get; set; }
    }
}
