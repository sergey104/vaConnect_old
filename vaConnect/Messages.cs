using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vaConnect
{
    public class Messages
    {
       private String Notification { get; set; }
        private String Result { get; set; }

        Messages()
        {
            Notification = "";
            Result = "";
        }
        Messages(String n, String r)
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
