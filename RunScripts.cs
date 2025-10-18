using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace FactoryManagerInstaller
{
    public class RunScripts
    {

        public static string database;
        public static void AddInitialData(string driver, string server)
        {
            string connectionString = "";
            //connectionString = "DRIVER= {" + driver + "}; server= " + server + "; Database= " + database + "; User=root; password=1234";

            MySqlConnection conn = new MySqlConnection();
            //conn.ConnectionString = connectionString;
            try
            {
                DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "SQL");
                FileInfo[] files = di.GetFiles("*.sql");

                foreach (FileInfo file in files)
                {
                    try
                    {
                        string script = file.OpenText().ReadToEnd();
                       string[] queries = script.Split(new string[] { ";go;" }, StringSplitOptions.RemoveEmptyEntries);

                      // string[] queries = script.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string query in queries)
                        {
                            if (query == "\r\nUSE wivib_x")
                            {
                                //continue;
                                RunScripts.database = "wivib_x";

                            }

                            if (RunScripts.database == "" || RunScripts.database == null)
                            {
                                //continue;
                                connectionString = "datasource= " + server + "; Database= sys; User=root; password=1234";
                            }
                            else
                            {
                                connectionString = "datasource= " + server + "; Database= " + database + "; User=root; password=1234";
                            }
                            conn.ConnectionString = connectionString;
                            //ended here.......


                            MySqlCommand cmd = new MySqlCommand();
                            cmd.CommandText = query;
                            cmd.Connection = conn;
                            cmd.CommandType = System.Data.CommandType.Text;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }

                    }
                    catch (Exception ex)
                    { 
                    }
                    finally
                    { 
                        conn.Close(); }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
