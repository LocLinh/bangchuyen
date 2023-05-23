using Cognex.DataMan.SDK.Discovery;
using Cognex.DataMan.SDK.Utils;
using Cognex.DataMan.SDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Xml;
using Intech_software.DTO;
using Intech_software.BUS;

namespace Intech_software.GUI
{
    public partial class frmOutBound : Form
    {

        private ResultCollector _results;
        EthSystemConnector myConn = null;
        private EthSystemDiscoverer _ethSystemDiscoverer = null;
        DataManSystem mySystem = null;
        private object _currentResultInfoSyncLock = new object();
        private ISystemConnector _connector = null;
        private SynchronizationContext syncContext = null;
        private bool _closing = false;
        private bool _autoconnect = false;
        private object _listAddItemLock = new object();
        private GuiLogger _logger;

        int count = 0;
        string subMaKH = string.Empty;
        OutBoundsBus outBoundsBus = new OutBoundsBus();
        public static string revAccountName = string.Empty;

        DataTable dtOutBound;

        public frmOutBound()
        {
            InitializeComponent();
            syncContext = WindowsFormsSynchronizationContext.Current;
            cbEnableKeepAlive.CheckedChanged += new System.EventHandler(this.cbEnableKeepAlive_CheckedChanged);
            cbLiveDisplay.CheckedChanged += new System.EventHandler(this.cbLiveDisplay_CheckedChanged);
            FormClosing += new FormClosingEventHandler(this.frmOutBound_FormClosing);
            btnTrigger.MouseDown += new MouseEventHandler(this.btnTrigger_MouseDown);
            btnTrigger.MouseUp += new MouseEventHandler(this.btnTrigger_MouseUp);
        }

        private void frmOutBound_Load(object sender, EventArgs e)
        {
            _ethSystemDiscoverer = new EthSystemDiscoverer();
            _ethSystemDiscoverer.SystemDiscovered += new EthSystemDiscoverer.SystemDiscoveredHandler(OnEthSystemDiscovered);
            _ethSystemDiscoverer.Discover();
            RefreshGui();

            txtQuantity.Text = count.ToString();
            RefreshDataGridView();

        }

        private void frmOutBound_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closing = true;
            _autoconnect = false;

            if (null != mySystem && mySystem.State == Cognex.DataMan.SDK.ConnectionState.Connected)
                mySystem.Disconnect();
            if (_ethSystemDiscoverer != null)
                _ethSystemDiscoverer.Dispose();
            _ethSystemDiscoverer = null;

            Application.Exit();
        }

        private void Results_ComplexResultCompleted(object sender, ComplexResult e)
        {
            syncContext.Post(
                delegate
                {
                    ShowResult(e);
                },
                null);
        }

        private void Results_SimpleResultDropped(object sender, SimpleResult e)
        {
            syncContext.Post(
                delegate
                {
                    ReportDroppedResult(e);
                },
                null);
        }

        private void ReportDroppedResult(SimpleResult result)
        {
            AddListItem(string.Format("Partial result dropped: {0}, id={1}", result.Id.Type.ToString(), result.Id.Id));
        }

