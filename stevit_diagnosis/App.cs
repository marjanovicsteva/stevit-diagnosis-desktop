using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using stevit_diagnosis.Properties;

namespace stevit_diagnosis
{
    public partial class App : Form
    {
        public string gender = Settings.Default.gender;
        public string age = Settings.Default.age;
        public string language = "";
        public string format = "json";
        public string token = "";

        public App()
        {
            InitializeComponent();
            string uri = "https://sandbox-authservice.priaid.ch/login";
            string api_key = "marjanovicstevan02@gmail.com";
            string secret_key = "s7GWj38Acz2T5Qyp4";
            byte[] secretBytes = Encoding.UTF8.GetBytes(secret_key);
            string computedHashString = "";
            using (HMACMD5 hmac = new HMACMD5(secretBytes))
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(uri);
                byte[] computedHash = hmac.ComputeHash(dataBytes);
                computedHashString = Convert.ToBase64String(computedHash);
            }

            WebClient client = new WebClient();

            client.Headers["Authorization"] = string.Concat("Bearer ", api_key, ":", computedHashString);

            string response = client.UploadString(uri, "POST", "");

            dynamic json = JObject.Parse(response);
            token = json.Token;
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

        private void RequestBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string symptoms = symptomBox.Text;
                symptoms = "[" + symptoms + "]";
                string what = "symptoms=" + symptoms + "&gender=" + gender + "&year_of_birth=" + age + "&token=" + token + "&language=" + language;
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

        private void App_Load(object sender, EventArgs e)
        {
            var data = new List<Language>
            {
                new Language() { Name = "English", Value = "en-gb" },
                new Language() { Name = "Español", Value = "es-es" },
                new Language() { Name = "Français", Value = "fr-fr" },
                new Language() { Name = "Deutsche", Value = "de-ch" },
                new Language() { Name = "Italiano", Value = "it-it" },
                new Language() { Name = "عربى", Value = "ar-sa" },
                new Language() { Name = "Русский", Value = "ru-ru" },
                new Language() { Name = "Türk", Value = "tr-tr" },
                new Language() { Name = "Srpski", Value = "sr-sp" },
                new Language() { Name = "Slovenskí", Value = "sk-sk" }
            };
            this.Languages.DataSource = data;
            this.Languages.DisplayMember = "Name";
            this.Languages.ValueMember = "Value";
            this.Languages.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Languages.SelectedIndex = 0;
            language = Languages.SelectedValue.ToString();

            if (Settings.Default.name == "")
            {
                ConfigureUser register = new ConfigureUser();
                register.Cancel.Enabled = false;
                register.ShowDialog();
            }
            else
            {
                UserName.Text = Settings.Default.name;
                UserName.ReadOnly = true;
            }
        }

        private void Configure_Click(object sender, EventArgs e)
        {
            ConfigureUser register = new ConfigureUser();
            register.ShowDialog();
            UserName.Text = Settings.Default.name;
        }

        private void Languages_SelectedIndexChanged(object sender, EventArgs e)
        {
            language = Languages.SelectedValue.ToString();
        }
    }

    public class Language
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
