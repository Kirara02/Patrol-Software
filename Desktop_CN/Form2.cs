using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace sample_CSharp2008
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CItemPatrol dev = new CItemPatrol();
            Fingerprint fp = new Fingerprint();
            UInt32 n = dev.ItemPatrol_EnumItemPatrolDevice();
            if (n > 0)
            {
                string DevPath = dev.Patrol_GetDevicePath(0);

                uint BufferSize = 2000/8;
                byte[] fpsbuffer = new byte[BufferSize];
                uint count;
                bool ret = fp.Fingerprint_GetFingerprintStatus(DevPath, fpsbuffer, BufferSize, out count);

                byte[] fpbuffer;                
                ret = fp.Fingerprint_GetFingerprintPattern(DevPath, 0,out fpbuffer);
//                bool ret = fp.SetFingerprintPattern(DevPath,9,fpbuffer);
                //ret = true;
                //CItemPatrol.INSPECTOR_INFO info;
                //dev.GetInspectorInfo(DevPath, 0, out info);
                //System.Windows.Forms.MessageBox.Show(info.name );

 /*               UInt32 count = dev.GetRecordCount(DevPath);
                if (count > 0)
                {
                    UInt16 start = 0;
                    do
                    {
                        CItemPatrol.ITEMRECORD[] records;
                        UInt32 ReadNum;
                        if (dev.GetItemRecord(DevPath, start, out records,out ReadNum))
                        {
                            start += (UInt16)records.Length;
                            System.Windows.Forms.MessageBox.Show("read records ok");
                        }
                    } while (start < count);
                }
  */
   
                CItemPatrol.SPOT_INFO SpotInfo = new CItemPatrol.SPOT_INFO();
                //dev.GetSpotInfo(DevPath, 0,out SpotInfo);
                SpotInfo.CardNumber = 0x2BDBFE;
                SpotInfo.TotalItem = 5;
                SpotInfo.isFingerCheck = false;
                SpotInfo.ItemList = new CItemPatrol.SPOT_ITEM[SpotInfo.TotalItem];

                int index = 0;

                SpotInfo.ItemList[index].isLogic = false;
                SpotInfo.ItemList[index].ItemNameOrder = 0;
                SpotInfo.ItemList[index].NumDefault = 12345678.4321;
                SpotInfo.ItemList[index].NumMax = 99999998.9998;
                SpotInfo.ItemList[index].NumMin = -99999998.9998;
                index++;
                SpotInfo.ItemList[index].isLogic = false;
                SpotInfo.ItemList[index].ItemNameOrder = 1;
                SpotInfo.ItemList[index].NumDefault = 12345678.4321;
                SpotInfo.ItemList[index].NumMax = 99999998.9998;
                SpotInfo.ItemList[index].NumMin = -99999998.9998;
                index++;
                SpotInfo.ItemList[index].isLogic = false;
                SpotInfo.ItemList[index].ItemNameOrder = 2;
                SpotInfo.ItemList[index].NumDefault = 12345678.4321;
                SpotInfo.ItemList[index].NumMax = 99999998.9998;
                SpotInfo.ItemList[index].NumMin = -99999998.9998;
                index++;
                SpotInfo.ItemList[index].isLogic = false;
                SpotInfo.ItemList[index].ItemNameOrder = 3;
                SpotInfo.ItemList[index].NumDefault = 12345678.4321;
                SpotInfo.ItemList[index].NumMax = 99999998.9998;
                SpotInfo.ItemList[index].NumMin = -99999998.9998;
                index++;
                SpotInfo.ItemList[index].isLogic = false;
                SpotInfo.ItemList[index].ItemNameOrder = 4;
                SpotInfo.ItemList[index].NumDefault = 12345678.4321;
                SpotInfo.ItemList[index].NumMax = 99999998.9998;
                SpotInfo.ItemList[index].NumMin = -99999998.9998;

                SpotInfo.SpotName = "卡‰一二三四56七八九十11十二13十四15十六";
                dev.ItemPatrol_SetSpotInfo(DevPath, 0, SpotInfo);

                dev.ItemPatrol_GetSpotInfo(DevPath, 0,out SpotInfo);
                CItemPatrol.ITEMRECORD[] record;
                UInt32 ReadNumber;
                dev.ItemPatrol_GetItemRecord(DevPath, 0, out record, out ReadNumber);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EventPatrol dev = new EventPatrol();
            Fingerprint fp = new Fingerprint();
            UInt32 n = dev.Event_EnumEventPatrolDevice();
            if (n > 0)
            {
                string DevPath = dev.Patrol_GetDevicePath(0);
                uint RecordCount = dev.Patrol_GetRecordCount(DevPath);
                //for (ushort i = 1; i < RecordCount; i++)
                {
                    EventPatrol.RECORD[] records;
                    dev.Event_GetEventRecord(DevPath, 1,out records);
                }

               /* EventPatrol.SPOT_INFO SpotInfo = new EventPatrol.SPOT_INFO();
                SpotInfo.CardNumber = 0x2BDBFE;
                SpotInfo.isFingerCheck = false;
                SpotInfo.SpotName = "地点一";
                SpotInfo.EventList = new UInt16[3];
                SpotInfo.EventList[0] = 1;
                SpotInfo.EventList[1] = 2;
                SpotInfo.EventList[2] = 3;
                dev.SetSpotInfo(DevPath, 0, SpotInfo);

                dev.GetSpotInfo(DevPath, 0,out SpotInfo);
                System.Windows.Forms.MessageBox.Show(SpotInfo.SpotName);
                */

                //dev.DeviceInitialize(DevPath);

/*                UInt32 count = dev.GetRecordCount(DevPath);
                if (count > 0)
                {
                    UInt16 start = 0;
                    do
                    {
                        EventPatrol.GPSRECORD[] records;
                        if (dev.GetGpsEventRecord(DevPath, start, out records))
                        {
                            start += (UInt16)records.Length;
                            System.Windows.Forms.MessageBox.Show("read records ok");
                        }
                    } while (start < count);
                }
 */
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CItemPatrol dev = new CItemPatrol();
            Fingerprint fp = new Fingerprint();
            UInt32 n = dev.ItemPatrol_EnumItemPatrolDevice();
            if (n > 0)
            {
                string DevPath = dev.Patrol_GetDevicePath(0);
                byte[] buffer = new byte[64];
                UInt32 count;
                if (fp.Fingerprint_GetFingerprintStatus(DevPath, buffer, 64, out count))
                {
//                    count = count;
                    MessageBox.Show("读取指纹信息成功");
                }
            }
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            CItemPatrol dev = new CItemPatrol();
            Fingerprint fp = new Fingerprint();
            UInt32 n = dev.ItemPatrol_EnumItemPatrolDevice();
            if (n > 0)
            {
                string DevPath = dev.Patrol_GetDevicePath(0);
                if (fp.Fingerprint_ClearFingerprintPattern(DevPath))
                {
                    MessageBox.Show("清除指纹成功");
                }
            }
        }
        private byte[] fpbuffer;
        private void button5_Click(object sender, EventArgs e)
        {
            CItemPatrol dev = new CItemPatrol();
            Fingerprint fp = new Fingerprint();
            UInt32 n = dev.ItemPatrol_EnumItemPatrolDevice();
            if (n > 0)
            {
                string DevPath = dev.Patrol_GetDevicePath(0);
                byte[] buffer;
                if (fp.Fingerprint_GetFingerprintPattern(DevPath,0x05,out buffer))
                {
                    fpbuffer = buffer;
                    FileStream file = new FileStream("F:\\FGTP.dat", FileMode.Open);
                    file.Seek(0,SeekOrigin.Begin);
                    file.Write(fpbuffer, 0, fpbuffer.Length);
                    file.Close();
                    MessageBox.Show("读取指纹模板成功");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CItemPatrol dev = new CItemPatrol();
            Fingerprint fp = new Fingerprint();
            UInt32 n = dev.ItemPatrol_EnumItemPatrolDevice();
            if (n > 0)
            {
                string DevPath = dev.Patrol_GetDevicePath(0);
                byte[] buffer = new byte[498*5];
                FileStream file = new FileStream("F:\\FGTP.dat", FileMode.Open);
                file.Seek(0,SeekOrigin.Begin);
                file.Read(buffer, 0, buffer.Length);
                file.Close();
                if (fp.Fingerprint_SetFingerprintPattern(DevPath, 0x01, buffer))
                {
                    MessageBox.Show("设置指纹模板成功");
                }
                else
                {
                    MessageBox.Show("设置指纹模板失败");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CItemPatrol dev = new CItemPatrol();
            DFile dfile = new DFile();
            UInt32 n = dev.ItemPatrol_EnumItemPatrolDevice();
            if (n > 0)
            {
                string DevPath = dev.Patrol_GetDevicePath(0);
                /*byte[] buffer = new byte[498 * 5];
                FileStream file = new FileStream("F:\\FGTP.dat", FileMode.Open);
                file.Seek(0, SeekOrigin.Begin);
                file.Read(buffer, 0, buffer.Length);
                file.Close();*/
                UInt32 length;
                if (dfile.Dfile_GetDFileInfo(DevPath, "0:/linknull/index.lst", out length))
                {
                    byte[] FileBuffer;
                    UInt32 ReadNum;
                    if (dfile.Dfile_ReadDFile(DevPath, out FileBuffer, length, out ReadNum, 2000))
                    {
                        MessageBox.Show("读取文件成功");
                    }
                }
                else
                {
                    MessageBox.Show("读取文件信息失败");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CNoDriverStick dev = new CNoDriverStick();
            UInt32 DevCount = dev.EnumNoDriverStickDevice();
            if (DevCount > 0)
            {
                string DevicePath = dev.Patrol_GetDevicePath(0);
                UInt32 DeviceID = dev.Patrol_GetDeviceID(DevicePath);
                Patrol.DEVICETYPE DeviceType;
                dev.Patrol_GetDeviceType(DevicePath,out DeviceType);
                UInt32 RecordCount = dev.Patrol_GetRecordCount(DevicePath);
                UInt32 NumberRead;
                CNoDriverStick.StickRecordAndDeviceID[] records = dev.GetStickRecord2(DevicePath, 0, RecordCount,out NumberRead);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CItemPatrol dev = new CItemPatrol();
            UInt32 n = dev.ItemPatrol_EnumItemPatrolDevice();
            if (n > 0)
            {
                string DevPath = dev.Patrol_GetDevicePath(0);
                int AlarmNumber = 3;
                Patrol.ALARMITEM[] items = new Patrol.ALARMITEM[AlarmNumber];

                for (int i = 0; i < AlarmNumber; i++)
                {
                    items[i] = new Patrol.ALARMITEM();
                    items[i].mode = 0x0000007F;
                    items[i].hour = 17;
                    items[i].minute = (byte)(40 + i);
                }

                if (dev.Patrol_SetAlarm(DevPath, items, AlarmNumber))
                {
                    MessageBox.Show("设置闹钟成功");
                }
                else
                {
                    MessageBox.Show("设置闹钟失败");
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CItemPatrol dev = new CItemPatrol();
            UInt32 n = dev.ItemPatrol_EnumItemPatrolDevice();
            if (n > 0)
            {
                string DevPath = dev.Patrol_GetDevicePath(0);
                uint count = dev.Patrol_GetRecordCount(DevPath);
                if (count > 0)
                {
                    CItemPatrol.ITEMRECORD[] records;
                    uint NumberRead;
                    if (dev.ItemPatrol_GetItemAndGPSRecord(DevPath, 0, out records, out NumberRead))
                    {
                        NumberRead = NumberRead;
                    }
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Form3 fr = new Form3();
            fr.ShowDialog();
        }
    }
}



