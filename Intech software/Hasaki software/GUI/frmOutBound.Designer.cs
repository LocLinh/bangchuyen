namespace Intech_software.GUI
{
    partial class frmOutBound
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
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabDataMan = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxDetectedSystems = new System.Windows.Forms.ListBox();
            this.btnRefreshDeviceList = new System.Windows.Forms.Button();
            this.cbEnableKeepAlive = new System.Windows.Forms.CheckBox();
            this.txtDeviceIP = new System.Windows.Forms.TextBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.cbAutoReconnect = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnRefreshDgv = new System.Windows.Forms.Button();
            this.btnExportToExcell = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dgvOutBound = new System.Windows.Forms.DataGridView();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ngay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thoiGian = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maKienHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tenTK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.picResultImage = new System.Windows.Forms.PictureBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.cbLiveDisplay = new System.Windows.Forms.CheckBox();
            this.lbReadString = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.btnTrigger = new System.Windows.Forms.Button();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listBoxStatus = new System.Windows.Forms.ListBox();
            this.tabControl2.SuspendLayout();
            this.tabDataMan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutBound)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picResultImage)).BeginInit();
            this.tabControl3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabDataMan);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl2.Location = new System.Drawing.Point(1096, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(281, 838);
            this.tabControl2.TabIndex = 2;
            // 
            // tabDataMan
            // 
            this.tabDataMan.BackColor = System.Drawing.SystemColors.Control;
            this.tabDataMan.Controls.Add(this.label1);
            this.tabDataMan.Controls.Add(this.pictureBox1);
            this.tabDataMan.Controls.Add(this.label2);
            this.tabDataMan.Controls.Add(this.listBoxDetectedSystems);
            this.tabDataMan.Controls.Add(this.btnRefreshDeviceList);
            this.tabDataMan.Controls.Add(this.cbEnableKeepAlive);
            this.tabDataMan.Controls.Add(this.txtDeviceIP);
            this.tabDataMan.Controls.Add(this.btnDisconnect);
            this.tabDataMan.Controls.Add(this.cbAutoReconnect);
            this.tabDataMan.Controls.Add(this.txtPassword);
            this.tabDataMan.Controls.Add(this.label3);
            this.tabDataMan.Controls.Add(this.btnConnect);
            this.tabDataMan.Location = new System.Drawing.Point(4, 25);
            this.tabDataMan.Name = "tabDataMan";
            this.tabDataMan.Padding = new System.Windows.Forms.Padding(3);
            this.tabDataMan.Size = new System.Drawing.Size(273, 809);
            this.tabDataMan.TabIndex = 0;
            this.tabDataMan.Text = "DataMan";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(82, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 18);
            this.label1.TabIndex = 16;
            this.label1.Text = "Connect to DataMan";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Intech_software.Properties.Resources.camera_icon2;
            this.pictureBox1.Location = new System.Drawing.Point(14, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 318);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Device";
            // 
            // listBoxDetectedSystems
            // 
            this.listBoxDetectedSystems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxDetectedSystems.FormattingEnabled = true;
            this.listBoxDetectedSystems.ItemHeight = 16;
            this.listBoxDetectedSystems.Location = new System.Drawing.Point(14, 115);
            this.listBoxDetectedSystems.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxDetectedSystems.Name = "listBoxDetectedSystems";
            this.listBoxDetectedSystems.Size = new System.Drawing.Size(247, 132);
            this.listBoxDetectedSystems.TabIndex = 14;
            this.listBoxDetectedSystems.SelectedIndexChanged += new System.EventHandler(this.listBoxDetectedSystems_SelectedIndexChanged_1);
            // 
            // btnRefreshDeviceList
            // 
            this.btnRefreshDeviceList.Image = global::Intech_software.Properties.Resources.Refresh_icon;
            this.btnRefreshDeviceList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefreshDeviceList.Location = new System.Drawing.Point(14, 254);
            this.btnRefreshDeviceList.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefreshDeviceList.Name = "btnRefreshDeviceList";
            this.btnRefreshDeviceList.Size = new System.Drawing.Size(246, 43);
            this.btnRefreshDeviceList.TabIndex = 11;
            this.btnRefreshDeviceList.Text = "Refresh";
            // 
            // cbEnableKeepAlive
            // 
            this.cbEnableKeepAlive.Location = new System.Drawing.Point(104, 379);
            this.cbEnableKeepAlive.Margin = new System.Windows.Forms.Padding(4);
            this.cbEnableKeepAlive.Name = "cbEnableKeepAlive";
            this.cbEnableKeepAlive.Size = new System.Drawing.Size(156, 21);
            this.cbEnableKeepAlive.TabIndex = 12;
            this.cbEnableKeepAlive.Text = "Keep alive";
            this.cbEnableKeepAlive.CheckedChanged += new System.EventHandler(this.cbEnableKeepAlive_CheckedChanged);
            // 
            // txtDeviceIP
            // 
            this.txtDeviceIP.Location = new System.Drawing.Point(104, 312);
            this.txtDeviceIP.Margin = new System.Windows.Forms.Padding(4);
            this.txtDeviceIP.Name = "txtDeviceIP";
            this.txtDeviceIP.ReadOnly = true;
            this.txtDeviceIP.Size = new System.Drawing.Size(156, 22);
            this.txtDeviceIP.TabIndex = 7;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Image = global::Intech_software.Properties.Resources.Disconnect_icon;
            this.btnDisconnect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDisconnect.Location = new System.Drawing.Point(137, 439);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(123, 43);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // cbAutoReconnect
            // 
            this.cbAutoReconnect.Location = new System.Drawing.Point(104, 408);
            this.cbAutoReconnect.Margin = new System.Windows.Forms.Padding(4);
            this.cbAutoReconnect.Name = "cbAutoReconnect";
            this.cbAutoReconnect.Size = new System.Drawing.Size(156, 21);
            this.cbAutoReconnect.TabIndex = 13;
            this.cbAutoReconnect.Text = "Auto reconnect";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(104, 346);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(156, 22);
            this.txtPassword.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 352);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Password";
            // 
            // btnConnect
            // 
            this.btnConnect.Image = global::Intech_software.Properties.Resources.connect_icon;
            this.btnConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConnect.Location = new System.Drawing.Point(14, 439);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(123, 43);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(-2, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1096, 631);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.Tag = "";
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.btnRefreshDgv);
            this.tabPage1.Controls.Add(this.btnExportToExcell);
            this.tabPage1.Controls.Add(this.btnSearch);
            this.tabPage1.Controls.Add(this.dateTimePicker1);
            this.tabPage1.Controls.Add(this.dgvOutBound);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1088, 602);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Home";
            // 
            // btnRefreshDgv
            // 
            this.btnRefreshDgv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshDgv.BackColor = System.Drawing.SystemColors.Control;
            this.btnRefreshDgv.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRefreshDgv.Image = global::Intech_software.Properties.Resources.refresh_icon2;
            this.btnRefreshDgv.Location = new System.Drawing.Point(424, 529);
            this.btnRefreshDgv.Name = "btnRefreshDgv";
            this.btnRefreshDgv.Size = new System.Drawing.Size(100, 40);
            this.btnRefreshDgv.TabIndex = 32;
            this.btnRefreshDgv.UseVisualStyleBackColor = false;
            this.btnRefreshDgv.Click += new System.EventHandler(this.btnRefreshDgv_Click);
            // 
            // btnExportToExcell
            // 
            this.btnExportToExcell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportToExcell.BackColor = System.Drawing.SystemColors.Control;
            this.btnExportToExcell.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExportToExcell.Image = global::Intech_software.Properties.Resources.Export_icon;
            this.btnExportToExcell.Location = new System.Drawing.Point(318, 529);
            this.btnExportToExcell.Name = "btnExportToExcell";
            this.btnExportToExcell.Size = new System.Drawing.Size(100, 40);
            this.btnExportToExcell.TabIndex = 31;
            this.btnExportToExcell.UseVisualStyleBackColor = false;
            this.btnExportToExcell.Click += new System.EventHandler(this.btnExportToExcell_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearch.Image = global::Intech_software.Properties.Resources.Search_icon;
            this.btnSearch.Location = new System.Drawing.Point(212, 529);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 40);
            this.btnSearch.TabIndex = 30;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(8, 539);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(198, 30);
            this.dateTimePicker1.TabIndex = 29;
            // 
            // dgvOutBound
            // 
            this.dgvOutBound.AllowUserToAddRows = false;
            this.dgvOutBound.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOutBound.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvOutBound.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvOutBound.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOutBound.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.ngay,
            this.thoiGian,
            this.maKienHang,
            this.tenTK});
            this.dgvOutBound.Location = new System.Drawing.Point(8, 7);
            this.dgvOutBound.Name = "dgvOutBound";
            this.dgvOutBound.ReadOnly = true;
            this.dgvOutBound.RowHeadersWidth = 51;
            this.dgvOutBound.RowTemplate.Height = 24;
            this.dgvOutBound.Size = new System.Drawing.Size(696, 510);
            this.dgvOutBound.TabIndex = 26;
            // 
            // STT
            // 
            this.STT.DataPropertyName = "STT";
            this.STT.HeaderText = "STT";
            this.STT.MinimumWidth = 6;
            this.STT.Name = "STT";
            this.STT.ReadOnly = true;
            this.STT.Width = 109;
            // 
            // ngay
            // 
            this.ngay.DataPropertyName = "ngay";
            this.ngay.HeaderText = "Ngày";
            this.ngay.MinimumWidth = 6;
            this.ngay.Name = "ngay";
            this.ngay.ReadOnly = true;
            this.ngay.Width = 150;
            // 
            // thoiGian
            // 
            this.thoiGian.DataPropertyName = "thoiGian";
            this.thoiGian.HeaderText = "Thời gian";
            this.thoiGian.MinimumWidth = 6;
            this.thoiGian.Name = "thoiGian";
            this.thoiGian.ReadOnly = true;
            this.thoiGian.Width = 150;
            // 
            // maKienHang
            // 
            this.maKienHang.DataPropertyName = "maKienHang";
            this.maKienHang.HeaderText = "Mã kiện hàng";
            this.maKienHang.MinimumWidth = 6;
            this.maKienHang.Name = "maKienHang";
            this.maKienHang.ReadOnly = true;
            this.maKienHang.Width = 300;
            // 
            // tenTK
            // 
            this.tenTK.DataPropertyName = "tenTK";
            this.tenTK.HeaderText = "Tên tài khoản";
            this.tenTK.MinimumWidth = 6;
            this.tenTK.Name = "tenTK";
            this.tenTK.ReadOnly = true;
            this.tenTK.Width = 150;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.picResultImage);
            this.groupBox6.Controls.Add(this.txtQuantity);
            this.groupBox6.Controls.Add(this.cbLiveDisplay);
            this.groupBox6.Controls.Add(this.lbReadString);
            this.groupBox6.Controls.Add(this.lblQuantity);
            this.groupBox6.Controls.Add(this.btnTrigger);
            this.groupBox6.Location = new System.Drawing.Point(710, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(368, 511);
            this.groupBox6.TabIndex = 28;
            this.groupBox6.TabStop = false;
            // 
            // picResultImage
            // 
            this.picResultImage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.picResultImage.Location = new System.Drawing.Point(7, 68);
            this.picResultImage.Margin = new System.Windows.Forms.Padding(4);
            this.picResultImage.Name = "picResultImage";
            this.picResultImage.Size = new System.Drawing.Size(353, 249);
            this.picResultImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picResultImage.TabIndex = 0;
            this.picResultImage.TabStop = false;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantity.Location = new System.Drawing.Point(287, 364);
            this.txtQuantity.Margin = new System.Windows.Forms.Padding(4);
            this.txtQuantity.Multiline = true;
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.ReadOnly = true;
            this.txtQuantity.Size = new System.Drawing.Size(70, 32);
            this.txtQuantity.TabIndex = 22;
            this.txtQuantity.Text = "0";
            // 
            // cbLiveDisplay
            // 
            this.cbLiveDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLiveDisplay.Location = new System.Drawing.Point(249, 31);
            this.cbLiveDisplay.Margin = new System.Windows.Forms.Padding(4);
            this.cbLiveDisplay.Name = "cbLiveDisplay";
            this.cbLiveDisplay.Size = new System.Drawing.Size(112, 21);
            this.cbLiveDisplay.TabIndex = 15;
            this.cbLiveDisplay.Text = "Live Display";
            this.cbLiveDisplay.CheckedChanged += new System.EventHandler(this.cbLiveDisplay_CheckedChanged);
            // 
            // lbReadString
            // 
            this.lbReadString.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbReadString.Location = new System.Drawing.Point(7, 325);
            this.lbReadString.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbReadString.Name = "lbReadString";
            this.lbReadString.Size = new System.Drawing.Size(219, 37);
            this.lbReadString.TabIndex = 16;
            // 
            // lblQuantity
            // 
            this.lblQuantity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.BackColor = System.Drawing.SystemColors.Control;
            this.lblQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblQuantity.Location = new System.Drawing.Point(202, 378);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(70, 18);
            this.lblQuantity.TabIndex = 25;
            this.lblQuantity.Text = "Quantity";
            // 
            // btnTrigger
            // 
            this.btnTrigger.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTrigger.Enabled = false;
            this.btnTrigger.Image = global::Intech_software.Properties.Resources.Trigger_icon;
            this.btnTrigger.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTrigger.Location = new System.Drawing.Point(7, 22);
            this.btnTrigger.Margin = new System.Windows.Forms.Padding(4);
            this.btnTrigger.Name = "btnTrigger";
            this.btnTrigger.Size = new System.Drawing.Size(234, 37);
            this.btnTrigger.TabIndex = 5;
            this.btnTrigger.Text = "Trigger";
            this.btnTrigger.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTrigger_MouseDown);
            this.btnTrigger.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTrigger_MouseUp);
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage2);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl3.Location = new System.Drawing.Point(0, 633);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1096, 205);
            this.tabControl3.TabIndex = 20;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.listBoxStatus);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1088, 176);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Status";
            // 
            // listBoxStatus
            // 
            this.listBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxStatus.ItemHeight = 16;
            this.listBoxStatus.Location = new System.Drawing.Point(9, 23);
            this.listBoxStatus.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxStatus.Name = "listBoxStatus";
            this.listBoxStatus.Size = new System.Drawing.Size(1067, 132);
            this.listBoxStatus.TabIndex = 18;
            // 
            // frmOutBound
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(1377, 838);
            this.Controls.Add(this.tabControl3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tabControl2);
            this.Name = "frmOutBound";
            this.Text = "frmOutBound";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmOutBound_FormClosing);
            this.Load += new System.EventHandler(this.frmOutBound_Load);
            this.tabControl2.ResumeLayout(false);
            this.tabDataMan.ResumeLayout(false);
            this.tabDataMan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutBound)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picResultImage)).EndInit();
            this.tabControl3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabDataMan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxDetectedSystems;
        private System.Windows.Forms.Button btnRefreshDeviceList;
        private System.Windows.Forms.CheckBox cbEnableKeepAlive;
        private System.Windows.Forms.TextBox txtDeviceIP;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.CheckBox cbAutoReconnect;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnExportToExcell;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DataGridView dgvOutBound;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.PictureBox picResultImage;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.CheckBox cbLiveDisplay;
        private System.Windows.Forms.Label lbReadString;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Button btnTrigger;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox listBoxStatus;
        private System.Windows.Forms.Button btnRefreshDgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ngay;
        private System.Windows.Forms.DataGridViewTextBoxColumn thoiGian;
        private System.Windows.Forms.DataGridViewTextBoxColumn maKienHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn tenTK;
    }
}