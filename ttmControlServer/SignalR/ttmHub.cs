using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ttmControlServer.SignalR
{
    public class ttmHub : Hub
    {
        public static int _mode = 0; //0:idle + DJ, 1:interactive
        public static string _hostID = "";
        public static string _backendID = "";

        public static serverData _serverData = new serverData();


        public ttmHub()
        {
            if(!_serverData.isInit)
            {
                _serverData.init();
            }
        }
        
        #region Override
        public override Task OnDisconnected(bool stopCalled)
        {
            if(Context.ConnectionId == _hostID)
            {
                _hostID = "";
            }
            return base.OnDisconnected(stopCalled);
        }
        #endregion

        #region Unity Host
        public void registerHost()
        {
            _hostID = Context.ConnectionId;
        }

        public string getNextIdleMsg()
        {
            if(_hostID != Context.ConnectionId)
            {
                return "";
            }

            return _serverData.getNextIdleMsg();
        }


        #endregion

        #region Backend
        public void registerBackend()
        {
            _backendID = Context.ConnectionId;
            
        }

        public void setMode(int mode)
        {
            if(_backendID != Context.ConnectionId)
            {
                return;
            }
            if(mode != 0 && mode != 1)
            {
                return;
            }
            _mode = mode;
            if(_hostID != "")
            {
                Clients.Client(_hostID).setMode(_mode);
            }
        }


        #region DJ Mode
        public void setDJGreetingMsg(string msg)
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                Clients.Client(_hostID).setDJGreetingMsg(msg);
            }
        }

        public void setDJMsg(string msg)
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                Clients.Client(_hostID).setDJMsg(msg);
            }
        }

        public void updateIdleMsg(string msgSet)
        {

        }
        #endregion

        #region Interactive Mode
        public void setQuestion(int idx)
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                Clients.Client(_hostID).setQuestion(idx);
            }
        }

        public void setAnswer(int idx, string ans)
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                Clients.Client(_hostID).setAnswer(idx, ans);
            }
        }

        public void showAnswer()
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                Clients.Client(_hostID).showAnswer();
            }
        }

        public void showCode(string code, float t)
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                Clients.Client(_hostID).showCode(code, t);
            }
        }
        #endregion

        #endregion
    }
}