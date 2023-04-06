namespace BasicProcessesManager
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Diagnostics;

    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            Preparing();
        }

        private bool Preparing()
        {
            bool res = false;
            CenterToScreen();
            UpdateList();
            return res;
        }

        private void AddNewProcess(List<Process> processes)
        {
            List<ListViewItem> lp = new List<ListViewItem>();

            for(int i=0;i<processes.Count;i++)
            {
                ListViewItem lvi;
                if (listView1.Items.Count > 0)
                {
                    lvi = listView1.FindItemWithText(processes[i].Id.ToString(), true, 1);

                    if (lvi is null)
                    {
                        lvi = new ListViewItem(processes[i].ProcessName);
                        lvi.SubItems.Add(processes[i].Id.ToString());
                        lp.Add(lvi);
                    }
                }
                else
                {
                    lvi = new ListViewItem(processes[i].ProcessName);
                    lvi.SubItems.Add(processes[i].Id.ToString());
                    lp.Add(lvi);
                }
            }

            listView1.Items.AddRange(lp.ToArray());
            toolStripStatusLabel2.Text = listView1.Items.Count.ToString();
        }

        private void RemoveDeadProcesses(List<Process> processes)
        {
            for(int i=0 ; i < listView1.Items.Count ; i++)
            {
                int pid = int.Parse(listView1.Items[i].SubItems[1].Text);
                Process pro = processes.Find(p => p.Id == pid);

                if(pro is null)
                {
                    listView1.Items.RemoveAt(i);
                }
            }

            toolStripStatusLabel2.Text = listView1.Items.Count.ToString();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAboutBox fab = new frmAboutBox();
            fab.ShowDialog();
        }

        private void UpdateList()
        {
            List<Process> processes = new List<Process>();
            processes.AddRange(Process.GetProcesses());

            AddNewProcess(processes);
            RemoveDeadProcesses(processes);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void killProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                int index = (listView1.SelectedItems[0].Index);
                int pid = int.Parse(listView1.Items[index].SubItems[1].Text);

                try
                {
                    Process pro = Process.GetProcessById(pid);

                    if(pro is null)
                    {
                        throw new Exception();
                    }

                    pro.Kill();
                }
                catch
                {
                    MessageBox.Show("OOPS, SOMETHING WENT WRONG :(");
                }

            }
        }
    }
}
