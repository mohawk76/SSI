namespace SSI1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            panelLewy = new Panel();
            txtRaport = new TextBox();
            btnRaport2 = new Button();
            btnRaport1 = new Button();
            panelLewy.SuspendLayout();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.Dock = DockStyle.Fill;
            formsPlot1.Name = "formsPlot1";
            formsPlot1.TabIndex = 0;
            // 
            // panelLewy
            // 
            panelLewy.Controls.Add(txtRaport);
            panelLewy.Controls.Add(btnRaport2);
            panelLewy.Controls.Add(btnRaport1);
            panelLewy.Dock = DockStyle.Left;
            panelLewy.Name = "panelLewy";
            panelLewy.Padding = new Padding(6);
            panelLewy.Size = new Size(620, 901);
            panelLewy.TabIndex = 1;
            // 
            // txtRaport
            // 
            txtRaport.Dock = DockStyle.Fill;
            txtRaport.Font = new Font("Consolas", 9F);
            txtRaport.Multiline = true;
            txtRaport.Name = "txtRaport";
            txtRaport.ReadOnly = true;
            txtRaport.ScrollBars = ScrollBars.Both;
            txtRaport.WordWrap = false;
            txtRaport.TabIndex = 3;
            // 
            // btnRaport2
            // 
            btnRaport2.Dock = DockStyle.Top;
            btnRaport2.Name = "btnRaport2";
            btnRaport2.Size = new Size(608, 40);
            btnRaport2.TabIndex = 1;
            btnRaport2.Text = "Raport 2: k-średnie, tylko x1, m=4, 10 iteracji";
            btnRaport2.Click += btnRaport2_Click;
            // 
            // btnRaport1
            // 
            btnRaport1.Dock = DockStyle.Top;
            btnRaport1.Name = "btnRaport1";
            btnRaport1.Size = new Size(608, 40);
            btnRaport1.TabIndex = 0;
            btnRaport1.Text = "Raport 1: k-średnie, euklidesowa, m=3, 10 iteracji";
            btnRaport1.Click += btnRaport1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1586, 901);
            Controls.Add(formsPlot1);
            Controls.Add(panelLewy);
            Name = "Form1";
            Text = "SSI lab 1 - grupowanie";
            Load += Form1_Load;
            panelLewy.ResumeLayout(false);
            panelLewy.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private Panel panelLewy;
        private TextBox txtRaport;
        private Button btnRaport1;
        private Button btnRaport2;
    }
}
