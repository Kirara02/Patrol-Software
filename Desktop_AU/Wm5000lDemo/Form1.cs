using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wm5000AEDemo
{
    public partial class frmMain : Form
    {
        private WMWEBUSBLib.Wwusb wmport;
        private long opened=-1;

        private Timer usbCheckTimer;
        private int loopInterval = 5000;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            wmport = new WMWEBUSBLib.Wwusb();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            ShowState(opened = wmport.OpenUsb(2050));
        }
        private void ShowState(long result) {
            if (result >= 0)
                MessageBox.Show("Success");
            else
                MessageBox.Show("Failure");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (opened >= 0)
            {
                ShowState(wmport.ErasureEvents());
            }
            else
                MessageBox.Show("Please open usb");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (opened >= 0)
            {
                ShowState(wmport.SetGuard(""));
            }
            else
                MessageBox.Show("Please open usb");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (opened >= 0)
            {
                ShowState(wmport.SetSitus(""));
            }
            else
                MessageBox.Show("Please open usb");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (opened >= 0)
            {
                ShowState(wmport.ErasureSitus());
            }
            else
                MessageBox.Show("Please open usb");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (opened >= 0)
            {
                listMessage.Items.Add(wmport.GetSitus());
            }
            else
                MessageBox.Show("Please open usb");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (opened >= 0)
            {
                listMessage.Items.Clear();

                string rawData = wmport.GetRecords();
                string[] lines = rawData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                string dataType = Environment.GetEnvironmentVariable("DATA_TYPE") ?? "1";
                string recorderType = Environment.GetEnvironmentVariable("RECORDER_TYPE") ?? "5";

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

                        string formattedLine = $"{dataType},{recorderType},{tagDecimal},{date},{time},{deviceId}";
                        listMessage.Items.Add(formattedLine);
                    }
                }

                if (lines.Length == 0)
                {
                    listMessage.Items.Add("No records found.");
                }
            }
            else
            {
                MessageBox.Show("Please open usb");
            }
        }


        private void button10_Click(object sender, EventArgs e)
        {
            if (opened >= 0)
            {
                ShowState(wmport.ErasureRecords());
            }
            else
                MessageBox.Show("Please open usb");
        }



        private void button13_Click(object sender, EventArgs e)
        {
            wmport.CloseUsb();
            opened = -1;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (opened >= 0)
            {
                DateTime d =DateTime.Now;
                ShowState(wmport.SetDateTime(d.Year,d.Month,d.Day,d.Hour,d.Minute,d.Second));
            }
            else
                MessageBox.Show("Please open usb");
        }





        private void button17_Click(object sender, EventArgs e)
        {
            if (opened >= 0)
            {
                //textse.Text must be number
                    ShowState(wmport.SetTermno(Convert.ToInt32(textse.Text).ToString()));
            }
            else
                MessageBox.Show("Please open usb");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (opened >= 0)
            {
                // if  wmport.GetTermno() return Value is "" or Value to Number < 0 then fail
                textge.Text = wmport.GetTermno();

            }
            else
                MessageBox.Show("Please open usb");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            listMessage.Items.Clear(); // Kosongkan list sebelum mulai log baru

            if (opened >= 0)
            {
                listMessage.Items.Add("USB connection is open.");
                string rawData = wmport.GetRecords();
                string[] lines = rawData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                listMessage.Items.Add($"Retrieved {lines.Length} record(s) from device.");

                string logPath = Environment.GetEnvironmentVariable("LOG_PATH") ?? @"C:\Patrol_Log\Logs";
                listMessage.Items.Add($"Log path set to: {logPath}");

                string dataType = Environment.GetEnvironmentVariable("DATA_TYPE") ?? "1";
                string recorderType = Environment.GetEnvironmentVariable("RECORDER_TYPE") ?? "5";

                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                    listMessage.Items.Add("Log directory not found, created new directory.");
                }
                else
                {
                    string[] existingFiles = Directory.GetFiles(logPath);
                    listMessage.Items.Add($"Found {existingFiles.Length} existing file(s) to delete.");
                    foreach (string file in existingFiles)
                    {
                        try
                        {
                            File.Delete(file);
                            listMessage.Items.Add($"Deleted file: {Path.GetFileName(file)}");
                        }
                        catch (Exception ex)
                        {
                            string err = $"Failed to delete file: {file}\nError: {ex.Message}";
                            listMessage.Items.Add(err);
                            MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                string filePath = Path.Combine(logPath, $"patrol_log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
                listMessage.Items.Add($"Log file will be saved to: {filePath}");

                try
                {
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
                                listMessage.Items.Add($"Logged: {logLine}");
                            }
                            else
                            {
                                listMessage.Items.Add($"Skipped invalid line: {line}");
                            }
                        }
                    }

                    listMessage.Items.Add("All records saved successfully.");
                    MessageBox.Show("Formatted records saved to file:\n" + filePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    wmport.ErasureRecords();
                    listMessage.Items.Add("Device records erased after saving.");
                    listMessage.Items.Add($"Data saved to: {filePath}");
                }
                catch (Exception ex)
                {
                    string errMsg = "Error writing file:\n" + ex.Message;
                    listMessage.Items.Add(errMsg);
                    MessageBox.Show(errMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                listMessage.Items.Add("USB is not open. Please open USB connection first.");
                MessageBox.Show("Please open usb");
            }
        }
    }
}
