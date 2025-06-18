using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


public class EventPatrol : Patrol
{
    private Encoding mEncoding = Encoding.GetEncoding(936);
    public Encoding SetEncoding(Encoding encoding)
    {
        Encoding ret = mEncoding;
        mEncoding = encoding;
        return ret;
    }

    public EventPatrol()
    {
        ZVDeviceDLL zvddll = ZVDeviceDLL.GetInstance();
        //
        funcEnumEventPatrolDevice = (Type_EnumEventPatrolDevice)zvddll.GetDelegateForFunction("EnumEventPatrolDevice", typeof(Type_EnumEventPatrolDevice));

        //Set tagID and tagName
        funcSetSpotInfo = (Type_SetSpotInfo)zvddll.GetDelegateForFunction("SetSpotInfo", typeof(Type_SetSpotInfo));
        //get data
        funcGetEventRecord = (Type_GetEventRecord)zvddll.GetDelegateForFunction("GetEventRecord", typeof(Type_GetEventRecord));

    }

    private delegate UInt32 Type_EnumEventPatrolDevice();
    private Type_EnumEventPatrolDevice funcEnumEventPatrolDevice;
    public UInt32 Event_EnumEventPatrolDevice()
    {
        return funcEnumEventPatrolDevice();
    }


    public const Int32 MAX_INSPECTOR_NAME_LEN = 16;
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct INSPECTORINFO
    {
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_INSPECTOR_NAME_LEN)]
        public byte[] Name;
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 CardNumber;
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 AuthenticationType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public UInt16[] FingerNo;
    };
    


    public const int MAX_SPOT_NAME_LEN = 20;
    public const int MAX_SPOT_EVENT_NUM = 15;
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SPOTINFO
    {
        public UInt16 index;
        public UInt16 NeedCheckFinger;	//bit0:标记是否需要指纹校对身份(1 = 需要,0=no)
        public UInt32 CardNumber;       //地点卡号

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPOT_NAME_LEN)]
        public Byte[] name;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPOT_EVENT_NUM)]
        public UInt16[] Event;
    };
        
    public struct SPOT_INFO
    {
        public bool NeedCheckFinger;	    //是否需要指纹校对身份
        public UInt32 CardNumber;		//卡号
        public string SpotName;         //最大长度MAX_SPOT_NAME_LEN字节
        public UInt16[] EventList;       //最多MAX_SPOT_EVENT_NUM个
    };

    private delegate bool Type_SetSpotInfo(byte[] path, ref SPOTINFO info);
    private Type_SetSpotInfo funcSetSpotInfo;
    public bool Event_SetSpotInfo(string DevicePath, UInt16 index, SPOT_INFO SpotInfo)
    {
        bool ret = false;
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        IntPtr pBuff = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SPOTINFO)));
        SPOTINFO info = (SPOTINFO)Marshal.PtrToStructure(pBuff, typeof(SPOTINFO));

        info.index = index;
        if (SpotInfo.NeedCheckFinger)
            info.NeedCheckFinger = 1;
        else
            info.NeedCheckFinger = 0;
        info.CardNumber = SpotInfo.CardNumber;
        SpotInfo.SpotName += '\0';
        byte[] gbkname = Encoding.GetEncoding(936).GetBytes(SpotInfo.SpotName + "\0");
        int n = SpotInfo.SpotName.Length;
        if (gbkname.Length <= (MAX_SPOT_NAME_LEN))
        {
            gbkname.CopyTo(info.name, 0);
            n = SpotInfo.EventList.Length;
            n = (n > MAX_SPOT_EVENT_NUM) ? MAX_SPOT_EVENT_NUM : n;
            for (int i = 0; i < n; i++)
            {
                info.Event[i] = SpotInfo.EventList[i];
            }
            for (int i = n; i < MAX_SPOT_EVENT_NUM; i++)
            {
                info.Event[i] = 0xFFFF;
            }
            ret = funcSetSpotInfo(path, ref info);
        }        
        Marshal.FreeHGlobal(pBuff);
        return ret;
    }


    public const int MAX_RECORD_EVENT_NUM = 3;
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct RECORDINFO
    {
        public UInt16 NeedCheckFinger;				//bit0标记巡逻人员是指纹登录还是卡登录(1=指纹,0=卡)
                                                    //bit1标记是否有代人巡逻(0:正常巡逻,1:代人巡逻)        
        public UInt32 ManCardNumorFingerNo;		    //巡逻人员卡号或者指纹编号
        public UInt32 SpotCardNum;				    //地点卡号
        public UInt32 time;		                    //time:Seconds from 1st January,2000 00:00:00 to record.

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_RECORD_EVENT_NUM + 2)]
        public UInt16[] EventList;	     
    }

    public struct RECORD
    {
        public bool NeedCheckFinger;
        public bool Authorization;          //是否代检,'false:代检      true:正常巡检
        public UInt32 IDOfInspector;        //如果NeedCheckFinger==true,此为指纹编号；如果NeedCheckFinger==false,此为人员卡号
        public UInt32 SpotCardNum;          //地点卡号
        public DateTime time;               //生成记录的时间
        public UInt16[] EventList;          //记录的事件列表
    }
    private delegate bool Type_GetEventRecord(byte[] path, UInt32 StartIndex, UInt32 NumberToRead, IntPtr info, out UInt32 ReadNumber);
    private Type_GetEventRecord funcGetEventRecord;
    public bool Event_GetEventRecord(string DevicePath, UInt32 StartIndex, out RECORD[] records)    
    {
        bool RetValue = false;
        UInt32 NumberToRead = 80;
        records = new RECORD[1];

        IntPtr pBuff = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RECORDINFO)) * (int)NumberToRead);
        if (pBuff != (IntPtr)0)
        {
            byte[] path = Encoding.ASCII.GetBytes(DevicePath);
            UInt32 ReadNumber;
            if (funcGetEventRecord(path, StartIndex, NumberToRead, pBuff, out ReadNumber))
            {
                records = new RECORD[ReadNumber];
                for (int i = 0; i < ReadNumber; i++)
                {
                    RECORDINFO info = (RECORDINFO)Marshal.PtrToStructure((IntPtr)(pBuff.ToInt32() + Marshal.SizeOf(typeof(RECORDINFO)) * (int)i), typeof(RECORDINFO));                    
                    if ((info.NeedCheckFinger & 0x0001) == 0x0001)
                        records[i].NeedCheckFinger = true;
                    else
                        records[i].NeedCheckFinger = false;
                    if ((info.NeedCheckFinger & 0x0002) == 0x0002)
                        records[i].Authorization = false;
                    else
                        records[i].Authorization = true;
                    records[i].IDOfInspector = info.ManCardNumorFingerNo;
                    records[i].SpotCardNum = info.SpotCardNum;
                    records[i].time = new DateTime(2000,1,1,0,0,0);
                    records[i].time = records[i].time.AddSeconds(info.time);
                    records[i].EventList = new UInt16[MAX_RECORD_EVENT_NUM];
                    for (int j = 0; j < MAX_RECORD_EVENT_NUM; j++)
                    {
                        records[i].EventList[j] = info.EventList[j];
                    }
                }
                RetValue = true;
            }
            Marshal.FreeHGlobal(pBuff);
        }
        return RetValue;
    }
    
    
}





