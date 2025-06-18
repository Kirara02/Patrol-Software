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
    public partial class FormCOMStick : Form
    {
        private CNoDriverStick mDevice = new CNoDriverStick();
        static private Dictionary<UInt32, string> DeviceList = new Dictionary<UInt32, string>();

        public FormCOMStick()
        {
            InitializeComponent();
            UInt32 n = mDevice.EnumDeviceByDeviceType_Sample(Patrol.DEVICETYPE.DT_Z6200F);
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

                UInt32 rc = mDevice.GetRecordCount_Sample(path);
                label8.Text = "Record count:" + rc.ToString();
                if (rc != 0)
                {
                    UInt32 ReadPoint = 0;
                    while (ReadPoint < rc)
                    {
                        UInt32 ReadNum;
                        CNoDriverStick.StickRecord[] record = mDevice.GetStickRecord_Sample(path, ReadPoint, 40, out ReadNum);
                        if (record != null)
                        {
                            //dataGridView2.RowCount = (int)(ReadPoint + ReadNum);
                            for (int i = 0; i < ReadNum; i++)
                            {
                                int index=this.dataGridView2.Rows.Add();
                                this.dataGridView2.Rows[index].Cells[0].Value = record[i].CardID.ToString("X8");
                                this.dataGridView2.Rows[index].Cells[1].Value = record[i].dt.ToString();
                            }
                        }
                        ReadPoint += ReadNum;
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
                LastSelectedID = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
            else
                LastSelectedID = Patrol.INVALID_DEVICE_ID;
            comboBox1.Items.Clear();
            UInt32 n = mDevice.EnumDeviceByDeviceType_Sample(Patrol.DEVICETYPE.DT_Z6200F);
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
