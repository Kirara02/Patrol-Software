using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

public class Patrol
{
    public const UInt32 INVALID_DEVICE_ID = 0xFFFFFFFE;
    public Patrol()
    {
        //Type_EnumDeviceByProtocolAndPort（Delegate type） ,EnumDeviceByProtocolAndPort:PCPS.dll（Method names）
        ZVDeviceDLL zvddll = ZVDeviceDLL.GetInstance();
        //根据协议版本和通信口类型获取当前与pc端联接设备, 当有2个设备时返回2，无设备返回
        funcEnumDeviceByProtocolAndPort = (Type_EnumDeviceByProtocolAndPort)zvddll.GetDelegateForFunction("EnumDeviceByProtocolAndPort", typeof(Type_EnumDeviceByProtocolAndPort));
        //根据设备类型枚举获取当前与pc端联接设备，当有2个设备时返回2，无设备返回0
        funcEnumDeviceByDeviceType = (Type_EnumDeviceByDeviceType)zvddll.GetDelegateForFunction("EnumDeviceByDeviceType", typeof(Type_EnumDeviceByDeviceType));
        //获取设备USB路径
        funcGetDevicePath = (Type_GetDevicePath)zvddll.GetDelegateForFunction("GetDevicePath", typeof(Type_GetDevicePath));
        //获取机器序列号
        funcGetDeviceID = (Type_GetDeviceID)zvddll.GetDelegateForFunction("GetDeviceID", typeof(Type_GetDeviceID));
        //获取设备类型
        funcGetDeviceType = (Type_GetDeviceType)zvddll.GetDelegateForFunction("GetDeviceType", typeof(Type_GetDeviceType));
        //获取机器时钟
        funcGetDeviceTime = (Type_GetDeviceTime)zvddll.GetDelegateForFunction("GetDeviceTime", typeof(Type_GetDeviceTime));
        //设置机器时钟
        funcSetDeviceTime = (Type_SetDeviceTime)zvddll.GetDelegateForFunction("SetDeviceTime", typeof(Type_SetDeviceTime));
        //设置机器时间
        funcSetDeviceTimeEx = (Type_SetDeviceTimeEx)zvddll.GetDelegateForFunction("SetDeviceTimeEx", typeof(Type_SetDeviceTimeEx));
        //获取软件版本号
        funcGetVersionString = (Type_GetVersionString)zvddll.GetDelegateForFunction("GetVersionString", typeof(Type_GetVersionString));
        //获取设备记录限制数量
        funcGetRecordCapacity = (Type_GetRecordCapacity)zvddll.GetDelegateForFunction("GetRecordCapacity", typeof(Type_GetRecordCapacity));
        //获取设备碰撞记录总数
        funcGetCrashRecordCount = (Type_GetCrashRecordCount)zvddll.GetDelegateForFunction("GetCrashRecordCount", typeof(Type_GetCrashRecordCount));
        //获取设备碰撞记录
        funcGetCrashRecord = (Type_GetCrashRecord)zvddll.GetDelegateForFunction("GetCrashRecord", typeof(Type_GetCrashRecord));
        //清除设备碰撞记录
        funcClearCrashRecord = (Type_ClearCrashRecord)zvddll.GetDelegateForFunction("ClearCrashRecord", typeof(Type_ClearCrashRecord));
        //获取记录数量
        funcGetRecordCount = (Type_GetRecordCount)zvddll.GetDelegateForFunction("GetRecordCount", typeof(Type_GetRecordCount));
        //清除记录
        funcClearRecord = (Type_ClearRecord)zvddll.GetDelegateForFunction("ClearRecord", typeof(Type_ClearRecord));
        //更新软件
        funcUpdateSoftware = (Type_UpdateSoftware)zvddll.GetDelegateForFunction("UpdateSoftware", typeof(Type_UpdateSoftware));
        //设置网络配置
        funcSetNetConfig = (Type_SetNetConfig)zvddll.GetDelegateForFunction("SetNetConfig", typeof(Type_SetNetConfig));
        //设置在线巡更设备（Z500C）网络参数
        funcSetNetworkInfo = (Type_SetNetworkInfo)zvddll.GetDelegateForFunction("SetNetworkInfo", typeof(Type_SetNetworkInfo));
        //获取巡逻记录
        funcGetRecordBytes = (Type_GetRecordBytes)zvddll.GetDelegateForFunction("GetRecordBytes", typeof(Type_GetRecordBytes));
        //设置闹钟
        funcSetAlarm = (Type_SetAlarm)zvddll.GetDelegateForFunction("SetAlarm", typeof(Type_SetAlarm));
        //设置网络配置
        funcSetNetConfig2 = (Type_SetNetConfig2)zvddll.GetDelegateForFunction("SetNetConfig2", typeof(Type_SetNetConfig2));
        //获取网络配置
        funcGetNetConfig2 = (Type_GetNetConfig2)zvddll.GetDelegateForFunction("GetNetConfig2", typeof(Type_GetNetConfig2));
        //设置通讯录
        funcSetContacts = (Type_SetContacts)zvddll.GetDelegateForFunction("SetContacts", typeof(Type_SetContacts));
        //设置设备生产日期
        funcGetProductTime = (Type_GetProductTime)zvddll.GetDelegateForFunction("GetProductTime", typeof(Type_GetProductTime));
        //设置通讯录
        funcSetWiseeyeID = (Type_SetWiseeyeID)zvddll.GetDelegateForFunction("SetWiseeyeID", typeof(Type_SetWiseeyeID));
        //设置通讯录
        funcGetWiseeyeID = (Type_GetWiseeyeID)zvddll.GetDelegateForFunction("GetWiseeyeID", typeof(Type_GetWiseeyeID));
    }

