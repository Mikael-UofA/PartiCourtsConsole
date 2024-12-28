namespace PrepareData
{
    partial class Dashboard
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
            CCourtsButton = new Button();
            DCourtsButton = new Button();
            JudgesButton = new Button();
            TestConnButton = new Button();
            StoreDBButton = new Button();
            DemoIntButton = new Button();
            DemoDelButton = new Button();
            DropTablesButton = new Button();
            StatusBox = new RichTextBox();
            UpdateTabsButton = new Button();
            SuspendLayout();
            // 
            // CCourtsButton
            // 
            CCourtsButton.BackColor = Color.IndianRed;
            CCourtsButton.Location = new Point(24, 21);
            CCourtsButton.Name = "CCourtsButton";
            CCourtsButton.Size = new Size(139, 56);
            CCourtsButton.TabIndex = 0;
            CCourtsButton.Text = "Generate CCs";
            CCourtsButton.UseVisualStyleBackColor = false;
            CCourtsButton.Click += CCourtsButton_Click;
            // 
            // DCourtsButton
            // 
            DCourtsButton.BackColor = Color.DimGray;
            DCourtsButton.Cursor = Cursors.No;
            DCourtsButton.Enabled = false;
            DCourtsButton.Location = new Point(184, 21);
            DCourtsButton.Name = "DCourtsButton";
            DCourtsButton.Size = new Size(139, 56);
            DCourtsButton.TabIndex = 1;
            DCourtsButton.Text = "Generate DCs";
            DCourtsButton.UseVisualStyleBackColor = false;
            DCourtsButton.Click += DCourtsButton_Click;
            // 
            // JudgesButton
            // 
            JudgesButton.BackColor = Color.DimGray;
            JudgesButton.Cursor = Cursors.No;
            JudgesButton.Enabled = false;
            JudgesButton.Location = new Point(348, 21);
            JudgesButton.Name = "JudgesButton";
            JudgesButton.Size = new Size(139, 56);
            JudgesButton.TabIndex = 2;
            JudgesButton.Text = "Load Judges";
            JudgesButton.UseVisualStyleBackColor = false;
            JudgesButton.Click += JudgesButton_Click;
            // 
            // TestConnButton
            // 
            TestConnButton.BackColor = Color.SandyBrown;
            TestConnButton.Location = new Point(597, 321);
            TestConnButton.Name = "TestConnButton";
            TestConnButton.Size = new Size(191, 117);
            TestConnButton.TabIndex = 3;
            TestConnButton.Text = "Test Connection";
            TestConnButton.UseVisualStyleBackColor = false;
            TestConnButton.Click += TestConnButton_Click;
            // 
            // StoreDBButton
            // 
            StoreDBButton.BackColor = Color.DimGray;
            StoreDBButton.Cursor = Cursors.No;
            StoreDBButton.Enabled = false;
            StoreDBButton.Location = new Point(516, 21);
            StoreDBButton.Name = "StoreDBButton";
            StoreDBButton.Size = new Size(139, 56);
            StoreDBButton.TabIndex = 4;
            StoreDBButton.Text = "Store in DB";
            StoreDBButton.UseVisualStyleBackColor = false;
            StoreDBButton.Click += StoreDBButton_Click;
            // 
            // DemoIntButton
            // 
            DemoIntButton.BackColor = Color.RosyBrown;
            DemoIntButton.Location = new Point(456, 321);
            DemoIntButton.Name = "DemoIntButton";
            DemoIntButton.Size = new Size(118, 55);
            DemoIntButton.TabIndex = 5;
            DemoIntButton.Text = "Insert Demo";
            DemoIntButton.UseVisualStyleBackColor = false;
            DemoIntButton.Click += DemoIntButton_Click;
            // 
            // DemoDelButton
            // 
            DemoDelButton.BackColor = Color.RosyBrown;
            DemoDelButton.Location = new Point(456, 382);
            DemoDelButton.Name = "DemoDelButton";
            DemoDelButton.Size = new Size(118, 55);
            DemoDelButton.TabIndex = 6;
            DemoDelButton.Text = "Delete Demo";
            DemoDelButton.UseVisualStyleBackColor = false;
            DemoDelButton.Click += DemoDelButton_Click;
            // 
            // DropTablesButton
            // 
            DropTablesButton.BackColor = Color.Black;
            DropTablesButton.Font = new Font("Verdana", 13.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            DropTablesButton.ForeColor = Color.Red;
            DropTablesButton.Location = new Point(12, 252);
            DropTablesButton.Name = "DropTablesButton";
            DropTablesButton.Size = new Size(211, 186);
            DropTablesButton.TabIndex = 7;
            DropTablesButton.Text = "Drop All Tables";
            DropTablesButton.UseVisualStyleBackColor = false;
            DropTablesButton.Click += DropTablesButton_Click;
            // 
            // StatusBox
            // 
            StatusBox.BackColor = Color.SeaShell;
            StatusBox.Font = new Font("Arial", 10.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            StatusBox.Location = new Point(549, 250);
            StatusBox.Name = "StatusBox";
            StatusBox.ScrollBars = RichTextBoxScrollBars.None;
            StatusBox.Size = new Size(239, 65);
            StatusBox.TabIndex = 8;
            StatusBox.Text = "Status: ";
            // 
            // UpdateTabsButton
            // 
            UpdateTabsButton.BackColor = Color.DarkGray;
            UpdateTabsButton.Font = new Font("Verdana", 13.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            UpdateTabsButton.ForeColor = Color.Teal;
            UpdateTabsButton.Location = new Point(239, 250);
            UpdateTabsButton.Name = "UpdateTabsButton";
            UpdateTabsButton.Size = new Size(211, 186);
            UpdateTabsButton.TabIndex = 9;
            UpdateTabsButton.Text = "Update All Tables";
            UpdateTabsButton.UseVisualStyleBackColor = false;
            UpdateTabsButton.Click += UpdateTabsButton_Click;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(UpdateTabsButton);
            Controls.Add(StatusBox);
            Controls.Add(DropTablesButton);
            Controls.Add(DemoDelButton);
            Controls.Add(DemoIntButton);
            Controls.Add(StoreDBButton);
            Controls.Add(TestConnButton);
            Controls.Add(JudgesButton);
            Controls.Add(DCourtsButton);
            Controls.Add(CCourtsButton);
            Name = "Dashboard";
            Text = "Form1";
            Load += Dashboard_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button CCourtsButton;
        private Button DCourtsButton;
        private Button JudgesButton;
        private Button TestConnButton;
        private Button StoreDBButton;
        private Button DemoIntButton;
        private Button DemoDelButton;
        private Button DropTablesButton;
        private RichTextBox StatusBox;
        private Button UpdateTabsButton;
    }
}
