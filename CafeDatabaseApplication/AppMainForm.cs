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
    public partial class DatabaseMainForm : Form
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

        public DatabaseMainForm(SqlConnection connection_)
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

        ~DatabaseMainForm()
        {
            connection.Close();
        }

        private void cafeDatabaseMainForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "cafedatabaseDataSet.cafes". При необходимости она может быть перемещена или удалена.
            this.cafesTableAdapter.Fill(this.cafedatabaseDataSet.cafes);

            bindingSource1.DataSource = dataSets.Tables["cafes"];
            cafeDataGridView.DataSource = bindingSource1;
            bindingNavigator1.BindingSource = bindingSource1;
       
            bindingSource2.DataSource = dataSets.Tables["garcons"];
            garconDataGridView.DataSource = bindingSource2;
            bindingNavigator2.BindingSource = bindingSource2;

            bindingSource3.DataSource = dataSets.Tables["assortiments"];
            assortimentGridView.DataSource = bindingSource3;
            bindingNavigator3.BindingSource = bindingSource3;


        
            SqlCommand cmd = new SqlCommand("SELECT title, price FROM assortiments ORDER BY price DESC", this.connection);
            SqlDataReader datareader;
            try{
                datareader = cmd.ExecuteReader();
                while (datareader.Read()){
                    this.chart1.Series["AssortimentPrice"].Points.AddXY( datareader.GetString(0), datareader.GetDouble(1) );
                }
                datareader.Close();
            }catch(Exception ex){
                MessageBox.Show(ex.Message);
            }
            

            SqlCommand cmd2 = new SqlCommand("SELECT c.name, COUNT(ord.id) FROM garcons c, orders ord WHERE c.id = ord.id_garcon GROUP BY c.name", this.connection);
            SqlDataReader datareader2;
            try
            {
                datareader2 = cmd2.ExecuteReader();
                while (datareader2.Read())
                {
                    this.chart2.Series["OrderNum"].Points.AddXY(datareader2.GetString(0), datareader2.GetInt32(1));
                }
                datareader2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SqlCommand cmd3 = new SqlCommand("SELECT g.name, SUM(ord.amount) FROM garcons g, orders ord WHERE ord.id_garcon = g.id GROUP BY g.name", this.connection);
            SqlDataReader datareader3;
            try
            {
                datareader3 = cmd3.ExecuteReader();
                while (datareader3.Read())
                {
                    this.chart3.Series["OrderAmount"].Points.AddXY(datareader3.GetString(0), datareader3.GetDouble(1));
                }
                datareader3.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            SqlCommand cmd4 = new SqlCommand("SELECT a.title, COUNT(ord.id_assortiment) FROM assortiments a, ordered_assortiments ord WHERE a.id = ord.id_assortiment GROUP BY a.title", this.connection);
            SqlDataReader datareader4;
            try
            {
                datareader4 = cmd4.ExecuteReader();
                while (datareader4.Read())
                {
                    this.chart4.Series["OrderNum"].Points.AddXY(datareader4.GetString(0), datareader4.GetInt32(1));
                }
                datareader4.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        }

        private void composeOrderButton_Click(object sender, EventArgs e)
        {

            int selectedCount =  this.garconDataGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedCount > 0)
            {
                int rowIndex = this.garconDataGridView.SelectedRows[0].Index;
                DataGridViewRow row = this.garconDataGridView.Rows[rowIndex];
                int id_garcon = 1;
                String id_g_str = row.Cells[0].Value.ToString();
                id_garcon = Convert.ToInt32(id_g_str);

                MessageBox.Show("Обслуговує офіціант: "+ row.Cells[1].Value.ToString());
                 
                composeOrderForm = new ComposeOrderForm(id_garcon, this.connection);
                composeOrderForm.Show();
            }else MessageBox.Show("Оберіть спочатку офіціанта");


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

        private void orderProductsButtonControll_Click(object sender, EventArgs e)
        {
            /*
 * Order Products
 * */
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            cafesTableAdapter.Update(this.cafedatabaseDataSet);
            //MessageBox.Show("Updated");
            //DataBind();

            /*
            SqlCommandBuilder builder = new SqlCommandBuilder(adapters["cafes"]);
            adapters["cafes"].UpdateCommand = builder.GetUpdateCommand();
            adapters["cafes"].Update(dataSets.Tables["cafes"]);
            */
        }

        private void garconDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {/*
            int rowIndex = e.RowIndex;
            DataGridViewRow row = this.garconDataGridView.Rows[rowIndex];
            int id_garcon = 1;
            String id_g_str = row.Cells[1].Value.ToString();
            id_garcon = Convert.ToInt32(id_g_str);

            MessageBox.Show("Happy garcon: " + id_garcon);

            composeOrderForm = new ComposeOrderForm(id_garcon, this.connection);
            composeOrderForm.Show();
            */

        }
    }//[end] class cafeDatabaseMainForm
}//[end] namespace CafeDatabaseApplication
