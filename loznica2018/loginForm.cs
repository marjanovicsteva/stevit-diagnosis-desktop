using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace loznica2018
{
    public partial class loginForm : Form
    {
        private static string _korisnicko_ime = String.Empty;

        public string KorisnickoIme
        {
            get
            {
                return _korisnicko_ime;
            }
            set
            {
                if (_korisnicko_ime != value)
                    _korisnicko_ime = value;
            }
        }

        public loginForm()
        {
            InitializeComponent();
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            _korisnicko_ime = usernameBox.Text;
            string password = passwordBox.Text;
            string error = "";
            if (_korisnicko_ime.Length < 6)
                error += "Username must be at least 6 characters long.\n";
            if (password.Length < 8)
                error += "Password must be at least 8 characters long.";
            if (error.Length > 0)
            {
                MessageBox.Show(
                    this,
                    "There was one or more errors in processing your request.\n" + error,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            else
            {
                try
                {
                    string connectionString = "Server=mysql.stackcp.com; Port=50449; Database=loznica-33338c04; Uid=loznica-33338c04; Pwd=fo94dw1sh1;";
                    MySqlConnection connection = new MySqlConnection(connectionString);

                    MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `korisnicko_ime`='" + _korisnicko_ime + "'", connection);

                    MySqlDataReader reader;

                    connection.Open();
                    reader = command.ExecuteReader();
                    int count = 0;
                    while (reader.Read())
                        count++;
                    if (count == 0)
                        MessageBox.Show("User does not exist, please register");
                    else
                    {
                        string dbPassword = reader["sifra"].ToString();
                        if (password != dbPassword)
                            MessageBox.Show("Wrong password");
                        else
                        {
                            app application = new app();
                            this.Hide();
                            application.ShowDialog();
                            this.Close();
                        }
                    }
                        
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            registerForm register = new registerForm();
            this.Hide();
            register.ShowDialog();
            this.Close();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
    }
}
