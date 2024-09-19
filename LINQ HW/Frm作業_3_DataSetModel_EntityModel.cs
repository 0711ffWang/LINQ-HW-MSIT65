using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinqLabs
{
    public partial class Frm作業_3_DataSetModel_EntityModel : Form
    {
        public Frm作業_3_DataSetModel_EntityModel()
        {
            InitializeComponent();

            this.productsTableAdapter1.Fill(this.northwindDataSet1.Products);
            this.ordersTableAdapter1.Fill(this.northwindDataSet1.Orders);
            this.order_DetailsTableAdapter1.Fill(this.northwindDataSet1.Order_Details);
            this.dbContext.Database.Log = Console.WriteLine;

        }
        //NOTE change 來源物件

        //Entity Model
        //global::LinqLabs.NorthwindEntities dbContext = new NorthwindEntities();

        //DataSet Model
        NorthwindDataSet northwindDataSet1 = new NorthwindDataSet();

        private void button4_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            List<int> smallList = new List<int>();
            List<int> mediumList = new List<int>();
            List<int> largeList = new List<int>();


            foreach (int n in nums)
            {
                if (n <= 5)
                {
                    smallList.Add(n);
                }
                else if (n > 5 && n <= 10)
                {
                    mediumList.Add(n);
                }
                else if (n > 10 && n <= 20)
                {
                    largeList.Add(n);
                }
            }
            this.treeView1.Nodes.Clear();
            TreeNode smallNode = this.treeView1.Nodes.Add("Small List");
            TreeNode mediumNode = this.treeView1.Nodes.Add("Medium List");
            TreeNode largeNode = this.treeView1.Nodes.Add("Large List");

            foreach (int n in smallList)
                smallNode.Nodes.Add(n.ToString());
            foreach (int n in  mediumList)
                mediumNode.Nodes.Add(n.ToString());
            foreach (int n in largeList)
                largeNode.Nodes.Add(n.ToString());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            //銷售最好的業務員
            //NOTE -  Entity Model - 導覽屬性 (不用 join) group od by new { }
           

            var q2 = from od in this.dbContext.Order_Details
                     group od by new { od.Order.EmployeeID, od.Order.Employee.FirstName, od.Order.Employee.LastName } into g
                     select new { g.Key, sales = g.Sum(od => od.UnitPrice * od.Quantity) };

            this.dataGridView1.DataSource = q2.OrderByDescending(od => od.sales).Take(5).ToList();

        }
        //Entity Model
        global::LinqLabs.NorthwindEntities dbContext = new NorthwindEntities();

        private void button2_Click_1(object sender, EventArgs e)
        {

            var TotalSales = this.dbContext.Order_Details.Sum(od =>(double) od.UnitPrice * od.Quantity * (1- od.Discount));
            MessageBox.Show($"總銷售金額 ={TotalSales.ToString("c2")}");

        }

        private void button38_Click(object sender, EventArgs e)
        {

            
            System.IO.DirectoryInfo dirs = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dirs.GetFiles();

            //NOTE orderby
            var q = from f in files
                    orderby f.Length descending
                    group f by MyFileGroup(f.Length) into g
                    select new MyData {MyKey= g.Key, MyCount = g.Count(), MyGroup = g};//select 具名型別


            this.bindingSource1.DataSource = q.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;

            this.lblMaster.Text = $"Files ({q.Count()})";
            //================================

            //treeview
            this.treeView1.Nodes.Clear();
            foreach (var group in q)
            {
                string s = $"{group.MyKey} ({group.MyCount})";
                TreeNode x = this.treeView1.Nodes.Add(s);

                foreach (var item in group.MyGroup)
                {
                    x.Nodes.Add(item.ToString());
                }
            }
            //======================


            //Chart
            this.chart1.DataSource = q.ToList();
            this.chart1.Series[0].XValueMember = "MyKey";
            this.chart1.Series[0].YValueMembers = "MyCount";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

        }

        string MyFileGroup(long length)
        {
            var kb = length / 1000;
            if (kb <= 100)
                return "小 (0-100KB)";
            else if (kb >= 1000)
                return "大 (> 1 MB)";
            else
                return "適中";

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            //NOTE: for 具名型別
            var Source = (MyData)this.bindingSource1.Current;
            this.lblDetails.Text = $"Files ({Source.MyCount})";

            var q = from f in Source.MyGroup
                    select new { f.Name, Length = $"{f.Length / 1024} KB", f.CreationTime, f.Extension };
            this.dataGridView2.DataSource = q.ToList();

           
        }

        private class MyData
        {
            public string MyKey { get; set; }
            public int MyCount { get; set; }
            public IGrouping<string, FileInfo> MyGroup { get; set; }
          
        }

        private void button6_Click(object sender, EventArgs e)
        {

            System.IO.DirectoryInfo dirs = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dirs.GetFiles();
            
            //NOTE orderby year
            var q = from f in files
                    orderby f.CreationTime.Year descending
                    group f by f.CreationTime.Year into g
                    select new { MyKey = g.Key, MyCount = g.Count(), MyGroup = g.ToList() };//select 匿名型別


            this.bindingSource2.DataSource = q.ToList();
            this.dataGridView1.DataSource = this.bindingSource2;

            this.lblMaster.Text = $"Files ({q.Count()})";


            //============================

            //treeview
            this.treeView1.Nodes.Clear();
            foreach (var group in q)
            {
                string s = $"{group.MyKey} ({group.MyCount})";
                TreeNode x = this.treeView1.Nodes.Add(s);

                foreach (var item in group.MyGroup)
                {
                    x.Nodes.Add(item.ToString());
                }
            }
            //======================

            //Chart
            this.chart1.DataSource = q.ToList();
            this.chart1.Series[0].XValueMember = "MyKey";
            this.chart1.Series[0].YValueMembers = "MyCount";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            this.chart1.Series[0].BorderWidth = 4;
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {
            //NOTE
            //for 匿名型別 - 使用 dynamic 來存取。
            try
            {
                dynamic d = this.bindingSource2.Current;
                this.lblDetails.Text = $"Files ({d.MyCount})";
                this.dataGridView2.DataSource = d.MyGroup;  //this.dataGridView2.DataSource =( (IGrouping<int, FileInfo> ) d.MyGroup).ToList(); dynamic的一個小坑-- RuntimeBinderException：“object”未包含“xxx”的定義
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            var q = dbContext.Products.OrderByDescending(p => p.UnitPrice).Take(5)
                                      .Select(p => new { p.ProductID, p.ProductName, p.Category.CategoryName, p.UnitPrice, p.UnitsInStock });
            this.dataGridView1.DataSource = q.ToList();


        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool result = this.dbContext.Products.Any(p => p.UnitPrice > 300);
            MessageBox.Show($"產品有任何一筆單價大於300 ?  {result}");

        }

        private void button8_Click(object sender, EventArgs e)
        {

            var q = from p in this.northwindDataSet1.Products.AsEnumerable()
                    orderby p.UnitPrice descending
                    group p by MyPriceGroup(p.UnitPrice) into g
                    select new  { MyKey = g.Key, MyCount = g.Count(), MyGroup = g.ToList() };


            this.bindingSource2.DataSource = q.ToList();
            this.dataGridView1.DataSource = this.bindingSource2;

            this.lblMaster.Text = $"Products ({q.Count()})";
            //================================

            //treeview
            this.treeView1.Nodes.Clear();
            foreach (var group in q)
            {
                string s = $"{group.MyKey} ({group.MyCount})";
                TreeNode x = this.treeView1.Nodes.Add(s);

                foreach (var item in group.MyGroup)
                {
                    x.Nodes.Add($"{item.ProductName} - {item.UnitPrice}");
                }
            }
            //======================


            //NOTE Pie Chart - One Series
            this.chart1.Series.Clear();
            this.chart1.Series.Add("UnitPrice");

            this.chart1.DataSource = q.ToList();
            this.chart1.Series[0].XValueMember = "MyKey";
            this.chart1.Series[0].YValueMembers = "MyCount";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

        }

        private object MyPriceGroup(decimal? unitPrice)
        {
            if (unitPrice<10)
            {
                return "低價";
            }

            else if (unitPrice < 100)
            {
                return "中價";
            }
            else
            {
                return "高價";
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {

            var q = from o in this.northwindDataSet1.Orders
                    orderby o.OrderDate.Year descending
                    group o by o.OrderDate.Year into g
                    select new { MyKey = g.Key, MyCount = g.Count(), MyGroup = g.ToList() };  //select 匿名型別


            this.bindingSource2.DataSource = q.ToList();
            this.dataGridView1.DataSource = this.bindingSource2;

            this.lblMaster.Text = $"Orders ({q.Count()})";


            //============================

            //treeview
            this.treeView1.Nodes.Clear();
            foreach (var group in q)
            {
                string s = $"{group.MyKey} ({group.MyCount})";
                TreeNode x = this.treeView1.Nodes.Add(s);

                foreach (var item in group.MyGroup)
                {
                    x.Nodes.Add(item.ToString());
                }
            }
            //======================


            this.chart1.DataSource = q.ToList();
            this.chart1.Series.Clear();
            this.chart1.Series.Add("Orders By Year");
            this.chart1.Series[0].XValueMember = "MyKey";
            this.chart1.Series[0].YValueMembers = "MyCount";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            this.chart1.Series[0].BorderWidth = 4;

        }

        private void button3_Click(object sender, EventArgs e)
        {

            var q = from p in this.northwindDataSet1.Products.AsEnumerable()
                    orderby p.UnitPrice descending
                    group p by MyPriceGroup(p.UnitPrice) into g
                    select new { MyKey = g.Key, MyCount = g.Count(),Max=g.Max(p=>p.UnitPrice), Min=g.Min(p=>p.UnitPrice), MyGroup = g.ToList() };


            this.bindingSource2.DataSource = q.ToList();
            this.dataGridView1.DataSource = this.bindingSource2;

            this.lblMaster.Text = $"Products ({q.Count()})";
            //================================

            //treeview
            this.treeView1.Nodes.Clear();
            foreach (var group in q)
            {
                string s = $"{group.MyKey} ({group.MyCount})";
                TreeNode x = this.treeView1.Nodes.Add(s);

                foreach (var item in group.MyGroup)
                {
                    x.Nodes.Add($"{item.ProductName} - {item.UnitPrice}");
                }
            }
            //======================

            //NOTE - 2 Series
            //Chart 

            this.chart1.Series.Clear();
            this.chart1.Series.Add("Max");
            this.chart1.Series.Add("Min");

            this.chart1.DataSource = q.ToList();
            this.chart1.Series[0].XValueMember = "MyKey";
            this.chart1.Series[0].YValueMembers = "Max";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            this.chart1.Series[1].XValueMember = "MyKey";
            this.chart1.Series[1].YValueMembers = "Min";
            this.chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;


        }

      
        private void button10_Click(object sender, EventArgs e)
        {
            var yearGroup = from o in this.northwindDataSet1.Orders
                            group o by o.OrderDate.Year into g
                            select new { Year = g.Key, MyCount = g.Count(), MyGroup = g };

            this.dataGridView1.DataSource = yearGroup.ToList();

            //NOTE subquery
            var q = from o in this.northwindDataSet1.Orders
                    group o by o.OrderDate.Year into g
                    select new
                    {
                        Year = g.Key,
                        YearCount = g.Count(),
                        MonthGroup = (from o in g
                                      group o by o.OrderDate.Month into g1
                                      select new { Month = g1.Key, MonthCount = g1.Count(), Orders = g1 })
                    };

            this.dataGridView2.DataSource = q.ToList();

            //treeview
            this.treeView1.Nodes.Clear();
            foreach (var group in q)
            {
                string s = $"{group.Year} 年 ({group.YearCount})";
                TreeNode x = this.treeView1.Nodes.Add(s);

                foreach (var monthGroup in group.MonthGroup)
                {
                    string s1 = $"{monthGroup.Month} 月 ({monthGroup.MonthCount})";
                    TreeNode x1 = x.Nodes.Add(s1);

                    foreach (var item in monthGroup.Orders)
                    {
                        x1.Nodes.Add(item.OrderDate.ToShortDateString() + item.OrderID);
                    }

                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
         
            NorthwindEntities dbContext = new NorthwindEntities();
            dbContext.Database.Log += Console.WriteLine;

            var q = from o in this.dbContext.Orders
                        select new
                        {
                            o.OrderID,
                            o.OrderDate,
                            訂單筆數 = o.Order_Details.Where(od => od.OrderID == o.OrderID).Count()
                        };

            this.dataGridView1.DataSource = q.ToList();

            var count1 =q.Sum(o=>o.訂單筆數);
            var count2 = this.dbContext.Order_Details.Count();
            //=======================

            //var q1x = from od in this.dbContext.Order_Details
            //         group od by od.OrderID into g
            //         from x in g
            //         select new
            //         {
            //             x.OrderID,
            //             x.Order.OrderDate,
            //             訂單筆數 = x.Order_Details.Where(od => od.OrderID == x.OrderID).Count()
            //         };

            var q1 = from od in this.dbContext.Order_Details
                     group od by od.OrderID into g
                     select new { OrderID =g.Key,   Count=g.Count() };

            this.dataGridView2.DataSource = q1.ToList();


        }
    }
}