    public enum PROTOCOL
    {
        PROTOCOL_V1 = 0,
        PROTOCOL_V2 = 1,
        PROTOCOL_V3 = 2,
        INVALID_PROTOCOL = 3,
    };
    public enum PROTTYPE
    {
        PORTTYPE_USB = 0,
        PORTTYPE_COM = 1,
        INVALID_PORTTYPE = 2,
    };
    public enum DEVICETYPE
    {
        DT_A1 = 0x10501,
        DT_FG_5 = 0x10504,
        DT_FG_1_429 = 0x10503,
        DT_Z6800 = 0x10502,
        DT_Z6500_s = 0x10301,
        DT_Z6500_b = 0x10302,
        DT_Z6500F = 0x10105,
        DT_Z6500D = 0x10106,
        DT_Z6900 = 0x10107,
        DT_FG_1NEW = 0x10505,
        DT_FG_2NEW = 0x10506,
        DT_Z8000 = 0x10508,
        DT_Z6700 = 0x10601,
        DT_Z6200F = 0x10001,

        DT_Z6600T = 0x10405,

        DT_Z6200E = 0x10407,

        DT_INVALID = -1,
    }
    public struct NETCONFIG2
    {
        public string url;
        public string port;
        public string AccessPoint;
        public string UserName;
        public string password;
        public string IPMode;
        public string ip;
        public string mask;
        public string gateway;
        public string auth;
        public string encry;
    }
    private delegate UInt32 Type_EnumDeviceByProtocolAndPort(UInt32 protocol, UInt32 PortType);
    private Type_EnumDeviceByProtocolAndPort funcEnumDeviceByProtocolAndPort;
    public UInt32 Patrol_EnumDeviceByProtocolAndPort(PROTOCOL protocol, PROTTYPE PortType)
    {
        return funcEnumDeviceByProtocolAndPort((UInt32)protocol, (UInt32)PortType);
    }

    private delegate UInt32 Type_EnumDeviceByDeviceType(UInt32 DeviceType);
    private Type_EnumDeviceByDeviceType funcEnumDeviceByDeviceType;
    public UInt32 EnumDeviceByDeviceType(DEVICETYPE DeviceType)
    {
        return funcEnumDeviceByDeviceType((UInt32)DeviceType);
    }

    private delegate bool Type_GetDevicePath(byte[] path, UInt32 index);
    private Type_GetDevicePath funcGetDevicePath;
    public string Patrol_GetDevicePath(UInt32 index)
    {
        byte[] buffer = new byte[32];
        if (funcGetDevicePath(buffer, index))
            return Encoding.ASCII.GetString(buffer);
        else
            return null;
    }

