using System;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace CafeDatabaseApplication
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            //  SqlConnection connection = new SqlConnection("Data Source=den1.mssql3.gear.host;Initial Catalog=cafedatabase;" +
            // "Persist Security Info=True;User ID=cafedatabase;Password=Sl18~Q43eq!s");
            SqlConnection connection = new SqlConnection("Server = DESKTOP-JSJDM0T\\SQLEXPRESS01; Database = cafedatabase; Trusted_Connection = True;");

            


            try
            {
                connection.Open();                
                if (connection != null && connection.State.ToString() == "Open")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);                    
                    Application.Run(new DatabaseMainForm(connection));
                }
                // it is up to main form to closed connection
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Main : Exception message: " + ex.Message);
            }

            
        }
    }
}
