using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace sample_CSharp2008
{
    public partial class FormGpsEventPatrol : Form
    {
        private EventPatrol mDevice = new EventPatrol();
        private DFile mDFile = new DFile();
        private Fingerprint mFingerprint = new Fingerprint();
        private Patrol.DEVICETYPE mDeviceType;
        static private Dictionary<UInt32, string> DeviceList = new Dictionary<UInt32, string>();

        public FormGpsEventPatrol(Patrol.DEVICETYPE DeviceType)
        {
            InitializeComponent();
            mDeviceType = DeviceType;
            UInt32 n = mDevice.EnumDeviceByDeviceType_Sample(mDeviceType);
            if (n != 0)
            {
                for (UInt32 i = 0; i < n; i++)
                {
                    string path = mDevice.GetDevicePath_Sample(i);
                    UInt32 id = mDevice.GetDeviceID_Sample(path);
                    DeviceList[id] = path;
                    comboBox1.Items.Add(id.ToString("X8"));                    
                }
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = -1;
            }
        }

        private void BtnRead_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                UInt32 id = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
                string path = DeviceList[id];
                label1.Text = "Device Path:" + path;

                //UInt32 id = mDevice.GetDeviceID(path);
                label2.Text = "Device ID:" + id.ToString();

                Patrol.DEVICETYPE type;
                label3.Text = "Device type:";
                if (mDevice.GetDeviceType_Sample(path,out type))
                    label3.Text += type.ToString();

                DateTime dt;
                label4.Text = "Device time:";
                if (mDevice.GetDeviceTime_Sample(path,out dt))
                    label4.Text += dt.ToString();

                string sv = mDevice.GetVersionString_Sample(path);
                label5.Text = "Version:" + sv;

                label6.Text = "Capacity:" + mDevice.GetRecordCapacity_Sample(path).ToString();

                UInt32 cc = mDevice.GetCrashRecordCount_Sample(path);
                label7.Text = "Crash count:" + cc.ToString();

                UInt32 rc = mDevice.GetRecordCount_Sample(path);
                label8.Text = "Record count:" + rc.ToString();

                int ReadPos = 0;
                dataGridView1.RowCount = 1;
                while (cc > ReadPos)
                {
                    UInt32 ReadNum;
                    DateTime[] crecord = mDevice.GetCrashRecord_Sample(path, (UInt32)ReadPos, cc - (UInt32)ReadPos, out ReadNum);
                    if (crecord != null)
                    {
                        dataGridView1.RowCount += (int)ReadNum;
                        for (int i = 0; i < ReadNum; i++)
                        {
                            dataGridView1.Rows[(int)ReadPos + i].Cells[0].Value = crecord[i].ToString();
                        }
                        ReadPos += (int)ReadNum;
                    }
                }

                ReadPos = 0;
                dataGridView2.RowCount = 1;
                while (rc > ReadPos)
                {   
                    EventPatrol.GPSRECORD[] records;
                    if (mDevice.GetGpsEventRecord_Sample(path, (UInt32)ReadPos, out records))
                    {
                        dataGridView2.RowCount += (int)records.Length;
                        for (int i = 0; i < records.Length; i++)
                        {
                            dataGridView2.Rows[(int)ReadPos + i].Cells[0].Value = records[i].SpotCardNum.ToString("X8");
                            dataGridView2.Rows[(int)ReadPos + i].Cells[1].Value = records[i].time.ToString();
                            if (records[i].EventList[0] != 0xFFFF)
                                dataGridView2.Rows[(int)ReadPos + i].Cells[2].Value = records[i].EventList[0].ToString();
                            if (records[i].EventList[1] != 0xFFFF)
                                dataGridView2.Rows[(int)ReadPos + i].Cells[3].Value = records[i].EventList[1].ToString();
                            if (records[i].EventList[2] != 0xFFFF)
                                dataGridView2.Rows[(int)ReadPos + i].Cells[4].Value = records[i].EventList[2].ToString();
                            double longitude = (double)records[i].longitude / 1000000.0;
                            double latitude = (double)records[i].latitude / 1000000.0;
                            if (longitude > -180 && longitude <= 180)
                            {
                                if (longitude >= 0)
                                    dataGridView2.Rows[(int)ReadPos + i].Cells[5].Value = "E " + longitude.ToString();
                                else
                                    dataGridView2.Rows[(int)ReadPos + i].Cells[5].Value = "W " + longitude.ToString();
                            }
                            else
                            {
                                dataGridView2.Rows[(int)ReadPos + i].Cells[5].Value = "invalid";
                            }

                            if (latitude >= -90 && latitude <= 90)
                            {
                                if (latitude >= 0)
                                    dataGridView2.Rows[(int)ReadPos + i].Cells[6].Value = "N " + latitude.ToString();
                                else
                                    dataGridView2.Rows[(int)ReadPos + i].Cells[6].Value = "S" + latitude.ToString();
                            }
                            else
                            {
                                dataGridView2.Rows[(int)ReadPos + i].Cells[6].Value = "invalid";
                            }
                        }
                        ReadPos += records.Length;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UInt32 LastSelectedID;
            if (comboBox1.SelectedIndex != -1)
            {
                LastSelectedID = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
                comboBox1.Items.Clear();
            }
            else
            {
                LastSelectedID = 0xFFFFFFFF;
            }
            UInt32 n = mDevice.EnumDeviceByDeviceType_Sample(mDeviceType);
            if (n != 0)
            {
                for (UInt32 i = 0; i < n; i++)
                {
                    string path = mDevice.GetDevicePath_Sample(i);
                    UInt32 id = mDevice.GetDeviceID_Sample(path);
                    DeviceList[id] = path;
                    comboBox1.Items.Add(id.ToString("X8"));
                    if (LastSelectedID == id)
                        comboBox1.SelectedIndex = (int)i;
                }
                if (comboBox1.SelectedIndex == -1)
                    comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = -1;
            }
        }

        private void FormGpsEventPatrol_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                UInt32 id = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
                string path = DeviceList[id];
                if (mDevice.SetDeviceTime_Sample(path))
                    MessageBox.Show("successed");
                else
                    MessageBox.Show("failed");
            }
        }

        

        

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                UInt32 id = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
                string path = DeviceList[id];

                EventPatrol.INSPECTOR_INFO info = new EventPatrol.INSPECTOR_INFO();
                info.AuthenticationType = 2;
                info.CardNumber = 0x76543210;

                info.FingerNo = new UInt16[2];
                info.FingerNo[0] = 1;
                info.FingerNo[1] = 3;
                   
                info.name = "Name of inspector";
                if (mDevice.SetInspectorInfo_Sample(path, 0, info))
                {
                    mDevice.SetCurrentInspector_Sample(path, 0);
                    MessageBox.Show("successed");
                }
                else
                    MessageBox.Show("failed");
            }   
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                UInt32 id = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
                string path = DeviceList[id];

                if (mDeviceType == Patrol.DEVICETYPE.DT_Z6700)
                {
                    MessageBox.Show("Unsupported");
                    return;
                }

                EventPatrol.SPOT_INFO info = new EventPatrol.SPOT_INFO();
                info.CardNumber = 0x00205f88;
                info.NeedCheckFinger = true;
                info.SpotName = "office";
                info.EventList = new UInt16[2];
                info.EventList[0] = 3;
                info.EventList[1] = 5;

                if (mDevice.SetSpotInfo_Sample(path, 0, info))
                {
                    MessageBox.Show("successed");
                }
                else
                    MessageBox.Show("failed");
            }   
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                UInt32 id = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
                string path = DeviceList[id];

                if (mDeviceType == Patrol.DEVICETYPE.DT_Z6700)
                {
                    MessageBox.Show("Unsupported");
                    return;
                }

                if (mDevice.SetLineName_Sample(path,0,"this is name of line"))
                {
                    MessageBox.Show("successed");
                }
                else
                    MessageBox.Show("failed");
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                UInt32 id = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
                string path = DeviceList[id];

                Byte[] buffer = new Byte[64];
                UInt32 OutSize;
                //mFingerprint.ClearFingerprintPattern(path);
                if (mFingerprint.GetFingerprintStatus_Sample(path, buffer,(UInt32)64, out OutSize))
                {
                    int MaxCount = (int)OutSize*8;
                    for (int i = 0; i < MaxCount; i++)
                    {
                        if ((buffer[i / 8] & (1 << (i % 8))) != 0)
                        {
                            byte[] pattern;
                            if (mFingerprint.GetFingerprintPattern_Sample(path, (UInt32)i, out pattern))
                            {
                                MessageBox.Show("Readed one pattern");
                                return;
                            }
                        }
                    }
                }
                MessageBox.Show("failed");
            }
        }
    }
}
