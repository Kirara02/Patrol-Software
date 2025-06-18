using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

class ZVDeviceDLL
{
    [DllImport("kernel32.dll")]
    private extern static IntPtr LoadLibrary(String path);

    [DllImport("kernel32.dll")]
    private extern static IntPtr GetProcAddress(IntPtr lib, String funcName);

    [DllImport("kernel32.dll")]
    private extern static bool FreeLibrary(IntPtr lib);

    [DllImport("kernel32.dll")]
    private extern static int GetLastError();

    private static IntPtr hLib = (IntPtr)0;
    private static ZVDeviceDLL instance = new ZVDeviceDLL();

    private ZVDeviceDLL()
    {
        hLib = LoadLibrary("PCPS.dll");
        if (hLib == (IntPtr)0)
        {
            System.Windows.Forms.MessageBox.Show("Can't load PCPS.dll");
        }
    }

    ~ZVDeviceDLL()
    {
        if (hLib != (IntPtr)0)
            FreeLibrary(hLib);
    }

    public static ZVDeviceDLL GetInstance()
    {   
        return instance;
    }

    public Delegate GetDelegateForFunction(String APIName, Type t)
    {
        if (hLib != (IntPtr)0)
        {
            IntPtr api = GetProcAddress(hLib, APIName);
            return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
        }
        else
        {
            return null;
        }
    }
}
