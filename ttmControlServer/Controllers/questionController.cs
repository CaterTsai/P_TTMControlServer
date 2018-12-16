using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ttmControlServer.Models;
using ttmControlServer.SignalR;

namespace ttmControlServer.Controllers
{
    public class questionController : ApiController
    {
        const int questionNum = 5;
        const int answerNum = 48;

        static questionData[] questionSet;        
        static answerUnit[] answerSet;
        static bool startEvent = false;
        static int answerId = -1;
        private dbMgr _dbMgr = new dbMgr();

        public questionController()
        {   
            if (answerSet == null)
            {
                answerSet = new answerUnit[answerNum];
                for (var i = 0; i < answerNum; i++)
                {
                    answerSet[i] = new answerUnit();
                    answerSet[i].randomSet();
                }
                answerId = -1;
            }

            if(questionSet == null)
            {
                _dbMgr.connDB();
                questionSet = new questionData[questionNum];
                for(int i = 0; i < questionNum; i++)
                {
                    questionSet[i] = new questionData();
                }
                _dbMgr.getQuestion(ref questionSet);
            }
        }

        #region Mobile
        [HttpGet]
        public string Get()
        {
            response rep = new response();
            rep.active = "question/";
            if (answerId != -1)
            {
                int index = -1;
                for (int i = 0; i < answerNum; i++)
                {
                    if (!answerSet[i].flag)
                    {
                        answerSet[i].flag = true;
                        index = i;
                        break;
                    }
                }
                rep.result = true;
                rep.data = answerSet[index].answer;
                rep.index = index;
            }
            else
            {
                rep.result = false;
                rep.msg = "out of time";
            }
            
            var repJson = JsonConvert.SerializeObject(rep);
            return repJson;
        }

        [HttpGet]
        [Route("api/question/check")]
        public string check()
        {
            response rep = new response();
            rep.active = "question/check";
            if (startEvent)
            {
                rep.result = true;
            }
            else
            {
                rep.result = false;
                rep.msg = "out of time";
            }

            var repJson = JsonConvert.SerializeObject(rep);
            return repJson;
        }

        [HttpGet]
        [Route("api/question/{val}")]
        public string cancal(int val)
        {
            response rep = new response();
            rep.active = "question/cancal/";
            if (val < 0 || val >= answerNum)
            {
                rep.msg = "Wrong Index";
                rep.result = false;
            }
            else
            {
                if (answerSet[val].flag)
                {
                    answerSet[val].flag = false;
                    rep.result = true;
                }
                else
                {
                    rep.msg = "Wrong Flag";
                    rep.result = false;
                }
            }
            var repJson = JsonConvert.SerializeObject(rep);
            return repJson;
        }

        [HttpGet]
        [Route("api/question/ans")]
        public string ans(int index, string ans)
        {
            response rep = new response();
            rep.active = "question/ans/";
            if (index < 0 || index >= answerNum)
            {
                rep.msg = "Wrong Index";
                rep.result = false;
            }
            else
            {
                if(!startEvent)
                {
                    rep.msg = "Out of Time";
                    rep.result = false;
                }

                if (ttmHub._hostID == "")
                {
                    rep.msg = "Host is disconnection";
                    rep.result = false;
                }
                else if(ttmHub._mode == 0)
                {
                    rep.msg = "Wrong Mode";
                    rep.result = false;
                }
                else
                {
                    if(!answerSet[index].flag || answerSet[index].isAnswer)
                    {
                        rep.msg = "question flag is wrong";
                        rep.result = false;
                    }
                    else
                    {
                        _dbMgr.connDB();
                        answerSet[index].isAnswer = true;
                        rep.result = true;
                        rep.data = _dbMgr.getDiscount();
                        try
                        {
                            var context = GlobalHost.ConnectionManager.GetHubContext<ttmHub>();
                            context.Clients.Client(ttmHub._hostID).setAnswer(index, ans);
                        }
                        catch (Exception e)
                        {
                            rep.result = false;
                            rep.msg = e.Message;
                        }
                    }
                }

            }
            var repJson = JsonConvert.SerializeObject(rep);
            return repJson;
        }



        #endregion

        #region Backend
        [HttpGet]
        [Route("api/question/start")]
        public string start(string code)
        {
            response rep = new response();
            rep.active = "question/start/";
            if (code == System.Web.Configuration.WebConfigurationManager.AppSettings["code"])
            {
                startEvent = true;
                rep.result = true;
            }
            else
            {
                rep.msg = "Wrong Code";
                rep.result = false;
            }
            var repJson = JsonConvert.SerializeObject(rep);
            return repJson;
        }

        [HttpGet]
        [Route("api/question/stop")]
        public string stop(string code)
        {
            response rep = new response();
            rep.active = "question/stop/";
            if (code == System.Web.Configuration.WebConfigurationManager.AppSettings["code"])
            {
                startEvent = false;
                rep.result = true;
            }
            else
            {
                rep.msg = "Wrong Code";
                rep.result = false;
            }
            var repJson = JsonConvert.SerializeObject(rep);
            return repJson;
        }

        [HttpGet]
        [Route("api/question/set")]
        public string set(string code, int index)
        {
            answerId = index;
            response rep = new response();
            rep.active = "question/set/";
            if (code == System.Web.Configuration.WebConfigurationManager.AppSettings["code"])
            {
                for (int i = 0; i < answerNum; i++)
                {
                    answerSet[i].reset();
                    bool[] answerList = new bool[16];
                    questionSet[index].getAnswer(i, ref answerList);
                    answerSet[i].set(ref answerList);
                    
                }
                rep.result = true;
            }
            else
            {
                rep.msg = "Wrong Code";
                rep.result = false;
            }
            var repJson = JsonConvert.SerializeObject(rep);
            return repJson;
        }

        [HttpGet]
        [Route("api/question/showAns")]
        public string showAns(string code)
        {
            response rep = new response();
            rep.active = "question/showAns/";
            if (code == System.Web.Configuration.WebConfigurationManager.AppSettings["code"])
            {
                answerId = -1;
                rep.result = true;
            }
            else
            {
                rep.msg = "Wrong Code";
                rep.result = false;
            }
            var repJson = JsonConvert.SerializeObject(rep);
            return repJson;
        }
        #endregion
    }
}