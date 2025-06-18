using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


public class CItemPatrol : Patrol
{
    private Encoding mEncoding = Encoding.GetEncoding(936);
    public Encoding SetEncoding(Encoding encoding)
    {
        Encoding ret = mEncoding;
        mEncoding = encoding;
        return ret;
    }

    public CItemPatrol()
    {
        ZVDeviceDLL zvddll = ZVDeviceDLL.GetInstance();
        funcEnumItemPatrolDevice = (Type_EnumItemPatrolDevice)zvddll.GetDelegateForFunction("EnumItemPatrolDevice", typeof(Type_EnumItemPatrolDevice));
        //设置人员信息
        funcSetInspectorInfo = (Type_SetInspectorInfo)zvddll.GetDelegateForFunction("SetInspectorInfo", typeof(Type_SetInspectorInfo));
        //获取人员信息
        funcGetInspectorInfo = (Type_GetInspectorInfo)zvddll.GetDelegateForFunction("GetInspectorInfo", typeof(Type_GetInspectorInfo));
        ///设置当前人员
        funcSetCurrentInspector = (Type_SetCurrentInspector)zvddll.GetDelegateForFunction("SetCurrentInspector", typeof(Type_SetCurrentInspector));
       //获取地点信息
        funcGetSpotInfo = (Type_GetSpotInfo)zvddll.GetDelegateForFunction("GetSpotInfo", typeof(Type_GetSpotInfo));
        //设置地点信息
        funcSetSpotInfo = (Type_SetSpotInfo)zvddll.GetDelegateForFunction("SetSpotInfo", typeof(Type_SetSpotInfo));
        //获取项目记录
        funcGetItemRecord = (Type_GetItemRecord)zvddll.GetDelegateForFunction("GetItemRecord", typeof(Type_GetItemRecord));
        //设置线路名称
        funcSetRouteName = (Type_SetRouteName)zvddll.GetDelegateForFunction("SetRouteName", typeof(Type_SetRouteName));
        //设置项目名称
        funcSetItemName = (Type_SetItemName)zvddll.GetDelegateForFunction("SetItemName", typeof(Type_SetItemName));
        //设置逻辑名称
        funcSetLogicName = (Type_SetLogicName)zvddll.GetDelegateForFunction("SetItemLogic", typeof(Type_SetLogicName));
        //设置单位名称
        funcSetUnitName = (Type_SetUnitName)zvddll.GetDelegateForFunction("SetItemUnit", typeof(Type_SetUnitName));
    }

