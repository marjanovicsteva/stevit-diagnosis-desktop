using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;

namespace loznica2018
{
    public partial class app : Form
    {
        public string gender = String.Empty;
        public string age = String.Empty;
        public string language = "en-gb";
        public string format = "json";
        public string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJlbWFpbCI6Im1hcmphbm92aWNzdGV2YW4wMkBnbWFpbC5jb20iLCJyb2xlIjoiVXNlciIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL3NpZCI6IjMxNDAiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3ZlcnNpb24iOiIyMDAiLCJodHRwOi8vZXhhbXBsZS5vcmcvY2xhaW1zL2xpbWl0IjoiOTk5OTk5OTk5IiwiaHR0cDovL2V4YW1wbGUub3JnL2NsYWltcy9tZW1iZXJzaGlwIjoiUHJlbWl1bSIsImh0dHA6Ly9leGFtcGxlLm9yZy9jbGFpbXMvbGFuZ3VhZ2UiOiJlbi1nYiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvZXhwaXJhdGlvbiI6IjIwOTktMTItMzEiLCJodHRwOi8vZXhhbXBsZS5vcmcvY2xhaW1zL21lbWJlcnNoaXBzdGFydCI6IjIwMTgtMDQtMDMiLCJpc3MiOiJodHRwczovL3NhbmRib3gtYXV0aHNlcnZpY2UucHJpYWlkLmNoIiwiYXVkIjoiaHR0cHM6Ly9oZWFsdGhzZXJ2aWNlLnByaWFpZC5jaCIsImV4cCI6MTUyMzE0NTEyNCwibmJmIjoxNTIzMTM3OTI0fQ.rk_WW5k7bA2wR5dceyuUrf6EJtccb_5xuzPYHqIgrNY";

        public app()
        {
            InitializeComponent();
        }

        public string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private void app_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            loginForm login = new loginForm();

            string korisnicko_ime = login.KorisnickoIme;

            try
            {
                string connectionString = "Server=mysql.stackcp.com; Port=50449; Database=loznica-33338c04; Uid=loznica-33338c04; Pwd=fo94dw1sh1;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `korisnicko_ime`='" + korisnicko_ime + "' LIMIT 1", connection);
                MySqlDataReader reader;
                connection.Open();
                reader = command.ExecuteReader();
                string name = String.Empty;
                while (reader.Read())
                {
                    name = reader.GetString(1);
                    age = reader.GetString(2);
                    gender = reader.GetString(3);
                }
                connection.Close();

                nameLbl.Text = "Dobrodošli, " + name + "!";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                        
        }

        private void requestBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string symptoms = symptomBox.Text;
                symptoms = "[" + symptoms + "]";
                string what = "symptoms=" + symptoms + "&gender=" + gender + "&year_of_birth=" + age + "&token=" + token + "&language=" + language + "&format=" + format;
                string where = "https://sandbox-healthservice.priaid.ch/diagnosis?";

                string json = Get(where + what);
                dynamic diagnosis = JArray.Parse(json);
                dynamic diagnose = diagnosis[0];
                string illness = diagnose.Issue.ProfName;
                string accuracy = diagnose.Issue.Accuracy;
                string doctor = diagnose.Specialisation[0].Name;

                MessageBox.Show("There is " + accuracy + "% chance that you have " + illness + " and you should go to a doctor specialized in " + doctor + ".", "You have been diagnosed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string where = "https://sandbox-healthservice.priaid.ch/symptoms?";
            string what = "token=" + token + "&language=" + language + "&format=" + format;

            string uri = where + what;
            System.Diagnostics.Process.Start(uri);

        }
    }
}
