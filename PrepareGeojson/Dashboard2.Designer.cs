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
            CrtGeoDCButton = new Button();
            CCourtsButton = new Button();
            CrtGeoCCButton = new Button();
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
            // CrtGeoDCButton
            // 
            CrtGeoDCButton.BackColor = Color.MediumTurquoise;
            CrtGeoDCButton.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CrtGeoDCButton.ForeColor = SystemColors.InfoText;
            CrtGeoDCButton.Location = new Point(173, 12);
            CrtGeoDCButton.Name = "CrtGeoDCButton";
            CrtGeoDCButton.Size = new Size(139, 56);
            CrtGeoDCButton.TabIndex = 12;
            CrtGeoDCButton.Text = "Create DC File";
            CrtGeoDCButton.UseVisualStyleBackColor = false;
            CrtGeoDCButton.Click += CrtGeoButton_Click;
            // 
            // CCourtsButton
            // 
            CCourtsButton.BackColor = Color.MediumTurquoise;
            CCourtsButton.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CCourtsButton.ForeColor = SystemColors.InfoText;
            CCourtsButton.Location = new Point(12, 83);
            CCourtsButton.Name = "CCourtsButton";
            CCourtsButton.Size = new Size(139, 56);
            CCourtsButton.TabIndex = 13;
            CCourtsButton.Text = "Query CCs";
            CCourtsButton.UseVisualStyleBackColor = false;
            CCourtsButton.Click += CCourtsButton_Click;
            // 
            // CrtGeoCCButton
            // 
            CrtGeoCCButton.BackColor = Color.MediumTurquoise;
            CrtGeoCCButton.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CrtGeoCCButton.ForeColor = SystemColors.InfoText;
            CrtGeoCCButton.Location = new Point(173, 83);
            CrtGeoCCButton.Name = "CrtGeoCCButton";
            CrtGeoCCButton.Size = new Size(139, 56);
            CrtGeoCCButton.TabIndex = 14;
            CrtGeoCCButton.Text = "Create CC File";
            CrtGeoCCButton.UseVisualStyleBackColor = false;
            CrtGeoCCButton.Click += CrtGeoCCButton_Click;
            // 
            // Dashboard2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(CrtGeoCCButton);
            Controls.Add(CCourtsButton);
            Controls.Add(CrtGeoDCButton);
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
        private Button CrtGeoDCButton;
        private Button CCourtsButton;
        private Button CrtGeoCCButton;
    }
}