        private void RefreshGui()
        {
            bool system_connected = mySystem != null && mySystem.State == Cognex.DataMan.SDK.ConnectionState.Connected;
            bool system_ready_to_connect = mySystem == null || mySystem.State == Cognex.DataMan.SDK.ConnectionState.Disconnected;
            bool gui_ready_to_connect = listBoxDetectedSystems.SelectedIndex != -1 && listBoxDetectedSystems.Items.Count > listBoxDetectedSystems.SelectedIndex;

            btnConnect.Enabled = system_ready_to_connect && gui_ready_to_connect;
            btnDisconnect.Enabled = system_connected;
            btnTrigger.Enabled = system_connected;
            cbLiveDisplay.Enabled = system_connected;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (listBoxDetectedSystems.SelectedIndex == -1 || listBoxDetectedSystems.SelectedIndex >= listBoxDetectedSystems.Items.Count)
                return;

            btnConnect.Enabled = false;
            _autoconnect = false;

            try
            {
                var system_info = listBoxDetectedSystems.Items[listBoxDetectedSystems.SelectedIndex];
                EthSystemDiscoverer.SystemInfo eth_system_info = system_info as EthSystemDiscoverer.SystemInfo;
                myConn = new EthSystemConnector(eth_system_info.IPAddress, eth_system_info.Port);

                _connector = myConn;

                myConn.UserName = "admin";
                myConn.Password = txtPassword.Text;

                mySystem = new DataManSystem(_connector);
                mySystem.DefaultTimeout = 5000;

                // Subscribe to events that are signalled when the system is connected / disconnected.
                mySystem.SystemConnected += new SystemConnectedHandler(OnSystemConnected);
                mySystem.SystemDisconnected += new SystemDisconnectedHandler(OnSystemDisconnected);
                mySystem.SystemWentOnline += new SystemWentOnlineHandler(OnSystemWentOnline);
                mySystem.SystemWentOffline += new SystemWentOfflineHandler(OnSystemWentOffline);
                mySystem.KeepAliveResponseMissed += new KeepAliveResponseMissedHandler(OnKeepAliveResponseMissed);
                mySystem.BinaryDataTransferProgress += new BinaryDataTransferProgressHandler(OnBinaryDataTransferProgress);
                mySystem.OffProtocolByteReceived += new OffProtocolByteReceivedHandler(OffProtocolByteReceived);
                mySystem.AutomaticResponseArrived += new AutomaticResponseArrivedHandler(AutomaticResponseArrived);

                ResultTypes requested_result_types = ResultTypes.ReadXml | ResultTypes.Image | ResultTypes.ImageGraphics;
                _results = new ResultCollector(mySystem, requested_result_types);
                _results.ComplexResultCompleted += Results_ComplexResultCompleted;
                _results.SimpleResultDropped += Results_SimpleResultDropped;


                mySystem.Connect();
                try
                {
                    mySystem.SetResultTypes(requested_result_types);
                }
                catch
                { }

                if (mySystem.State == Cognex.DataMan.SDK.ConnectionState.Connected)
                {
                    MessageBox.Show("DataMan: Connected", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                CleanupConnection();

                AddListItem("DataMan: Failed to connect: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            count = 0;
            txtQuantity.Text = count.ToString();

            if (mySystem == null || mySystem.State != Cognex.DataMan.SDK.ConnectionState.Connected)
                return;
            try
            {
                mySystem.Disconnect();
                if (mySystem.State == Cognex.DataMan.SDK.ConnectionState.Disconnected)
                    MessageBox.Show("DataMan: Disconected", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBoxDetectedSystems_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBoxDetectedSystems.SelectedIndex != -1 && listBoxDetectedSystems.Items.Count > listBoxDetectedSystems.SelectedIndex)
            {
                var system_info = listBoxDetectedSystems.Items[listBoxDetectedSystems.SelectedIndex];

                if (system_info is EthSystemDiscoverer.SystemInfo)
                {
                    EthSystemDiscoverer.SystemInfo eth_system_info = system_info as EthSystemDiscoverer.SystemInfo;

                    txtDeviceIP.Text = eth_system_info.IPAddress.ToString();
                }
            }
            RefreshGui();
        }

        private void btnRefreshDeviceList_Click(object sender, EventArgs e)
        {
            try
            {
                if (_ethSystemDiscoverer.IsDiscoveryInProgress)
                    return;

                listBoxDetectedSystems.Items.Clear();

                _ethSystemDiscoverer.Discover();
            }
            finally
            {
                RefreshGui();
            }
        }

        private void btnTrigger_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                mySystem.SendCommand("TRIGGER OFF");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send TRIGGER OFF command: " + ex.ToString(), "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTrigger_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                mySystem.SendCommand("TRIGGER ON");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send TRIGGER ON command: " + ex.ToString(), "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbLiveDisplay_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbLiveDisplay.Checked)
                {
                    btnTrigger.Enabled = false;

                    mySystem.SendCommand("SET LIVEIMG.MODE 2");
                    mySystem.BeginGetLiveImage(
                        ImageFormat.jpeg,
                        ImageSize.Sixteenth,
                        ImageQuality.Medium,
                        OnLiveImageArrived,
                        null);

                }
                else
                {
                    btnTrigger.Enabled = true;
                    mySystem.SendCommand("SET LIVEIMG.MODE 0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to set live image mode: " + ex.ToString(), "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbEnableKeepAlive_CheckedChanged(object sender, EventArgs e)
        {
            if (null != mySystem)
                mySystem.SetKeepAliveOptions(cbEnableKeepAlive.Checked, 3000, 1000);
        }

        private void Log(string function, string message)
        {
            if (_logger != null)
                _logger.Log(function, message);
        }

        #region Device Discovery Events
        private void OnEthSystemDiscovered(EthSystemDiscoverer.SystemInfo systemInfo)
        {
            syncContext.Post(
                new SendOrPostCallback(
                    delegate
                    {
                        listBoxDetectedSystems.Items.Add(systemInfo);
                    }),
                    null);
        }
        #endregion

        #region Device Events

        private void OnSystemConnected(object sender, EventArgs args)
        {
            syncContext.Post(
                delegate
                {
                    AddListItem("System connected");
                    RefreshGui();
                },
                null);
        }

        private void OnSystemDisconnected(object sender, EventArgs args)
        {
            syncContext.Post(
                delegate
                {
                    AddListItem("System disconnected");
                    bool reset_gui = false;

                    if (!_closing && _autoconnect && cbAutoReconnect.Checked)
                    {
                        frmReconnecting frm = new frmReconnecting(this, mySystem);

                        if (frm.ShowDialog() == DialogResult.Cancel)
                            reset_gui = true;
                    }
                    else
                    {
                        reset_gui = true;
                    }

                    if (reset_gui)
                    {
                        btnConnect.Enabled = true;
                        btnDisconnect.Enabled = false;
                        btnTrigger.Enabled = false;
                        cbLiveDisplay.Enabled = false;

                        picResultImage.Image = null;
                        lbReadString.Text = "";
                    }
                },
                null);
        }

        private void OnKeepAliveResponseMissed(object sender, EventArgs args)
        {
            syncContext.Post(
                delegate
                {
                    AddListItem("Keep-alive response missed");
                },
                null);
        }

        private void OnSystemWentOnline(object sender, EventArgs args)
        {
            syncContext.Post(
                delegate
                {
                    AddListItem("System went online");
                },
                null);
        }

        private void OnSystemWentOffline(object sender, EventArgs args)
        {
            syncContext.Post(
                delegate
                {
                    AddListItem("System went offline");
                },
                null);
        }

        private void OnBinaryDataTransferProgress(object sender, BinaryDataTransferProgressEventArgs args)
        {
            Log("OnBinaryDataTransferProgress", string.Format("{0}: {1}% of {2} bytes (Type={3}, Id={4})", args.Direction == TransferDirection.Incoming ? "Receiving" : "Sending", args.TotalDataSize > 0 ? (int)(100 * (args.BytesTransferred / (double)args.TotalDataSize)) : -1, args.TotalDataSize, args.ResultType.ToString(), args.ResponseId));
        }

        private void OffProtocolByteReceived(object sender, OffProtocolByteReceivedEventArgs args)
        {
            Log("OffProtocolByteReceived", string.Format("{0}", (char)args.Byte));
        }

        private void AutomaticResponseArrived(object sender, AutomaticResponseArrivedEventArgs args)
        {
            Log("AutomaticResponseArrived", string.Format("Type={0}, Id={1}, Data={2} bytes", args.DataType.ToString(), args.ResponseId, args.Data != null ? args.Data.Length : 0));
        }

        #endregion

        #region Auxiliary Methods

        private void CleanupConnection()
        {
            if (null != mySystem)
            {
                mySystem.SystemConnected -= OnSystemConnected;
                mySystem.SystemDisconnected -= OnSystemDisconnected;
                mySystem.SystemWentOnline -= OnSystemWentOnline;
                mySystem.SystemWentOffline -= OnSystemWentOffline;
                mySystem.KeepAliveResponseMissed -= OnKeepAliveResponseMissed;
                mySystem.BinaryDataTransferProgress -= OnBinaryDataTransferProgress;
                mySystem.OffProtocolByteReceived -= OffProtocolByteReceived;
                mySystem.AutomaticResponseArrived -= AutomaticResponseArrived;
            }

            _connector = null;
            mySystem = null;
        }
        private void OnLiveImageArrived(IAsyncResult result)
        {
            try
            {
                Image image = mySystem.EndGetLiveImage(result);

                syncContext.Post(
                    delegate
                    {
                        Size image_size = Gui.FitImageInControl(image.Size, picResultImage.Size);
                        Image fitted_image = Gui.ResizeImageToBitmap(image, image_size);
                        picResultImage.Image = fitted_image;
                        picResultImage.Invalidate();

                        if (cbLiveDisplay.Checked)
                        {
                            mySystem.BeginGetLiveImage(
                                ImageFormat.jpeg,
                                ImageSize.Sixteenth,
                                ImageQuality.Medium,
                                OnLiveImageArrived,
                                null);
                        }
                    },
                null);
            }
            catch
            {
            }
        }

        private string GetReadStringFromResultXml(string resultXml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(resultXml);

                XmlNode full_string_node = doc.SelectSingleNode("result/general/full_string");

                if (full_string_node != null && mySystem != null && mySystem.State == Cognex.DataMan.SDK.ConnectionState.Connected)
                {
                    XmlAttribute encoding = full_string_node.Attributes["encoding"];
                    if (encoding != null && encoding.InnerText == "base64")
                    {
                        if (!string.IsNullOrEmpty(full_string_node.InnerText))
                        {
                            byte[] code = Convert.FromBase64String(full_string_node.InnerText);
                            return mySystem.Encoding.GetString(code, 0, code.Length);
                        }
                        else
                        {
                            return "";
                        }
                    }

                    return full_string_node.InnerText;
                }
            }
            catch
            {
            }

            return "";
        }

        private void ShowResult(ComplexResult complexResult)
        {
            List<Image> images = new List<Image>();
            List<string> image_graphics = new List<string>();
            string read_result = null;
            int result_id = -1;
            ResultTypes collected_results = ResultTypes.None;

            // Take a reference or copy values from the locked result info object. This is done
            // so that the lock is used only for a short period of time.
            lock (_currentResultInfoSyncLock)
            {
                foreach (var simple_result in complexResult.SimpleResults)
                {
                    collected_results |= simple_result.Id.Type;

                    switch (simple_result.Id.Type)
                    {
                        case ResultTypes.Image:
                            Image image = ImageArrivedEventArgs.GetImageFromImageBytes(simple_result.Data);
                            if (image != null)
                                images.Add(image);
                            break;

                        case ResultTypes.ImageGraphics:
                            image_graphics.Add(simple_result.GetDataAsString());
                            break;

                        case ResultTypes.ReadXml:
                            read_result = GetReadStringFromResultXml(simple_result.GetDataAsString());
                            result_id = simple_result.Id.Id;
                            break;

                        case ResultTypes.ReadString:
                            read_result = simple_result.GetDataAsString();
                            result_id = simple_result.Id.Id;
                            break;
                    }

                }
            }
            if (read_result != string.Empty && read_result != null)
            {
                try
                {
                    subMaKH = read_result.Substring(0, 15);
                    count++;
                    outBoundsBus.SetTable(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm:ss"), subMaKH, revAccountName);

                    txtQuantity.Text = count.ToString();                   
                    RefreshDataGridView();
                    RefreshDataGridView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Log("Complex result contains", string.Format("{0}", collected_results.ToString()));

            if (images.Count > 0)
            {
                Image first_image = images[0];

                Size image_size = Gui.FitImageInControl(first_image.Size, picResultImage.Size);
                Image fitted_image = Gui.ResizeImageToBitmap(first_image, image_size);

                if (image_graphics.Count > 0)
                {
                    using (Graphics g = Graphics.FromImage(fitted_image))
                    {
                        foreach (var graphics in image_graphics)
                        {
                            ResultGraphics rg = GraphicsResultParser.Parse(graphics, new Rectangle(0, 0, image_size.Width, image_size.Height));
                            ResultGraphicsRenderer.PaintResults(g, rg);
                        }
                    }
                }

                if (picResultImage.Image != null)
                {
                    var image = picResultImage.Image;
                    picResultImage.Image = null;
                    image.Dispose();
                }

                picResultImage.Image = fitted_image;
                picResultImage.Invalidate();
            }

            if (read_result != null)
                lbReadString.Text = read_result;
        }

        private void AddListItem(object item)
        {
            lock (_listAddItemLock)
            {
                listBoxStatus.Items.Add(item);

                if (listBoxStatus.Items.Count > 500)
                    listBoxStatus.Items.RemoveAt(0);

                if (listBoxStatus.Items.Count > 0)
                    listBoxStatus.SelectedIndex = listBoxStatus.Items.Count - 1;
            }
        }
        #endregion


        void RefreshDataGridView()
        {
            dtOutBound = new DataTable();
            dtOutBound = outBoundsBus.GetTable();
            dgvOutBound.DataSource = dtOutBound;
            dgvOutBound.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOutBound.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);
        }

        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void btnExportToExcell_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ToExcel(dgvOutBound, saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ToExcel(DataGridView dgv, string fileName)
        {
            //khai báo thư viện hỗ trợ Microsoft.Office.Interop.Excel
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet worksheet;
            try
            {
                //Tạo đối tượng COM.
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                //tạo mới một Workbooks bằng phương thức add()
                workbook = excel.Workbooks.Add(Type.Missing);
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                //đặt tên cho sheet
                worksheet.Name = "Thống kê kiện hàng";

                // export header trong DataGridView
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dgv.Columns[i].HeaderText;
                }
                // export nội dung trong DataGridView
                for (int i = 0; i < dgv.RowCount; i++)
                {
                    for (int j = 0; j < dgv.ColumnCount; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgv.Rows[i].Cells[j].Value.ToString();
                    }
                }
                // sử dụng phương thức SaveAs() để lưu workbook với filename
                workbook.SaveAs(fileName);
                //đóng workbook
                workbook.Close();
                excel.Quit();
                MessageBox.Show("Đã xuất dữ liệu ra file Excel.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                workbook = null;
                worksheet = null;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dtOutBound = new DataTable();
            dtOutBound = outBoundsBus.Search(dateTimePicker1.Text);
            dgvOutBound.DataSource = dtOutBound;
            dgvOutBound.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOutBound.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);
        }

        private void btnRefreshDgv_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

    }
}
