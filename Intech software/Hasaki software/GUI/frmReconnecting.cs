using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Threading;
using Cognex.DataMan.SDK;

namespace Intech_software.GUI
{
    public partial class frmReconnecting : Form
    {
        private Form _parent = null;
        private DataManSystem _system = null;
        private SynchronizationContext _syncContext = null;
        private Thread _thread = null;
        private bool _cancel = false;
        public frmReconnecting()
        {
            InitializeComponent();
        }     
        public frmReconnecting(Form parent, DataManSystem system)
        {
            _parent = parent;
            _system = system;
            _syncContext = WindowsFormsSynchronizationContext.Current;

            InitializeComponent();
        }

        private void frmReconnecting_Load(object sender, EventArgs e)
        {
            _thread = new Thread(ReconnectThread);
            _thread.Name = "frmReconnecting.ReconnectThread";
            _thread.Start();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            _cancel = true;
        }

        private void ReconnectThread()
        {
            while (!_cancel)
            {
                try
                {
                    _system.Connect();
                }
                catch
                {
                    Thread.Sleep(500);
                    continue;
                }

                _syncContext.Post(
                    delegate
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    },
                    null);

                break;
            }
        }
    }
}
