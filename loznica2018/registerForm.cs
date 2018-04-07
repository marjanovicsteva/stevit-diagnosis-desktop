using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace loznica2018
{
    public partial class registerForm : Form
    {
        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public registerForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            loginForm login = new loginForm();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }

        private void registerForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void registerBtn_Click(object sender, EventArgs e)
        {
            string name = nameBox.Text;
            string username = usernameBox.Text;
            string password = passwordBox.Text;
            string age = ageBox.Text;
            string gender = genderBox.SelectedIndex.ToString();
            switch (gender)
            {
                case "0":
                    gender = "male";
                    break;
                case "1":
                    gender = "female";
                    break;
                default:
                    gender = "male";
                    break;
            }

            string error = "";
            if (name.Length < 3)
                error += "Name cannot be less than 3 characters long.\n";
            if (username.Length < 6)
                error += "Username cannot be less than 6 characters long.\n";
            if (password.Length < 8)
                error += "Password cannot be less than 8 characters long.\n";
            if (age.Length != 4 && !IsDigitsOnly(age))
                error += "Year of birth is invalid.";
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

                    MySqlCommand command = new MySqlCommand("INSERT INTO `users`(`ime`, `godiste`, `pol`, `korisnicko_ime`, `sifra`) VALUES ('" + name + "', '" + age + "', '" + gender + "', '" + username + "', '" + password + "')", connection);

                    connection.Open();
                    if (command.ExecuteNonQuery() == 0)
                        MessageBox.Show("There was an error trying to regiter a new user. Please try again later.");
                    else
                    {
                        MessageBox.Show(
                            this,
                            "You may log in now.",
                            "Registration successful",
                            MessageBoxButtons.OK
                        );

                        loginForm login = new loginForm();
                        this.Hide();
                        login.ShowDialog();
                        this.Close();
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
