using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ttmControlServer.Models
{
    public class djMsgData
    {
        public int type { get; set; }
        public string typeName { get; set; }
        public string msg { get; set; }
        public djMsgData()
        {
            type = -1;
            typeName = "";
            msg = "";
        }
    }
}