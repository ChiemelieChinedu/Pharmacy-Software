using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Pharmaceutical
{
    public partial class SalesForm : Form
    {
        public static DataTable da = new DataTable();
        DataRow dy;
        DataTable dt;
        private static List<Stream> m_streams;
        private static int m_currentPageIndex = 0;
        public SalesForm()
        {
            InitializeComponent();
        }

        private void SalesForm_Load(object sender, EventArgs e)
        {
          
            GetProductName();
            GetPharmCategory();
            GetPharmCategory3();
            bindApp();
            getProductBrand();
            cmbExpTrans.SelectedIndex = 0;
            cmbTransBy.SelectedIndex = 0;
            lblAttendant.Text = Home.adama;

        }
        public void GetPharmCategory3()
        {
            comboBox2.Items.Clear();
            string app = "SELECT DISTINCT Category FROM Products";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = app;
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string category = (string)dr["category"];
                comboBox2.Items.Add(category);
            }
            dr.Close();
            con.Close();
            cmd.Dispose();
        }
        
        public void getProductBrand()
        {

            string app = "SELECT ProductBrand FROM DefineBrandName ORDER BY ProductBrand";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = app;
            SQLiteDataReader dr = cmd.ExecuteReader();
            AutoCompleteStringCollection aut = new AutoCompleteStringCollection();
            while (dr.Read())
            {

                string category = (string)dr["ProductBrand"];
                aut.Add(category);
            }
            txtBrand.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtBrand.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtBrand.AutoCompleteCustomSource = aut;
            dr.Close();
            con.Close();
            cmd.Dispose();
        }

        private void txtGenericName_TextChanged(object sender, EventArgs e)
        {
            if (txtGenericName.Text != string.Empty)
            {
                dt = getProductDetails(txtGenericName.Text);
                dataGridView2.DataSource = dt;
                dataGridView2.Columns[0].Width = 140;
                dataGridView2.Columns[1].Width = 120;
                dataGridView2.Columns[2].Width = 90;
                dataGridView2.Columns[3].Width = 50;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Width = 90;
                dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
            }

        }
        public void GetProductName()
        {

            string app = "SELECT ProductName FROM ProductList ORDER BY ProductName";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = app;
            SQLiteDataReader dr = cmd.ExecuteReader();
            AutoCompleteStringCollection aut = new AutoCompleteStringCollection();
            while (dr.Read())
            {
     
                string category = (string)dr["ProductName"];
                listBox1.Items.Add(category);
                aut.Add(category);
            }
            txtGenericName.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtGenericName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtGenericName.AutoCompleteCustomSource = aut;
            dr.Close();
            con.Close();
            cmd.Dispose();
        }
        private void txtBrand_TextChanged(object sender, EventArgs e)
        {
            if (txtBrand.Text != string.Empty)
            {
                label10.Visible = false;
                txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = txtUnitPrice.Text = txtPurchaseUnit.Text = txtPurchasePrice.Text = txtDiscount.Text = txtDiscountRate.Text = "0";
                txtQty.Text = "1";
                dt = getProductBrand(txtBrand.Text);
                foreach (DataRow row in dt.Rows)
                {
                    int app = int.Parse(row[2].ToString());
                    if (app > 0)
                    {
                        txtProdName.Text = row[0].ToString();
                        txtProdBrand.Text = row[1].ToString();
                        txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = txtUnitPrice.Text = row[2].ToString();
                        txtPurchaseUnit.Text = txtPurchasePrice.Text = row[3].ToString();
                        txtBalance.Text = "0";
                    }

                    else
                    {
                        MessageBox.Show("Items has finished in store", "Empy store Alert");
                    }
                }
                dataGridView2.DataSource = dt;
                dataGridView2.Columns[0].Width = 140;
                dataGridView2.Columns[1].Width = 120;
                dataGridView2.Columns[2].Width = 90;
                dataGridView2.Columns[3].Width = 50;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Width = 90;
                dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem != null)
            {
                dt = getProductDetails(listBox1.Text);
                dataGridView2.DataSource = dt;
                dataGridView2.Columns[0].Width = 140;
                dataGridView2.Columns[1].Width = 120;
                dataGridView2.Columns[2].Width = 90;
                dataGridView2.Columns[3].Width = 50;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Width = 90;
                dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
            }
        }

        private void DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            label10.Visible = false;
            txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = txtUnitPrice.Text = txtPurchaseUnit.Text = txtPurchasePrice.Text = txtDiscount.Text = txtDiscountRate.Text = "0";
            txtQty.Text = "1";
            txtProdName.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtProdBrand.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = txtUnitPrice.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPurchaseUnit.Text = txtPurchasePrice.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtBalance.Text = "0";
        }
        public void GetPharmCategory()
        {
            comboBox1.Items.Clear();
            string app = "SELECT category FROM Category";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = app;
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string category = (string)dr["category"];
                comboBox1.Items.Add(category);
            }
            dr.Close();
            con.Close();
            cmd.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns.Count > 0)
            {
                progressBar1.Visible = true;
                label10.Visible = true;
                label10.ForeColor = Color.Red;
                label10.Text = "processing..............";
                int current = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = dataGridView1.Rows.Count;
                progressBar1.Step = 10;
                LocalReport localReport = new LocalReport();
                ReportDataSource rds = new ReportDataSource("DataSet2", da);
                localReport.ReportPath = Application.StartupPath + "\\Report2.rdlc";
                localReport.DataSources.Add(rds);
                PrintToPrinter(localReport);
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {

                 insertSales(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString(), row.Cells[4].Value.ToString(), row.Cells[5].Value.ToString(), row.Cells[6].Value.ToString(), row.Cells[7].Value.ToString(), row.Cells[8].Value.ToString(), row.Cells[9].Value.ToString());
                 insertSalesUpdate(row.Cells[0].Value.ToString(), int.Parse(row.Cells[3].Value.ToString()), int.Parse(row.Cells[9].Value.ToString()), int.Parse(row.Cells[2].Value.ToString()), int.Parse(row.Cells[6].Value.ToString()), int.Parse(row.Cells[4].Value.ToString()));
                 UpdateProducts(row.Cells[0].Value.ToString(), int.Parse(row.Cells[3].Value.ToString()));
                    current++;
                    progressBar1.PerformStep();
                    progressBar1.BeginInvoke(new Action(() => progressBar1.Value = current));
                    progressBar1.CreateGraphics().DrawString(current.ToString() + "%", new Font("Arial",
                    (float)10.25, FontStyle.Bold),
                    Brushes.Blue, new PointF(progressBar1.Width / 2 - 10, progressBar1.Height / 2 - 7));
                }
               
                label10.Text = "Sales Recorded";
                da.Rows.Clear();
                dataGridView1.DataSource = "";
                progressBar1.Visible = false;
            }
            else
            {
                MessageBox.Show("Empty Cart");
            }
        }
        public void insertSales(string ProductName, string UnitPrice, string SalesPrice, string Qty, string Discount, string DiscountRate, string NetPrice, string TransBy, string Attendant, string PurchasePrice)
        {
            string app = "INSERT INTO SoldItems(ProductName,UnitPrice, SalesPrice, Qty,Discount,DiscountRate,NetPrice,TransactionMode,Attendant,PurchasePrice,SalesDate)VALUES(@ProductName, @UnitPrice, @SalesPrice, @Qty,@Discount, @DiscountRate,@NetPrice,@TransactionMode,@Attendant, @PurchasePrice,DATETIME('now'))";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = app;
            cmd.Parameters.AddWithValue("@ProductName", ProductName);
            cmd.Parameters.AddWithValue("@UnitPrice", UnitPrice);
            cmd.Parameters.AddWithValue("@SalesPrice", SalesPrice);
            cmd.Parameters.AddWithValue("@Qty", Qty);
            cmd.Parameters.AddWithValue("@Discount", Discount);
            cmd.Parameters.AddWithValue("@DiscountRate", DiscountRate);
            cmd.Parameters.AddWithValue("@NetPrice", NetPrice);
            cmd.Parameters.AddWithValue("@TransactionMode", TransBy);
            cmd.Parameters.AddWithValue("@Attendant", Attendant);
            cmd.Parameters.AddWithValue("@PurchasePrice", PurchasePrice);
            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
        }
      
        public void UpdateProducts(string ProductName, int Qty)
        {
            string app = "UPDATE [Products] SET Qty = Qty - @Qty WHERE ProductBrand = @ProductBrand";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = app;
            cmd.Parameters.AddWithValue("@ProductBrand", ProductName);
            cmd.Parameters.AddWithValue("@Qty", Qty);
            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
        }
        public void insertSalesUpdate(string ProductName, int QtySold, int PurchasePrice, int SalesPrice, int NetSales, int Discount)
        {
            string app = "INSERT INTO SalesUpdate(ProductName, QtySold, LastSoldDate,PurchasePrice,SalesPrice, NetSales, Discount)VALUES(@ProductName, @QtySold, DATETIME('now'), @PurchasePrice,@SalesPrice, @NetSales, @Discount)  ON CONFLICT (ProductName) DO UPDATE SET QtySold = QtySold + @QtySold, LastSoldDate = DATETIME('now'), PurchasePrice = PurchasePrice + @PurchasePrice, SalesPrice = SalesPrice + @SalesPrice, NetSales = NetSales + @NetSales, Discount = Discount + @Discount";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = app;
            cmd.Parameters.AddWithValue("@ProductName", ProductName);
            cmd.Parameters.AddWithValue("@QtySold", QtySold);
            cmd.Parameters.AddWithValue("@PurchasePrice", PurchasePrice);
            cmd.Parameters.AddWithValue("@SalesPrice", SalesPrice);
            cmd.Parameters.AddWithValue("@NetSales", NetSales);
            cmd.Parameters.AddWithValue("@Discount", Discount);
            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
        }
       private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtQty.Text != string.Empty)
                {
                    int act = int.Parse(txtQty.Text);
                    int apt = int.Parse(txtUnitPrice.Text);
                    int CPrice = int.Parse(txtPurchaseUnit.Text);
                    int actual = act * apt;
                    int actualCP = act * CPrice;
                    txtActualPrice.Text = actual.ToString();
                    txtPurchaseUnit.Text = actualCP.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public DataTable getProductBrand(string ProductBrand)
        {

            string applet = "SELECT ProductBrand, ProductName AS GenericName,SalesPrice, Qty, PurchasePrice,ShelfNo FROM Products WHERE ProductBrand = @ProductName";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = applet;
            cmd.Parameters.AddWithValue("@ProductName", ProductBrand);
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable("");
            dt.Load(dr);
            dr.Close();
            con.Close();
            cmd.Dispose();
            return dt;
        }
        public DataTable getProductDetails(string ProductName)
        {

            string applet = "SELECT ProductBrand, ProductName AS GenericName,SalesPrice, Qty, PurchasePrice, ShelfNo FROM Products WHERE ProductName = @ProductName";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = applet;
            cmd.Parameters.AddWithValue("@ProductName", ProductName);
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable("");
            dt.Load(dr);
            dr.Close();
            con.Close();
            cmd.Dispose();
            return dt;
        }
       
        private void button5_Click(object sender, EventArgs e)
        {
            if (txtProdName.Text != string.Empty)
            {
                if (da.Columns.Count > 0)
                {
                    dy = da.NewRow();
                    dy["ProductName"] = txtProdName.Text;
                    dy["UnitPrice"] = txtUnitPrice.Text;
                    dy["SalesPrice"] = txtActualPrice.Text;
                    dy["Qty"] = txtQty.Text;
                    dy["Discount"] = txtDiscount.Text;
                    dy["DiscountRate"] = txtDiscountRate.Text;
                    dy["Price"] = txtNetPrice.Text;
                    dy["TransactionMode"] = cmbTransBy.Text;
                    dy["Attendant"] = lblAttendant.Text;
                    dy["PurchasePrice"] = txtPurchasePrice.Text;
                    dy["Name"] = Properties.Settings.Default.Name;
                    dy["Address"] = Properties.Settings.Default.Address;
                    dy["Mobile"] = Properties.Settings.Default.Mobile;
                    dy["Email"] = Properties.Settings.Default.Email;
                    da.Rows.Add(dy);
                    dataGridView1.DataSource = da;
                }
                else
                {
                    da.Columns.Add("ProductName", typeof(string));
                    da.Columns.Add("UnitPrice", typeof(string));
                    da.Columns.Add("SalesPrice", typeof(int));
                    da.Columns.Add("Qty", typeof(string));
                    da.Columns.Add("Discount", typeof(string));
                    da.Columns.Add("DiscountRate", typeof(string));
                    da.Columns.Add("Price", typeof(int));
                    da.Columns.Add("TransactionMode", typeof(string));
                    da.Columns.Add("Attendant", typeof(string));
                    da.Columns.Add("PurchasePrice", typeof(string));
                    da.Columns.Add("Name", typeof(string));
                    da.Columns.Add("Address", typeof(string));
                    da.Columns.Add("Mobile", typeof(string));
                    da.Columns.Add("Email", typeof(string));
                    dy = da.NewRow();
                    dy["ProductName"] = txtProdName.Text;
                    dy["UnitPrice"] = txtUnitPrice.Text;
                    dy["SalesPrice"] = txtActualPrice.Text;
                    dy["Qty"] = txtQty.Text;
                    dy["Discount"] = txtDiscount.Text;
                    dy["DiscountRate"] = txtDiscountRate.Text;
                    dy["Price"] = txtNetPrice.Text;
                    dy["TransactionMode"] = cmbTransBy.Text;
                    dy["Attendant"] = lblAttendant.Text;
                    dy["PurchasePrice"] = txtPurchasePrice.Text;
                    dy["Name"] = Properties.Settings.Default.Name;
                    dy["Address"] = Properties.Settings.Default.Address;
                    dy["Mobile"] = Properties.Settings.Default.Mobile;
                    dy["Email"] = Properties.Settings.Default.Email;
                    da.Rows.Add(dy);
                    dataGridView1.DataSource = da;
                }
                var sum = da.Compute("SUM(Price)", string.Empty);
                txtTotal.Text = sum.ToString();
                //textBox3.Text = "1";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            dt = getCategoryDetails(comboBox1.Text);
            dataGridView2.DataSource = dt;
            dataGridView2.Columns[0].Width = 140;
            dataGridView2.Columns[1].Width = 120;
            dataGridView2.Columns[2].Width = 90;
            dataGridView2.Columns[3].Width = 50;
            dataGridView2.Columns[4].Visible = false;
            dataGridView2.Columns[5].Width = 90;
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView2.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
        }
        public DataTable getCategoryDetails(string Category)
        {

            string applet = "SELECT [ProductBrand],[ProductName],[SalesPrice],[Qty],[PurchasePrice],[ShelfNo] FROM Products WHERE Category = @Category";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = applet;
            cmd.Parameters.AddWithValue("@Category", Category);
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable("Products");
            dt.Load(dr);
            dr.Close();
            con.Close();
            cmd.Dispose();
            return dt;
        }
       
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dt = getAll();
            dataGridView2.DataSource = dt;
            dataGridView2.Columns[0].Width = 140;
            dataGridView2.Columns[1].Width = 120;
            dataGridView2.Columns[2].Width = 90;
            dataGridView2.Columns[3].Width = 50;
            dataGridView2.Columns[4].Visible = false;
            dataGridView2.Columns[5].Width = 90;
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView2.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
        }
        public DataTable getAll()
        {

            string applet = "SELECT [ProductBrand],[ProductName],[SalesPrice],[Qty],[PurchasePrice],[ShelfNo] FROM Products";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = applet;
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable("Products");
            dt.Load(dr);
            dr.Close();
            con.Close();
            cmd.Dispose();
            return dt;
        }
        public static void PrintToPrinter(LocalReport report)
        {
            Export(report);

        }

        public static void Export(LocalReport report, bool print = true)
        {
            string deviceInfo =
             @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>3in</PageWidth>
                <PageHeight>8.3in</PageHeight>
                <MarginTop>0in</MarginTop>
                <MarginLeft>0.1in</MarginLeft>
                <MarginRight>0.1in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                Print();
            }
        }


        public static void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }

        public static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        public static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        public static void DisposePrint()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            da.Rows.Clear();
            dataGridView1.DataSource = "";
            this.Close();
        }

        private void txtBC_TextChanged(object sender, EventArgs e)
        {
            if (txtBC.Text != string.Empty)
            {
                txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = txtUnitPrice.Text = txtPurchaseUnit.Text = txtPurchasePrice.Text = txtDiscount.Text = txtDiscountRate.Text = "0";
                txtQty.Text = "1";
                dt = getByBarCode(txtBC.Text);
                foreach (DataRow row in dt.Rows)
                {
                    int app = int.Parse(row[3].ToString());
                    if (app > 0)
                    {
                        txtProdName.Text = row[0].ToString();
                        txtBrand.Text = row[1].ToString();
                        txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = txtUnitPrice.Text = row[2].ToString();
                        txtPurchaseUnit.Text = txtPurchasePrice.Text = row[4].ToString();
                        txtBalance.Text = "0";
                    }

                    else
                    {
                        MessageBox.Show("Items has finished in store", "Empy store Alert");
                    }
                }
                dataGridView2.DataSource = dt;
                dataGridView2.Columns[0].Width = 140;
                dataGridView2.Columns[1].Width = 120;
                dataGridView2.Columns[2].Width = 90;
                dataGridView2.Columns[3].Width = 50;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Width = 90;
                dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
            }
        }
        public DataTable getByBarCode(string BarCode)
        {

            string applet = "SELECT ProductBrand,ProductName, SalesPrice, Qty, PurchasePrice,ShelfNo FROM Products WHERE BarCode = @BarCode";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = applet;
            cmd.Parameters.AddWithValue("@BarCode", BarCode);
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable("Products");
            dt.Load(dr);
            dr.Close();
            con.Close();
            cmd.Dispose();
            return dt;
        }
        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            if (txtQty.Text != string.Empty)
            {
                txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = txtDiscount.Text = txtDiscountRate.Text = "0";
                int Qnt = int.Parse(txtQty.Text);
                int Unit = int.Parse(txtUnitPrice.Text);
                int CPrice = int.Parse(txtPurchaseUnit.Text);
                int actualSp = Qnt * Unit;
                int actualCP = Qnt * CPrice;
                txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = actualSp.ToString();
                txtPurchasePrice.Text = actualCP.ToString();
                txtBalance.Text = "0";
               
            }
        }
        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            txtDiscountRate.TextChanged -= new EventHandler(txtDiscountRate_TextChanged);
            this.txtAmtPaid.TextChanged -= new EventHandler(this.txtAmtPaid_TextChanged);
            if (txtDiscount.Text != string.Empty && Regex.IsMatch(txtDiscount.Text, @"^[0-9]+$"))
            {
                int Actual = int.Parse(txtActualPrice.Text);
                int Discount = int.Parse(txtDiscount.Text);
                int Net = Actual - Discount;
                double DiscountPercent = (double)Discount / Actual * 100;
                double apc = Math.Round(DiscountPercent, 3);
                txtAmtPaid.Text = txtNetPrice.Text = Net.ToString();
                txtDiscountRate.Text = apc.ToString();
            }
            txtDiscountRate.TextChanged += new EventHandler(txtDiscountRate_TextChanged);
            this.txtAmtPaid.TextChanged += new EventHandler(this.txtAmtPaid_TextChanged);
        }

        private void txtDiscountRate_TextChanged(object sender, EventArgs e)
        {
            txtDiscount.TextChanged -= new EventHandler(txtDiscount_TextChanged);
            this.txtAmtPaid.TextChanged -= new EventHandler(this.txtAmtPaid_TextChanged);
            if (txtDiscountRate.Text != string.Empty)
            {
                int Actual = int.Parse(txtActualPrice.Text);
                double DiscountRate = double.Parse(txtDiscountRate.Text);
                double DiscountPercent = DiscountRate / 100 * Actual;
                double apc = Math.Round(DiscountPercent, 3);
                double Net = Actual - apc;
                txtAmtPaid.Text = txtNetPrice.Text = Net.ToString();
                txtDiscount.Text = apc.ToString();
            }
            txtDiscount.TextChanged += new EventHandler(txtDiscount_TextChanged);
            this.txtAmtPaid.TextChanged += new EventHandler(this.txtAmtPaid_TextChanged);
        }

        private void txtAmtPaid_TextChanged(object sender, EventArgs e)
        {
            if (txtAmtPaid.Text != string.Empty && txtNetPrice.Text != string.Empty && Regex.IsMatch(txtDiscount.Text, @"^[0-9]+$"))
            {
                int Net = int.Parse(txtNetPrice.Text);
                int AmtPaid = int.Parse(txtAmtPaid.Text);
                int Balm = Math.Abs(AmtPaid - Net);
                txtBalance.Text = Balm.ToString();
            }

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(row.Index);
            }
            var sum = da.Compute("SUM(SalesPrice)", string.Empty);
            txtTotal.Text = sum.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (da.Columns.Count == 0)
            {
                MessageBox.Show("Nothing to Clear");
            }
            else
            {
                da.Rows.Clear();
            }
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            /*txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = txtUnitPrice.Text = txtPurchaseUnit.Text = txtPurchasePrice.Text = txtDiscount.Text = txtDiscountRate.Text = "0";
            txtQty.Text = "1";
            txtProdName.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtUnitPrice.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtPurchaseUnit.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = txtUnitPrice.Text;
            txtPurchasePrice.Text = txtPurchaseUnit.Text;
            txtBalance.Text = "0";
            label10.Visible = false;*/
            label10.Visible = false;
            txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = txtUnitPrice.Text = txtPurchaseUnit.Text = txtPurchasePrice.Text = txtDiscount.Text = txtDiscountRate.Text = "0";
            txtQty.Text = "1";
            txtProdName.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtProdBrand.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtActualPrice.Text = txtNetPrice.Text = txtAmtPaid.Text = txtUnitPrice.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPurchaseUnit.Text = txtPurchasePrice.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtBalance.Text = "0";
        }

        private void ExpSubmit_Click(object sender, EventArgs e)
        {
            if (Regex.IsMatch(txtExpAmt.Text, @"^[0-9]+$"))
            {

                int app = insertExpenses(Convert.ToDateTime(ExpTimePicker1.Text), txtExpType.Text, int.Parse(txtExpAmt.Text),lblAttendant.Text, cmbExpTrans.Text);
                if (app > 0)
                {
                    bindApp();
                }
                else
                {
                    MessageBox.Show("DATA UPLOAD ERROR");
                }
            }
            else
            {
                txtExpAmt.Text = "";
                MessageBox.Show("MUST ENTER NUMERIC VALUE");
            }
        }
        public void bindApp()
        {
            DataTable dt = SelectExpenses();
            ExpdataGridView.DataSource = dt;
            ExpdataGridView.Font = new Font("Georgia", 10);
            ExpdataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            ExpdataGridView.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
            ExpdataGridView.Columns[5].Visible = false;
        }
        public DataTable SelectExpenses()
        {

            string applet = "SELECT * FROM Expenses";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = applet;
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable("Expenses");
            dt.Load(dr);
            dr.Close();
            con.Close();
            cmd.Dispose();
            return dt;
        }
        public int insertExpenses(DateTime ExpensesDate, string ExpensesType, int Amount,string Role, string TransactionBy)
        {

            int id;
            string app = "INSERT INTO Expenses(ExpensesDate,ExpensesType,Amount,Role,TransactionBy)VALUES(@ExpensesDate,@ExpensesType,@Amount,@Role,@TransactionBy)";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = app;
            cmd.Parameters.AddWithValue("@ExpensesDate", ExpensesDate);
            cmd.Parameters.AddWithValue("@ExpensesType", ExpensesType);
            cmd.Parameters.AddWithValue("@Amount", Amount);
            cmd.Parameters.AddWithValue("@Role", Role);
            cmd.Parameters.AddWithValue("@TransactionBy", TransactionBy);
            id = cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
            return id;
        }


        private void ExpUpdate_Click(object sender, EventArgs e)
        {
            if (txtExpType.Text == string.Empty || txtExpAmt.Text == string.Empty)
            {
                MessageBox.Show("Invalid type and amount", "Expenditure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int app = updExp(int.Parse(label5.Text), txtExpType.Text, int.Parse(txtExpAmt.Text), cmbExpTrans.Text);
                string message = string.Empty;
                switch (app)
                {
                    case 1:
                        bindApp();
                        txtExpType.Text = "";
                        txtExpAmt.Text = "";
                        cmbExpTrans.Text = "";
                        break;
                    default:
                        message = "RECORD NOT UPDATED SUCCESFULLY\\nPlease try again";
                        MessageBox.Show(message, "Expenditure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

            }
        }
        public int updExp(int Id, string ExpensesType, int Amount, string TransactionBy)
        {
            int id;
            string app = "UPDATE[Expenses] SET[ExpensesType] = @ExpensesType,[Amount] = @Amount, [TransactionBy] = @TransactionBy WHERE Id = @Id";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = app;
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@ExpensesType", ExpensesType);
            cmd.Parameters.AddWithValue("@Amount", Amount);
            cmd.Parameters.AddWithValue("@TransactionBy", TransactionBy);
            id = cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
            return id;
        }


        private void ExpClear_Click(object sender, EventArgs e)
        {
            txtExpType.Text = "";
            txtExpAmt.Text = "";
            cmbExpTrans.Text = "";
        }

        private void ExpDelete_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to delete Record?", "Confirm Delete!!!", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                if (txtExpType.Text == string.Empty || txtExpAmt.Text == string.Empty)
                {
                    MessageBox.Show("Nothing to delete", "Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int app = deleteLogin(int.Parse(label5.Text));
                    if (app > 0)
                    {
                        MessageBox.Show("RECORD REMOVED!!!", "Contact", MessageBoxButtons.OK);
                        txtExpType.Text = "";
                        txtExpAmt.Text = "";
                        cmbExpTrans.Text = "";
                        bindApp();
                    }
                    else
                    {
                        MessageBox.Show("Record Not Deleted", "Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bindApp();
                    }
                }
            }
            else
            {
                MessageBox.Show("DELETE ABORTED!!!");
            }
        }
        public int deleteLogin(int Id)
        {
            string appleting = "DELETE FROM [Expenses] WHERE Id = @id";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = appleting;
            cmd.Parameters.AddWithValue("@Id", Id);
            int row = cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
            return row;
        }
      
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DataTable dt = getExpenses(Convert.ToDateTime(ExpTimePicker1.Text));
            ExpdataGridView.DataSource = dt;
            ExpdataGridView.Font = new Font("Georgia", 10);
            ExpdataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            ExpdataGridView.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
            ExpdataGridView.Columns[5].Visible = false;
        }
        public DataTable getExpenses(DateTime ExpensesDate)
        {

            string applet = "SELECT * FROM Expenses WHERE ExpensesDate = @ExpensesDate";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = applet;
            cmd.Parameters.AddWithValue("@ExpensesDate", ExpensesDate);
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable("Expenses");
            dt.Load(dr);
            con.Close();
            cmd.Dispose();
            return dt;
        }

        private void ExpdataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ExpTimePicker1.Text = ExpdataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtExpType.Text = ExpdataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtExpAmt.Text = ExpdataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            cmbExpTrans.Text = ExpdataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
            label5.Text = ExpdataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
        }

        private void btnSaveOnly_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns.Count > 0)
            {
                progressBar1.Visible = true;
                label10.Visible = true;
                label10.ForeColor = Color.Red;
                label10.Text = "processing..............";
                int current = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = dataGridView1.Rows.Count;
                progressBar1.Step = 10;
                foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                insertSales(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString(), row.Cells[4].Value.ToString(), row.Cells[5].Value.ToString(), row.Cells[6].Value.ToString(), row.Cells[7].Value.ToString(), row.Cells[8].Value.ToString(), row.Cells[9].Value.ToString());
                    insertSalesUpdate(row.Cells[0].Value.ToString(), int.Parse(row.Cells[3].Value.ToString()), int.Parse(row.Cells[9].Value.ToString()), int.Parse(row.Cells[2].Value.ToString()), int.Parse(row.Cells[6].Value.ToString()), int.Parse(row.Cells[4].Value.ToString()));
                    UpdateProducts(row.Cells[0].Value.ToString(), int.Parse(row.Cells[3].Value.ToString()));
                    current++;
                    progressBar1.PerformStep();
                    progressBar1.BeginInvoke(new Action(() => progressBar1.Value = current));
                    progressBar1.CreateGraphics().DrawString(current.ToString() + "%", new Font("Arial",
                    (float)10.25, FontStyle.Bold),
                    Brushes.Blue, new PointF(progressBar1.Width / 2 - 10, progressBar1.Height / 2 - 7));
                }
            label10.Text = "Sales Recorded";
            da.Rows.Clear();
            dataGridView1.DataSource = "";
            progressBar1.Visible = false;
            }
            else
            {
                MessageBox.Show("Empty Cart");
            }
        }

        private void SalesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            da.Rows.Clear();
            dataGridView1.DataSource = "";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt = getCategoryDetails(comboBox2.Text);
            dataGridView2.DataSource = dt;
            dataGridView2.Columns[0].Width = 140;
            dataGridView2.Columns[1].Width = 120;
            dataGridView2.Columns[2].Width = 90;
            dataGridView2.Columns[3].Width = 50;
            dataGridView2.Columns[4].Visible = false;
            dataGridView2.Columns[5].Width = 90;
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView2.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            BarCodeReader rd = new BarCodeReader();
            if (rd.ShowDialog() == DialogResult.OK)
            {
                this.txtBC.Text = rd.barCodeType;
            }
        }

    }
}