using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
//using DevExpress.XtraMap;

namespace CafeDatabaseApplication
{
    public partial class cafeDatabaseMainForm : Form
    {

        private SqlConnection connection = null;
        private Dictionary<string, SqlDataAdapter> adapters;
        private DataSet dataSets;
        private string[] tableNames = { "cafes", "admins",
                                        "products", "supplies", "suppliers", "supplied_products", "stored_products",
                                        "garcons",
                                        "assortiments", "orders", "ordered_assortiments", "assortiments_products",
                                        "available_assortiments",
                                        "discounts"
                                      };
        private ComposeOrderForm composeOrderForm;
       // private Form2 form;

        public cafeDatabaseMainForm(SqlConnection connection_)
        {
            // Connection to remote database is successfully opened and passed to form constructor
            connection = connection_;
            adapters = new Dictionary<string, SqlDataAdapter>();
            dataSets = new DataSet();
            try
            {                
                if (connection != null && connection.State.ToString() == "Open")
                {                    
                    /* Fill data sets 
                     * */
                    foreach (string t in tableNames)
                    {
                        adapters.Add(t, new SqlDataAdapter("SELECT * FROM " + t, connection));
                        adapters[t].Fill(dataSets, t);
                    }
                    /* Initialize form components 
                    * */
                    InitializeComponent();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Main Form: Can not open connection ! Exception message: " + ex.Message);
            }
        }

        ~cafeDatabaseMainForm()
        {
            //connection.Close();
        }

        private void cafeDatabaseMainForm_Load(object sender, EventArgs e)
        {
            /* Link data to grid view components 
            * */
            cafeDataGridView.DataSource = dataSets.Tables["cafes"];
          
            garconDataGridView.DataSource = dataSets.Tables["garcons"];
            assortimentGridView.DataSource = dataSets.Tables["assortiments"];

            cafeDataGridView.Columns[0].Name="#";
            cafeDataGridView.Columns[0].Width = 40;
            cafeDataGridView.Columns[1].Name="Title";
            cafeDataGridView.Columns[1].Width = 200;
            /*
            bindingNavigator1.BindingSource.DataSource = dataSets.Tables["cafes"];
            bindingNavigator2.BindingSource.DataSource = dataSets.Tables["garcons"];
            bindingNavigator3.BindingSource.DataSource = dataSets.Tables["assortiments"];
            */
            /*
            // Create a map control.
            MapControl map = new MapControl();

            // Specify the map position on the form.           
            map.Dock = DockStyle.Fill;

            // Create a layer.
            ImageLayer layer = new ImageLayer();
            map.Layers.Add(layer);

            // Create a data provider.
            OpenStreetMapDataProvider provider = new OpenStreetMapDataProvider();
            layer.DataProvider = provider;

            // Specify the map zoom level and center point. 
            map.ZoomLevel = 2;
            map.CenterPoint = new GeoPoint(38, -100);

            // Add the map control to the window.
            this.Controls.Add(map);
            */
        }

        private void orderProductsButton_Click(object sender, EventArgs e)
        {
            /*
             * Order Products
             * */
        }

        private void composeOrderButton_Click(object sender, EventArgs e)
        {
          
            
            int id_garcon = 1;
            String id_g_str = this.garconDataGridView.CurrentRow.Cells[0].ToString();
            id_garcon = Convert.ToInt32("2");

           // MessageBox.Show("Happy garcon: " + id_garcon);

            composeOrderForm = new ComposeOrderForm(id_garcon, this.connection );
            composeOrderForm.Show();
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void updateCafesTable(object sender, EventArgs e)
        {

            SqlCommandBuilder builder = new SqlCommandBuilder( adapters["cafes"] );
            adapters["cafes"].UpdateCommand = builder.GetUpdateCommand();
            adapters["cafes"].Update(dataSets.Tables["cafes"]);


            /*
             * 
             * /*
            DataRow row = table.NewRow();            
            row["title"] = "Cafe on Kreschatyk";
            row["address"] = "Khreschatyk street, 10";
            table.Rows.Add(row);
            table.AcceptChanges();
            dataGridView1.DataSource = table;
            string cmdString = "INSERT INTO cafes (title,address) VALUES (@val1, @val2)";
            using (SqlCommand comm = new SqlCommand())
            {
                    comm.Connection = connection;
                    comm.CommandText = cmdString;                    
                    comm.Parameters.AddWithValue("@val1", "Tiny street");
                    comm.Parameters.AddWithValue("@val2", "Dead end");
                    try
                    {   comm.ExecuteNonQuery();
                    }catch(SqlException e)
                    {   // do smth with the exception                        
                    }
            }
*/
            /*
             *  string tableName = "cafe";

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", tableName);

                string sql = "SELECT * FROM " + tableName; // + " ORDER BY id DESC LIMIT 10";
                SqlCommand command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;
                DataSet dataSet = new DataSet(tableName);
                adapter.Fill(dataSet);              

                //for INSERT, UPDATE, DELETE command use ExecuteNonQuery()
                //command.ExecuteNonQuery();
                //for SELECT command use
                SqlDataReader dataReader = command.ExecuteReader();
                DataTable schemaTable = dataReader.GetSchemaTable();
                // do smth
                command.Dispose();
                dataReader.Close();
             * */

        }

        private void updateGarconsTable(object sender, EventArgs e)
        {
            
            SqlCommandBuilder builder = new SqlCommandBuilder(adapters["garcons"]);
            adapters["garcons"].UpdateCommand = builder.GetUpdateCommand();
            adapters["garcons"].Update(dataSets.Tables["garcons"]);
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void updateAssortimentsTable(object sender, EventArgs e)
        {
            SqlCommandBuilder builder = new SqlCommandBuilder(adapters["assortiments"]);
            adapters["assortiments"].UpdateCommand = builder.GetUpdateCommand();
            adapters["assortiments"].Update(dataSets.Tables["assortiments"]);
        }
    }//[end] class cafeDatabaseMainForm
}//[end] namespace CafeDatabaseApplication
