namespace stevit_diagnosis
{
    partial class AddSymptom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PartBox = new System.Windows.Forms.ComboBox();
            this.SubpartBox = new System.Windows.Forms.ComboBox();
            this.SymptomBox = new System.Windows.Forms.ComboBox();
            this.Add = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PartBox
            // 
            this.PartBox.FormattingEnabled = true;
            this.PartBox.Location = new System.Drawing.Point(139, 12);
            this.PartBox.Name = "PartBox";
            this.PartBox.Size = new System.Drawing.Size(121, 21);
            this.PartBox.TabIndex = 0;
            this.PartBox.SelectedIndexChanged += new System.EventHandler(this.PartBox_SelectedIndexChanged);
            // 
            // SubpartBox
            // 
            this.SubpartBox.FormattingEnabled = true;
            this.SubpartBox.Location = new System.Drawing.Point(139, 39);
            this.SubpartBox.Name = "SubpartBox";
            this.SubpartBox.Size = new System.Drawing.Size(121, 21);
            this.SubpartBox.TabIndex = 1;
            this.SubpartBox.SelectedIndexChanged += new System.EventHandler(this.SubpartBox_SelectedIndexChanged);
            // 
            // SymptomBox
            // 
            this.SymptomBox.FormattingEnabled = true;
            this.SymptomBox.Location = new System.Drawing.Point(139, 66);
            this.SymptomBox.Name = "SymptomBox";
            this.SymptomBox.Size = new System.Drawing.Size(121, 21);
            this.SymptomBox.TabIndex = 2;
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(12, 93);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(248, 23);
            this.Add.TabIndex = 3;
            this.Add.Text = "Add Symptom";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Body Part";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Body Subpart";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Symptom";
            // 
            // AddSymptom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 129);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.SymptomBox);
            this.Controls.Add(this.SubpartBox);
            this.Controls.Add(this.PartBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddSymptom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Symptom";
            this.Load += new System.EventHandler(this.AddSymptom_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox PartBox;
        private System.Windows.Forms.ComboBox SubpartBox;
        private System.Windows.Forms.ComboBox SymptomBox;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}