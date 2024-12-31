namespace PrepareGeojson
{
    partial class Dashboard2
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
            DCourtsButton = new Button();
            TestConnButton = new Button();
            StatusBox = new RichTextBox();
            InputGeoButton = new Button();
            OutputGeoButton = new Button();
            CrtGeoButton = new Button();
            SuspendLayout();
            // 
            // DCourtsButton
            // 
            DCourtsButton.BackColor = Color.MediumTurquoise;
            DCourtsButton.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            DCourtsButton.ForeColor = SystemColors.InfoText;
            DCourtsButton.Location = new Point(12, 12);
            DCourtsButton.Name = "DCourtsButton";
            DCourtsButton.Size = new Size(139, 56);
            DCourtsButton.TabIndex = 2;
            DCourtsButton.Text = "Query DCs";
            DCourtsButton.UseVisualStyleBackColor = false;
            DCourtsButton.Click += DCourtsButton_Click;
            // 
            // TestConnButton
            // 
            TestConnButton.BackColor = Color.SandyBrown;
            TestConnButton.Location = new Point(597, 321);
            TestConnButton.Name = "TestConnButton";
            TestConnButton.Size = new Size(191, 117);
            TestConnButton.TabIndex = 4;
            TestConnButton.Text = "Test Connection";
            TestConnButton.UseVisualStyleBackColor = false;
            TestConnButton.Click += TestConnButton_Click;
            // 
            // StatusBox
            // 
            StatusBox.BackColor = Color.SeaShell;
            StatusBox.Font = new Font("Arial", 10.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            StatusBox.Location = new Point(549, 250);
            StatusBox.Name = "StatusBox";
            StatusBox.ScrollBars = RichTextBoxScrollBars.None;
            StatusBox.Size = new Size(239, 65);
            StatusBox.TabIndex = 9;
            StatusBox.Text = "Status: ";
            // 
            // InputGeoButton
            // 
            InputGeoButton.BackColor = Color.SandyBrown;
            InputGeoButton.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            InputGeoButton.ForeColor = SystemColors.InfoText;
            InputGeoButton.Location = new Point(549, 126);
            InputGeoButton.Name = "InputGeoButton";
            InputGeoButton.Size = new Size(115, 79);
            InputGeoButton.TabIndex = 10;
            InputGeoButton.Text = "Test Input Geojson";
            InputGeoButton.UseVisualStyleBackColor = false;
            InputGeoButton.Click += InputGeoButton_Click;
            // 
            // OutputGeoButton
            // 
            OutputGeoButton.BackColor = Color.SandyBrown;
            OutputGeoButton.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OutputGeoButton.ForeColor = SystemColors.InfoText;
            OutputGeoButton.Location = new Point(673, 126);
            OutputGeoButton.Name = "OutputGeoButton";
            OutputGeoButton.Size = new Size(115, 79);
            OutputGeoButton.TabIndex = 11;
            OutputGeoButton.Text = "Test Output Geojson";
            OutputGeoButton.UseVisualStyleBackColor = false;
            OutputGeoButton.Click += OutputGeoButton_Click;
            // 
            // CrtGeoButton
            // 
            CrtGeoButton.BackColor = Color.MediumTurquoise;
            CrtGeoButton.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CrtGeoButton.ForeColor = SystemColors.InfoText;
            CrtGeoButton.Location = new Point(173, 12);
            CrtGeoButton.Name = "CrtGeoButton";
            CrtGeoButton.Size = new Size(139, 56);
            CrtGeoButton.TabIndex = 12;
            CrtGeoButton.Text = "Create File";
            CrtGeoButton.UseVisualStyleBackColor = false;
            CrtGeoButton.Click += CrtGeoButton_Click;
            // 
            // Dashboard2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(CrtGeoButton);
            Controls.Add(OutputGeoButton);
            Controls.Add(InputGeoButton);
            Controls.Add(StatusBox);
            Controls.Add(TestConnButton);
            Controls.Add(DCourtsButton);
            Name = "Dashboard2";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button DCourtsButton;
        private Button TestConnButton;
        private RichTextBox StatusBox;
        private Button InputGeoButton;
        private Button OutputGeoButton;
        private Button CrtGeoButton;
    }
}
