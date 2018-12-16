using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using ttmControlServer.Models;
using Newtonsoft.Json;

namespace ttmControlServer.SignalR
{
    public class ttmHub : Hub
    {
        public enum interactiveState
        {
            sidle = 0,

            sInteractiveCountdown,
            sInteractiveStart,
            sInteractiveResult
        }
        const int _cFinishNum = 2;
        public static int _mode = 0; //0:idle + DJ, 1:interactive
        public static interactiveState _state = interactiveState.sidle;
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
        public string registerBackend()
        {
            _backendID = Context.ConnectionId;
            response r = new response();
            r.active = "registerBackend";
            r.result = true;
            r.data = _mode;
            var repJson = JsonConvert.SerializeObject(r);
            return repJson;
        }

        public string setMode(int mode)
        {
            response r = new response();
            r.active = "setMode";
            if (_backendID != Context.ConnectionId || _hostID == "")
            {
                r.result = false;
                r.msg = "Wrong connection Id";
            }
            else if(mode != 0 && mode != 1)
            {
                r.result = false;
                r.msg = "Wrong Mode";
            }
            else
            {
                _mode = mode;
                Clients.Client(_hostID).setMode(_mode);
                r.result = true;
            }

            var repJson = JsonConvert.SerializeObject(r);
            return repJson;
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

        public void setDJMsg(string msg, int finishId)
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                var fid = Math.Min(Math.Max(finishId, 0), _cFinishNum);                
                Clients.Client(_hostID).setDJMsg(msg, fid);
            }
        }
        
        public void setDJCancal()
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                Clients.Client(_hostID).setDJCancal();
            }
        }

        public void updateIdleMsg(string msgSet)
        {

        }
        #endregion

        #region Interactive Mode
        public void setCheckerBoard()
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                Clients.Client(_hostID).setCheckerBoard();
            }
        }

        public void setAllWhite()
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                Clients.Client(_hostID).setAllWhite();
            }
        }

        public void readyQuestion()
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                Clients.Client(_hostID).readyQuestion();
                _state = interactiveState.sInteractiveCountdown;
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

        public void showAnswer(int qid)
        {
            if (_backendID != Context.ConnectionId)
            {
                return;
            }

            if (_hostID != "")
            {
                Clients.Client(_hostID).showAnswer(qid);
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