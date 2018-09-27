using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using stevit_diagnosis.Properties;

namespace stevit_diagnosis
{
    public partial class AddSymptom : Form
    {
        private const string uriParts = "https://sandbox-healthservice.priaid.ch/body/locations";
        private const string uriSymptoms = "https://sandbox-healthservice.priaid.ch/symptoms";
        private string token;
        readonly string language = Settings.Default.language;
        readonly string age = Settings.Default.age;
        readonly string gender = Settings.Default.gender;

        public AddSymptom()
        {
            InitializeComponent();
        }

        static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
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

        private void AddSymptom_Load(object sender, EventArgs e)
        {
            PartBox.Enabled = false;
            SubpartBox.Enabled = false;
            SymptomBox.Enabled = false;
            string uriToken = "https://sandbox-authservice.priaid.ch/login";
            string api_key = "marjanovicstevan02@gmail.com";
            string secret_key = "s7GWj38Acz2T5Qyp4";
            byte[] secretBytes = Encoding.UTF8.GetBytes(secret_key);
            string computedHashString = "";
            using (HMACMD5 hmac = new HMACMD5(secretBytes))
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(uriToken);
                byte[] computedHash = hmac.ComputeHash(dataBytes);
                computedHashString = Convert.ToBase64String(computedHash);
            }

            using (WebClient client = new WebClient())
            {
                client.Headers["Authorization"] = string.Concat("Bearer ", api_key, ":", computedHashString);
                string responseToken = client.UploadString(uriToken, "POST", "");
                dynamic json = JObject.Parse(responseToken);
                token = json.Token;
            }

            string responseParts = Get(uriParts + "?token=" + token + "&language=" + language);
            dynamic jsonPartsArray = JArray.Parse(responseParts);

            var dataParts = new List<Part>();
            foreach (dynamic jsonPart in jsonPartsArray)
            {
                dataParts.Add(new Part() { ID = jsonPart.ID, Name = jsonPart.Name });
            }

            PartBox.DataSource = dataParts;
            PartBox.DisplayMember = "Name";
            PartBox.ValueMember = "ID";
            PartBox.Enabled = true;
            PartBox.DropDownStyle = ComboBoxStyle.DropDownList;            
        }

        private void PartBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsDigitsOnly(PartBox.SelectedValue.ToString()))
            {
                string responseSubparts = Get(uriParts + "/" + PartBox.SelectedValue.ToString() + "?token=" + token + "&language=" + language);
                dynamic jsonSubpartsArray = JArray.Parse(responseSubparts);

                var dataSubparts = new List<Part>();
                foreach (dynamic jsonSubpart in jsonSubpartsArray)
                {
                    dataSubparts.Add(new Part() { ID = jsonSubpart.ID, Name = jsonSubpart.Name });
                }

                SubpartBox.DataSource = dataSubparts;
                SubpartBox.DisplayMember = "Name";
                SubpartBox.ValueMember = "ID";
                SubpartBox.Enabled = true;
                SubpartBox.DropDownStyle = ComboBoxStyle.DropDownList;

                SymptomBox.Enabled = false;
            }
        }

        private void SubpartBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsDigitsOnly(SubpartBox.SelectedValue.ToString()))
            {
                string selectorStatus = "man";
                bool isAdult = DateTime.Now.Year - Int16.Parse(age) > 11;
                switch (gender)
                {
                    case "male":
                        selectorStatus = (isAdult) ? "man" : "boy";
                        break;
                    case "female":
                        selectorStatus = (isAdult) ? "woman" : "girl";
                        break;
                }
                string responseSymptoms = Get(uriSymptoms + "/" + SubpartBox.SelectedValue.ToString() + "/" + selectorStatus + "?token=" + token + "&language=" + language);
                dynamic jsonSymptomsArray = JArray.Parse(responseSymptoms);

                var dataSymptoms = new List<Symptom>();
                foreach (dynamic jsonSymptom in jsonSymptomsArray)
                {
                    bool HasRedFlag = false;
                    string name = jsonSymptom.Name;
                    if (jsonSymptom.HasRedFlag is JValue jv)
                    {
                        HasRedFlag = (bool)jv.Value;
                    }
                    if (HasRedFlag)
                    {
                        name += " (!)";
                    }
                    dataSymptoms.Add(new Symptom() { ID = jsonSymptom.ID, Name = name });
                }

                SymptomBox.DataSource = dataSymptoms;
                SymptomBox.DisplayMember = "Name";
                SymptomBox.ValueMember = "ID";
                SymptomBox.Enabled = true;
                SymptomBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (Settings.Default.symptomsIDs == "")
            {
                Settings.Default.symptomsIDs += SymptomBox.SelectedValue.ToString();
            } else
            {
                Settings.Default.symptomsIDs += "," + SymptomBox.SelectedValue.ToString();
            }

            if (Settings.Default.symptomsNames == "")
            {
                Settings.Default.symptomsNames += SymptomBox.Text;
            }
            else
            {
                Settings.Default.symptomsNames += ", " + SymptomBox.Text;
            }

            this.Close();
        }
    }

    public class Part
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class Symptom
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
