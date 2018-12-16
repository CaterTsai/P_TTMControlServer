using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace ttmControlServer.SignalR
{

    public class serverData
    {
        public class idleMsgData
        {
            public idleMsgData(int i, string m)
            {
                id = i;
                msg = m;
            }
            public int id { get; set; }
            public string msg { get; set; }
        }

        public bool isInit { get; set; }
        public int idleIndex { get; set; }

        private List<idleMsgData> _idleMsgMgr = new List<idleMsgData>();

        public serverData()
        {
            isInit = false;
            idleIndex = 0;
        }

        public void init()
        {
            loadIdleMsg();
            isInit = true;
        }

        private void loadIdleMsg()
        {
            using (StreamReader sr = new StreamReader(HostingEnvironment.MapPath("~/assets/idleMsg.json")))
            {
                _idleMsgMgr = JsonConvert.DeserializeObject<List<idleMsgData>>(sr.ReadToEnd());
            }

            _idleMsgMgr.Add(new idleMsgData(4, "測試"));

        }

        private void saveIdleMsg()
        {
            string jsonStr = JsonConvert.SerializeObject(_idleMsgMgr);
            using (StreamWriter sw = new StreamWriter(HostingEnvironment.MapPath("~/assets/idleMsg.json"), false, Encoding.UTF8))
            {
                sw.WriteLine(jsonStr);
                sw.Close();
            }
        }

        public void updateIdleMsg(ref List<idleMsgData> newList)
        {

        }

        public List<idleMsgData> getAllIdleMsg()
        {
            return _idleMsgMgr;
        }

        public string getNextIdleMsg()
        {
            if (idleIndex < 0 || idleIndex >= _idleMsgMgr.Count)
            {
                return "";
            }

            var idleMsg = _idleMsgMgr[idleIndex].msg;
            idleIndex = (idleIndex + 1) % _idleMsgMgr.Count;

            return idleMsg;
        }


    }
}