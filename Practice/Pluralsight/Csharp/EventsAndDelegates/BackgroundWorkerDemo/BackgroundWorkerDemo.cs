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

namespace BackgroundWorkerDemo
{
    public partial class BackgroundWorkerDemo : Form
    {
        private delegate void ShowProgressDelegate(int val);
        private delegate void StartProcessDelegate(int val);

        public BackgroundWorkerDemo()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            btnCancel.Enabled = false;
        }

        private void ShowProgress(int i)
        {
            //On helper thread so invoke on UI thread to avoid 
            //updating UI controls from alternate thread			
            if (lblProgressNumeric.InvokeRequired == true)
            {
                ShowProgressDelegate del = new ShowProgressDelegate(ShowProgress);
                //this.BeginInvoke executes delegate on thread used by form (UI thread)
                this.BeginInvoke(del, new object[] { i });
            }
            else
            { //On UI thread so we are safe to update
                this.lblProgressNumeric.Text = i.ToString();
            }
        }

        //Called Asynchronously
        private void StartProcess(int max)
        {
            ShowProgress(0);
            for (int i = 0; i <= max; i++)
            {
                Thread.Sleep(10);
                ShowProgress(i);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var startDel = new StartProcessDelegate(StartProcess);
            startDel.BeginInvoke(100, null, null);
            btnStart.Enabled = false;
            btnCancel.Enabled = true;
            lblOutput.Text = "";
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = Calculate(sender as BackgroundWorker, e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.Enabled = true;
            progressBar1.Value = 0;
            if (!e.Cancelled)
            {
                lblOutput.Text = "BackgroundWorker Completed";
            }
            else
            {
                lblOutput.Text = "Cancelled";
            }
        }

        private long Calculate(BackgroundWorker instance, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                if (instance.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                Thread.Sleep(100);
                instance.ReportProgress(i);
            }

            return 0L;
        }
    }
}
