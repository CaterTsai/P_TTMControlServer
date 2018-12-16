using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace ttmControlServer.Models
{
    public class dbMgr
    {
        private SqlConnection _sqlConn = null;
        public dbMgr()
        {

        }

        public void connDB()
        {
            var host = System.Web.Configuration.WebConfigurationManager.AppSettings["dbHost"];
            var db = System.Web.Configuration.WebConfigurationManager.AppSettings["dbDatabase"];
            var user = System.Web.Configuration.WebConfigurationManager.AppSettings["dbUser"];
            var pw = System.Web.Configuration.WebConfigurationManager.AppSettings["dbPW"];
            string strConn = "server=" + host + ";database=" + db + ";uid=" + user + ";pwd=" + pw;

            _sqlConn = new SqlConnection(strConn);
        }

        public void getQuestion(ref questionData[] questionSet)
        {
            using (SqlCommand cmd = new SqlCommand("getQuestion", _sqlConn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    _sqlConn.Open();
                    using (var data = cmd.ExecuteReader())
                    {
                        int index = 0;
                        while (data.Read())
                        {
                            if (data[0].Equals(DBNull.Value))
                            {
                                break;
                            }
                            questionSet[index].setQuestion(data["question"].ToString());
                            index++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.GetBaseException();
                }
                finally
                {
                    _sqlConn.Close();
                    cmd.Dispose();
                }
            }
        }

        public string getDiscount()
        {
            string code = "";
            using (SqlCommand cmd = new SqlCommand("getDiscountCode", _sqlConn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    _sqlConn.Open();
                    using (var data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            data.Read();
                            code = data["code"].ToString();
                            code = code.Replace(" ", string.Empty);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.GetBaseException();
                }
                finally
                {
                    _sqlConn.Close();
                    cmd.Dispose();
                }
            }
            return code;
        }

        public void getDJMassage(ref List<djMsgData> djMsgList)
        {
            using (SqlCommand cmd = new SqlCommand("getDJMassage", _sqlConn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    _sqlConn.Open();
                    using (var data = cmd.ExecuteReader())
                    {
                        djMsgList.Clear();
                        while (data.Read())
                        {
                            if (data[0].Equals(DBNull.Value))
                            {
                                break;
                            }

                            djMsgData djMsg = new djMsgData();
                            djMsg.type = Convert.ToInt32(data["type"]);
                            djMsg.typeName = data["typeName"].ToString();
                            djMsg.msg = data["msg"].ToString();

                            djMsgList.Add(djMsg);
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw ex.GetBaseException();
                }
                finally
                {
                    _sqlConn.Close();
                    cmd.Dispose();
                }
            }
        }
    }
}