using System;
using System.Windows.Forms;
using stevit_diagnosis.Properties;

namespace stevit_diagnosis {
    public partial class ConfigureUser : Form {
        public static bool IsDigitsOnly(string str) {
            foreach (char c in str) {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public ConfigureUser() {
            InitializeComponent();
        }


        private void RegisterBtn_Click(object sender, EventArgs e) {
            string name = NameBox.Text;
            string age = AgeBox.Text;
            string gender = GenderBox.SelectedIndex.ToString();
            switch (gender) {
                case "0":
                    gender = "male";
                    break;
                case "1":
                    gender = "female";
                    break;
                default:
                    gender = "undefined";
                    break;
            }

            string error = "";
            if (name.Length < 3)
                error += "Name cannot be less than 3 characters long.\n";
            if (age.Length != 4 && !IsDigitsOnly(age))
                error += "Year of birth is invalid.\n";
            if (gender == "undefined")
                error += "Gender must be specified";
            if (error.Length > 0) {
                MessageBox.Show(
                    this,
                    "There was one or more errors in processing your request.\n" + error,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            } else {
                // Register user
                Settings.Default.name = name;
                Settings.Default.age = age;
                Settings.Default.gender = gender;
                Settings.Default.Save();
                this.Close();
            }
        }

        private void Cancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void ConfigureUser_Load(object sender, EventArgs e) {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
    }
}
