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
    public partial class FormEventPatrol : Form
    {
        private EventPatrol mDevice = new EventPatrol();
        private DFile mDFile = new DFile();
        private Fingerprint mFingerprint = new Fingerprint();
        private Patrol.DEVICETYPE mDeviceType;
        static private Dictionary<UInt32, string> DeviceList = new Dictionary<UInt32, string>();

        public FormEventPatrol(Patrol.DEVICETYPE DeviceType)
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
                    EventPatrol.RECORD[] records;
                    if (mDevice.GetEventRecord_Sample(path, (UInt32)ReadPos, out records))
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
            if (comboBox1.SelectedIndex != -1)
            {
                UInt32 LastSelectedID = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
                comboBox1.Items.Clear();
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
        }

        private void FormEventPatrol_FormClosed(object sender, FormClosedEventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                UInt32 id = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
                string path = DeviceList[id];

                UInt32 FileLength;
                UInt32 ReadNumber;

                mDFile.GetDFileInfo_Sample(path, "0:/linknull/index.lst", out FileLength);
                if (FileLength > 0)
                {   
                    Byte[] data;
                    if (mDFile.ReadDFile_Sample(path, out data, FileLength, out ReadNumber, 5000))
                    {
                        UInt32 FileNumber = ReadNumber / (DFile.FILE_NAME_LENGTH1 + DFile.SEPARATOR_LENGTH);
                        Byte[] FileNameBytes = new Byte[DFile.FILE_NAME_LENGTH1];
                        for (int i = 0;i < FileNumber;i++)
                        {
                            for (int j = 0; j < DFile.FILE_NAME_LENGTH1; j++)
                            {
                                FileNameBytes[j] = data[(DFile.FILE_NAME_LENGTH1 + DFile.SEPARATOR_LENGTH) * i + j];
                            }
                            String FileName = System.Text.Encoding.ASCII.GetString(FileNameBytes);
                            treeView1.Nodes[0].Nodes.Add(FileName + ".bmp");
                        }
                    }                    
                }
                                
                mDFile.GetDFileInfo_Sample(path, "0:/index.lst", out FileLength);
                if (FileLength > 0)
                {
                    Byte[] SpotListBuffer = new Byte[FileLength];
                    if (mDFile.ReadDFile_Sample(path, out SpotListBuffer, FileLength, out ReadNumber, 3000))
                    {
                        int SpotNumber = (int)ReadNumber / (DFile.CARD_NUMBER_LENGHT + DFile.SEPARATOR_LENGTH);
                        Byte[]SpotCardIDBytes = new Byte[DFile.CARD_NUMBER_LENGHT];
                        for (int i = 0;i < SpotNumber;i++)
                        {
                            for (int j = 0;j < DFile.CARD_NUMBER_LENGHT;j++)
                                SpotCardIDBytes[j] = SpotListBuffer[(DFile.CARD_NUMBER_LENGHT + DFile.SEPARATOR_LENGTH) * i + j];
                            String SpotName = System.Text.Encoding.ASCII.GetString(SpotCardIDBytes);
                            treeView1.Nodes[1].Nodes.Add(SpotName);
                            mDFile.GetDFileInfo_Sample(path, "0:/" + SpotName + "/index.lst", out FileLength);
                            if (FileLength > 0)
                            {
                                Byte[] FileListBuffer = new Byte[FileLength];
                                if (mDFile.ReadDFile_Sample(path, out FileListBuffer, FileLength, out ReadNumber, 5000))
                                {
                                    int FileNumber = (int)ReadNumber / (DFile.FILE_NAME_LENGTH2 + DFile.SEPARATOR_LENGTH);
                                    Byte[] FileNameBytes = new Byte[DFile.FILE_NAME_LENGTH2];
                                    for (int k = 0; k < FileNumber; k++)
                                    {
                                        for (int m = 0; m < DFile.FILE_NAME_LENGTH2;m++ )
                                            FileNameBytes[m] = FileListBuffer[(DFile.FILE_NAME_LENGTH2 + DFile.SEPARATOR_LENGTH) * k + m];
                                        String FileName = System.Text.Encoding.ASCII.GetString(FileNameBytes);
                                        treeView1.Nodes[1].Nodes[i].Nodes.Add(FileName + ".bmp");
                                    }
                                }
                            }
                        }
                    }
                    treeView1.Nodes[1].Expand();
                }
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                UInt32 DeviceID = Convert.ToUInt32(comboBox1.Items[comboBox1.SelectedIndex].ToString(), 16);
                String path = DeviceList[DeviceID];

                TreeNode tn = treeView1.SelectedNode;
                if (tn != null)
                {
                    if (tn.Text.Contains(".bmp") || tn.Text.Contains(".jpg"))
                    {
                        UInt32 length;
                        UInt32 ReadLen;

                        String FilePath = null;
                        if (tn.Parent.Equals(treeView1.Nodes[0]))
                        {
                            FilePath = "0:/linknull/";
                        }
                        else if (tn.Parent.Parent != null)
                        {
                            if (tn.Parent.Parent.Equals(treeView1.Nodes[1]))
                                FilePath = "0:/" + tn.Parent.Text + '/';
                        }

                        if (mDFile.GetDFileInfo_Sample(path, FilePath + tn.Text + '\0', out length))
                        {
                            Byte[] buffer;
                            if (mDFile.ReadDFile_Sample(path, out buffer, length, out ReadLen, 3000))
                            {
                                if (pictureBox1.Image != null)
                                    pictureBox1.Image.Dispose();

                                Stream ms = new MemoryStream(buffer);
                                Image im = Image.FromStream(ms);
                                pictureBox1.Image = im;
                            }
                        }
                    }
                }
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
