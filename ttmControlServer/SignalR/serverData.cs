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

        public class djMsgData
        {
            public int id { get; set; }
            public int count { get; set; }
            public int type { get; set; }
            public string msg { get; set; }
        }

        public bool isInit { get; set; }
        public int idleIndex { get; set; }

        private List<idleMsgData> _idleMsgMgr = new List<idleMsgData>();
        private Dictionary<int, string> _djMsgType = new Dictionary<int, string>();
        private List<djMsgData> _djMsgData = new List<djMsgData>();

        public serverData()
        {
            isInit = false;
            idleIndex = 0;
        }

        public void init()
        {
            loadDJType();
            loadDJMsg();
            loadIdleMsg();
            isInit = true;
        }

        private void loadDJType()
        {
            using (StreamReader sr = new StreamReader(HostingEnvironment.MapPath("~/assets/djMsgType.json")))
            {
                _djMsgType = JsonConvert.DeserializeObject<Dictionary<int, string>>(sr.ReadToEnd());
            }
        }

        public Dictionary<int, string> getDJType()
        {
            return _djMsgType;
        }

        private void loadDJMsg()
        {
            using (StreamReader sr = new StreamReader(HostingEnvironment.MapPath("~/assets/djMsg.json")))
            {
                _djMsgData = JsonConvert.DeserializeObject<List<djMsgData>>(sr.ReadToEnd());
            }
        }

        public List<djMsgData> getAllDJMsg()
        {
            return _djMsgData;
        }

        private void loadIdleMsg()
        {
            using (StreamReader sr = new StreamReader(HostingEnvironment.MapPath("~/assets/idleMsg.json")))
            {
                _idleMsgMgr = JsonConvert.DeserializeObject<List<idleMsgData>>(sr.ReadToEnd());
            }
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
            _idleMsgMgr.Clear();
            _idleMsgMgr = newList.ToList();
            saveIdleMsg();
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