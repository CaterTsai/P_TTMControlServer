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
        const int questionNum = 48;
        static questionUnit[] questionSet;
        static int questionId = -1;

        public questionController()
        {
            if (questionSet == null)
            {
                questionSet = new questionUnit[questionNum];
                for (var i = 0; i < questionNum; i++)
                {
                    questionSet[i] = new questionUnit();
                    questionSet[i].randomSet();
                }
                questionId = -1;
            }
        }

        #region Mobile
        [HttpGet]
        public string Get()
        {
            response rep = new response();
            rep.active = "question/";
            if (questionId != -1)
            {
                int index = -1;
                for (int i = 0; i < questionNum; i++)
                {
                    if (!questionSet[i].flag)
                    {
                        questionSet[i].flag = true;
                        index = i;
                        break;
                    }
                }
                rep.result = true;
                rep.data = questionSet[index].question;
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
            if (questionId != -1)
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
            if (val < 0 || val >= questionNum)
            {
                rep.msg = "Wrong Index";
                rep.result = false;
            }
            else
            {
                if (questionSet[val].flag)
                {
                    questionSet[val].flag = false;
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
            if (index < 0 || index >= questionNum)
            {
                rep.msg = "Wrong Index";
                rep.result = false;
            }
            else
            {
                if (ttmHub._hostID == "")
                {
                    rep.msg = "Host is disconnection";
                    rep.result = true;
                }
                else
                {
                    rep.result = true;
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
            var repJson = JsonConvert.SerializeObject(rep);
            return repJson;
        }
        #endregion

        #region Backend
        [HttpGet]
        [Route("api/question/set")]
        public string set(string code, int index)
        {
            questionId = index;
            response rep = new response();
            rep.active = "question/set/";
            if (code == System.Web.Configuration.WebConfigurationManager.AppSettings["code"])
            {
                for (int i = 0; i < questionNum; i++)
                {
                    questionSet[i].reset();
                    questionSet[i].randomSet(); //TODO
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
        #endregion
    }
}