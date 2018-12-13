using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            _idleMsgMgr.Add(new idleMsgData(0, "測試待機1"));
            _idleMsgMgr.Add(new idleMsgData(1, "測試待機2"));
            _idleMsgMgr.Add(new idleMsgData(2, "測試待機3"));
            _idleMsgMgr.Add(new idleMsgData(3, "測試待機4"));
        }
        
        private void saveIdleMsg()
        {

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