    private delegate UInt32 Type_EnumItemPatrolDevice();
    private Type_EnumItemPatrolDevice funcEnumItemPatrolDevice;
    public UInt32 ItemPatrol_EnumItemPatrolDevice()
    {
        return funcEnumItemPatrolDevice();
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

    public struct INSPECTOR_INFO
    {
        public string name;         //最大长度为MAX_INSPECTOR_NAME_LEN，单位为byte
        public UInt32 CardNumber;
        public UInt16 AuthenticationType;   //0:只有卡号    1:只有指纹  2:有指纹和卡号
        public UInt16[] FingerNo;           //长度为3
    };
    private delegate bool Type_SetInspectorInfo(byte[] path,ref INSPECTORINFO info);
    private Type_SetInspectorInfo funcSetInspectorInfo;
    public bool ItemPatrol_SetInspectorInfo(string DevicePath,UInt16 index,INSPECTOR_INFO InspectorInfo)
    {
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        byte[] gbkname = Encoding.GetEncoding(936).GetBytes(InspectorInfo.name);
        INSPECTORINFO info = new INSPECTORINFO();
        info.index = index;
        gbkname.CopyTo(info.Name, 0);
        info.CardNumber = InspectorInfo.CardNumber;
        info.AuthenticationType = InspectorInfo.AuthenticationType;
        info.FingerNo[0] = InspectorInfo.FingerNo[0];
        info.FingerNo[1] = InspectorInfo.FingerNo[1];
        info.FingerNo[2] = InspectorInfo.FingerNo[2];
        bool ret = funcSetInspectorInfo(path,ref info);
        return ret;
    }

    private delegate bool Type_GetInspectorInfo(byte[] path,ref INSPECTORINFO info);
    private Type_GetInspectorInfo funcGetInspectorInfo;
    public bool ItemPatrol_GetInspectorInfo(string DevicePath,UInt16 index,out INSPECTOR_INFO InspectorInfo)
    {
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        INSPECTORINFO info = new INSPECTORINFO();
        info.index  = index;
        InspectorInfo = new INSPECTOR_INFO();
        if (funcGetInspectorInfo(path, ref info))
        {
            InspectorInfo.name = Encoding.GetEncoding(936).GetString(info.Name);
            InspectorInfo.CardNumber = info.CardNumber;
            InspectorInfo.AuthenticationType = info.AuthenticationType;
            InspectorInfo.FingerNo = new UInt16[info.FingerNo.GetLength(0)];
            for (int i = 0; i < info.FingerNo.GetLength(0); i++)
            {
                InspectorInfo.FingerNo[i] = info.FingerNo[i];
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private delegate bool Type_SetCurrentInspector(byte[] path,UInt16 index);
    private Type_SetCurrentInspector funcSetCurrentInspector;
    public bool ItemPatrol_SetCurrentInspector(string DevicePath, UInt16 index)
    {
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        return funcSetCurrentInspector(path,index);
    }

    public const Int32 MAX_ITEM_LOGIC_NUM = 8;
    public const Int32 MAX_SPOT_NAME_LEN = 64;
    public const Int32 MAX_SPOT_ITEM_NUM = 30;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct _PNumeric
    {
        public UInt16 FractionalPart;
        public UInt32 IntegerPart;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct _Logic
	{
		public UInt16 Default;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ITEM_LOGIC_NUM)]
        public UInt16[] Options;	//0xFF表示无选择项
	}
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    struct _PContent
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public _PNumeric[] numeric;

        [FieldOffset(0)]
        public _Logic logic;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SPOTITEM
    {
        public UInt16 isLogicType;         //bit0:标记是逻辑型还是数值型项目(1 = logic,0=numerical)
        public UInt16 ItemUnitIndex;		//单位表序号(数值型才有单位)
        public UInt16 ItemNameOrder;		//项目名称序号
        public _PContent content;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SPOTINFO
    {
        public UInt16 index;
        public UInt16 isFingerCheck;	//bit0:标记是否需要指纹校对身份(1 = 需要,0=no)
        public UInt16 TotalItem;		//本巡逻点有多少个项目
        public UInt32 CardNo;			//卡号

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPOT_NAME_LEN)]
        public Byte[] TagName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPOT_ITEM_NUM)]
        public SPOTITEM[] tagList;
    };


    public struct SPOT_ITEM
    {
        public bool isLogic;                //逻辑型还是数值型项目(true = logic,false=numerical)
        public UInt16 ItemUnitIndex;		//单位表序号(数值型才有单位)
        public UInt16 ItemNameOrder;		//项目名称序号

        public UInt16[] logic;            //5个逻辑项序号,没有的必须为0XFF;
        public UInt16 DefaultLogic;       //默认逻辑选项
        public double NumDefault;       //数值型项目默认值
        public double NumMax;           //数值型项目最大值
        public double NumMin;           //数值型项目最小值
    };
    public struct SPOT_INFO
    {
        public bool isFingerCheck;	    //是否需要指纹校对身份
        public UInt16 TotalItem;		//本巡逻点有多少个项目,最大值为MAX_SPOT_ITEM_NUM
        public UInt32 CardNumber;		//卡号
        public string SpotName;         //最大长度MAX_SPOT_NAME_LEN字节
        public SPOT_ITEM[] ItemList;    //TotalItem个
    };

