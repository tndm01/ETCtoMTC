using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace ITD.ETC.VETC.Synchonization.Controller.Database
{
    public class DBConnection
    {
        protected SqlConnection conn = new SqlConnection();
        public DBConnection()
        {
            try
            {

                conn.ConnectionString = @"Data Source=DESKTOP-4LNR8GO\SQLEXPRESS;Initial Catalog=Synchronization_ETC;Integrated Security=True";

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
