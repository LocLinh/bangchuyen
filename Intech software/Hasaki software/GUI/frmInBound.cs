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

using Cognex.DataMan.SDK.Discovery;
using Cognex.DataMan.SDK.Utils;
using Cognex.DataMan.SDK;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Xml;
using System.Net.Sockets;
using System.Net;
using System.IO.Ports;
using Intech_software.BUS;
using System.Security.Policy;
using Intech_software.DTO;
using System.IO;

namespace Intech_software.GUI
{
    public partial class frmInBound : Form
    {
        //Khởi tạo các lớp
        HasakiSystemBus hasakiSystemBus = new HasakiSystemBus();
        HasakiSystemDTO hasakiSystemDTO = new HasakiSystemDTO();
        InBoundsBus inBoundsBus = new InBoundsBus();
        public static string revAccountName = string.Empty;

        //DataTable
        DataTable dtHasakiSystem;
        DataTable dtInBound;

        //DataMan
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
        int countNG = 0;
        string maKH = string.Empty;


        //Dim
        private const int BUFFER_SIZE = 1024;
        private string IP_ADDRESS_DIM = "192.168.3.218";
        private const int PORT_NUMBER_DIM = 23;
        private static Socket client;
        private static byte[] data = new byte[BUFFER_SIZE];
        string strDimValue = string.Empty;
        string lengthValue = string.Empty;
        string widthValue = string.Empty;
        string heightValue = string.Empty;

        //PLC
        PLC plc = null;
        private string IP_ADDRESS_PLC = "192.168.3.250";
        private const int PORT_NUMBER_PLC = 24;
        string zone = string.Empty;
        string state = string.Empty;
        int readConveyorState; // doc trang thai chay dung
        int readInputRegister = 0;
        bool checkConnectPLC = false;// kiem tra ket noi plc

        //Scale
        int byte11 = 0;
        int byte12 = 0;
        double weightValue = 0.0;

        public frmInBound()
        {
            InitializeComponent();

            syncContext = WindowsFormsSynchronizationContext.Current;
            cbEnableKeepAlive.CheckedChanged += new System.EventHandler(this.cbEnableKeepAlive_CheckedChanged);
            cbLiveDisplay.CheckedChanged += new System.EventHandler(this.cbLiveDisplay_CheckedChanged);
            FormClosing += new FormClosingEventHandler(this.frmInBound_FormClosing);
            btnTrigger.MouseDown += new MouseEventHandler(this.btnTrigger_MouseDown);
            btnTrigger.MouseUp += new MouseEventHandler(this.btnTrigger_MouseUp);

            timerDateTime.Start();

            DefaulSetting();           
        }
        private void frmInBound_Load(object sender, EventArgs e)
        {
            try
            {
                if (!hasakiSystemBus.CheckDate(DateTime.Now.ToString("M/d/yyyy")))
                    MessageBox.Show("Chưa có dữ liệu bảng HasakiSystem ngày hôm nay.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtQuantity.Text = count.ToString();
                txtNG.Text = countNG.ToString() +"/"+ count.ToString();
                RefreshDgvInBound();

                #region DataMan Connection

                _ethSystemDiscoverer = new EthSystemDiscoverer();
                _ethSystemDiscoverer.SystemDiscovered += new EthSystemDiscoverer.SystemDiscoveredHandler(OnEthSystemDiscovered);
                _ethSystemDiscoverer.Discover();
                RefreshGui();

                #endregion

                #region Dim Connection

                listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " Dim: Connecting...");
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP_ADDRESS_DIM), PORT_NUMBER_DIM);
                client.BeginConnect(iep, new AsyncCallback(Connected), client);

                #endregion

                #region PLC Connection

                plc = new PLC(IP_ADDRESS_PLC, PORT_NUMBER_PLC);
                Thread thConnect = new Thread(() =>
                {
                    ConnectPLC();
                    
                })
                { IsBackground = true };
                thConnect.Start();
                timer1.Start();

                #endregion

                #region Scale Connection                             

