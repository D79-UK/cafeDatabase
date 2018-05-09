using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeDatabaseApplication
{
    public partial class ComposeOrderForm : Form
    {
        private int id_garcon;
        

        private SqlConnection connection = null;
        private Dictionary<string, SqlDataAdapter> adapters;
        private DataSet dataSets;
        private string[] tableNames = {
                                        "assortiments", "orders", "ordered_assortiments", "assortiments_products",
                                        "available_assortiments",
                                        "discounts"
                                      };

        public ComposeOrderForm(int id_g, SqlConnection con)
        {
            id_garcon = id_g;
            //MessageBox.Show("Happy garcon: " + id_garcon);
            
            connection = con;
            adapters = new Dictionary<string, SqlDataAdapter>();
            dataSets = new DataSet();

            try
            {
                if (connection != null && connection.State.ToString() == "Open")
                {
                    
                    foreach (string t in tableNames)
                    {
                        adapters.Add(t, new SqlDataAdapter("SELECT * FROM " + t, connection));
                        adapters[t].Fill(dataSets, t);
                    }
                    InitializeComponent();

                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Compose Form: Can not open connection ! Exception message: " + ex.Message);
            }
            
            InitializeComponent();
            
        }

        private void OnLoad(object sender, EventArgs e)
        {
            String query = "SELECT a.title, a.id, a.price, aa.quantity FROM available_assortiments aa, assortiments a WHERE aa.id_assortiment = a.id AND aa.id_cafe = (SELECT id_cafe FROM garcons WHERE id = 2);";
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.Text;
            adapter.SelectCommand = command;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Visible = true;
            dataGridView1.Enabled = true;
            dataGridView1.AutoGenerateColumns = true;            
        }

        private void AddButton_Click(object sender, EventArgs e)
        {

        }
    }
}
