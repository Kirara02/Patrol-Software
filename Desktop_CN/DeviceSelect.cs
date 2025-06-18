using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sample_CSharp2008
{
    public partial class DeviceSelect : Form
    {
        public DeviceSelect()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            (new FormUSBStick()).Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (new FormCOMStick()).Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (new FormEventPatrol(Patrol.DEVICETYPE.DT_Z6500F)).Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new FormGpsEventPatrol(Patrol.DEVICETYPE.DT_Z6900)).Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            (new FormEventPatrol(Patrol.DEVICETYPE.DT_Z6800)).Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            (new FormUSBStick()).Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            (new FormEventPatrol(Patrol.DEVICETYPE.DT_Z6500D)).Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            (new FormGpsEventPatrol(Patrol.DEVICETYPE.DT_Z6700)).Show();
            this.Hide();
        }
    }
}
