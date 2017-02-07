namespace WinformsPractice
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.listView1 = new System.Windows.Forms.ListView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnAddOutputUnsafe = new System.Windows.Forms.Button();
            this.btnAddOutputSafe = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(32, 77);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(159, 330);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.Leave += new System.EventHandler(this.listView1_Leave);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(215, 42);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(353, 20);
            this.txtOutput.TabIndex = 1;
            // 
            // btnAddOutputUnsafe
            // 
            this.btnAddOutputUnsafe.Location = new System.Drawing.Point(215, 13);
            this.btnAddOutputUnsafe.Name = "btnAddOutputUnsafe";
            this.btnAddOutputUnsafe.Size = new System.Drawing.Size(112, 23);
            this.btnAddOutputUnsafe.TabIndex = 2;
            this.btnAddOutputUnsafe.Text = "AddOutputUnsafe";
            this.btnAddOutputUnsafe.UseVisualStyleBackColor = true;
            this.btnAddOutputUnsafe.Click += new System.EventHandler(this.btnAddOutputUnsafe_Click);
            // 
            // btnAddOutputSafe
            // 
            this.btnAddOutputSafe.Location = new System.Drawing.Point(333, 12);
            this.btnAddOutputSafe.Name = "btnAddOutputSafe";
            this.btnAddOutputSafe.Size = new System.Drawing.Size(112, 23);
            this.btnAddOutputSafe.TabIndex = 3;
            this.btnAddOutputSafe.Text = "AddOutputSafe";
            this.btnAddOutputSafe.UseVisualStyleBackColor = true;
            this.btnAddOutputSafe.Click += new System.EventHandler(this.btnAddOutputSafe_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 447);
            this.Controls.Add(this.btnAddOutputSafe);
            this.Controls.Add(this.btnAddOutputUnsafe);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.listView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnAddOutputUnsafe;
        private System.Windows.Forms.Button btnAddOutputSafe;
    }
}

