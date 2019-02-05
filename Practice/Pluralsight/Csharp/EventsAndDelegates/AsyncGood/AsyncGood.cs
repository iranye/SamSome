using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncGood
{
    public partial class AsyncGood : Form
    {
        private delegate void ShowProgressDelegate(int val);
        private delegate void StartProcessDelegate(int val);

        public AsyncGood()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var startDel = new StartProcessDelegate(StartProcess);
            startDel.BeginInvoke(100, null, null);

            MessageBox.Show("Called after async process started.");
        }

        private void StartProcess(int max)
        {
            ShowProgress(0);
            for (int i = 0; i <= max; i++)
            {
                Thread.Sleep(10);
                ShowProgress(i);
            }
        }

        private void ShowProgress(int i)
        {
            if (lblOutput.InvokeRequired)
            {
                ShowProgressDelegate del = new ShowProgressDelegate(ShowProgress);
                this.BeginInvoke(del, new object[] {i});
            }
            else
            {
                this.lblOutput.Text = i.ToString();
                this.pbStatus.Value = i;
            }
        }
    }
}
