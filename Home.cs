using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace Pharmaceutical
{
    public partial class Home : Form
    {
        public static string adama;
        public string roles;
        int timeLeft;
        public Home()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();
            int one = rand.Next(0, 255);
            int tone = rand.Next(0, 255);
            int fone = rand.Next(0, 255);
            int sone = rand.Next(0, 255);
            label1.ForeColor = Color.FromArgb(one, tone, fone, sone);
        }
        public void GetPharmCategory()
        {
            comboBox1.Items.Clear();
            string app = "SELECT Fullname FROM Users";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = app;
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string category = (string)dr["Fullname"];
                comboBox1.Items.Add(category);

            }
            dr.Close();
            con.Close();
            dr.Close();
            cmd.Dispose();

        }
        private void Home_Load(object sender, EventArgs e)
        {
         //   Read();
            timer1.Start();
            timer1.Enabled = true;
            GetPharmCategory();
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            textBox2.UseSystemPasswordChar = true;
        }
       
        public void Read()
        {
            string dateRate, dateCounter;
            string que = "SELECT startDate, dateCount FROM ProductKey where serialKey = '123456'";
            SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand(que, con);
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                dateRate = reader["startDate"].ToString();
                dateCounter = reader["dateCount"].ToString();
                DateTime Current = DateTime.Now;
                DateTime before = Convert.ToDateTime(dateRate);
                TimeSpan CurrentNo = Current.Subtract(before);
                reader.Close();
                int dateCount = CurrentNo.Days;
                textBox2.Text = CurrentNo.ToString();
                if (dateCount == int.Parse(dateCounter))
                {
                    string qry = "UPDATE ProductKey SET dateCount = dateCount + @dateCount  ";
                    cmd = new SQLiteCommand(qry, con);
                    cmd.Parameters.AddWithValue("@dateCount", dateCount);
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
                else
                {
                    MessageBox.Show("Please Update your System Time");
                }
            }
            else
            {
                string app = "UPDATE [ProductKey] SET [startDate] = DateTime('now'), dateCount = 0  WHERE serialKey = '123456' ";
              //  SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
                cmd = new SQLiteCommand();
               // con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = app;
                cmd.ExecuteNonQuery();
                con.Close();
                cmd.Dispose();
            }
        }
        int attempt = 3;
        private void button1_Click(object sender, EventArgs e)
        {
            if ((comboBox1.Text.Trim() != "") && (textBox2.Text.Trim() != "")) // to validate if user and pass have data

            {
                string que = "SELECT Status FROM Users WHERE Fullname = @Fullname AND Password = @Password";
                SQLiteConnection con = new SQLiteConnection(conState.ConnectionString);
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand(que, con);
                cmd.Parameters.AddWithValue("@Fullname", comboBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", textBox2.Text.Trim());
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    roles = reader["Status"].ToString();
                    attempt = 0;
                    reader.Close();
                    if (roles == "Admin")
                    {
                        string qry = "UPDATE Users SET LastLoginDate = DATETIME('now') WHERE Fullname = @Fullname AND Password = @Password";
                        cmd = new SQLiteCommand(qry, con);
                        cmd.Parameters.AddWithValue("@Fullname", comboBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", textBox2.Text.Trim());
                        cmd.ExecuteNonQuery();
                        con.Close();
                        adama = comboBox1.Text; //"WELCOME ADMIN" + " " + 
                        AdminDash cu = new AdminDash();
                        cu.Show();
                        this.Hide();
                    }
                    else if (roles == "Staff")
                    {
                        string qry = "UPDATE Users SET LastLoginDate = DATETIME('now') WHERE Fullname = @Fullname AND Password = @Password";
                        cmd = new SQLiteCommand(qry, con);
                        cmd.Parameters.AddWithValue("@Fullname", comboBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", textBox2.Text.Trim());
                        cmd.ExecuteNonQuery();
                        con.Close();
                        adama = comboBox1.Text;
                        SalesForm fm = new SalesForm();
                        fm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Prohibited", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if ((attempt == 3) && (attempt > 0))
                {
                    label4.Visible = true;
                    label4.Text = ("Invalid Username or Password, You Have Only " + Convert.ToString(attempt) + " Attempt Left To Try");
                    --attempt;
                }
                else if ((attempt == 2) && (attempt > 0))
                {
                    label4.Text = ("Invalid Username or Password, You Have Only " + Convert.ToString(attempt) + " Attempt Left To Try");
                    --attempt;
                }
                else if ((attempt == 1) && (attempt > 0))
                {
                    label4.Text = ("Invalid Username or Password, You Have Only " + Convert.ToString(attempt) + " Attempt Left To Try");
                    --attempt;
                }
                else
                {
                    label4.Text = ("ACCESS DENIED!!! Attempt AFTER 3 Mins");
                    button1.Enabled = false;
                    label5.Visible = true;
                    timeLabel.Visible = true;
                    timeLeft = 120;
                    timeLabel.Text = "3mins";
                    timer2.Start();

                }
            }
            else
            {
                MessageBox.Show("Enter username and password", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft = timeLeft - 1;
                timeLabel.Text = timeLeft + "Secs";
            }
            else
            {
                timer2.Stop();
                timeLabel.Text = "Ready!!!";
                attempt = 3;
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
               textBox2.UseSystemPasswordChar = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(Control.IsKeyLocked(Keys.CapsLock))
            {
                label4.Text = "The Caps Lock Key is ON.";
            }
        }

        private void Home_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
