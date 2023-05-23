using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyModbus;

namespace Intech_software
{
    class PLC
    {
        private string ipAddress;
        private int port;
        ModbusClient modbus;
        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public PLC()
        {
            this.ipAddress = "127.0.0.1";
            this.port = 0;
        }

        public PLC(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }
        public bool Connect()
        {
            modbus = new ModbusClient();
            modbus.IPAddress = this.ipAddress;
            modbus.Port = this.port;
            try
            {
                modbus.Connect();
                if (modbus.Connected == true)
                    return true;
                else
                    return false;
            }
            catch (EasyModbus.Exceptions.ConnectionException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public bool Disconnect()
        {
            try
            {
                modbus.Disconnect();
                if (modbus.Connected == true)
                    return true;
                else
                    return false;
            }
            catch (EasyModbus.Exceptions.ConnectionException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ReadHoldingRegisters(int startAdds)
        {
         
            try
            {
                if (modbus.Connected)
                {
                    return modbus.ReadHoldingRegisters(startAdds, 1)[0];
                }
            }           
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }

        public bool ReadDiscreteInputs(int startAdds)
        {
            try
            {
                if (modbus.Connected)
                {
                    return modbus.ReadDiscreteInputs(startAdds, 1)[0];
                }
                else
                    return false;
            }
            catch (EasyModbus.Exceptions.ConnectionException ex)
            {
                throw ex;
            }
            catch (EasyModbus.Exceptions.CRCCheckFailedException ex)
            {
                throw ex;
            }
            catch (EasyModbus.Exceptions.FunctionCodeNotSupportedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteSingleCoid(int startAdds, bool values)
        {
            try
            {
                if (modbus.Connected)
                {
                    modbus.WriteSingleCoil(startAdds, values);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void WriteSingleRegister(int startAdds, int value)
        {
            try
            {
                if (modbus.Connected)
                {
                    modbus.WriteSingleRegister(startAdds, value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}