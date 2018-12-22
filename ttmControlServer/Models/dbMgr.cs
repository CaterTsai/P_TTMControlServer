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
        private bool _usedDB = false;
        private SqlConnection _sqlConn = null;
        public dbMgr()
        {
            _usedDB = (System.Web.Configuration.WebConfigurationManager.AppSettings["useDB"] == "1");
        }

        public void connDB()
        {
            if(!_usedDB)
            {
                return;
            }
            var host = System.Web.Configuration.WebConfigurationManager.AppSettings["dbHost"];
            var db = System.Web.Configuration.WebConfigurationManager.AppSettings["dbDatabase"];
            var user = System.Web.Configuration.WebConfigurationManager.AppSettings["dbUser"];
            var pw = System.Web.Configuration.WebConfigurationManager.AppSettings["dbPW"];
            string strConn = "server=" + host + ";database=" + db + ";uid=" + user + ";pwd=" + pw;

            _sqlConn = new SqlConnection(strConn);
        }

        public void getQuestion(ref questionData[] questionSet)
        {
            if (!_usedDB)
            {
                return;
            }
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
            if (!_usedDB)
            {
                return "";
            }
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
        
        public void setLog(string hello, int type, int msgId)
        {
            if (!_usedDB)
            {
                return;
            }
            using (SqlCommand cmd = new SqlCommand("addLog", _sqlConn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@hello", SqlDbType.VarChar).Value = hello;
                cmd.Parameters.Add("@type", SqlDbType.TinyInt).Value = type;
                cmd.Parameters.Add("@msgId", SqlDbType.TinyInt).Value = msgId;
                try
                {
                    _sqlConn.Open();
                    cmd.ExecuteNonQuery();
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