namespace DDDTools
{
    partial class Form2
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
            this.OpenPdf = new System.Windows.Forms.Button();
            this.receiptbutton = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.id = new System.Windows.Forms.TextBox();
            this.OpenXlsx = new System.Windows.Forms.Button();
            this.UpdateData = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // OpenPdf
            // 
            this.OpenPdf.Location = new System.Drawing.Point(12, 223);
            this.OpenPdf.Name = "OpenPdf";
            this.OpenPdf.Size = new System.Drawing.Size(75, 35);
            this.OpenPdf.TabIndex = 33;
            this.OpenPdf.Text = "Open PDF";
            this.OpenPdf.UseVisualStyleBackColor = true;
            this.OpenPdf.Click += new System.EventHandler(this.OpenPdf_Click);
            // 
            // receiptbutton
            // 
            this.receiptbutton.Location = new System.Drawing.Point(12, 183);
            this.receiptbutton.Name = "receiptbutton";
            this.receiptbutton.Size = new System.Drawing.Size(116, 34);
            this.receiptbutton.TabIndex = 32;
            this.receiptbutton.Text = "Generate Reciept";
            this.receiptbutton.UseVisualStyleBackColor = true;
            this.receiptbutton.Click += new System.EventHandler(this.receiptbutton_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 128);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(18, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "ID";
            // 
            // id
            // 
            this.id.Location = new System.Drawing.Point(12, 144);
            this.id.Name = "id";
            this.id.Size = new System.Drawing.Size(100, 20);
            this.id.TabIndex = 30;
            // 
            // OpenXlsx
            // 
            this.OpenXlsx.Location = new System.Drawing.Point(12, 264);
            this.OpenXlsx.Name = "OpenXlsx";
            this.OpenXlsx.Size = new System.Drawing.Size(75, 35);
            this.OpenXlsx.TabIndex = 34;
            this.OpenXlsx.Text = "Open XLSX";
            this.OpenXlsx.UseVisualStyleBackColor = true;
            this.OpenXlsx.Click += new System.EventHandler(this.OpenXlsx_Click);
            // 
            // UpdateData
            // 
            this.UpdateData.Location = new System.Drawing.Point(12, 12);
            this.UpdateData.Name = "UpdateData";
            this.UpdateData.Size = new System.Drawing.Size(75, 35);
            this.UpdateData.TabIndex = 35;
            this.UpdateData.Text = "Load excel";
            this.UpdateData.UseVisualStyleBackColor = true;
            this.UpdateData.Click += new System.EventHandler(this.UpdateData_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 53);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(269, 32);
            this.progressBar1.TabIndex = 36;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 319);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.UpdateData);
            this.Controls.Add(this.OpenXlsx);
            this.Controls.Add(this.OpenPdf);
            this.Controls.Add(this.receiptbutton);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.id);
            this.Name = "Form2";
            this.Text = "DDDTools";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenPdf;
        private System.Windows.Forms.Button receiptbutton;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox id;
        private System.Windows.Forms.Button OpenXlsx;
        private System.Windows.Forms.Button UpdateData;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}