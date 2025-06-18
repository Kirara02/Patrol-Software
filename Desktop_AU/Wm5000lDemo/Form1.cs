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
                textBox1.Text = wmport.GetSitus();
            }
            else
                MessageBox.Show("Please open usb");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (opened >= 0)
            {
                textBox1.Text = wmport.GetRecords();
            }
            else
                MessageBox.Show("Please open usb");
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
            if (opened >= 0)
            {
                string rawData = wmport.GetRecords();
                string[] lines = rawData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                string logPath = @"C:\Patrol_Log\Logs";

                if (!System.IO.Directory.Exists(logPath))
                {
                    System.IO.Directory.CreateDirectory(logPath);
                }
                else
                {
                    string[] existingFiles = System.IO.Directory.GetFiles(logPath);
                    foreach (string file in existingFiles)
                    {
                        try
                        {
                            System.IO.File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to delete existing file:\n{file}\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                string filePath = System.IO.Path.Combine(logPath, $"patrol_log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");

                try
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, true))
                    {
                        foreach (var line in lines)
                        {
                            string[] parts = line.Split(',');
                            if (parts.Length == 3)
                            {
                                string cardId = parts[0];

                                string tagRaw = parts[1];
                                string trimmed = tagRaw.TrimStart('0');
                                string tag = trimmed.Length > 10
                                    ? trimmed.Substring(trimmed.Length - 10)
                                    : trimmed.PadLeft(10, '0');

                                string datetime = parts[2];
                                string date = datetime.Substring(0, 4) + "/" + datetime.Substring(4, 2) + "/" + datetime.Substring(6, 2);
                                string time = datetime.Substring(8, 2) + ":" + datetime.Substring(10, 2) + ":" + datetime.Substring(12, 2);

                                string logLine = $"1,1,{tag},{date},{time},{cardId}";
                                writer.WriteLine(logLine);
                            }
                        }
                    }

                    MessageBox.Show("Formatted records saved to file:\n" + filePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "Data saved";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing file:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please open usb");
            }
        }



    }
}
