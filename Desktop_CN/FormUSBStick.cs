using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace sample_CSharp2008
{
    public partial class FormUSBStick : Form
    {
        private CNoDriverStick mDevice = new CNoDriverStick();
        static private Dictionary<UInt32, string> DeviceList = new Dictionary<UInt32, string>();

        public FormUSBStick()
        {
            InitializeComponent();
            UInt32 n = mDevice.EnumNoDriverStickDevice_Sample();
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
                label1.Text = "Path:" + path;

                //UInt32 id = mDevice.GetDeviceID(path);
                label2.Text = "DeviceID:" + id.ToString();

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

                if (cc != 0)
                {
                    UInt32 ReadNum;
                    DateTime[] crecord = mDevice.GetCrashRecord_Sample(path, 0, 40, out ReadNum);
                    if (crecord != null)
                    {
                        dataGridView1.RowCount = (int)ReadNum;
                        for (int i = 0; i < ReadNum; i++)
                        {
                            dataGridView1.Rows[i].Cells[0].Value = crecord[i].ToString();
                        }
                    }
                }

                if (rc != 0)
                {
                    UInt32 ReadNum;
                    CNoDriverStick.StickRecord[] record = mDevice.GetStickRecord_Sample(path, 0, 40, out ReadNum);
                    if (record != null)
                    {
                        dataGridView2.RowCount = (int)ReadNum;
                        for (int i = 0; i < ReadNum; i++)
                        {
                            dataGridView2.Rows[i].Cells[0].Value = record[i].CardID.ToString("X8");
                            dataGridView2.Rows[i].Cells[1].Value = record[i].dt.ToString();
                        }
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
            if (comboBox1.SelectedIndex != -1)
            {
                UInt32 LastSelectedID = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
                comboBox1.Items.Clear();
                UInt32 n = mDevice.EnumNoDriverStickDevice_Sample();
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
        }

        private void FormUSBStick_FormClosed(object sender, FormClosedEventArgs e)
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
    }
}