                serialPort1.PortName = cboComPort.Text;
                serialPort1.BaudRate = baudRate;
                serialPort1.DataBits = Convert.ToInt32(dataBit);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBit);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), parity);

                if (serialPort1.IsOpen == false)
                {
                    serialPort1.Open();
                }           
                if (serialPort1.IsOpen)
                {
                    btnOpen.Enabled = false;
                    btnClose.Enabled = true;
                    progressBarScale.Value = 100;
                }
                else this.Focus();

                #endregion



            }            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmInBound_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closing = true;
            _autoconnect = false;

            if (null != mySystem && mySystem.State == Cognex.DataMan.SDK.ConnectionState.Connected)
                mySystem.Disconnect();
            if (_ethSystemDiscoverer != null)
                _ethSystemDiscoverer.Dispose();
            _ethSystemDiscoverer = null;

            if (client != null && client.Connected == true)
                client.Close();           
            if (plc != null)           
                DisconnectPLC();           
            if (serialPort1 != null &&  serialPort1.IsOpen)          
                serialPort1.Close();

            client = null;
            plc = null;
            serialPort1 = null;
 
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



        #region Dim Events
        //private void btnConnectDim_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " Dim: Connecting...");
        //        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //        IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP_ADDRESS_DIM), PORT_NUMBER_DIM);
        //        client.BeginConnect(iep, new AsyncCallback(Connected), client);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //private void btnDisconnectDim_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        strDimValue = "q";
        //        byte[] message = Encoding.ASCII.GetBytes(strDimValue);
        //        client.Close();

        //        progressBarDIM.Value = 0;
        //        listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " Dim: Connection stopped");

        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        void Connected(IAsyncResult iar)
        {
            try
            {
                client.EndConnect(iar);

                listBoxStatus.Invoke(new Action(() =>
                {
                    listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " Dim: System is online: " + client.RemoteEndPoint.ToString());
                    progressBarDIM.Value = 100;
                }));

                Thread receive = new Thread(() =>
                {
                    ReceiveData();
                });
                receive.Start();
            }
            catch (Exception ex)
            {
                listBoxStatus.Invoke(new Action(() =>
                {
                    listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " Dim: Error Connecting");
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    progressBarDIM.Value = 0;
                }));
            }
        }
        void ReceiveData()
        {
            try
            {
                int recv;

                while (true)
                {
                    recv = client.Receive(data);
                    strDimValue = Encoding.ASCII.GetString(data, 0, recv);
                    string[] arrListStr = strDimValue.Split(',');
                    if (strDimValue == "q")
                    {
                        break;
                    }  

                    this.lengthValue = arrListStr[0];
                    this.widthValue = arrListStr[1];
                    this.heightValue = arrListStr[2];
             
                    //listBoxStatus.Invoke(new Action(() =>
                    //{
                    //    listBoxStatus.Items.Add(strDimValue);
                    //}));
                    txtLength.Invoke(new Action(() =>
                    {
                        txtLength.Text = arrListStr[0];
                    }));
                    txtWidth.Invoke(new Action(() =>
                    {
                        txtWidth.Text = arrListStr[1];
                    }));
                    txtHeight.Invoke(new Action(() =>
                    {
                        txtHeight.Text = arrListStr[2];
                    }));
                }
                strDimValue = "q";
                byte[] message = Encoding.ASCII.GetBytes(strDimValue);
                client.Close();
                listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " Dim: Connection stopped");
                return;
            }
            catch (System.Net.Sockets.SocketException ex)
            {               
                listBoxStatus.Invoke(new Action(() =>
                {
                    listBoxStatus.Items.Add(ex.Message);
                    progressBarDIM.Value = 0;
                }));
            }catch(Exception ex)
            {
                listBoxStatus.Invoke(new Action(() =>
                {
                    listBoxStatus.Items.Add(ex.Message);
                }));
            }
        }
        #endregion


        #region PLC Events     
        //private void btnConnectPLC_Click(object sender, EventArgs e)
        //{
        //    plc = new PLC(IP_ADDRESS_PLC, PORT_NUMBER_PLC);
        //    Thread thConnect = new Thread(() =>
        //    {
        //        ConnectPLC();
        //    })
        //    { IsBackground = true };
        //    thConnect.Start();
        //}

        //private void btnDisconnectPLC_Click(object sender, EventArgs e)
        //{
        //    Thread thDisconnect = new Thread(() =>
        //    {
        //        DisconnectPLC();
        //    })
        //    { IsBackground = true };
        //    thDisconnect.Start();
        //}
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (plc != null)
                {
                    plc.WriteSingleRegister(1002, 1);
                }
                else
                    MessageBox.Show("PLC not connected");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (plc != null)
                {
                    plc.WriteSingleRegister(1002, 2);
                }
                else
                    MessageBox.Show("PLC not connected");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void btnReadPLC_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (plc != null)
        //        {
        //            txtValueRegister.Text = plc.ReadHoldingRegisters(int.Parse(txtRegister.Text)).ToString();
        //        }
        //        else
        //            MessageBox.Show("PLC not connected");

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void btnWritePLC_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (plc != null)
        //        {
        //            plc.WriteSingleRegister(int.Parse(txtRegister.Text), int.Parse(txtValueRegister.Text));
        //        }
        //        else
        //            MessageBox.Show("PLC not connected");

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        void ConnectPLC()
        {
            try
            {
                checkConnectPLC = plc.Connect();
                if (checkConnectPLC)
                {
                    progressBarPLC.Value = 100;                  

                    listBoxStatus.Invoke(new Action(() =>
                    {
                        listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " PLC: Connected.");
                    }));
                }
            }
            catch (EasyModbus.Exceptions.ConnectionException ex)
            {
                listBoxStatus.Invoke(new Action(() =>
                {
                    listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " PLC: " + ex.Message);
                }));
            }
            catch (Exception ex)
            {
                listBoxStatus.Invoke(new Action(() =>
                {
                    listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " PLC: " + ex.Message);
                }));
            }
        }
        void DisconnectPLC()
        {
            try
            {
                checkConnectPLC = plc.Disconnect();
                if (checkConnectPLC)
                {
                    listBoxStatus.Invoke(new Action(() =>
                    {
                        listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " PLC: Disconnected.");
                        progressBarPLC.Value = 0;
                    }));
                }
            }
            catch (EasyModbus.Exceptions.ConnectionException ex)
            {
                listBoxStatus.Invoke(new Action(() =>
                {
                    listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " PLC: " + ex.Message);
                }));
            }
            catch (Exception ex)
            {
                listBoxStatus.Invoke(new Action(() =>
                {
                    listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " PLC: " + ex.Message);
                }));
            }
        }

        #endregion


        #region DataMan Events
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
                    progressBarDataMan.Value = 100;
                }
            }
            catch (Exception ex)
            {
                CleanupConnection();
                progressBarDataMan.Value = 0;
                AddListItem(DateTime.Now.ToString("HH:mm:ss") + " DataMan: Failed to connect: " + ex.ToString());
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
                {
                    MessageBox.Show("DataMan: Disconected", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    progressBarDataMan.Value = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBoxDetectedSystems_SelectedIndexChanged(object sender, EventArgs e)
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

        #endregion


        #region Scale Events        

        int baudRate;
        string parity;
        uint dataBit;
        string stopBit;
        public void DefaulSetting()
        {
            try
            {
                foreach (var item in SerialPort.GetPortNames())
                {
                    cboComPort.Items.Add(item);
                }
                cboComPort.SelectedIndex = 1;

                int[] baudRateArr = new int[] { 75, 110, 134, 300, 600, 1200, 1800, 2400, 4800, 7200, 9600, 14400, 19200, 38400 };
                foreach (var item in baudRateArr)
                {
                    cboBaudRate.Items.Add(item);
                }
                cboBaudRate.Text = baudRateArr[12].ToString();
                baudRate = baudRateArr[12];

                string[] parityArr = new string[] { "Even", "Odd", "None", "Mark", "Space" };
                //foreach (var item in parityArr)
                //{
                //    cboParityBits.Items.Add(item);
                //}
                //cboParityBits.Text = parityArr[2];
                parity = parityArr[2];

                uint[] dataBitArr = new uint[] { 4, 5, 6, 7, 8, 9 };
                //foreach (var item in dataBitArr)
                //{
                //    cboDataBits.Items.Add(item);
                //}
                //cboDataBits.Text = dataBitArr[4].ToString();
                dataBit = dataBitArr[4];

                string[] stopBitArr = new string[] { "One", "Two" };
                //foreach (var item in stopBitArr)
                //{
                //    cboStopBits.Items.Add(item);
                //}
                //cboStopBits.Text = stopBitArr[0];
                stopBit = stopBitArr[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen == true)
                {
                    serialPort1.Close();
                    btnOpen.Enabled = true;
                    btnClose.Enabled = false;
                }
                else
                {
                    //serialPort1.PortName = cboComPort.Text;
                    //serialPort1.BaudRate = Convert.ToInt32(cboBaudRate.Text);
                    //serialPort1.DataBits = Convert.ToInt32(cboDataBits.Text);
                    //serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cboStopBits.Text);
                    //serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cboParityBits.Text);

                    serialPort1.PortName = cboComPort.Text;
                    serialPort1.BaudRate = Convert.ToInt32(cboBaudRate.Text);
                    serialPort1.DataBits = Convert.ToInt32(dataBit);
                    serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBit);
                    serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), parity);
                    serialPort1.Open();
                    btnOpen.Enabled = false;
                    btnClose.Enabled = true;
                    progressBarScale.Value = 100;
                }

            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    btnOpen.Enabled = true;
                    btnClose.Enabled = false;
                    progressBarScale.Value = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(50);
            try
            {
                int bytes = serialPort1.BytesToRead;
                byte[] buffer = new byte[bytes];
                if (serialPort1.BytesToRead > 1)
                {
                    serialPort1.Read(buffer, 0, bytes);
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        if (i == 11)
                        {
                            byte11 = buffer[i];
                        }
                        if (i == 12)
                        {
                            byte12 = buffer[i];
                        }
                        this.weightValue = ((double)byte11 * 256 + (double)byte12) / 100;
                    }                 
                    this.Invoke(new EventHandler(displayData_event));

                }
            }
            catch (Exception ex)
            {
                listBoxStatus.Items.Add("Scale: " + ex.Message);
            }
        }
        private void displayData_event(object sender, EventArgs e)
        {
            txtWeight.Text = weightValue.ToString();

            if (this.maKH == "NG")
            {
                inBoundsBus.SetTable(DateTime.Now.ToString("M/d/yyyy"), DateTime.Now.ToString("HH:mm:ss"), this.maKH, this.weightValue.ToString(), this.lengthValue, this.widthValue, this.heightValue, "99", string.Empty, revAccountName);
                this.weightValue = 0;
                this.lengthValue = string.Empty;
                this.widthValue = string.Empty;
                this.heightValue = string.Empty;
                this.maKH = string.Empty;
                this.zone = string.Empty;
                this.state = string.Empty;
                RefreshDgvInBoundDate();
            }
            else if (this.maKH == "NE")
            {
                inBoundsBus.SetTable(DateTime.Now.ToString("M/d/yyyy"), DateTime.Now.ToString("HH:mm:ss"), this.maKH, this.weightValue.ToString(), this.lengthValue, this.widthValue, this.heightValue, "99", string.Empty, revAccountName);
                this.weightValue = 0;
                this.lengthValue = string.Empty;
                this.widthValue = string.Empty;
                this.heightValue = string.Empty;
                this.maKH = string.Empty;
                this.zone = string.Empty;
                this.state = string.Empty;
                RefreshDgvInBoundDate();
            }
            else if(this.maKH == string.Empty)
            {
                inBoundsBus.SetTable(DateTime.Now.ToString("M/d/yyyy"), DateTime.Now.ToString("HH:mm:ss"), "NG", this.weightValue.ToString(), this.lengthValue, this.widthValue, this.heightValue, "99", string.Empty, revAccountName);
                this.weightValue = 0;
                this.lengthValue = string.Empty;
                this.widthValue = string.Empty;
                this.heightValue = string.Empty;
                this.maKH = string.Empty;
                this.zone = string.Empty;
                this.state = string.Empty;
                RefreshDgvInBoundDate();
            }
            else
            {
                inBoundsBus.SetTable(DateTime.Now.ToString("M/d/yyyy"), DateTime.Now.ToString("HH:mm:ss"), this.maKH, this.weightValue.ToString(), this.lengthValue, this.widthValue, this.heightValue, this.zone, this.state, revAccountName);
                this.weightValue = 0.0;
                this.lengthValue = string.Empty;
                this.widthValue = string.Empty;
                this.heightValue = string.Empty;
                this.maKH = string.Empty;
                this.zone = string.Empty;
                this.state = string.Empty;
                RefreshDgvInBoundDate();
            }


        }
        #endregion


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
                    //đổ dữ liệu vào dtHasakiSystem
                    if (read_result == "NG")
                    {
                        if (plc != null)                        
                            plc.WriteSingleRegister(1000, 99);
                        this.maKH = read_result;
                        countNG++;
                        count++;

                        lblZone.Text = "99";
                        txtQuantity.Text = count.ToString();
                        txtNG.Text = countNG.ToString() + "/" + count.ToString();
                        listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " NG");

                    } 
                    else if (hasakiSystemBus.CheckCode(read_result) == false)
                    {
                        if (plc != null)                                              
                            plc.WriteSingleRegister(1000, 99);
                        this.maKH = "NE"; //not exist
                        countNG++;
                        count++;

                        lblZone.Text = "99";
                        txtQuantity.Text = count.ToString();
                        txtNG.Text = countNG.ToString() + "/" + count.ToString();
                        listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " Not exist");

                    }
                    else
                    {
                        this.maKH = read_result;
                        dtHasakiSystem = hasakiSystemBus.GetTable(read_result);
                        count++;

                        //Lấy dữ liệu từ dtHasakiSystem
                        if (dtHasakiSystem != null)
                        {
                            hasakiSystemDTO.SetNgay(dtHasakiSystem.Rows[0][0].ToString());
                            hasakiSystemDTO.SetMaKienHang(dtHasakiSystem.Rows[0][1].ToString());
                            hasakiSystemDTO.SetMaZone(dtHasakiSystem.Rows[0][2].ToString());
                            hasakiSystemDTO.SetTrangThai(dtHasakiSystem.Rows[0][3].ToString());
                        }
                        this.zone = hasakiSystemDTO.GetMaZone();
                        this.state = hasakiSystemDTO.GetTrangThai();

                        //gửi Zone xuống PLC
                        if (plc != null)
                        {
                            switch (zone)
                            {
                                case "1":
                                    plc.WriteSingleRegister(1000, 1);
                                    break;
                                case "2":
                                    plc.WriteSingleRegister(1000, 2);
                                    break;
                                case "3":
                                    plc.WriteSingleRegister(1000, 3);
                                    break;
                                case "4":
                                    plc.WriteSingleRegister(1000, 4);
                                    break;
                                case "5":
                                    plc.WriteSingleRegister(1000, 5);
                                    break;
                                case "6":
                                    plc.WriteSingleRegister(1000, 6);
                                    break;
                                case "7":
                                    plc.WriteSingleRegister(1000, 7);
                                    break;
                                case "8":
                                    plc.WriteSingleRegister(1000, 8);
                                    break;
                                case "9":
                                    plc.WriteSingleRegister(1000, 9);
                                    break;
                                case "10":
                                    plc.WriteSingleRegister(1000, 10);
                                    break;
                            }
                        }
                        else
                            listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " PLC chưa được kết nối");

                        lblZone.Text = this.zone;
                        txtQuantity.Text = count.ToString();
                        txtNG.Text = countNG.ToString() + "/" + count.ToString();
                    }
                   
                }
                catch (Exception ex)
                {
                    listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);
                }
                finally
                {

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


        private void btnSearch_Click(object sender, EventArgs e)
        {
            dtInBound = new DataTable();
            dtInBound = inBoundsBus.Search(dateTimePicker1.Text);
            dgvInBound.DataSource = dtInBound;
            dgvInBound.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInBound.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);
        }



        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void btnExportToExcell_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ToExcel(dgvInBound, saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        OpenFileDialog dlg = null;
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application application;
                Microsoft.Office.Interop.Excel.Workbook workbook;
                Microsoft.Office.Interop.Excel.Worksheet worksheet;
                Microsoft.Office.Interop.Excel.Range range;

                int row;
                string strFileName;
                dlg = new OpenFileDialog();

                dlg.Filter = "Excel Office |*.xls; *xlsx";
                dlg.ShowDialog();
                strFileName = dlg.FileName;

                if (strFileName != "")
                {
                    application = new Microsoft.Office.Interop.Excel.Application();
                    workbook = application.Workbooks.Open(strFileName);
                    worksheet = workbook.Worksheets["Sheet1"];
                    range = worksheet.UsedRange;

                    for (row = 2; row <= range.Rows.Count; row++)
                    {
                        if (range.Cells[row, 1].Text != "")
                        {
                            hasakiSystemBus.SetTable(range.Cells[row, 1].Text, range.Cells[row, 2].Text, range.Cells[row, 3].Text, range.Cells[row, 4].Text);
                        }
                    }
                    MessageBox.Show("Đã thêm dữ liệu vào bảng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    workbook.Close();
                    application.Quit();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefreshDgv_Click(object sender, EventArgs e)
        {
            RefreshDgvInBound();
        }


        #region function helper
        void RefreshDgvInBound()
        {
            dtInBound = new DataTable();
            dtInBound = inBoundsBus.GetTable();
            dgvInBound.DataSource = dtInBound;
            dgvInBound.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInBound.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);

        }

        void RefreshDgvInBoundDate()
        {
            dtInBound = new DataTable();
            dtInBound = inBoundsBus.GetTableDate(DateTime.Now.ToString("M/d/yyyy"));
            dgvInBound.DataSource = dtInBound;
            dgvInBound.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInBound.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);
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
                MessageBox.Show("Đã xuất dữ liệu ra file Excel.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        #endregion


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (checkConnectPLC == true)
                {                                 
                    readConveyorState = plc.ReadHoldingRegisters(1004);
                    if(readConveyorState == 1)
                    {                       
                        btnStart.BackColor = Color.Lime;
                        btnStop.BackColor = Color.LightGray;
                        lblConveyorStatus.ForeColor = Color.LimeGreen;
                        lblConveyorStatus.Text = "Conveyor is running";
                    }
                    if (readConveyorState == 0)
                    {
                        btnStart.BackColor = Color.LightGray;
                        btnStop.BackColor = Color.Orange;
                        lblConveyorStatus.ForeColor = Color.Orange;
                        lblConveyorStatus.Text = "Conveyor is stop";
                    }
                    if (readConveyorState == 2)
                    {
                        btnStart.BackColor = Color.LightGray;
                        btnStop.BackColor = Color.LightGray;
                        lblConveyorStatus.ForeColor = Color.Gray;
                        lblConveyorStatus.Text = "Conveyor is Pause";
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxStatus.Items.Add("PLC: " + ex.Message);
                progressBarPLC.Value = 0;
                timer1.Stop();
            }
        }

        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            lblDate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblTime1.Text = DateTime.Now.ToString("HH:mm:ss");
        }


        //Reconnect PLC and Dim
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (client != null && client.Connected == true)
                client.Close();
            if (plc != null)
                DisconnectPLC();

            client = null;
            plc = null;

            try
            {
                listBoxStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " Dim: Connecting...");
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP_ADDRESS_DIM), PORT_NUMBER_DIM);
                client.BeginConnect(iep, new AsyncCallback(Connected), client);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            plc = new PLC(IP_ADDRESS_PLC, PORT_NUMBER_PLC);
            Thread thConnect = new Thread(() =>
            {
                ConnectPLC();
            })
            { IsBackground = true };
            thConnect.Start();
            timer1.Start();
        }

        private void picBoxResetQuantity_Click(object sender, EventArgs e)
        {
            count = 0;
            countNG = 0;
            txtQuantity.Text = count.ToString();
            txtNG.Text = countNG.ToString() + "/" + count.ToString();
        }

    }
}
