using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace EarBiometric
{
    class sql_connection
    {
        public MySqlConnection connection = null;
        public MySqlCommand command = null;
        public MySqlDataReader reader = null;
        public sql_connection(){
            activate_SQLConnection();
        }
        public void activate_SQLConnection()
        {
            connection = new MySqlConnection("server=localhost;username=jeffreyongcay;password=server123;database=ear_biometric");
            try
            {
                open();
            }catch(Exception ex)
            {
                MessageBox.Show("SQL Connection error. " + ex.Message);
            }
            finally
            {
                close();
            }
        }

        // Open the SQL Connection
        public void open()
        {
            connection.Open();
        }

        // Close the SQL Connection
        public void close()
        {
            connection.Close();
        }

        // all the request of SQL command to the database here ( Function )
        public MySqlCommand SQLCommand(string sql)
        {
            command = new MySqlCommand(sql, connection);
            return command;
        }
        public MySqlDbType MysqlTypeBlob()
        {
            return MySqlDbType.Blob;
        }
    }
}
