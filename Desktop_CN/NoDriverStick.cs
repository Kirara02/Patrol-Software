using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

public class CNoDriverStick : Patrol
{
    public CNoDriverStick()
    {
        ZVDeviceDLL zvddll = ZVDeviceDLL.GetInstance();
        funcEnumNoDriverStickDevice = (Type_EnumNoDriverStickDevice)zvddll.GetDelegateForFunction("EnumNoDriverStickDevice", typeof(Type_EnumNoDriverStickDevice));
        funcGetStickRecord = (Type_GetStickRecord)zvddll.GetDelegateForFunction("GetStickRecord", typeof(Type_GetStickRecord));
    }

    private delegate UInt32 Type_EnumNoDriverStickDevice();
    private Type_EnumNoDriverStickDevice funcEnumNoDriverStickDevice;
    public UInt32 EnumNoDriverStickDevice()
    {
        return funcEnumNoDriverStickDevice();
    }

    private delegate bool Type_GetStickRecord(
        byte[] path,
        UInt32 StartIndex,
        UInt32 NumberToRead,
        UInt32[] record,
        out UInt32 NumberRead
        );
    private Type_GetStickRecord funcGetStickRecord;
    public StickRecord[] GetStickRecord(
        string DevicePath,
        UInt32 StartIndex,
        UInt32 NumberToRead,
        out UInt32 NumberRead
        )
    {
        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(DevicePath);
        UInt32[] record = new UInt32[NumberToRead * 2];
        bool ret = funcGetStickRecord(buffer, StartIndex, NumberToRead, record, out NumberRead);
        if (ret && NumberRead != 0)
        {
            StickRecord[] sr = new StickRecord[NumberRead];
            for (int i = 0; i < NumberRead; i++)
            {
                sr[i].CardID = record[i*2];
                sr[i].dt = new DateTime(2000, 1, 1, 0, 0, 0);
                sr[i].dt = sr[i].dt.AddSeconds(record[i * 2 + 1]);
            }
            return sr;
        }
        else
        {
            return null;
        }
    }

    public StickRecordAndDeviceID[] GetStickRecord2(
        string DevicePath,
        UInt32 StartIndex,
        UInt32 NumberToRead,
        out UInt32 NumberRead
        )
    {   
        byte[] RecordBytes;
        UInt32 RecordBytesNumber;
        bool ret = Patrol_GetRecordBytes(DevicePath, StartIndex, NumberToRead, out RecordBytes, out RecordBytesNumber);
        if (ret && RecordBytesNumber != 0)
        {
            UInt32 RecordSize = (UInt32)Marshal.SizeOf(typeof(StickRcdAndDevID));
            NumberRead = (UInt32)RecordBytesNumber/RecordSize;
            StickRecordAndDeviceID[] sr = new StickRecordAndDeviceID[NumberRead];
            for (int i = 0; i < NumberRead; i++)
            {
                StickRcdAndDevID oRecord = (StickRcdAndDevID)BytesToStruct(RecordBytes,(int)(RecordSize * i), typeof(StickRcdAndDevID));
                sr[i].DeviceID = (UInt32)System.Net.IPAddress.NetworkToHostOrder((int)oRecord.DeviceID);
                sr[i].CardID = (UInt32)System.Net.IPAddress.NetworkToHostOrder((int)oRecord.CardID);
                sr[i].dt = new DateTime(2000, 1, 1, 0, 0, 0);
                int seconds = System.Net.IPAddress.NetworkToHostOrder((int)oRecord.dt);
                sr[i].dt = sr[i].dt.AddSeconds(seconds);
            }
            return sr;
        }
        else
        {
            NumberRead = 0;
            return null;
        }
    }

    public struct StickRecord
    {
        public UInt32 CardID;
        public DateTime dt;
    };

    public struct StickRecordAndDeviceID
    {
        public UInt32 DeviceID;
        public UInt32 CardID;
        public DateTime dt;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct StickRcdAndDevID
    {
        public UInt32 DeviceID;
        public UInt32 CardID;
        public UInt32 dt;
    };
}



