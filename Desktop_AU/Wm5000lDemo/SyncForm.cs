using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wm5000AEDemo
{
    public partial class SyncForm : Form
    {
        private WMWEBUSBLib.Wwusb wmport;
        private long opened = -1;

        private Timer usbCheckTimer;
        private int loopInterval = 5000;

        private bool isConnected = false;

        private string lastDeviceTermno = "";
        private bool hasFetchedRecords = false;

        private Timer countdownTimer;
        private int countdownRemaining;

        public SyncForm()
        {
            InitializeComponent();
        }

        private void SycnForm_Load(object sender, EventArgs e)
        {

            AddMessage("Initializing...");

            wmport = new WMWEBUSBLib.Wwusb();

            lbLog.Text = Environment.GetEnvironmentVariable("LOG_PATH");
            lbInterval.Text = $"{Environment.GetEnvironmentVariable("INTERVAL_LOOP")} seconds";
            lbDtype.Text = Environment.GetEnvironmentVariable("DATA_TYPE");
            lbRtype.Text = Environment.GetEnvironmentVariable("RECORDER_TYPE");

            string loopEnv = Environment.GetEnvironmentVariable("INTERVAL_LOOP");
            if (!string.IsNullOrEmpty(loopEnv) && int.TryParse(loopEnv, out int interval))
            {
                loopInterval = interval * 1000;
            }

            usbCheckTimer = new Timer();
            usbCheckTimer.Interval = loopInterval; // e.g. 5000 ms
            usbCheckTimer.Tick += UsbCheckTimer_Tick;
            usbCheckTimer.Start();

            countdownRemaining = loopInterval / 1000;

            countdownTimer = new Timer();
            countdownTimer.Interval = 1000; // 1 detik
            countdownTimer.Tick += CountdownTimer_Tick;
            countdownTimer.Start();

            AddMessage("Scanning started");
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (countdownRemaining > 0)
            {
                countdownRemaining--;
            }

            int totalSeconds = loopInterval / 1000;
            lbInterval.Text = $"{totalSeconds} seconds ({totalSeconds - countdownRemaining}/{totalSeconds})";
        }

        private void AddMessage(string message, bool withTime = false)
        {
            if (withTime)
                listMessage.Items.Add($"{message} at {DateTime.Now:HH:mm:ss}");
            else
                listMessage.Items.Add(message);

            listMessage.TopIndex = listMessage.Items.Count - 1;
        }

        private void UsbCheckTimer_Tick(object sender, EventArgs e)
        {
            usbCheckTimer.Stop(); // Hindari overlap

            bool wasConnected = isConnected;
            bool nowConnected = false;

            try
            {
                if (opened < 0)
                {
                    opened = wmport.OpenUsb(2050);
                }

                if (opened >= 0)
                {
                    string termno = wmport.GetTermno();
                    if (!string.IsNullOrEmpty(termno))
                    {
                        nowConnected = true;

                        if (!wasConnected && nowConnected)
                        {
                            AddMessage("Device connected");
                        }

                        // Cek apakah TermNo berubah
                        if (termno != lastDeviceTermno)
                        {
                            lastDeviceTermno = termno;
                            AddMessage($"Device found (TermNo: {termno})");
                            hasFetchedRecords = false;
                        }
                    }

                    if (nowConnected && !hasFetchedRecords)
                    {
                        AddMessage("Fetching records");

                        string rawData = wmport.GetRecords();
                        string[] lines = rawData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        if (lines.Length > 0)
                        {
                            string logPath = Environment.GetEnvironmentVariable("LOG_PATH") ?? @"C:\Patrol_Log\Logs";

                            if (!Directory.Exists(logPath))
                            {
                                Directory.CreateDirectory(logPath);
                            }
                            else
                            {
                                foreach (var file in Directory.GetFiles(logPath))
                                {
                                    try { File.Delete(file); } catch { /* Optional: log error */ }
                                }
                            }

                            string dataType = Environment.GetEnvironmentVariable("DATA_TYPE") ?? "1";
                            string recorderType = Environment.GetEnvironmentVariable("RECORDER_TYPE") ?? "5";

                            string filePath = Path.Combine(logPath, $"patrol_log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");

                            AddMessage($"Saving {lines.Length} record(s) to file: {filePath}");

                            using (StreamWriter writer = new StreamWriter(filePath, true))
                            {
                                foreach (var line in lines)
                                {
                                    string[] parts = line.Split(',');
                                    if (parts.Length == 3)
                                    {
                                        string deviceId = parts[0];
                                        string tagRaw = parts[1];
                                        string trimmed = tagRaw.TrimStart('0');
                                        string tag = trimmed.Length > 10
                                            ? trimmed.Substring(trimmed.Length - 10)
                                            : trimmed.PadLeft(10, '0');
                                        ulong tagDecimal = Convert.ToUInt64(tag, 16);

                                        string datetime = parts[2];
                                        string date = datetime.Substring(6, 2) + "/" + datetime.Substring(4, 2) + "/" + datetime.Substring(0, 4);
                                        string time = datetime.Substring(8, 2) + ":" + datetime.Substring(10, 2) + ":" + datetime.Substring(12, 2);

                                        string logLine = $"{dataType},{recorderType},{tagDecimal},{date},{time},{deviceId}";
                                        writer.WriteLine(logLine);
                                    }
                                }
                            }

                            AddMessage("Erasing records from device...");
                            wmport.ErasureRecords();

                            DateTime now = DateTime.Now;
                            AddMessage($"Syncing device time to {now:yyyy-MM-dd HH:mm:ss}");
                            wmport.SetDateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

                            AddMessage("✅ Data saved and device synced", true);

                            hasFetchedRecords = true;
                        }
                        else
                        {
                            // Tambahkan pesan hanya jika belum ada sebelumnya untuk menghindari spam
                            string lastItem = listMessage.Items.Count > 0 ? listMessage.Items[listMessage.Items.Count - 1].ToString() : "";
                            if (!lastItem.StartsWith("No records found"))
                            {
                                AddMessage("No records found");
                            }

                            hasFetchedRecords = true;
                        }
                    }
                }

                // Handle perubahan koneksi
                if (wasConnected && !nowConnected)
                {
                    AddMessage($"Device disconnected");
                    lastDeviceTermno = "";
                    hasFetchedRecords = false;
                }
                else if (!nowConnected)
                {
                    string scanningMessage = "Scanning...";
                    string lastItem = listMessage.Items.Count > 0 ? listMessage.Items[listMessage.Items.Count - 1].ToString() : "";

                    if (!lastItem.StartsWith("Scanning"))
                    {
                        AddMessage(scanningMessage);
                    }
                }

                isConnected = nowConnected;
            }
            catch (Exception ex)
            {
                AddMessage($"❌ Error: {ex.Message}");
            }

            countdownRemaining = loopInterval / 1000;
            usbCheckTimer.Start();
        }
    }
}