    private delegate UInt32 Type_GetSpotInfo(byte[] path, IntPtr info);
    private Type_GetSpotInfo funcGetSpotInfo;
    public bool ItemPatrol_GetSpotInfo(string DevicePath,UInt16 index,out SPOT_INFO SpotInfo)
    {
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        IntPtr pBuff = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SPOTINFO)));
        Marshal.WriteInt16(pBuff, (short)index);
        UInt32 ret = funcGetSpotInfo(path, pBuff);
        if (ret != 0)
        {
            pBuff = (IntPtr)(pBuff.ToInt64() + 2);
            UInt16 isFingerCheck = (UInt16)Marshal.ReadInt16(pBuff);
            if ((isFingerCheck & 0x0001) == 0x0001)
                SpotInfo.isFingerCheck  = true;
            else
                SpotInfo.isFingerCheck  = false;
            pBuff = (IntPtr)(pBuff.ToInt64() + 2);

            SpotInfo.TotalItem = (UInt16)Marshal.ReadInt16(pBuff);
            pBuff = (IntPtr)(pBuff.ToInt64() + 2);

            SpotInfo.CardNumber = (UInt32)Marshal.ReadInt32(pBuff);
            pBuff = (IntPtr)(pBuff.ToInt64() + 4);

            Byte[] TagName = new Byte[MAX_SPOT_NAME_LEN];
            Marshal.Copy(pBuff, TagName, 0, MAX_SPOT_NAME_LEN);
            pBuff = (IntPtr)(pBuff.ToInt64() + MAX_SPOT_NAME_LEN);
            SpotInfo.SpotName = Encoding.GetEncoding(936).GetString(TagName);
            SpotInfo.ItemList = new SPOT_ITEM[SpotInfo.TotalItem];
            for (int i = 0; i < SpotInfo.TotalItem; i++)
            {
                UInt16 isLogicType = (UInt16)Marshal.ReadInt16(pBuff);
                pBuff = (IntPtr)(pBuff.ToInt64() + 2);
                if (isLogicType == 0)
                    SpotInfo.ItemList[i].isLogic = false;
                else
                    SpotInfo.ItemList[i].isLogic = true;

                SpotInfo.ItemList[i].ItemUnitIndex = (UInt16)Marshal.ReadInt16(pBuff);
                pBuff = (IntPtr)(pBuff.ToInt64() + 2);
                SpotInfo.ItemList[i].ItemNameOrder = (UInt16)Marshal.ReadInt16(pBuff);
                pBuff = (IntPtr)(pBuff.ToInt64() + 2);

                if (SpotInfo.ItemList[i].isLogic)
                {
                    SpotInfo.ItemList[i].logic = new UInt16[MAX_ITEM_LOGIC_NUM];
                    SpotInfo.ItemList[i].DefaultLogic = (UInt16)Marshal.ReadInt16(pBuff);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 2);
                    for (int j = 0; j < MAX_ITEM_LOGIC_NUM; j++)
                    {
                        SpotInfo.ItemList[i].logic[j] = (UInt16)Marshal.ReadInt16(pBuff);
                        pBuff = (IntPtr)(pBuff.ToInt64() + 2);
                    }
                }
                else
                {
                    UInt16 FractionalPart = (UInt16)Marshal.ReadInt16(pBuff);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 2);
                    UInt32 IntegerPart = (UInt32)Marshal.ReadInt32(pBuff);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 4);
                    SpotInfo.ItemList[i].NumDefault = (IntegerPart & 0x7FFFFFFF);
                    SpotInfo.ItemList[i].NumDefault += ((double)FractionalPart) / 10000;
                    if ((IntegerPart & 0x80000000) != 0)
                        SpotInfo.ItemList[i].NumDefault *= -1;

                    FractionalPart = (UInt16)Marshal.ReadInt16(pBuff);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 2);
                    IntegerPart = (UInt32)Marshal.ReadInt32(pBuff);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 4);
                    SpotInfo.ItemList[i].NumMin = (IntegerPart & 0x7FFFFFFF);
                    SpotInfo.ItemList[i].NumMin += ((double)FractionalPart) / 10000;
                    if ((IntegerPart & 0x80000000) != 0)
                        SpotInfo.ItemList[i].NumMin *= -1;

                    FractionalPart = (UInt16)Marshal.ReadInt16(pBuff);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 2);
                    IntegerPart = (UInt32)Marshal.ReadInt32(pBuff);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 4);
                    SpotInfo.ItemList[i].NumMax = (IntegerPart & 0x7FFFFFFF);
                    SpotInfo.ItemList[i].NumMax += ((double)FractionalPart) / 10000;
                    if ((IntegerPart & 0x80000000) != 0)
                        SpotInfo.ItemList[i].NumMax *= -1;
                }
            }
            return true;
        }
        else
        {
            SpotInfo = new SPOT_INFO();
            return false;
        }        
    }


    private delegate bool Type_SetSpotInfo(byte[] path,IntPtr info);
    private Type_SetSpotInfo funcSetSpotInfo;
    public bool ItemPatrol_SetSpotInfo(string DevicePath, UInt16 index, SPOT_INFO SpotInfo)
    {
        bool ret = false;
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        IntPtr pBuff = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SPOTINFO)));
        IntPtr pStart = pBuff;

        Marshal.WriteInt16(pBuff, (short)index);
        pBuff = (IntPtr)(pBuff.ToInt64() + 2);

        if (SpotInfo.isFingerCheck)
            Marshal.WriteInt16(pBuff, 1);
        else
            Marshal.WriteInt16(pBuff, 0);
        pBuff = (IntPtr)(pBuff.ToInt64() + 2);

        Marshal.WriteInt16(pBuff, (short)SpotInfo.TotalItem);
        pBuff = (IntPtr)(pBuff.ToInt64() + 2);

        Marshal.WriteInt32(pBuff, (int)SpotInfo.CardNumber);
        pBuff = (IntPtr)(pBuff.ToInt64() + 4);

        byte[] gbkname = Encoding.GetEncoding(936).GetBytes(SpotInfo.SpotName);
        if (gbkname.Length <= MAX_SPOT_NAME_LEN - 1)
        {
            Marshal.Copy(gbkname, 0, pBuff, gbkname.Length);
            IntPtr p = (IntPtr)(pBuff.ToInt64() + gbkname.Length);
            Marshal.WriteByte(p, 0);
            p = (IntPtr)(pBuff.ToInt64() + MAX_SPOT_NAME_LEN - 1);
            Marshal.WriteByte(p, 0);
            pBuff = (IntPtr)(pBuff.ToInt64() + MAX_SPOT_NAME_LEN);

            for (int i = 0; i < SpotInfo.TotalItem; i++)
            {
                if (SpotInfo.ItemList[i].isLogic)
                    Marshal.WriteInt16(pBuff,1);
                else
                    Marshal.WriteInt16(pBuff,0);
                pBuff = (IntPtr)(pBuff.ToInt64() + 2);

                Marshal.WriteInt16(pBuff, (short)SpotInfo.ItemList[i].ItemUnitIndex);
                pBuff = (IntPtr)(pBuff.ToInt64() + 2);

                Marshal.WriteInt16(pBuff, (short)SpotInfo.ItemList[i].ItemNameOrder);
                pBuff = (IntPtr)(pBuff.ToInt64() + 2);

                if (SpotInfo.ItemList[i].isLogic)
                {
                    Marshal.WriteInt16(pBuff, (short)SpotInfo.ItemList[i].DefaultLogic);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 2);
                    for (int j = 0; j < SpotInfo.ItemList[i].logic.Length; j++)
                    {
                        Marshal.WriteInt16(pBuff, (short)SpotInfo.ItemList[i].logic[j]);
                        pBuff = (IntPtr)(pBuff.ToInt64() + 2);
                    }
                    for (int j = SpotInfo.ItemList[i].logic.Length; j < MAX_ITEM_LOGIC_NUM; j++)
                    {
                        Marshal.WriteInt16(pBuff, (short)-1);
                        pBuff = (IntPtr)(pBuff.ToInt64() + 2);
                    }
                }
                else
                {
                    UInt16 FractionalPart;
                    UInt32 IntegerPart;

                    if (SpotInfo.ItemList[i].NumDefault < 0)
                    {
                        IntegerPart = 0x80000000;
                        SpotInfo.ItemList[i].NumDefault *= -1;
                    }
                    else
                    {
                        IntegerPart = 0;
                    }

                    UInt32 IntValue = (UInt32)SpotInfo.ItemList[i].NumDefault;
                    IntegerPart += IntValue;
                    SpotInfo.ItemList[i].NumDefault -= IntValue;
                    UInt32 Decimal = (UInt32)(SpotInfo.ItemList[i].NumDefault * 100000);
                    if ((Decimal % 10) >= 5)
                        Decimal += 10;
                    FractionalPart = (UInt16)(Decimal / 10);

                    Marshal.WriteInt16(pBuff, (short)FractionalPart);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 2);

                    Marshal.WriteInt32(pBuff, (int)IntegerPart);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 4);


                    if (SpotInfo.ItemList[i].NumMin < 0)
                    {
                        IntegerPart = 0x80000000;
                        SpotInfo.ItemList[i].NumMin *= -1;
                    }
                    else
                    {
                        IntegerPart = 0;
                    }

                    IntValue = (UInt32)SpotInfo.ItemList[i].NumMin;
                    IntegerPart += IntValue;
                    SpotInfo.ItemList[i].NumMin -= IntValue;
                    Decimal = (UInt32)(SpotInfo.ItemList[i].NumMin * 100000);
                    if ((Decimal % 10) >= 5)
                        Decimal += 10;
                    FractionalPart = (UInt16)(Decimal / 10);

                    Marshal.WriteInt16(pBuff, (short)FractionalPart);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 2);

                    Marshal.WriteInt32(pBuff, (int)IntegerPart);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 4);


                    if (SpotInfo.ItemList[i].NumMax < 0)
                    {
                        IntegerPart = 0x80000000;
                        SpotInfo.ItemList[i].NumMax *= -1;
                    }
                    else
                    {
                        IntegerPart = 0;
                    }
                    IntValue = (UInt32)SpotInfo.ItemList[i].NumMax;
                    IntegerPart += IntValue;
                    SpotInfo.ItemList[i].NumMax -= IntValue;
                    Decimal = (UInt32)(SpotInfo.ItemList[i].NumMax * 100000);
                    if ((Decimal % 10) >= 5)
                        Decimal += 10;
                    FractionalPart = (UInt16)(Decimal / 10);

                    Marshal.WriteInt16(pBuff, (short)FractionalPart);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 2);

                    Marshal.WriteInt32(pBuff, (int)IntegerPart);
                    pBuff = (IntPtr)(pBuff.ToInt64() + 4);

                }
            }

            ret = funcSetSpotInfo(path, pStart);
        }
        Marshal.FreeHGlobal(pStart);
        return ret;
    }

    public const Int32 REC_DATETIME_LEN = 6;
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct _RNumeric
    {
        public UInt16 FractionalPart;
        public UInt32 IntegerPart;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    struct _RContent
    {
        [FieldOffset(0)]
        public _RNumeric numeric;

        [FieldOffset(0)]
        public UInt16 logic;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct RECORDITEM
    {
        public UInt16 isLogic;              //标记是逻辑型还是数值型项目(1 = logic,0=numerical)
        public UInt16 ItemNameIndex;		//项目名称序号
        public _RContent content;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct RECORDINFO
    {
        public UInt16 isFingerCheck;				//bit0标记巡逻人员是指纹登录还是卡登录(1=指纹,0=卡)
                                                    //bit1标记是否有代人巡逻(0:正常巡逻,1:代人巡逻)
                                                    //高8位用REC_OK标记是一条有效记录
        public UInt32 ManCardNumorFingerNo;		    //巡逻人员卡号或者指纹编号
	    public UInt32 PostionCardNum;				//地点卡号

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = REC_DATETIME_LEN)]
	    public Byte[] DateTime;		         //时间日期6bytes(依次为年,月,日,时,分,秒),年从0-100,表示2000-2100年

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPOT_ITEM_NUM)]
        public RECORDITEM[] RecordItem;	     //一个巡逻点最大项目数
    }
    public struct RECORD_ITEM
    {
        public bool isLogic;                //标记是逻辑型还是数值型项目(true = logic,false=numerical)
        public UInt16 ItemNameIndex;		//项目名称序号
        public UInt32 LogicIndex;           //逻辑型记录的逻辑型序号
        public double NumericalValue;       //数值型内容
    }
    public struct ITEMRECORD
    {
        public bool isFingerCheck;
        public bool Authorization;          //是否代检,'false:代检      true:正常巡检
        public UInt32 IDOfInspector;        //如果isFingerCheck==true,此为指纹编号；如果isFingerCheck==false,此为人员卡号
        public UInt32 SpotCardNum;          //地点卡号
        public DateTime time;               //生成记录的时间
        public RECORD_ITEM[] RecordItem;    //有效项目数需要通过地点信息查询
    }
       

    private delegate bool Type_GetItemRecord(byte[] path,UInt32 StartIndex,UInt32 NumberToRead,IntPtr pRecordBuffer,out UInt32 NumberRead);
    private Type_GetItemRecord funcGetItemRecord;
    public bool ItemPatrol_GetItemRecord(string DevicePath, UInt32 StartIndex, out ITEMRECORD[] records, out UInt32 NumberRead)
    {
        bool RetValue = false;
        UInt32 NumberToRead = 20;
        records = new ITEMRECORD[1];
        NumberRead = 0;

        IntPtr pBuff = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RECORDINFO)) * (int)NumberToRead);
        if (pBuff != (IntPtr)0)
        {
            byte[] path = Encoding.ASCII.GetBytes(DevicePath);
            UInt32 ReadNumber;
            if (funcGetItemRecord(path, StartIndex, NumberToRead, pBuff, out ReadNumber))
            {
                records = new ITEMRECORD[ReadNumber];
                for (int i = 0; i < ReadNumber;i++ )
                {
                    RECORDINFO info = (RECORDINFO)Marshal.PtrToStructure((IntPtr)(pBuff.ToInt32() + Marshal.SizeOf(typeof(RECORDINFO))*(int)i), typeof(RECORDINFO));
                    records[i] = new ITEMRECORD();
                    if ((info.isFingerCheck & 0x0001) == 0x0001)
                        records[i].isFingerCheck = true;
                    else
                        records[i].isFingerCheck = false ;
                    if ((info.isFingerCheck & 0x0002) == 0x0002)
                        records[i].Authorization = false;
                    else
                        records[i].Authorization = true;
                    records[i].IDOfInspector = info.ManCardNumorFingerNo;
                    records[i].SpotCardNum = info.PostionCardNum;
                    records[i].time = new DateTime(2000 + info.DateTime[0], info.DateTime[1], info.DateTime[2], info.DateTime[3], info.DateTime[4], info.DateTime[5]);
                    records[i].RecordItem = new RECORD_ITEM[MAX_SPOT_ITEM_NUM];
                    for (int j = 0;j < MAX_SPOT_ITEM_NUM;j++)
                    {
                        if (info.RecordItem[j].isLogic == 1)
                        {
                            records[i].RecordItem[j].isLogic = true;
                            records[i].RecordItem[j].LogicIndex = info.RecordItem[j].content.logic;
                        }
                        else
                        {
                            records[i].RecordItem[j].isLogic = false;
                            records[i].RecordItem[j].NumericalValue = ((double)info.RecordItem[j].content.numeric.FractionalPart)/10000;
                            records[i].RecordItem[j].NumericalValue += info.RecordItem[j].content.numeric.IntegerPart & 0x7FFFFFFF;
                            if ((info.RecordItem[j].content.numeric.IntegerPart & 0x80000000) != 0)
                                records[i].RecordItem[j].NumericalValue *= -1;
                        }
                        records[i].RecordItem[j].ItemNameIndex = info.RecordItem[j].ItemNameIndex;
                    }
                }
                NumberRead = ReadNumber;
                RetValue = true;
            }
            Marshal.FreeHGlobal(pBuff);
        }
        return RetValue;
    }

    /// 项目带GPS记录
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct ItemAndGpsRecord
    {
        /// <summary>
        /// bit0标记巡逻人员是指纹登录还是卡登录(1=指纹,0=卡)
        /// bit1标记是否有代人巡逻(0:正常巡逻,1:代人巡逻)
        /// </summary>
        public UInt16 isFingerCheck;
        //高8位用REC_OK标记是一条有效记录
        /// <summary>
        /// 巡逻人员卡号或者指纹编号
        /// </summary>
        public UInt32 ManCardNumorFingerNo;
        /// <summary>
        /// 地点卡号
        /// </summary>
        public UInt32 PostionCardNum;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = REC_DATETIME_LEN)]
        public Byte[] DateTime;		         //日期6bytes(依次为年,月,日,时,分,秒),年从0-100,表示2000-2100年
        /// <summary>
        /// 一个巡逻点最大项目数
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPOT_ITEM_NUM)]
        public RECORDITEM[] RecordItem;
        /// <summary>
        /// 经度　X(度)*1000000  有效值范围(-179999999 -- +180000000)	小于0:西经　大于0:东经
        /// </summary>
        public int longitude;
        /// <summary>
        /// 纬度　X(度)*1000000  有效值范围(-90000000 -- +90000000)	小于0:南纬　大于0:北纬
        /// </summary>
        public int latitude;
    }

    public bool ItemPatrol_GetItemAndGPSRecord(string DevicePath, UInt16 StartIndex, out ITEMRECORD[] records, out UInt32 NumberRead)
    {
        bool RetValue = false;
        UInt32 NumberToRead = 20;
        records = new ITEMRECORD[1];
        NumberRead = 0;

        IntPtr pBuff = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ItemAndGpsRecord)) * (int)NumberToRead);
        if (pBuff != (IntPtr)0)
        {
            byte[] path = Encoding.ASCII.GetBytes(DevicePath);
            UInt32 ReadNumber;
            if (funcGetItemRecord(path, StartIndex, NumberToRead, pBuff, out ReadNumber))
            {
                records = new ITEMRECORD[ReadNumber];
                for (int i = 0; i < ReadNumber; i++)
                {
                    try
                    {
                        ItemAndGpsRecord info = (ItemAndGpsRecord)Marshal.PtrToStructure((IntPtr)(pBuff.ToInt32() + Marshal.SizeOf(typeof(ItemAndGpsRecord)) * (int)i), typeof(ItemAndGpsRecord));
                        records[i] = new ITEMRECORD();
                        if ((info.isFingerCheck & 0x0001) == 0x0001)
                            records[i].isFingerCheck = true;
                        else
                            records[i].isFingerCheck = false;
                        if ((info.isFingerCheck & 0x0002) == 0x0002)
                            records[i].Authorization = false;
                        else
                            records[i].Authorization = true;
                        records[i].IDOfInspector = info.ManCardNumorFingerNo;
                        records[i].SpotCardNum = info.PostionCardNum;
                        records[i].time = new DateTime(2000 + info.DateTime[0], info.DateTime[1], info.DateTime[2], info.DateTime[3], info.DateTime[4], info.DateTime[5]);
                        records[i].RecordItem = new RECORD_ITEM[MAX_SPOT_ITEM_NUM];
                        for (int j = 0; j < MAX_SPOT_ITEM_NUM; j++)
                        {
                            if (info.RecordItem[j].isLogic == 1)
                            {
                                records[i].RecordItem[j].isLogic = true;
                                records[i].RecordItem[j].LogicIndex = info.RecordItem[j].content.logic;
                            }
                            else
                            {
                                records[i].RecordItem[j].isLogic = false;
                                records[i].RecordItem[j].NumericalValue = ((double)info.RecordItem[j].content.numeric.FractionalPart) / 10000;
                                records[i].RecordItem[j].NumericalValue += info.RecordItem[j].content.numeric.IntegerPart & 0x7FFFFFFF;
                                if ((info.RecordItem[j].content.numeric.IntegerPart & 0x80000000) != 0)
                                    records[i].RecordItem[j].NumericalValue *= -1;
                            }
                            records[i].RecordItem[j].ItemNameIndex = info.RecordItem[j].ItemNameIndex;
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
                NumberRead = ReadNumber;
                RetValue = true;
            }
            Marshal.FreeHGlobal(pBuff);
        }
        return RetValue;
    }



    private delegate bool Type_SetRouteName(byte[] path, UInt16 index, byte[] LineName);
    private Type_SetRouteName funcSetRouteName;
    public bool ItemPatrol_SetLineName(string DevicePath, UInt16 index, string LineName)
    {
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        byte[] lname = Encoding.GetEncoding(936).GetBytes(LineName);
        return funcSetRouteName(path, index, lname);
    }

    private delegate bool Type_SetItemName(byte[] path, UInt16 StartIndex,UInt16 number,IntPtr[] Names);
    private Type_SetItemName funcSetItemName;
    public bool ItemPatrol_SetItemName(string DevicePath, UInt16 index, string[] Names)
    {
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        IntPtr[] names = new IntPtr[Names.Length];
        for (int i = 0; i < Names.Length; i++)
        {
            byte[] lname = Encoding.GetEncoding(936).GetBytes(Names[i]);
            names[i] = Marshal.AllocHGlobal(lname.Length+2);
            Marshal.Copy(lname, 0, names[i], lname.Length);
            Marshal.WriteByte((IntPtr)((int)names[i] + lname.Length), 0);
        }
        bool ret = funcSetItemName(path, index, (UInt16)Names.Length, names);
        for (int i = 0; i < Names.Length; i++)
        {
            Marshal.FreeHGlobal(names[i]);
        }
        return ret;
    }


    private delegate bool Type_SetLogicName(byte[] path, UInt16 StartIndex, UInt16 number, IntPtr[] Names);
    private Type_SetLogicName funcSetLogicName;
    public bool ItemPatrol_SetLogicName(string DevicePath, UInt16 index, string[] Names)
    {
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        IntPtr[] names = new IntPtr[Names.Length];
        for (int i = 0; i < Names.Length; i++)
        {
            byte[] lname = Encoding.GetEncoding(936).GetBytes(Names[i]);
            names[i] = Marshal.AllocHGlobal(lname.Length + 2);
            Marshal.Copy(lname, 0, names[i], lname.Length);
            Marshal.WriteByte((IntPtr)((int)names[i] + lname.Length), 0);
        }
        bool ret = funcSetLogicName(path, index, (UInt16)Names.Length, names);
        for (int i = 0; i < Names.Length; i++)
        {
            Marshal.FreeHGlobal(names[i]);
        }
        return ret;
    }
    


    private delegate bool Type_SetUnitName(byte[] path, UInt16 StartIndex, UInt16 number, IntPtr[] Names);
    private Type_SetUnitName funcSetUnitName;
    public bool ItemPatrol_SetUnitName(string DevicePath, UInt16 index, string[] Names)
    {
        byte[] path = Encoding.ASCII.GetBytes(DevicePath);
        IntPtr[] names = new IntPtr[Names.Length];
        for (int i = 0; i < Names.Length; i++)
        {
            byte[] lname = Encoding.GetEncoding(936).GetBytes(Names[i]);
            names[i] = Marshal.AllocHGlobal(lname.Length + 2);
            Marshal.Copy(lname, 0, names[i], lname.Length);
            Marshal.WriteByte((IntPtr)((int)names[i] + lname.Length), 0);
        }
        bool ret = funcSetUnitName(path, index, (UInt16)Names.Length, names);
        for (int i = 0; i < Names.Length; i++)
        {
            Marshal.FreeHGlobal(names[i]);
        }
        return ret;
    }
}








