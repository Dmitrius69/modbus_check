using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Modbus;
using Modbus.Device;
using Modbus.IO;
using System.Threading;
using System.Windows.Forms;

namespace modbus_check
{
    public partial class mainForm : Form
    {
       SerialPort portRS485;
       ModbusSerialMaster serialTransportRS485;
       ushort STATUSW = 1148;
       
       private void InitSystem()
        {
            //
            portRS485 = new SerialPort();
            
        }
        public mainForm()
        {
            InitializeComponent();
            label1.Text = "00.0";
            label2.Text = "00.0";
            label3.Text = "00.0";
            label4.Text = "00.0";
            label5.Text = "00.0";
            label6.Text = "00.0";
            label7.Text = "00.0";
            label8.Text = "00.0";
            InitSystem();

        }

        private void openRS485(object sender, MouseEventArgs e)
        {
            MessageBox.Show(listPorts.Text+" "+comboBox2.Text+" "+comboBox4.Text+" "+comboBox1.Text+" "+comboBox3.Text);
            try 
            {
                portRS485.PortName = listPorts.Text;
                portRS485.BaudRate = Convert.ToInt32(comboBox2.Text);
                portRS485.DataBits = Convert.ToInt32(comboBox4.Text);
                switch (comboBox1.Text)
                {
                    case "None":
                        portRS485.Parity = Parity.None;
                        break;
                    case "Odd":
                        portRS485.Parity = Parity.Odd;
                        break;
                    case "Even":
                        portRS485.Parity = Parity.Even;
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
                portRS485.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                if (portRS485.IsOpen)
                { 
                    serialTransportRS485 = ModbusSerialMaster.CreateRtu(portRS485);
                }
                else
                { MessageBox.Show("порт " + listPorts.Text+" закрыт, проверьте правильность настроек!" ); }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            try {
                //MessageBox.Show(String.Format("{0}", e.NewValue));
                serialTransportRS485.WriteSingleRegister(1, 49999, STATUSW);
                Thread.Sleep(100);
                serialTransportRS485.WriteSingleRegister(1, 50009, Convert.ToUInt16(e.NewValue));
                Thread.Sleep(100);
                //MessageBox.Show("All write!");
            }
           catch (Exception ex)
            {
                String sMessage = String.Format("{0} {1}", DateTime.Now.ToString("hh:mm:ss "),ex.Message);
                logBox.Items.Add(sMessage);
            }
        }

        private void TEST_Click(object sender, EventArgs e)
        {
            try
            {
                serialTransportRS485.WriteSingleCoil(1, 4, true);
                Thread.Sleep(100);
                serialTransportRS485.WriteSingleCoil(1, 6, true);
                Thread.Sleep(100);
                serialTransportRS485.WriteSingleRegister(1, 49999, STATUSW);
                Thread.Sleep(100);
                serialTransportRS485.WriteSingleRegister(1, 50009, 8192);
                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                String sMessage = String.Format("{0} {1}", DateTime.Now.ToString("hh:mm:ss "), ex.Message);
                logBox.Items.Add(sMessage);
            }
        }
    }
}
