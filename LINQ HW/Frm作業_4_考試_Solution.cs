using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LinqLabs
{
    public partial class Frm作業_4_考試_Solution : Form
    {
        public Frm作業_4_考試_Solution()
        {
            InitializeComponent();

            students_scores = new List<Student>()
                                         {
                                            new Student{ Name = "aaa", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Male" },
                                            new Student{ Name = "bbb", Class = "CS_102", Chi = 80, Eng = 80, Math = 100, Gender = "Male" },
                                            new Student{ Name = "ccc", Class = "CS_101", Chi = 60, Eng = 50, Math = 75, Gender = "Female" },
                                            new Student{ Name = "ddd", Class = "CS_102", Chi = 80, Eng = 70, Math = 85, Gender = "Female" },
                                            new Student{ Name = "eee", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Female" },
                                            new Student{ Name = "fff", Class = "CS_102", Chi = 80, Eng = 80, Math = 80, Gender = "Female" },

                                          };
          
        }

        List<Student> students_scores;

        public class Student
        {
            public string Name { get; internal set; }
            public string Class { get; internal set; }
            public int Chi { get; internal set; }
            public int Eng { get; internal set; }
            public int Math { get; internal set; }
            public string Gender { get; internal set; }

            //NOTE override ToString()
            public override string ToString()
            {
                return $"{this.Name,5}{this.Class,7}{this.Chi,4}{this.Eng,4}{this.Math,4}{this.Gender,6}";
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
         
            // 共幾個 學員成績 ?						
             this.listBox1.Items.Clear();
            this.listBox1.BringToFront();
            this.lblMaster.Text=$"共 {students_scores.Count()} 學員成績"; this.lblDetails.Text = "";
            this.dataGridView1.DataSource = students_scores;

           
            // 找出 前面三個 的學員所有科目成績		
            var q = students_scores.Take(3);
            ShowStudents(q, "前面三個 的學員所有科目成績");

            // 找出 後面兩個 的學員所有科目成績					
            q = students_scores.Skip(students_scores.Count() - 2);
            ShowStudents(q, "後面兩個 的學員所有科目成績");

            // 找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績		
            // NOTE 匿名型別
            string[] names = { "aaa", "bbb", "ccc" };
            var q1 = students_scores.Where(s => names.Contains(s.Name))
                                     .Select(s => new { s.Name, s.Chi, s.Eng });
            ShowStudents(q1, "Name 'aaa','bbb','ccc' 的學員國文英文科目成績");


            // 找出學員 'bbb' 的成績	                       
            q = students_scores.Where(s => s.Name == "bbb");
            ShowStudents(q, "學員 'bbb' 的成績");

            // 找出除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)	!=
            q = students_scores.Where(s => s.Name != "bbb");
            ShowStudents(q, "除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)");

            //數學不及格...是誰
            q = students_scores.Where(s => s.Math < 60);
            ShowStudents(q, "數學不及格...是誰");

        }

        private void ShowStudents(IEnumerable<object> q, string header)
        {
            this.listBox1.Items.Add(header);

            foreach(var item in q)
            {
                this.listBox1.Items.Add(item);
            }
            this.listBox1.Items.Add("==============================");
        }

        private void button33_Click(object sender, EventArgs e)
        {
            // split=> 分成 三群 '待加強'(60~69) '佳'(70~89) '優良'(90~100) 
            // print 每一群是哪幾個 ? (每一群 sort by 分數 descending)

            this.lblMaster.Text = "學生成績";
            List<int> scores = new List<int>();
            Random r = new Random();
            for (int i = 1; i <= 100; i++)
            {
                scores.Add(r.Next(60, 101));
            }

            var q1 = from s in scores
                     group s by MyGroup(s) into g
                     select new MyData
                     {
                         MyKey = g.Key,
                         MyCount = g.Count(),
                         MyGroup1 = g.OrderByDescending(s => s),

                     };
            this.bindingSource1.DataSource = q1.OrderByDescending(item => item.MyGroup1.Average()).ToList();
            this.dataGridView1.DataSource = this.bindingSource1;

        }


        internal class MyData
        {
            public object MyKey { get; set; }
            public int MyCount { get; set; }
            public IOrderedEnumerable<int> MyGroup1 { get; internal set; }
        }

        private object MyGroup(int s)
        {
            //三群 '待加強'(60~69) '佳'(70~89) '優良'(90~100)

            if (s >= 90)
                return "優良";
            else if (s >= 70)
                return "佳";
            else
                return "待加強";
        }

        private void button35_Click(object sender, EventArgs e)
        {
            // 統計 :　所有隨機分數出現的次數/比率; sort ascending or descending
            // 63     7.00%
            // 100    6.00%
            // 78     6.00%
            // 89     5.00%
            // 83     5.00%
            // 61     4.00%
            // 64     4.00%
            // 91     4.00%
            // 79     4.00%
            // 84     3.00%
            // 62     3.00%
            // 73     3.00%
            // 74     3.00%
            // 75     3.00%

            List<int> scores = new List<int>();
            Random r = new Random();
            for (int i = 1; i <= 100; i++)
            {
                scores.Add(r.Next(60, 101));
            }

            var q = from s in scores
                     group s by s into g
                     select new
                     {
                         Score=$"{g.Key} 分",
                         Count = g.Count(),
                         Percent = ((double)g.Count() / scores.Count).ToString("p2"),
                         Percent1 = ((double)g.Count() / scores.Count*100)
                     };

            var data =q.OrderByDescending(s => s.Count).ToList();
            this.dataGridView1.DataSource = data;

            this.chart1.BringToFront();
            this.lblDetails.Text = "Chart";
            this.chart1.DataSource = data.ToList();
            this.chart1.Series[0].XValueMember = "Score";
            this.chart1.Series[0].YValueMembers = "Percent1";
            this.chart1.Series[0].ChartType = SeriesChartType.Column ;
   
            //NOTE Display Percentages on a Pie Chart
            //this.chart1.Series[0]["PieLabelStyle"] = "Outside";
            this.chart1.Series[0].BorderWidth = 1;
            this.chart1.Series[0].BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
            this.chart1.Series[0].Label = "#PERCENT{P2}";
            
            
            // Add a legend to the chart and dock it to the bottom-center
            this.chart1.Legends.Clear();
            this.chart1.Legends.Add("Legend1");
            this.chart1.Legends[0].Enabled = true;
            this.chart1.Legends[0].Docking = Docking.Bottom;
            this.chart1.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
            this.chart1.Series[0].LegendText ="#VALX";

            // Set the threshold under which all points will be collected
            Series series1 = chart1.Series[0];
            series1["CollectedThreshold"] = "5";
            series1["CollectedThresholdUsePercent"] = "true";

            series1["CollectedLabel"] = "Other";
            series1["CollectedLegendText"] = "Other";
            series1["CollectedSliceExploded"] = "true";

            series1["CollectedColor"] = "Green";
            series1["CollectedToolTip"] = "Other";


            //NOTE - Test 100 %
            //MessageBox.Show( (from x in q select x.Percent1).Sum().ToString());
        }

        bool IsShowYear = false;

        private void button34_Click(object sender, EventArgs e)
        {
            // 年度最高銷售金額 年度最低銷售金額
            // 那一年總銷售最好 ? 那一年總銷售最不好 ?  
            // 那一個月總銷售最好 ? 那一個月總銷售最不好 ?

            // 每年 總銷售分析 圖
            // 每月 總銷售分析 圖


            global::LinqLabs.NorthwindEntities dbContext = new NorthwindEntities();
            var q = from o in dbContext.Orders
                    from od in o.Order_Details
                    select new { o.OrderID, o.OrderDate, od.UnitPrice, od.Quantity, totalSales = od.UnitPrice * od.Quantity };

            this.dataGridView1.DataSource = q.ToList();
            this.listBox1.Items.Add($"{q.Count()} 筆   total sales : {q.Sum(item => item.totalSales)}");
           
            //  某年 總銷售金額 ? 
            q = from o in dbContext.Orders
                from od in o.Order_Details
                where o.OrderDate.Value.Year == 1996
                select new { o.OrderID, o.OrderDate, od.UnitPrice, od.Quantity, totalSales = od.UnitPrice * od.Quantity };
            this.listBox1.Items.Add(" 1996 total sales :" + q.Sum(item => item.totalSales));

            //  某幾年 總銷售金額 ? 
            int[] years = { 1996, 1997 };
            q = from o in dbContext.Orders
                from od in o.Order_Details
                where years.Contains(o.OrderDate.Value.Year)
                select new { o.OrderID, o.OrderDate, od.UnitPrice, od.Quantity, totalSales = od.UnitPrice * od.Quantity };
            this.listBox1.Items.Add(" 1996 1997 total sales :" + q.Sum(item => item.totalSales));

            this.chart1.Series.Clear();
            this.chart1.Series.Add("Sales by Year/Month");
            this.chart1.BringToFront();

            //=============================================
            // or
            // NOTE : 導覽屬性 / AsEnumerable()
            //# 年度最高銷售金額 年度最低銷售金額
            //# 那一年總銷售最好 ? 那一年總銷售最不好 ?  

            // NOTE : show by year or by month (可 簡化) 
          
            IsShowYear = !IsShowYear;
            if (IsShowYear)
            {
                this.lblMaster.Text = "銷售成長/年"; this.lblDetails.Text = "";
                var q1 = from od in this.dbContext.Order_Details.AsEnumerable()
                         group od by od.Order.OrderDate.Value.Year into g
                         select new { Key = $"{g.Key} 年", TotalSales = Math.Round(g.Sum(o => o.UnitPrice * o.Quantity), 2) };

                var result = q1.OrderByDescending(o => o.TotalSales).ToList();
                this.dataGridView1.DataSource = result;
                string s = "";
                s+=$"年度最高銷售金額   : {result.FirstOrDefault().TotalSales}\n";
                s+=$"年度最低銷售金額   : {result.LastOrDefault().TotalSales}\n";
                s+=$"那一年總銷售最好   : {result.FirstOrDefault().Key}\n";
                s+=$"那一年總銷售最不好 : {result.LastOrDefault().Key}\n";
                MessageBox.Show(s);
                Debug.WriteLine(s);

                //# 每年 總銷售分析 圖
                this.chart1.DataSource = result.OrderBy(o => o.Key).ToList();
                
                this.chart1.Series[0].XValueMember = "Key";
                this.chart1.Series[0].YValueMembers = "TotalSales";
                this.chart1.Series[0].ChartType = SeriesChartType.Line;
                this.chart1.ChartAreas[0].AxisX.Interval = 1;
                this.chart1.Series[0].BorderWidth = 4;
            }
            else
            {
                //===========================
                //# 那一個月總銷售最好 ? 那一個月總銷售最不好 ?
                this.lblMaster.Text = "銷售成長/月"; this.lblDetails.Text = "";
                var q2 = from od in this.dbContext.Order_Details.AsEnumerable()
                         group od by od.Order.OrderDate.Value.Month into g
                         select new { Key= g.Key, TotalSales = Math.Round(g.Sum(o => o.UnitPrice * o.Quantity), 2) };

                var result2 = q2.OrderByDescending(o => o.TotalSales).ToList();
                this.dataGridView1.DataSource = result2;
                string s = "";
                s+=$"那一個月總銷售最好   : {result2.FirstOrDefault().Key}\n";
                s+=$"那一個月總銷售最不好 : {result2.LastOrDefault().Key}\n";
                MessageBox.Show(s);
                Debug.WriteLine(s);

                //# 每月 總銷售分析 圖
                this.chart1.DataSource = q2.OrderBy(o => o.Key).ToList();
                this.chart1.Series[0].XValueMember = "Key";
                this.chart1.Series[0].YValueMembers = "TotalSales";
                this.chart1.Series[0].ChartType =   SeriesChartType.Line;
                this.chart1.ChartAreas[0].AxisX.Interval = 1;
                this.chart1.Series[0].BorderWidth = 4;
            }

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                //NOTE BringToFront()
                this.dataGridView2.BringToFront();
                MyData data = ((MyData)this.bindingSource1.Current);
                this.lblDetails.Text = $"{data.MyKey}({data.MyCount})";
                this.dataGridView2.DataSource = data.MyGroup1.Select(n => new { 分數 = n }).ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            // 統計 每個學生個人成績
            // Rank by 三科成績加總 並排序
            // 國文權重加倍
            // 依平均分計算 Grade & Pass 

            // NOTE Rank
            this.lblMaster.Text = "Rank";
            this.lblDetails.Text = "";

            int rank = 0;
            var q = from s in students_scores
                    let scores = new int[] { s.Chi, s.Eng, s.Math }
                    let Avg = Math.Round(scores.Average(), 2)
                    orderby Avg descending
                    select new
                    {
                        s.Name,
                        s.Gender,
                        s.Class,
                        s.Chi,
                        s.Eng,
                        s.Math,
                        Min = scores.Min(),
                        Max = scores.Max(),
                        Avg = Avg,
                        Sum = scores.Sum(),
                        Weight = MyWeight(scores),
                        Pass = MyPass(Avg),
                        Grade = MyGrade(Avg),
                        Rank =++rank,
                    };

            this.dataGridView1.DataSource = q.ToList();


            //NOTE Rank - Select(s, i) Method
            var q1 = students_scores.OrderByDescending(s => s.Chi + s.Eng + s.Math)
                                    .Select((s, i) => new
                                    {
                                        s.Name,
                                        Rank = i + 1,
                                        s
                                    });

            this.dataGridView2.BringToFront();
            this.dataGridView2.DataSource = q1.ToList();

        }


        private object MyPass(double avg)
        {
            return avg >= 60 ? "P" : "F";
        }

        private Char MyGrade(double avg)
        {
            if (avg >= 90)
                return 'A';
            else if (avg >= 80)
                return 'B';
            else if (avg >= 70)
                return 'C';
            else if (avg >= 60)
                return 'D';
            else
                return 'E';
        }

        private object MyWeight(int[] scores)
        {
            return scores[0] * 0.5 + scores[1] * 0.25 + scores[2] * 0.25;
        }

       NorthwindEntities dbContext = new NorthwindEntities();

      
        private void button6_Click(object sender, EventArgs e)
        {
            this.chart1.BringToFront();
            this.lblMaster.Text = "年銷售成長";this.lblDetails.Text = "";
            var q = from od in this.dbContext.Order_Details.AsEnumerable()
                    group od by od.Order.OrderDate.Value.Year into g
                    let lastyear = g.Key - 1
                    let lastYearSales = MyLastYearSales(lastyear)
                    let thisYearSales = Math.Round(g.Sum(od => od.UnitPrice * od.Quantity), 2)
                    select new
                    {
                        ThisYear = g.Key,
                        ThisYearSales = thisYearSales.ToString("c2"),
                        LastYear = lastyear,
                        LastYearSales = lastYearSales.ToString("c2"),
                        成長率 = lastYearSales != 0 ? ((thisYearSales - lastYearSales) / lastYearSales)*100:0//.ToString("P2"):""
                    };

            this.dataGridView1.DataSource = q.ToList();
            this.chart1.DataSource = q.ToList();

            this.chart1.Series.Clear();
            this.chart1.Series.Add("ThisYearSales");
            this.chart1.Series.Add("LastYearSales");
            this.chart1.Series.Add("Growth");
            
            this.chart1.Series[0].XValueMember = "thisYear";
            this.chart1.Series[0].YValueMembers = "thisYearSales";
            this.chart1.Series[0].ChartType = SeriesChartType.StackedColumn; //NOTE stackedColumn
          

            this.chart1.Series[1].XValueMember = "thisYear";
            this.chart1.Series[1].YValueMembers = "lastYearSales";
            this.chart1.Series[1].IsValueShownAsLabel = true;
            this.chart1.Series[1].ChartType = SeriesChartType.StackedColumn;


           

            //======================================================
            //combinational chart
            //this.chart1.Series[0].YAxisType = AxisType.Primary;
            //this.chart1.Series[1].YAxisType = AxisType.Primary;
            this.chart1.Series[2].YAxisType = AxisType.Secondary;
            var strChartArea= this.chart1.Series[2].ChartArea;
            this.chart1.ChartAreas[strChartArea].AxisY2.Maximum = 200;

            this.chart1.Series[2].XValueMember = "thisYear";
            this.chart1.Series[2].YValueMembers = "成長率";
            this.chart1.Series[2].ChartType = SeriesChartType.Line;
            this.chart1.Series[2].Color = Color.Red;
            this.chart1.Series[2].BorderWidth = 3;


        }
        private decimal MyLastYearSales(int lastyear)
        {
            var q = from o in this.dbContext.Orders.Where(o => o.OrderDate.Value.Year == lastyear)
                    select o.Order_Details.Sum(od => od.UnitPrice * od.Quantity);

            return q.ToList().Sum();
        }

        

        private object mySum2(IGrouping<int, Order> g, int lastyear)
        {
            var q = from o in g
                    select o.Order_Details.Sum(od => od.UnitPrice * od.Quantity);

            var thisyearsales = q.ToList().Sum();

            var q1 = from o in this.dbContext.Orders.Where(o => o.OrderDate.Value.Year == lastyear)
                     select o.Order_Details.Sum(od => od.UnitPrice * od.Quantity);

            var lastyearsales = q1.ToList().Sum();

            if (lastyearsales == 0)
                return "";
            else
                return (thisyearsales - lastyearsales) / lastyearsales;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var q = from od in this.dbContext.Order_Details.AsEnumerable()
                    group od by od.Order.OrderDate.Value.Year  into g
                    select new
                    {
                        Year = g.Key,
                        CategoryGroup = from od in g
                                     group od by od.Product.Category.CategoryName into g1
                                     select new
                                     {
                                         CatogoryName = g1.Key,
                                         Sales = g1.Sum(od=>od.Quantity*od.UnitPrice* (decimal)(1-od.Discount))
                                     }
                    };
            this.dataGridView1.DataSource = q.ToList();

            //To Pivot Table
            //DataTable / DataColumn Schema 
            DataTable table = new DataTable();
            table.Columns.Add("Year");
            table.Columns.AddRange( (from c in this.dbContext.Categories.Distinct().AsEnumerable()
                                     select new DataColumn(c.CategoryName)).ToArray());

            //DataRow data
            foreach (var group in q)
            {
                int i = 0;
                DataRow dr = table.NewRow();
                dr[i] = group.Year;
                foreach (var item in group.CategoryGroup)
                {
                    i += 1;
                    dr[item.CatogoryName] = item.Sales;
                }
                table.Rows.Add(dr);
            }

            this.dataGridView1.DataSource = table;

            //for multi series
            this.chart1.BringToFront();
            this.chart1.Series.Clear();

            for (int column = 1; column <= table.Columns.Count - 1; column++)
            {
                var series = this.chart1.Series.Add(table.Columns[column].ToString());
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

                foreach (DataRow row in table.Rows)
                {
                    series.Points.AddXY(row[0], row[column]);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
       
            //NOTE subquery
            var q = from o in this.dbContext.Orders
                    group o by o.OrderDate.Value.Year into g
                    select new
                    {
                        Year = g.Key,
                        YearCount = g.Count(),
                        MonthGroup = (from o in g
                                      group o by o.OrderDate.Value.Month into g1
                                      select new { Month = g1.Key, MonthCount = g1.Count() })
                    };

            this.dataGridView1.DataSource = q.ToList();

            //treeview
            this.treeView1.Nodes.Clear();
            foreach (var group in q)
            {
                string s = $"{group.Year} 年 ({group.YearCount})";
                TreeNode x = this.treeView1.Nodes.Add(s);

                foreach (var monthGroup in group.MonthGroup)
                {
                    string s1 = $"{monthGroup.Month} 月 ({monthGroup.MonthCount})";
                    x.Nodes.Add(s1);
                }
            }
            //==========================
          
            this.chart2.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        //用 LINQ 實作 Pivot 轉換
        //static void Mainxxx(string[] args)
        //{
        //    var allCols = rawData.Select(o => o.StatusCode).Distinct().OrderBy(o => o).ToList();
        //    var res = rawData.GroupBy(o => o.LogTime)
        //        .Select(o =>
        //        {
        //            dynamic d = new ExpandoObject();
        //            d.LogTime = o.Key;
        //            var dict =
        //                d as IDictionary<string, object>;
        //            allCols.ForEach(c =>
        //            {
        //                dict["S" + c] = o.Where(p => p.StatusCode == c).Sum(p => p.Count);
        //            });
        //            return d;
        //        }).ToArray();
        //    Console.WriteLine(JsonConvert.SerializeObject(res, Formatting.Indented));
        //    Console.ReadLine();
        //}
    }
}
