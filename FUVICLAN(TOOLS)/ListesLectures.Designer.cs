namespace FUVICLAN_TOOLS_
{
    partial class ListesLectures
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListesLectures));
            this.radListView1 = new Telerik.WinControls.UI.RadListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radProgressBar1 = new Telerik.WinControls.UI.RadProgressBar();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.radListView1)).BeginInit();
            this.radListView1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // radListView1
            // 
            this.radListView1.Controls.Add(this.panel1);
            this.radListView1.Location = new System.Drawing.Point(13, 11);
            this.radListView1.Name = "radListView1";
            this.radListView1.Size = new System.Drawing.Size(432, 426);
            this.radListView1.TabIndex = 0;
            this.radListView1.ItemRemoving += new Telerik.WinControls.UI.ListViewItemCancelEventHandler(this.radListView1_ItemRemoving);
            this.radListView1.ItemRemoved += new Telerik.WinControls.UI.ListViewItemEventHandler(this.radListView1_ItemRemoved);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radProgressBar1);
            this.panel1.Location = new System.Drawing.Point(106, 152);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(221, 122);
            this.panel1.TabIndex = 0;
            this.panel1.Visible = false;
            // 
            // radProgressBar1
            // 
            this.radProgressBar1.Location = new System.Drawing.Point(3, 3);
            this.radProgressBar1.Name = "radProgressBar1";
            this.radProgressBar1.Size = new System.Drawing.Size(215, 116);
            this.radProgressBar1.TabIndex = 1;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 325);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(125, 56);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            this.richTextBox1.WordWrap = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 443);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(432, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Mettre à jour la listes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 472);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(432, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Supprimer toutes les listes de lectures";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ListesLectures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 507);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radListView1);
            this.Controls.Add(this.richTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ListesLectures";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ListesLectures";
            this.Load += new System.EventHandler(this.ListesLectures_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radListView1)).EndInit();
            this.radListView1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadListView radListView1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadProgressBar radProgressBar1;
    }
}