    private delegate UInt32 Type_GetDeviceID(byte[] path);
    private Type_GetDeviceID funcGetDeviceID;
    public UInt32 Patrol_GetDeviceID(string DevicePath)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        return funcGetDeviceID(buffer);
    }

    private delegate bool Type_GetDeviceType(byte[] path, out UInt32 type);
    private Type_GetDeviceType funcGetDeviceType;
    public bool Patrol_GetDeviceType(string DevicePath, out Patrol.DEVICETYPE type)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        UInt32 DevType;
        bool rst = funcGetDeviceType(buffer, out DevType);
        type = rst ? (Patrol.DEVICETYPE)DevType : Patrol.DEVICETYPE.DT_INVALID;
        return rst;
    }

    private delegate bool Type_GetDeviceTime(
        byte[] path,
        out UInt16 year,
        out UInt16 month,
        out UInt16 day,
        out UInt16 hour,
        out UInt16 minute,
        out UInt16 second
        );
    private Type_GetDeviceTime funcGetDeviceTime;
    public bool Patrol_GetDeviceTime(string DevicePath, out DateTime dt)
    {
        UInt16 year, month, day, hour, minute, second;
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        if (funcGetDeviceTime(buffer, out year, out month, out day, out hour, out minute, out second))
        {
            dt = new DateTime(year, month, day, hour, minute, second);
            return true;
        }
        else
        {
            dt = new DateTime();
            return false;
        }
    }

    private delegate bool Type_SetDeviceTime(byte[] path);
    private Type_SetDeviceTime funcSetDeviceTime;
    public bool Patrol_SetDeviceTime(string DevicePath)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        return funcSetDeviceTime(buffer);
    }

    private delegate bool Type_SetDeviceTimeEx(
        byte[] path,
        UInt16 year,
        UInt16 month,
        UInt16 day,
        UInt16 hour,
        UInt16 minute,
        UInt16 second);
    private Type_SetDeviceTimeEx funcSetDeviceTimeEx;
    public bool Patrol_SetDeviceTime(string DevicePath, DateTime dt)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        return funcSetDeviceTimeEx(
                    buffer,
                    (UInt16)dt.Year,
                    (UInt16)dt.Month,
                    (UInt16)dt.Day,
                    (UInt16)dt.Hour,
                    (UInt16)dt.Minute,
                    (UInt16)dt.Second);
    }

    private delegate bool Type_GetVersionString(byte[] path, byte[] version);
    private Type_GetVersionString funcGetVersionString;
    public string Patrol_GetVersionString(string DevicePath)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        byte[] version = new byte[32];
        if (funcGetVersionString(buffer, version))
            return Encoding.ASCII.GetString(version);
        else
            return null;
    }

    private delegate bool Type_GetRecordCapacity(byte[] path, out UInt32 RecordCapacity);
    private Type_GetRecordCapacity funcGetRecordCapacity;
    public UInt32 GetRecordCapacity(string DevicePath)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        UInt32 RecordCapacity;
        if (funcGetRecordCapacity(buffer, out RecordCapacity))
            return RecordCapacity;
        else
            return 0;
    }


    public delegate void PROGRESSCALLBACK(UInt32 MaxValue, UInt32 CurrentValue);
    private delegate bool Type_UpdateSoftware(byte[] path, byte[] SoftwareFilePath, PROGRESSCALLBACK CallbackFunc);
    private Type_UpdateSoftware funcUpdateSoftware;
    public bool Patrol_UpdateSoftware(string DevicePath, string SoftwareFilePath, PROGRESSCALLBACK CallbackFunc)
    {
        byte[] dpBuffer = Encoding.ASCII.GetBytes(DevicePath);
        byte[] sfBuffer = Encoding.GetEncoding(936).GetBytes(SoftwareFilePath);

        return funcUpdateSoftware(dpBuffer, sfBuffer, CallbackFunc);
    }

    private delegate bool Type_GetCrashRecordCount(byte[] path, out UInt32 CrashRecordCount);
    private Type_GetCrashRecordCount funcGetCrashRecordCount;
    public UInt32 GetCrashRecordCount(string DevicePath)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        UInt32 CrashRecordCount;
        if (funcGetCrashRecordCount(buffer, out CrashRecordCount))
            return CrashRecordCount;
        else
            return 0;
    }


    private delegate bool Type_GetCrashRecord(
        byte[] path,
        UInt32 StartIndex,
        UInt32 NumberToRead,
        UInt32[] record,
        out UInt32 NumberRead
        );
    private Type_GetCrashRecord funcGetCrashRecord;
    public DateTime[] Patrol_GetCrashRecord(
        string DevicePath,
        UInt32 StartIndex,
        UInt32 NumberToRead,
        out UInt32 NumberRead
        )
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        UInt32[] record = new UInt32[NumberToRead];
        bool ret = funcGetCrashRecord(buffer, StartIndex, NumberToRead, record, out NumberRead);
        if (ret && NumberRead != 0)
        {
            DateTime[] dt = new DateTime[NumberRead];
            for (int i = 0; i < NumberRead; i++)
            {
                dt[i] = new DateTime(2000, 1, 1, 0, 0, 0);
                dt[i] = dt[i].AddSeconds(record[i]);
            }
            return dt;
        }
        else
        {
            return null;
        }
    }


    private delegate bool Type_ClearCrashRecord(byte[] path);
    private Type_ClearCrashRecord funcClearCrashRecord;
    public bool Patrol_ClearCrashRecord(string DevicePath)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        return funcClearCrashRecord(buffer);
    }


    private delegate bool Type_GetRecordCount(byte[] path, out UInt32 RecordCount);
    private Type_GetRecordCount funcGetRecordCount;
    public UInt32 Patrol_GetRecordCount(string DevicePath)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        UInt32 RecordCount;
        if (funcGetRecordCount(buffer, out RecordCount))
            return RecordCount;
        else
            return 0;
    }

    private delegate bool Type_ClearRecord(byte[] path);
    private Type_ClearRecord funcClearRecord;
    public bool Patrol_ClearRecord(string DevicePath)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        return funcClearRecord(buffer);
    }

    private delegate bool Type_SetNetConfig(
        byte[] path,
        byte[] url,
        UInt16 port,
        byte[] AccessPoint,
        byte[] UserName,
        byte[] password,
        byte[] IPMode,
        byte[] ip,
        byte[] mask,
        byte[] gateway);
    private Type_SetNetConfig funcSetNetConfig;
    public bool SetNetConfig(
        string DevicePath,
        string url,
        UInt16 port,
        string AccessPoint,
        string UserName,
        string password,
        string IPMode,
        string ip,
        string mask,
        string gateway)
    {
        bool ret = false;
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        byte[] bURL = Encoding.ASCII.GetBytes(url + "\0");
        byte[] bAccessPoint = Encoding.ASCII.GetBytes(AccessPoint + "\0");
        byte[] bUserName = Encoding.ASCII.GetBytes(UserName + "\0");
        byte[] bPassword = Encoding.ASCII.GetBytes(password + "\0");
        byte[] bIPMode = Encoding.ASCII.GetBytes(IPMode + "\0");
        byte[] bip = Encoding.ASCII.GetBytes(ip + "\0");
        byte[] bmask = Encoding.ASCII.GetBytes(mask + "\0");
        byte[] bgateway = Encoding.ASCII.GetBytes(gateway + "\0");
        ret = funcSetNetConfig(path, bURL, port, bAccessPoint, bUserName, bPassword, bIPMode, bip, bmask, bgateway);
        return ret;
    }


    private delegate bool Type_SetNetworkInfo(byte[] path, byte[] info, UInt32 SizeOfInfoInByte);
    private Type_SetNetworkInfo funcSetNetworkInfo;
    public bool Patrol_SetNetworkInfo(string DevicePath, byte[] info, UInt32 SizeOfInfoInByte)
    {
        bool ret = false;
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        ret = funcSetNetworkInfo(path, info, SizeOfInfoInByte);
        return ret;
    }

    private delegate bool Type_GetRecordBytes(byte[] path, UInt32 StartIndex, UInt32 NumberToRead, byte[] pRecord, out UInt32 BytesNumberRead);
    private Type_GetRecordBytes funcGetRecordBytes;
    public bool Patrol_GetRecordBytes(string DevicePath, UInt32 StartIndex, UInt32 NumberToRead, out byte[] RecordBytes, out UInt32 RecordBytesNumber)
    {
        bool ret = false;
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        RecordBytes = new byte[2014];
        ret = funcGetRecordBytes(path, StartIndex, NumberToRead, RecordBytes, out RecordBytesNumber);
        return ret;
    }

    protected object BytesToStruct(byte[] bytes, int start, Type type)
    {
        int size = Marshal.SizeOf(type);
        if (size > bytes.Length)
        {
            return null;
        }
        //分配结构体内存空间
        IntPtr structPtr = Marshal.AllocHGlobal(size);
        //将byte数组拷贝到分配好的内存空间
        Marshal.Copy(bytes, start, structPtr, size);
        //将内存空间转换为目标结构体
        object obj = Marshal.PtrToStructure(structPtr, type);
        //释放内存空间
        Marshal.FreeHGlobal(structPtr);
        return obj;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ALARMITEM
    {
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 mode;

        [MarshalAs(UnmanagedType.U1)]
        public byte hour;

        [MarshalAs(UnmanagedType.U1)]
        public byte minute;
    };
    private delegate bool Type_SetAlarm(byte[] path, IntPtr pAlarmItem, UInt32 count);
    private Type_SetAlarm funcSetAlarm;
    public bool Patrol_SetAlarm(string DevicePath, ALARMITEM[] AlarmItem, int count)
    {
        bool ret = false;
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        Type t = typeof(ALARMITEM);
        int cb = Marshal.SizeOf(t) * count;
        IntPtr pAlarmItem = Marshal.AllocHGlobal(cb);
        if (pAlarmItem != (IntPtr)0)
        {
            for (int i = 0; i < count; i++)
            {
                Marshal.StructureToPtr(AlarmItem[i], (IntPtr)((UInt32)pAlarmItem + Marshal.SizeOf(typeof(ALARMITEM)) * i), true);
            }
            ret = funcSetAlarm(path, pAlarmItem, (UInt32)count);
            Marshal.FreeHGlobal(pAlarmItem);
        }
        return ret;
    }
    private delegate bool Type_GetNetConfig2(byte[] path, IntPtr info);
    private Type_GetNetConfig2 funcGetNetConfig2;
    public bool Patrol_GetNetConfig2(string DevicePath, out NETCONFIG2 NetConfig2)
    {
        bool ret = false;
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        NetConfig2 = new NETCONFIG2();
        Type t = typeof(__NETCONFIG2);
        int cb = Marshal.SizeOf(t);
        IntPtr pContactsItems = Marshal.AllocHGlobal(cb);
        if (pContactsItems != (IntPtr)0)
        {
            ret = funcGetNetConfig2(path, pContactsItems);
            if (ret)
            {
                __NETCONFIG2 __NetConfig2 = (__NETCONFIG2)Marshal.PtrToStructure(pContactsItems, typeof(__NETCONFIG2));
                int start = 0, end = 0;
                start = ByteArrayIndexOf(__NetConfig2.WebAddr, (byte)'=', 1);
                if (start > 0)
                {
                    end = ByteArrayIndexOf(__NetConfig2.WebAddr, (byte)'\r', 1);
                    NetConfig2.url = Encoding.Default.GetString(__NetConfig2.WebAddr, start + 2, end - start - 3);
                    int len = ByteArrayIndexOf(__NetConfig2.PortAddr, (byte)'\r', 1);
                    NetConfig2.port = Encoding.Default.GetString(__NetConfig2.PortAddr, 1, len - 1);
                    start = 0;
                    end = ByteArrayIndexOf(__NetConfig2.OperatorName, (byte)'\r', 1);
                    if (end >= 0)
                    {
                        NetConfig2.AccessPoint = Encoding.Default.GetString(__NetConfig2.OperatorName, 0, end);
                        start = ByteArrayIndexOf(__NetConfig2.OperatorInfo, (byte)'\"', 3);
                        if (start >= 0)
                        {
                            end = ByteArrayIndexOf(__NetConfig2.OperatorInfo, (byte)'\"', 4);
                            if (end >= 0)
                            {
                                NetConfig2.UserName = Encoding.Default.GetString(__NetConfig2.OperatorInfo, start + 1, end - start - 1);
                                start = ByteArrayIndexOf(__NetConfig2.OperatorInfo, (byte)'\"', 5);
                                if (start >= 0)
                                {
                                    end = ByteArrayIndexOf(__NetConfig2.OperatorInfo, (byte)'\"', 6);
                                    if (end >= 0)
                                    {
                                        NetConfig2.password = Encoding.Default.GetString(__NetConfig2.OperatorInfo, start + 1, end - start - 1);
                                    }
                                }
                            }
                        }
                        NetConfig2.auth = Encoding.Default.GetString(__NetConfig2.auth).Trim("\0".ToCharArray());
                        NetConfig2.encry = Encoding.Default.GetString(__NetConfig2.encry).Trim("\0".ToCharArray());
                    }
                }
            }
            Marshal.FreeHGlobal(pContactsItems);
        }
        return ret;
    }
    private delegate bool Type_SetNetConfig2(
        byte[] path,
        byte[] url,
        UInt16 port,
        byte[] AccessPoint,
        byte[] UserName,
        byte[] password,
        byte[] IPMode,
        byte[] ip,
        byte[] mask,
        byte[] gateway,
        byte[] auth,
        byte[] encry
        );
    private Type_SetNetConfig2 funcSetNetConfig2;
    public bool Patrol_SetNetConfig2(
        string DevicePath,
        string url,
        UInt16 port,
        string AccessPoint,
        string UserName,
        string password,
        string IPMode,
        string ip,
        string mask,
        string gateway,
        string auth,
        string encry
        )
    {
        bool ret = false;
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        byte[] bURL = Encoding.ASCII.GetBytes(url + "\0");
        byte[] bAccessPoint = Encoding.ASCII.GetBytes(AccessPoint + "\0");
        byte[] bUserName = Encoding.ASCII.GetBytes(UserName + "\0");
        byte[] bPassword = Encoding.ASCII.GetBytes(password + "\0");
        byte[] bIPMode = Encoding.ASCII.GetBytes(IPMode + "\0");
        byte[] bip = Encoding.ASCII.GetBytes(ip + "\0");
        byte[] bmask = Encoding.ASCII.GetBytes(mask + "\0");
        byte[] bgateway = Encoding.ASCII.GetBytes(gateway + "\0");
        byte[] bauth = Encoding.ASCII.GetBytes(auth + "\0");
        byte[] bencry = Encoding.ASCII.GetBytes(encry + "\0");
        ret = funcSetNetConfig2(path, bURL, port, bAccessPoint, bUserName, bPassword, bIPMode, bip, bmask, bgateway, bauth, bencry);
        return ret;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct __NETCONFIG2
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public byte[] WebAddr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] PortAddr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] OperatorName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] OperatorInfo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] auth;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] encry;
    };

    private int ByteArrayIndexOf(byte[] array, byte t, int index)
    {
        int n = 0;
        int count = 0;
        foreach (var i in array)
        {
            if (i == t)
            {
                count++;
                if (count >= index)
                    break;
            }
            n++;
        }
        if (n >= array.Length)
            n = -1;
        return n;
    }

    private delegate bool Type_GetProductTime(byte[] path, byte[] data);
    private Type_GetProductTime funcGetProductTime;
    public string Patrol_GetProductTime(string DevicePath)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        byte[] bs = new byte[100];
        if (funcGetProductTime(buffer, bs))
            return new DateTime(bs[1] << 8 | bs[0], bs[2], bs[3], bs[4], bs[5], bs[6]).ToString("yyyy-MM-dd HH:mm:ss");
        else
            return " Failure to obtain production time ";
    }


    private delegate bool Type_SetWiseeyeID(byte[] path, UInt32 pass);
    private Type_SetWiseeyeID funcSetWiseeyeID;
    public bool Patrol_SetWiseeyeID(string DevicePath, UInt32 pass)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        return funcSetWiseeyeID(buffer, pass);
    }

    private delegate bool Type_GetWiseeyeID(byte[] path, ref UInt32 paa);
    private Type_GetWiseeyeID funcGetWiseeyeID;
    public string Patrol_GetWiseeyeID(string DevicePath)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        UInt32 paa = 0;
        if (funcGetWiseeyeID(buffer, ref paa))
            return paa.ToString();
        else
            return string.Empty;
    }

    #region 通讯录 8000专用
    private delegate bool Type_SetContacts(byte[] path, int StartIndex, int count, CONTACT pContacts);
    private Type_SetContacts funcSetContacts;
    public bool SetContacts(string DevicePath, int StartIndex, int count, CONTACT pContacts)
    {
        bool ret = false;
        byte[] buffer = Encoding.ASCII.GetBytes(DevicePath);
        ret = funcSetContacts(buffer, StartIndex, count, pContacts);
        return ret;
    }
    public struct CONTACT
    {
        public string name;
        public string number;
    }
    #endregion
}
