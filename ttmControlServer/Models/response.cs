using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ttmControlServer.Models
{
    public class response
    {
        public string active { get; set; }
        public bool result { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
        public int index { get; set; }
        public response()
        {
            active = "unknow";
            result = false;
            msg = "";
            index = -1;
            data = null;
        }
    }
}