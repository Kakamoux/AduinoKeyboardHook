using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;


namespace KeyboardHook
{
    class Program
    {
        private SerialPort serialPort;
        globalKeyboardHook gkh = new globalKeyboardHook();

        string portName;
   
        static void Main(string[] args)
        {
            new Program().hook();
            Console.Read();
        }

        private string AutodetectArduinoPort()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();

                    if (desc.Contains("Arduino"))
                    {
                        return deviceId;
                    }
                }
            }
            catch (ManagementException e)
            {
                /* Do Nothing */
            }

            return null;
        }

        void hook()
        {
            serialConnect();
            gkh.HookedKeys.Add(Keys.A);
            gkh.HookedKeys.Add(Keys.B);
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
            
         

        }

        void initHookKeys()
        {
            gkh.HookedKeys.Add(Keys.A);
            gkh.HookedKeys.Add(Keys.B);
        }
        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Up\t" + e.KeyCode.ToString());
           
        }
        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Down\t" + e.KeyCode.ToString());
         
          
        }

        private void HookManager_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            Console.WriteLine(e.KeyChar);
        }

        void serialConnect()
        {
            try
            {
                Console.WriteLine();
                serialPort = new SerialPort();
                serialPort.BaudRate = 9600;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;
                serialPort.Parity = Parity.None;
                serialPort.PortName = AutodetectArduinoPort();
                serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                serialPort.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
           
        }


    }
}
