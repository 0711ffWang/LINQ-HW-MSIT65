using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinqLabs
{
    public partial class Frm作業_1 : Form
    {
        public Frm作業_1()
        {
            InitializeComponent();

            this.productsTableAdapter1.Fill(this.northwindDataSet1.Products);
            this.ordersTableAdapter1.Fill(this.northwindDataSet1.Orders);
            this.order_DetailsTableAdapter1.Fill(this.northwindDataSet1.Order_Details);

            this.productPhotoTableAdapter1.Fill(this.adventureWorksDataSet1.ProductPhoto);
        }

        private void button14_Click(object sender, EventArgs e)
        { 
            //files[0].Extension
            //files[0].CreationTime.Year

            this.lblMaster.Text = "LOG Files";
            System.IO.DirectoryInfo dirs = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dirs.GetFiles();

            var q = from f in files
                    where f.Extension.ToUpper() == ".LOG"
                    select f;
            this.dataGridView1.DataSource = q.ToList();

           
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            int year = 1997;
            int.TryParse(this.comboBox1.Text, out year);

            var q2 = from o in this.northwindDataSet1.Orders
                    where o.OrderDate.Year == year
                    select o;

            //NOTE  BindingSource
            this.bindingSource1.DataSource = q2.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.lblMaster.Text = "2017 Files";
            System.IO.DirectoryInfo dirs = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dirs.GetFiles();

            var q = from f in files
                    where f.CreationTime.Year==2017
                    orderby f.CreationTime descending
                    select f;
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            this.lblMaster.Text = "Large Files";
            System.IO.DirectoryInfo dirs = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dirs.GetFiles();

            var q = from f in files
                    where f.Length>2000
                    select f;
            this.dataGridView1.DataSource = q.ToList();
        }

        int page = -1;
        int countPerPage = 10;

        private void button13_Click(object sender, EventArgs e)
        {
            int.TryParse(this.textBox1.Text, out countPerPage);
         
             page += 1; 
            this.dataGridView1.DataSource = this.northwindDataSet1.Products.Skip(countPerPage * page).Take(countPerPage).ToList();

            // NOTE 可防呆
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
    
            int.TryParse(this.textBox1.Text, out countPerPage);

            page -= 1;
            this.dataGridView1.DataSource = this.northwindDataSet1.Products.Skip(countPerPage * page).Take(countPerPage).ToList();

          
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
            var TotalSales = this.northwindDataSet1.Order_Details.Sum(od => od.UnitPrice * od.Quantity);
            MessageBox.Show($"總銷售金額 ={TotalSales.ToString("c2")}");

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblDetails.Text = "Order details";

                //NOTE
                var OrderID = ((NorthwindDataSet.OrdersRow)this.bindingSource1.Current).OrderID;

                var q = from o in this.northwindDataSet1.Order_Details
                        where o.OrderID == OrderID
                        select o;

                this.dataGridView2.DataSource = q.ToList();

            }
            catch
            {

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblMaster.Text = "Orders";

            int year = 1997;
            int.TryParse(this.comboBox1.Text, out year);

            var q = from o in this.northwindDataSet1.Orders
                    where o.OrderDate.Year == year
                    select o;

            //NOTE - BindingSource
            this.bindingSource1.DataSource = q.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;

                
                          

        }


        public static string Season( DateTime source)
        {
            switch (source.Month)
            {
                case 1:
                case 2:
                case 3:
                    return "第一季";
                case 4:
                case 5:
                case 6:
                    return "第二季";
                case 7:
                case 8:
                case 9:
                    return "第三季";
                case 10:
                case 11:
                case 12:
                    return "第四季";

                default:
                  return "";

            }
           


        }

        private void button6_Click(object sender, EventArgs e)
        {

            this.lblMaster.Text = "Orders";

            var q1 = from o in this.northwindDataSet1.Orders
                         select o.OrderDate.Year;
           
            //NOTE Distinct()
            this.comboBox1.DataSource = q1.Distinct().ToList();

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //NOTE  supress error
            //
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //supress error
        }
    }
}
