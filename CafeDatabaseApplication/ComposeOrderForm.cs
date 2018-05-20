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
        private int id_order;

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
                    String query = "SELECT a.title, a.id, a.price, aa.quantity FROM available_assortiments aa, assortiments a WHERE aa.id_assortiment = a.id AND aa.id_cafe = (SELECT id_cafe FROM garcons WHERE id = 2);";
                    //SqlCommand command = new SqlCommand(query, this.connection);
                    //SqlDataAdapter adapter = new SqlDataAdapter(command);
                    //DataSet dataset = new DataSet();
                    //adapter.Fill(dataset, "AvailableAssesories");
                    adapters.Add("AvailableAssesories", new SqlDataAdapter(query, connection));
                    adapters["AvailableAssesories"].Fill(dataSets, "AvailableAssesories");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Compose Form: Can not open connection ! Exception message: " + ex.Message);
            }
            
            InitializeComponent();

            try
            {
                SqlCommand cmd_ins = new SqlCommand("INSERT INTO orders (id_garcon, id_discount) VALUES (@gar, @dis)", this.connection);
                SqlParameter par1 = new SqlParameter("@gar", SqlDbType.Int);
                par1.Value = this.id_garcon;
                cmd_ins.Parameters.Add(par1);

                SqlParameter par2 = new SqlParameter("@dis", SqlDbType.Int);
                par2.Value = 1;
                cmd_ins.Parameters.Add(par2);

                this.id_order = cmd_ins.ExecuteNonQuery();
                MessageBox.Show("id order : " + this.id_order);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to insert into orders : " + ex.Message);
            }




            try
            {
                int n = dataSets.Tables["AvailableAssesories"].Rows.Count;

                
                for (int i = 0; i < n; i++)
                {
                    Image myImage = Image.FromFile(@"..\..\Images\image" + (i+1) + ".jpg");
                    // listBox1.Items.Add(new ListBoxItem(dataSets.Tables["AvailableAssesories"].Rows[i].Field<String>("title"), dataSets.Tables["AvailableAssesories"].Rows[i].Field<int>("id") ));
                    String description;
                    description = " Price for 1 piece : " + dataSets.Tables["AvailableAssesories"].Rows[i].Field<Double>("price");
                    listBox1.Items.Add(
                        new exListBoxItem(i, 
                            dataSets.Tables["AvailableAssesories"].Rows[i].Field<String>("title"),
                            description, 
                            myImage)
                        );
                   
                }               
            }
            catch(Exception ex)
            {
                MessageBox.Show("Faild to populate list box. Error message : "+ ex.Message);
            }

        }

        private void ComposeOrderForm_Load(object sender, EventArgs e)
        {
            


            //listBox1.DataSource = dataSets.Tables["AvailableAssesories"];
            /*
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = true;
            //BindingSource bs = new BindingSource();
            //bs.DataSource = 
            dataGridView1.DataSource = 
            dataGridView1.Refresh();

            MessageBox.Show("Here is " + dataSets.Tables["AvailableAssesories"].Rows.Count + " rows");
            */
/*            dataGridView1.Visible = true;
            dataGridView1.Enabled = true;          
            */
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            this.Text = ((exListBoxItem)listBox1.SelectedItem).Id.ToString();
        }

        private void exListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
        }

        private void exListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = ((exListBoxItem)listBox1.SelectedItem).Id.ToString();
        }
    }
}
