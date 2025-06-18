using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Patrol;

namespace sample_CSharp2008
{
    public partial class EventPatrolForm : Form
    {
        public EventPatrolForm()
        {
            InitializeComponent();
        }
        string devPath = "";
        EventPatrol dev = new EventPatrol();

        

        private void button1_Click(object sender, EventArgs e)
        {
            
            UInt32 n = dev.Event_EnumEventPatrolDevice();
            if (n > 0)
            {   
                
                devPath = dev.Patrol_GetDevicePath(0);
                listBox1.Items.Add(devPath);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            uint devID=dev.Patrol_GetDeviceID(devPath);
            listBox1.Items.Add(devID);
        }

        private void button4_Click(object sender, EventArgs e)

        {
            DateTime dt;
            bool b=dev.Patrol_GetDeviceTime(devPath, out dt);
            listBox1.Items.Add(dt);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //set device clock to your pc time
            bool b=dev.Patrol_SetDeviceTime(devPath);
            if (b)
            {
                listBox1.Items.Add("Done");
            }
            else
            {
                listBox1.Items.Add("Fail");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CNoDriverStick dev1 = new CNoDriverStick();
            UInt32 n = dev1.EnumNoDriverStickDevice();
            if (n > 0)
            {
                string DevPath = dev1.Patrol_GetDevicePath(0);

                uint RecordCount = dev1.Patrol_GetRecordCount(DevPath);

                if (RecordCount > 0)
                {

                    Patrol.DEVICETYPE DeviceType;
                    dev1.Patrol_GetDeviceType(DevPath, out DeviceType);
                    listBox1.Items.Add("deviceType:" + DeviceType.ToString("X"));
                    UInt32 NumberRead;
                    CNoDriverStick.StickRecord[] records = dev1.GetStickRecord(DevPath, 0, RecordCount, out NumberRead);
                    for (int i = 0; i < records.Length; i++)
                {
                    listBox1.Items.Add("spot:" + records[i].CardID.ToString("X") + " time:" + records[i].dt);
                }
                }
            }
            //    EventPatrol dev = new EventPatrol();
            //UInt32 n = dev.Event_EnumEventPatrolDevice();
            //if (n > 0)
            //{
            //    string DevPath = dev.Patrol_GetDevicePath(0);
            //    //get crash data count
            //    uint crashCount = dev.GetCrashRecordCount(devPath);
            //    uint readedNum;
            //    uint startIndex = 0;
            //    //get crash data
            //    DateTime[] crashDT = dev.Patrol_GetCrashRecord(devPath, startIndex, crashCount, out readedNum);
            //    if (crashDT != null)
            //    {
            //        for (int i = 0; i < readedNum; i++)
            //        {
            //            listBox1.Items.Add("crashTime:"+crashDT[i].ToString());
            //        }
            //        startIndex += readedNum;
            //    }

            //    //get data count
            //    uint RecordCount = dev.Patrol_GetRecordCount(DevPath);

            //    //get data
            //    EventPatrol.RECORD[] records;
            //    dev.Event_GetEventRecord(DevPath, 1, out records);
            //    for (int i = 0; i < records.Length; i++)
            //    {
            //        listBox1.Items.Add("spot:" + records[i].SpotCardNum.ToString("X") + " time:" + records[i].time);
            //    }

            //}
        }

        private void button6_Click(object sender, EventArgs e)
        {

                NETCONFIG2 netconfiginfo = new NETCONFIG2();
                Patrol p = new Patrol();
                if (p.Patrol_GetNetConfig2(devPath, out netconfiginfo))
                {
                listBox1.Items.Add("url:"+ netconfiginfo.url);
                listBox1.Items.Add("port:" + netconfiginfo.port);
                listBox1.Items.Add("accessPoint:" + netconfiginfo.AccessPoint);
                listBox1.Items.Add("userName:" + netconfiginfo.UserName);
                listBox1.Items.Add("password:" + netconfiginfo.password);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            EventPatrol.SPOT_INFO tag = new EventPatrol.SPOT_INFO();

            tag.CardNumber = System.Convert.ToUInt32(textBox1.Text,16);
            tag.SpotName = textBox2.Text;
            //GS6100S set eventList to zero
            ushort[] eventList = new ushort[0];
            tag.EventList = eventList;
            bool b=dev.Event_SetSpotInfo(devPath, 0, tag);
            if (b)
            {
                listBox1.Items.Add("Done");
            }
            else
            {
                listBox1.Items.Add("Fail");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string ServerIP = txtIP.Text;
            ushort port = ushort.Parse(txtPort.Text);
            string accessPoint = txtAP.Text;
            string username = txtUser.Text;
            string userpass = txtPSWD.Text;

            bool b=dev.Patrol_SetNetConfig2(devPath, ServerIP, port, accessPoint, username, userpass, "", "", "", "", "", "");
            if (b)
            {
                listBox1.Items.Add("Done");
            }
            else
            {
                listBox1.Items.Add("Fail");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //data
            bool b = dev.Patrol_ClearRecord(devPath);
            if (b)
            {
                listBox1.Items.Add("Done");
            }
            else
            {
                listBox1.Items.Add("Fail");
            }
            //crash data
            b = dev.Patrol_ClearCrashRecord(devPath);
            if (b)
            {
                listBox1.Items.Add("Done");
            }
            else
            {
                listBox1.Items.Add("Fail");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CNoDriverStick dev1 = new CNoDriverStick();
            UInt32 n = dev1.EnumNoDriverStickDevice();

            if (n > 0)
            {
                string DevPath = dev1.Patrol_GetDevicePath(0);
                uint RecordCount = dev1.Patrol_GetRecordCount(DevPath);

                if (RecordCount > 0)
                {
                    // Setup folder path
                    string logPath = @"C:\Patrol_Log\Logs";

                    if (!System.IO.Directory.Exists(logPath))
                    {
                        System.IO.Directory.CreateDirectory(logPath);
                    }
                    else
                    {
                        // Delete all existing files in the folder
                        string[] existingFiles = System.IO.Directory.GetFiles(logPath);
                        foreach (string file in existingFiles)
                        {
                            try
                            {
                                System.IO.File.Delete(file);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Failed to delete existing file:\n{file}\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                    // File name
                    string filePath = System.IO.Path.Combine(logPath, $"patrol_log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");

                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, true))
                    {
                        UInt32 NumberRead;
                        CNoDriverStick.StickRecord[] records = dev1.GetStickRecord(DevPath, 0, RecordCount, out NumberRead);
                        uint devID = dev.Patrol_GetDeviceID(devPath);

                        for (int i = 0; i < records.Length; i++)
                        {
                            string spotHex = records[i].CardID.ToString().PadLeft(10,'0');
                            string datePart = records[i].dt.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                            string timePart = records[i].dt.ToString("HH:mm:ss");

                            string logLine = $"1,1,{spotHex},{datePart},{timePart},{devID}";
                            writer.WriteLine(logLine);
                        }
                    }

                    MessageBox.Show("Formatted records saved to file:\n" + filePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No records found on device.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No device detected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
