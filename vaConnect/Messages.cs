using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vaConnect
{
    public class Messages
    {
       public String Notification { get; set; }
       public String Result { get; set; }

        public Messages()
        {
            Notification = "";
            Result = "";
        }
        public Messages(String n, String r)
        {
            Notification = n;
            Result = r;
        }
       
        public void Clear()
        {
            Notification = "";
            Result = "";
        }
    }
}
