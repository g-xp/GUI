using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace helloword
{
    public partial class FileForm : Form
    {
        private StreamReader sr;
        private Queue<int> dataQueue1 = new Queue<int>(20);
        private Queue<int> dataQueue2 = new Queue<int>(20);

        public FileForm()
        {
            InitializeComponent();
            InitDataQueue();
            this.timer2.Start();
        }

        private void InitDataQueue()
        {
            for (int i = 0;i<20;i++)
            {
                dataQueue1.Enqueue(0);
                dataQueue2.Enqueue(0);
            }
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            this.sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            int i = 0;
            string str, str1, str2;

            if(sr!=null)
            {
                if (sr.Peek() != -1)
                {
                    str = this.sr.ReadLine();
                    i = str.IndexOf(" ");
                    str1 = str.Substring(0, i);
                    str = str.Remove(0, i + 1);
                    str2 = str;
                    dataQueue1.Dequeue();
                    dataQueue2.Dequeue();
                    dataQueue1.Enqueue(Convert.ToInt32(str1));
                    dataQueue2.Enqueue(Convert.ToInt32(str2));
                }
                else
                {
                    this.timer2.Stop();
                    this.sr.Close();
                }
            }
            this.chart1.Series["Series1"].Points.Clear();
            this.chart2.Series["Series1"].Points.Clear();
            for (int n = 0; n < 20; n++)
            {
                chart1.Series["Series1"].Points.AddY(dataQueue1.ElementAt(n));
                chart2.Series["Series1"].Points.AddY(dataQueue2.ElementAt(n));
            }
        }
    }
}
