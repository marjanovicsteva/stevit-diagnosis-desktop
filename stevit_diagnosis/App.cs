using System;
using System.Windows.Forms;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using stevit_diagnosis.Properties;

namespace stevit_diagnosis {
    public partial class App : Form {
        readonly string gender = Settings.Default.gender;
        readonly string age = Settings.Default.age;
        public string language = Settings.Default.language;
        public string format = "json";
        private string token = "";

        public App() {
            InitializeComponent();
            string uri = "https://sandbox-authservice.priaid.ch/login";
            string api_key = "marjanovicstevan02@gmail.com";
            string secret_key = "s7GWj38Acz2T5Qyp4";
            byte[] secretBytes = Encoding.UTF8.GetBytes(secret_key);
            string computedHashString = "";
            string response = "";

            using (HMACMD5 hmac = new HMACMD5(secretBytes)) {
                byte[] dataBytes = Encoding.UTF8.GetBytes(uri);
                byte[] computedHash = hmac.ComputeHash(dataBytes);
                computedHashString = Convert.ToBase64String(computedHash);
            }

            using (WebClient client = new WebClient()) {
                client.Headers["Authorization"] = string.Concat("Bearer ", api_key, ":", computedHashString);
                response = client.UploadString(uri, "POST", "");

            }

            dynamic json = JObject.Parse(response);
            token = json.Token;
        }

        public static string Get(string uri) {
            var client = new WebClient();
            var content = client.DownloadData(uri);
            return Encoding.UTF8.GetString(content);
        }

        private void RequestBtn_Click(object sender, EventArgs e) {
            try {
                string symptoms = Settings.Default.symptomsIDs;
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
                Settings.Default.symptomsIDs = "";
                Settings.Default.symptomsNames = "";
                SymptomBox.Text = Settings.Default.symptomsNames;
            } catch (Exception ex) {
                // Implement issue reporting
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void App_Load(object sender, EventArgs e) {
            var data = new List<Language>
            {
                new Language { Name = "English", Value = "en-gb" },
                new Language { Name = "Español", Value = "es-es" },
                new Language { Name = "Français", Value = "fr-fr" },
                new Language { Name = "Deutsche", Value = "de-ch" },
                new Language { Name = "Italiano", Value = "it-it" },
                new Language { Name = "عربى", Value = "ar-sa" },
                new Language { Name = "Русский", Value = "ru-ru" },
                new Language { Name = "Türk", Value = "tr-tr" },
                new Language { Name = "Srpski", Value = "sr-sp" },
                new Language { Name = "Slovenskí", Value = "sk-sk" }
            };
            this.Languages.DataSource = data;
            this.Languages.DisplayMember = "Name";
            this.Languages.ValueMember = "Value";
            this.Languages.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Languages.SelectedIndex = 0;
            language = Languages.SelectedValue.ToString();
            Settings.Default.language = language;

            if (Settings.Default.name == "") {
                ConfigureUser register = new ConfigureUser();
                register.Cancel.Enabled = false;
                register.ShowDialog();
            }
            UserName.Text = Settings.Default.name;
            UserName.ReadOnly = true;
        }

        private void Configure_Click(object sender, EventArgs e) {
            ConfigureUser register = new ConfigureUser();
            register.ShowDialog();
            UserName.Text = Settings.Default.name;
        }

        private void Languages_SelectedIndexChanged(object sender, EventArgs e) {
            language = Languages.SelectedValue.ToString();
            Settings.Default.language = language;
        }

        private void AddSymptoms_Click(object sender, EventArgs e) {
            AddSymptom addSymptom = new AddSymptom();
            addSymptom.ShowDialog();
            SymptomBox.Text = Settings.Default.symptomsNames;
        }
    }

    public class Language {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
