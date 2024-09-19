using LinqLabs;
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
    public partial class Frm作業_2_AdventureWorksDataSet : Form
    {
        public Frm作業_2_AdventureWorksDataSet()
        {
            InitializeComponent();

            this.productsTableAdapter1.Fill(this.northwindDataSet1.Products);
            this.ordersTableAdapter1.Fill(this.northwindDataSet1.Orders);
            this.order_DetailsTableAdapter1.Fill(this.northwindDataSet1.Order_Details);

            this.productPhotoTableAdapter1.Fill(this.adventureWorksDataSet1.ProductPhoto);
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblDetails.Text = "Image details";

                //NOTE  byte[]
                byte[] LargePhoto= ((AdventureWorksDataSet.ProductPhotoRow)this.bindingSource1.Current).LargePhoto;
                System.IO.MemoryStream ms = new MemoryStream(LargePhoto);
                this.pictureBox1.Image = Image.FromStream(ms);

            }
            catch
            {

            }
        }

   
        private void button11_Click(object sender, EventArgs e)
        {
            this.lblMaster.Text = "All AdventureWorks ProductPhotos";

            //NOTE - BindingSource =>Show Image Details
            this.bindingSource1.DataSource = this.adventureWorksDataSet1.ProductPhoto.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;

            
            var q1 = from p in this.adventureWorksDataSet1.ProductPhoto
                         select p.ModifiedDate.Year;

            //NOTE Distinct()
            this.comboBox3.DataSource = q1.Distinct().ToList();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var q = from p in this.adventureWorksDataSet1.ProductPhoto
                    where p.ModifiedDate >= this.dateTimePicker1.Value && p.ModifiedDate <= this.dateTimePicker2.Value
                    select p;

            this.dataGridView1.DataSource = q.ToList();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            int year = DateTime.Now.Year;
            int.TryParse(this.comboBox3.Text, out year);

            var q = from p in this.adventureWorksDataSet1.ProductPhoto
                    where p.ModifiedDate .Year == year 
                    select p;

            this.dataGridView1.DataSource = q.ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var q = from p in this.adventureWorksDataSet1.ProductPhoto
                    where Season(p.ModifiedDate) == this.comboBox2.Text
                    select p;

            this.dataGridView1.DataSource = q.ToList();

            this.lblMaster.Text = $"{this.comboBox2.Text} 腳踏車 - 共 {q.Count()} 筆";


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

   
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //NOTE  supress error
          
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //supress error
        }

          }
}
