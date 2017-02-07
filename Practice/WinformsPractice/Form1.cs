using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace WinformsPractice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Scroll Control", 100);

            for(int i = 0; i < 100; i++)
            {
                listView1.Items.Add(string.Format("Item #{0:000}", i));
            }

            timer1.Interval = 1000;
            timer1.Start();
        }

        private int newItem = 10;
        private bool inspectingList = false;

        private void timer1_Tick(object sender, EventArgs e)
        {
            listView1.Items.Add(string.Format("Item #{0:000}", newItem));
            newItem += 10;

            if (!inspectingList)
            {
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }
            listView1.Refresh();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            inspectingList = true;
            if (listView1.SelectedIndices.Count > 0)
            {
                listView1.Items[listView1.SelectedIndices[0]].EnsureVisible();
            }
        }

        private void listView1_Leave(object sender, EventArgs e)
        {
            inspectingList = false;
        }

        private Thread demoThread;
        private void btnAddOutputUnsafe_Click(object sender, EventArgs e)
        {
            demoThread = new Thread(ThreadProcUnsafe);
            demoThread.Start();
        }

        private void ThreadProcUnsafe()
        {
            txtOutput.Text = "This text was set unsafely";
        }

        private void btnAddOutputSafe_Click(object sender, EventArgs e)
        {
            SetText("This text was set safely.");
        }

        delegate void StringArgReturningVoidDelegate(string text);

        private void SetText(string text)
        {
            if (txtOutput.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetText);
                Invoke(d, new object[] {text});
            }
            else
            {
                txtOutput.Text = text;
            }
        }
    }
}
