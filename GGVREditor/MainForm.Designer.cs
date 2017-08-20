namespace GGVREditor
{
    partial class MainForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvDataFixed = new System.Windows.Forms.DataGridView();
            this.clmGirlID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGirlName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmHSR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mainGridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsMainRestoreAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMainShowAddresses = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvOutfit = new System.Windows.Forms.DataGridView();
            this.cellContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsRestoreOriginal = new System.Windows.Forms.ToolStripMenuItem();
            this.tsCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsShowFieldAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvAppearance = new System.Windows.Forms.DataGridView();
            this.clmAGirlID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAGirlName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAHair = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAFace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmASkin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAEyeColor = new System.Windows.Forms.DataGridViewButtonColumn();
            this.clmAEyeBColor = new System.Windows.Forms.DataGridViewButtonColumn();
            this.clmOGirlID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOGirlName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOOutfit = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.clmOAccessory = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.clmOSocks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOShoes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataFixed)).BeginInit();
            this.mainGridContextMenu.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutfit)).BeginInit();
            this.cellContextMenu.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppearance)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(815, 617);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvDataFixed);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(959, 590);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Size";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvDataFixed
            // 
            this.dgvDataFixed.AllowUserToAddRows = false;
            this.dgvDataFixed.AllowUserToDeleteRows = false;
            this.dgvDataFixed.AllowUserToResizeRows = false;
            this.dgvDataFixed.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDataFixed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataFixed.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmGirlID,
            this.clmGirlName,
            this.clmAddress,
            this.clmHeight,
            this.clmHSR,
            this.clmB,
            this.clmW,
            this.clmH});
            this.dgvDataFixed.ContextMenuStrip = this.mainGridContextMenu;
            this.dgvDataFixed.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvDataFixed.Location = new System.Drawing.Point(6, 6);
            this.dgvDataFixed.MultiSelect = false;
            this.dgvDataFixed.Name = "dgvDataFixed";
            this.dgvDataFixed.RowHeadersVisible = false;
            this.dgvDataFixed.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvDataFixed.Size = new System.Drawing.Size(947, 578);
            this.dgvDataFixed.TabIndex = 5;
            this.dgvDataFixed.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellClick);
            this.dgvDataFixed.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellLeave);
            this.dgvDataFixed.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellClick);
            this.dgvDataFixed.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellLeave);
            this.dgvDataFixed.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvData_EditingControlShowing);
            this.dgvDataFixed.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dgvData_SortCompare);
            // 
            // clmGirlID
            // 
            this.clmGirlID.HeaderText = "ID";
            this.clmGirlID.Name = "clmGirlID";
            this.clmGirlID.ReadOnly = true;
            this.clmGirlID.Width = 30;
            // 
            // clmGirlName
            // 
            this.clmGirlName.HeaderText = "Girl name";
            this.clmGirlName.Name = "clmGirlName";
            this.clmGirlName.ReadOnly = true;
            this.clmGirlName.Width = 150;
            // 
            // clmAddress
            // 
            this.clmAddress.HeaderText = "Address";
            this.clmAddress.Name = "clmAddress";
            this.clmAddress.ReadOnly = true;
            this.clmAddress.Visible = false;
            this.clmAddress.Width = 85;
            // 
            // clmHeight
            // 
            this.clmHeight.HeaderText = "Height";
            this.clmHeight.Name = "clmHeight";
            this.clmHeight.Width = 80;
            // 
            // clmHSR
            // 
            this.clmHSR.HeaderText = "Head Size Ratio";
            this.clmHSR.Name = "clmHSR";
            this.clmHSR.Width = 80;
            // 
            // clmB
            // 
            this.clmB.HeaderText = "Bust";
            this.clmB.Name = "clmB";
            this.clmB.Width = 80;
            // 
            // clmW
            // 
            this.clmW.HeaderText = "Waist";
            this.clmW.Name = "clmW";
            this.clmW.Width = 80;
            // 
            // clmH
            // 
            this.clmH.HeaderText = "Hip";
            this.clmH.Name = "clmH";
            this.clmH.Width = 80;
            // 
            // mainGridContextMenu
            // 
            this.mainGridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMainRestoreAll,
            this.tsMainShowAddresses});
            this.mainGridContextMenu.Name = "mainGridContextMenu";
            this.mainGridContextMenu.Size = new System.Drawing.Size(160, 48);
            // 
            // tsMainRestoreAll
            // 
            this.tsMainRestoreAll.Name = "tsMainRestoreAll";
            this.tsMainRestoreAll.Size = new System.Drawing.Size(159, 22);
            this.tsMainRestoreAll.Text = "Restore all";
            // 
            // tsMainShowAddresses
            // 
            this.tsMainShowAddresses.CheckOnClick = true;
            this.tsMainShowAddresses.Name = "tsMainShowAddresses";
            this.tsMainShowAddresses.Size = new System.Drawing.Size(159, 22);
            this.tsMainShowAddresses.Text = "Show Addresses";
            this.tsMainShowAddresses.Click += new System.EventHandler(this.tsMainShowAddresses_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvOutfit);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(807, 591);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Outfit";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvOutfit
            // 
            this.dgvOutfit.AllowUserToAddRows = false;
            this.dgvOutfit.AllowUserToDeleteRows = false;
            this.dgvOutfit.AllowUserToResizeRows = false;
            this.dgvOutfit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOutfit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOutfit.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmOGirlID,
            this.clmOGirlName,
            this.clmOAddress,
            this.clmOOutfit,
            this.clmOAccessory,
            this.clmOSocks,
            this.clmOShoes});
            this.dgvOutfit.ContextMenuStrip = this.mainGridContextMenu;
            this.dgvOutfit.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvOutfit.Location = new System.Drawing.Point(6, 6);
            this.dgvOutfit.MultiSelect = false;
            this.dgvOutfit.Name = "dgvOutfit";
            this.dgvOutfit.RowHeadersVisible = false;
            this.dgvOutfit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvOutfit.Size = new System.Drawing.Size(795, 579);
            this.dgvOutfit.TabIndex = 6;
            this.dgvOutfit.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellClick);
            this.dgvOutfit.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellLeave);
            this.dgvOutfit.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellClick);
            this.dgvOutfit.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellLeave);
            this.dgvOutfit.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvData_EditingControlShowing);
            this.dgvOutfit.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dgvData_SortCompare);
            // 
            // cellContextMenu
            // 
            this.cellContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsRestoreOriginal,
            this.tsCopy,
            this.tsPaste,
            this.toolStripSeparator1,
            this.tsShowFieldAddress});
            this.cellContextMenu.Name = "cellContextMenu";
            this.cellContextMenu.Size = new System.Drawing.Size(173, 98);
            // 
            // tsRestoreOriginal
            // 
            this.tsRestoreOriginal.Name = "tsRestoreOriginal";
            this.tsRestoreOriginal.Size = new System.Drawing.Size(172, 22);
            this.tsRestoreOriginal.Text = "Restore original";
            this.tsRestoreOriginal.Click += new System.EventHandler(this.tsRestoreOriginal_Click);
            // 
            // tsCopy
            // 
            this.tsCopy.Name = "tsCopy";
            this.tsCopy.ShortcutKeyDisplayString = "Ctrl+C";
            this.tsCopy.Size = new System.Drawing.Size(172, 22);
            this.tsCopy.Text = "Copy";
            this.tsCopy.Click += new System.EventHandler(this.tsCopy_Click);
            // 
            // tsPaste
            // 
            this.tsPaste.Name = "tsPaste";
            this.tsPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.tsPaste.Size = new System.Drawing.Size(172, 22);
            this.tsPaste.Text = "Paste";
            this.tsPaste.Click += new System.EventHandler(this.tsPaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // tsShowFieldAddress
            // 
            this.tsShowFieldAddress.Name = "tsShowFieldAddress";
            this.tsShowFieldAddress.Size = new System.Drawing.Size(172, 22);
            this.tsShowFieldAddress.Text = "Show field address";
            this.tsShowFieldAddress.Click += new System.EventHandler(this.tsShowFieldAddress_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(16, 635);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(811, 33);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgvAppearance);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(959, 590);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Appearance";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvAppearance
            // 
            this.dgvAppearance.AllowUserToAddRows = false;
            this.dgvAppearance.AllowUserToDeleteRows = false;
            this.dgvAppearance.AllowUserToResizeRows = false;
            this.dgvAppearance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAppearance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAppearance.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmAGirlID,
            this.clmAGirlName,
            this.clmAAddress,
            this.clmAHair,
            this.clmAFace,
            this.clmASkin,
            this.clmAEyeColor,
            this.clmAEyeBColor});
            this.dgvAppearance.ContextMenuStrip = this.mainGridContextMenu;
            this.dgvAppearance.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvAppearance.Location = new System.Drawing.Point(6, 6);
            this.dgvAppearance.MultiSelect = false;
            this.dgvAppearance.Name = "dgvAppearance";
            this.dgvAppearance.RowHeadersVisible = false;
            this.dgvAppearance.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvAppearance.Size = new System.Drawing.Size(947, 578);
            this.dgvAppearance.TabIndex = 7;
            // 
            // clmAGirlID
            // 
            this.clmAGirlID.HeaderText = "ID";
            this.clmAGirlID.Name = "clmAGirlID";
            this.clmAGirlID.ReadOnly = true;
            this.clmAGirlID.Width = 30;
            // 
            // clmAGirlName
            // 
            this.clmAGirlName.HeaderText = "Girl name";
            this.clmAGirlName.Name = "clmAGirlName";
            this.clmAGirlName.ReadOnly = true;
            this.clmAGirlName.Width = 150;
            // 
            // clmAAddress
            // 
            this.clmAAddress.HeaderText = "Address";
            this.clmAAddress.Name = "clmAAddress";
            this.clmAAddress.ReadOnly = true;
            this.clmAAddress.Visible = false;
            this.clmAAddress.Width = 85;
            // 
            // clmAHair
            // 
            this.clmAHair.HeaderText = "Hair";
            this.clmAHair.Name = "clmAHair";
            this.clmAHair.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clmAHair.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clmAFace
            // 
            this.clmAFace.HeaderText = "Face";
            this.clmAFace.Name = "clmAFace";
            // 
            // clmASkin
            // 
            this.clmASkin.HeaderText = "Skin";
            this.clmASkin.Name = "clmASkin";
            // 
            // clmAEyeColor
            // 
            this.clmAEyeColor.HeaderText = "Eye colour";
            this.clmAEyeColor.Name = "clmAEyeColor";
            this.clmAEyeColor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clmAEyeColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // clmAEyeBColor
            // 
            this.clmAEyeBColor.HeaderText = "Eyebrow Colour";
            this.clmAEyeBColor.Name = "clmAEyeBColor";
            this.clmAEyeBColor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clmAEyeBColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // clmOGirlID
            // 
            this.clmOGirlID.HeaderText = "ID";
            this.clmOGirlID.Name = "clmOGirlID";
            this.clmOGirlID.ReadOnly = true;
            this.clmOGirlID.Width = 30;
            // 
            // clmOGirlName
            // 
            this.clmOGirlName.HeaderText = "Girl name";
            this.clmOGirlName.Name = "clmOGirlName";
            this.clmOGirlName.ReadOnly = true;
            this.clmOGirlName.Width = 150;
            // 
            // clmOAddress
            // 
            this.clmOAddress.HeaderText = "Address";
            this.clmOAddress.Name = "clmOAddress";
            this.clmOAddress.ReadOnly = true;
            this.clmOAddress.Visible = false;
            this.clmOAddress.Width = 85;
            // 
            // clmOOutfit
            // 
            this.clmOOutfit.HeaderText = "Outfit";
            this.clmOOutfit.Name = "clmOOutfit";
            this.clmOOutfit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clmOOutfit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.clmOOutfit.Width = 80;
            // 
            // clmOAccessory
            // 
            this.clmOAccessory.HeaderText = "Accessory";
            this.clmOAccessory.Name = "clmOAccessory";
            this.clmOAccessory.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clmOAccessory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.clmOAccessory.Width = 80;
            // 
            // clmOSocks
            // 
            this.clmOSocks.HeaderText = "Socks";
            this.clmOSocks.Name = "clmOSocks";
            this.clmOSocks.Width = 40;
            // 
            // clmOShoes
            // 
            this.clmOShoes.HeaderText = "Shoes";
            this.clmOShoes.Name = "clmOShoes";
            this.clmOShoes.Width = 40;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 680);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataFixed)).EndInit();
            this.mainGridContextMenu.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutfit)).EndInit();
            this.cellContextMenu.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppearance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvDataFixed;
        private System.Windows.Forms.ContextMenuStrip cellContextMenu;
        private System.Windows.Forms.ToolStripMenuItem tsRestoreOriginal;
        private System.Windows.Forms.ToolStripMenuItem tsCopy;
        private System.Windows.Forms.ToolStripMenuItem tsPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsShowFieldAddress;
        private System.Windows.Forms.ContextMenuStrip mainGridContextMenu;
        private System.Windows.Forms.ToolStripMenuItem tsMainRestoreAll;
        private System.Windows.Forms.ToolStripMenuItem tsMainShowAddresses;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmGirlID;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmGirlName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmHSR;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmB;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmW;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmH;
        private System.Windows.Forms.DataGridView dgvOutfit;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvAppearance;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAGirlID;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAGirlName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAHair;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAFace;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmASkin;
        private System.Windows.Forms.DataGridViewButtonColumn clmAEyeColor;
        private System.Windows.Forms.DataGridViewButtonColumn clmAEyeBColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOGirlID;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOGirlName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOAddress;
        private System.Windows.Forms.DataGridViewComboBoxColumn clmOOutfit;
        private System.Windows.Forms.DataGridViewComboBoxColumn clmOAccessory;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOSocks;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOShoes;
    }
}