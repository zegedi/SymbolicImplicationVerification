namespace SimImplyGUI
{
    partial class SimImplyForm
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
            menuStrip=new MenuStrip();
            fileMenu=new ToolStripMenuItem();
            resetMenuItem=new ToolStripMenuItem();
            toolStripSeparator1=new ToolStripSeparator();
            saveMenuItem=new ToolStripMenuItem();
            loadMenuItem=new ToolStripMenuItem();
            toolStripSeparator2=new ToolStripSeparator();
            exitMenuItem=new ToolStripMenuItem();
            editMenu=new ToolStripMenuItem();
            stateSpaceMenu=new ToolStripMenuItem();
            formulaMenu=new ToolStripMenuItem();
            implyMenu=new ToolStripMenuItem();
            resultMenu=new ToolStripMenuItem();
            statusStrip=new StatusStrip();
            descriptionLabel=new ToolStripStatusLabel();
            toolStripStatusLabel1=new ToolStripStatusLabel();
            descriptionTextLabel=new ToolStripStatusLabel();
            latexLabel=new ToolStripStatusLabel();
            latexCodeLabel=new ToolStripStatusLabel();
            counterNameLabel=new ToolStripStatusLabel();
            counterLabel=new ToolStripStatusLabel();
            tableLayoutPanel=new TableLayoutPanel();
            saveFileDialog=new SaveFileDialog();
            openFileDialog=new OpenFileDialog();
            emptyStatusLabel=new ToolStripStatusLabel();
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, editMenu, resultMenu });
            menuStrip.Location=new Point(0, 0);
            menuStrip.Name="menuStrip";
            menuStrip.Padding=new Padding(7, 2, 0, 2);
            menuStrip.Size=new Size(934, 24);
            menuStrip.TabIndex=1;
            menuStrip.Text="menuStrip1";
            // 
            // fileMenu
            // 
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { resetMenuItem, toolStripSeparator1, saveMenuItem, loadMenuItem, toolStripSeparator2, exitMenuItem });
            fileMenu.Name="fileMenu";
            fileMenu.Size=new Size(37, 20);
            fileMenu.Text="File";
            // 
            // resetMenuItem
            // 
            resetMenuItem.Name="resetMenuItem";
            resetMenuItem.Size=new Size(181, 22);
            resetMenuItem.Text="Alaphelyzetbe állítás";
            resetMenuItem.Click+=ResetMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name="toolStripSeparator1";
            toolStripSeparator1.Size=new Size(178, 6);
            // 
            // saveMenuItem
            // 
            saveMenuItem.Name="saveMenuItem";
            saveMenuItem.Size=new Size(181, 22);
            saveMenuItem.Text="Adatok mentése";
            saveMenuItem.Click+=SaveMenuItem_Click;
            // 
            // loadMenuItem
            // 
            loadMenuItem.Name="loadMenuItem";
            loadMenuItem.Size=new Size(181, 22);
            loadMenuItem.Text="Adatok betöltése";
            loadMenuItem.Click+=SaveMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name="toolStripSeparator2";
            toolStripSeparator2.Size=new Size(178, 6);
            // 
            // exitMenuItem
            // 
            exitMenuItem.Name="exitMenuItem";
            exitMenuItem.Size=new Size(181, 22);
            exitMenuItem.Text="Kilépés";
            exitMenuItem.Click+=ExitMenuItem_Click;
            // 
            // editMenu
            // 
            editMenu.DropDownItems.AddRange(new ToolStripItem[] { stateSpaceMenu, formulaMenu, implyMenu });
            editMenu.Name="editMenu";
            editMenu.Size=new Size(77, 20);
            editMenu.Text="Szerkesztés";
            // 
            // stateSpaceMenu
            // 
            stateSpaceMenu.Name="stateSpaceMenu";
            stateSpaceMenu.Size=new Size(201, 22);
            stateSpaceMenu.Text="Állapottér szerkesztése";
            stateSpaceMenu.Click+=StateSpaceMenu_Click;
            // 
            // formulaMenu
            // 
            formulaMenu.Name="formulaMenu";
            formulaMenu.Size=new Size(201, 22);
            formulaMenu.Text="Formulák szerkesztése";
            formulaMenu.Click+=FormulaMenu_Click;
            // 
            // implyMenu
            // 
            implyMenu.Name="implyMenu";
            implyMenu.Size=new Size(201, 22);
            implyMenu.Text="Implikációk szerkesztése";
            implyMenu.Click+=ImplyMenu_Click;
            // 
            // resultMenu
            // 
            resultMenu.Name="resultMenu";
            resultMenu.Size=new Size(75, 20);
            resultMenu.Text="Kiértékelés";
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { descriptionLabel, toolStripStatusLabel1, descriptionTextLabel, latexLabel, latexCodeLabel, emptyStatusLabel, counterNameLabel, counterLabel });
            statusStrip.Location=new Point(0, 631);
            statusStrip.MinimumSize=new Size(0, 30);
            statusStrip.Name="statusStrip";
            statusStrip.Size=new Size(934, 30);
            statusStrip.TabIndex=2;
            statusStrip.Text="statusStrip1";
            // 
            // descriptionLabel
            // 
            descriptionLabel.Font=new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            descriptionLabel.Name="descriptionLabel";
            descriptionLabel.Size=new Size(109, 25);
            descriptionLabel.Text="Megnevezés:";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name="toolStripStatusLabel1";
            toolStripStatusLabel1.Size=new Size(0, 25);
            // 
            // descriptionTextLabel
            // 
            descriptionTextLabel.BorderSides=ToolStripStatusLabelBorderSides.Right;
            descriptionTextLabel.Font=new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            descriptionTextLabel.Name="descriptionTextLabel";
            descriptionTextLabel.Size=new Size(55, 25);
            descriptionTextLabel.Text="Leírás";
            // 
            // latexLabel
            // 
            latexLabel.Font=new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            latexLabel.Name="latexLabel";
            latexLabel.Size=new Size(88, 25);
            latexLabel.Text="Latex kód:";
            // 
            // latexCodeLabel
            // 
            latexCodeLabel.Font=new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            latexCodeLabel.Name="latexCodeLabel";
            latexCodeLabel.Size=new Size(16, 25);
            latexCodeLabel.Text="-";
            // 
            // counterNameLabel
            // 
            counterNameLabel.BorderSides=ToolStripStatusLabelBorderSides.Left;
            counterNameLabel.Font=new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            counterNameLabel.Name="counterNameLabel";
            counterNameLabel.Size=new Size(82, 25);
            counterNameLabel.Text="Sorszám:";
            // 
            // counterLabel
            // 
            counterLabel.Font=new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            counterLabel.Name="counterLabel";
            counterLabel.Size=new Size(42, 25);
            counterLabel.Text="1 / 1";
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount=2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Dock=DockStyle.Fill;
            tableLayoutPanel.Location=new Point(0, 24);
            tableLayoutPanel.Name="tableLayoutPanel";
            tableLayoutPanel.RowCount=2;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Size=new Size(934, 607);
            tableLayoutPanel.TabIndex=3;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName="openFileDialog1";
            // 
            // emptyStatusLabel
            // 
            emptyStatusLabel.Name="emptyStatusLabel";
            emptyStatusLabel.Size=new Size(531, 25);
            emptyStatusLabel.Spring=true;
            // 
            // SimImplyForm
            // 
            AutoScaleDimensions=new SizeF(8F, 20F);
            AutoScaleMode=AutoScaleMode.Font;
            ClientSize=new Size(934, 661);
            Controls.Add(tableLayoutPanel);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            Font=new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            MainMenuStrip=menuStrip;
            Margin=new Padding(3, 4, 3, 4);
            Name="SimImplyForm";
            Text="Form1";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem editMenu;
        private ToolStripMenuItem stateSpaceMenu;
        private ToolStripMenuItem formulaMenu;
        private ToolStripMenuItem implyMenu;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel descriptionLabel;
        private TableLayoutPanel tableLayoutPanel;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel descriptionTextLabel;
        private ToolStripStatusLabel latexLabel;
        private ToolStripStatusLabel latexCodeLabel;
        private ToolStripMenuItem saveMenuItem;
        private ToolStripMenuItem loadMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem resetMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitMenuItem;
        private ToolStripMenuItem resultMenu;
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;
        private ToolStripStatusLabel counterNameLabel;
        private ToolStripStatusLabel counterLabel;
        private ToolStripStatusLabel emptyStatusLabel;
    }
}