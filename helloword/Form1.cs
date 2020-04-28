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
    public partial class Form1 : Form
    {
        private Queue<int> dataQueue1 = new Queue<int>(20);
        private Queue<int> dataQueue2 = new Queue<int>(20);
        public Form1()
        {
            InitializeComponent();
            InitQueue();
            this.timer1.Start();
        }

        private  void InitQueue()
        {
            for (int i = 0;i<20;i++)
            {
                dataQueue1.Enqueue(0);
                dataQueue2.Enqueue(0);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox5.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //将可能产生异常的代码放置在try块中
                //根据当前串口属性来判断是否打开
                if (serialPort1.IsOpen)
                {
                    //串口已经处于打开状态
                    serialPort1.Close();    //关闭串口
                    button1.Text = "打开串口";
                    button1.BackColor = Color.ForestGreen;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    comboBox5.Enabled = true;
                    textBox_receive.Text = "";  //清空接收区
                    //textBox_send.Text = "";     //清空发送区
                }
                else
                {
                    //串口已经处于关闭状态，则设置好串口属性后打开
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    serialPort1.PortName = comboBox5.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox1.Text);
                    serialPort1.DataBits = Convert.ToInt16(comboBox2.Text);

                    if (comboBox3.Text.Equals("None"))
                        serialPort1.Parity = System.IO.Ports.Parity.None;
                    else if (comboBox3.Text.Equals("Odd"))
                        serialPort1.Parity = System.IO.Ports.Parity.Odd;
                    else if (comboBox3.Text.Equals("Even"))
                        serialPort1.Parity = System.IO.Ports.Parity.Even;
                    else if (comboBox3.Text.Equals("Mark"))
                        serialPort1.Parity = System.IO.Ports.Parity.Mark;
                    else if (comboBox3.Text.Equals("Space"))
                        serialPort1.Parity = System.IO.Ports.Parity.Space;

                    if (comboBox4.Text.Equals("1"))
                        serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    else if (comboBox4.Text.Equals("1.5"))
                        serialPort1.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    else if (comboBox4.Text.Equals("2"))
                        serialPort1.StopBits = System.IO.Ports.StopBits.Two;

                    serialPort1.Open();     //打开串口
                    button1.Text = "关闭串口";
                    button1.BackColor = Color.Firebrick;
                }
            }
            catch (Exception ex)
            {
                //捕获可能发生的异常并进行处理

                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox5.Items.Clear();
                comboBox5.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button1.Text = "打开串口";
                button1.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    //串口处于开启状态，将发送区文本发送
                    serialPort1.Write("00000001");
                }*/
                this.Hide();
                FileForm fileForm = new FileForm();
                fileForm.ShowDialog();
                fileForm.Dispose();
                this.Show();
            }
            catch (Exception ex)
            {
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button1.Text = "打开串口";
                button1.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
            }
        }

        public static int i = 0;
        public static string[] save_str = new string[5];
        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int n = 10, m;
            string str1;
            string str2;
            try
            {
                //因为要访问UI资源，所以需要使用invoke方式同步ui
                this.Invoke((EventHandler)(delegate
                {
                    string str = serialPort1.ReadExisting();
                    n = str.IndexOf("01");
                    if (n == 0)
                    {
                        m = str.IndexOf("e");
                        str1 = str.Substring(2, m - 2);
                        label6.Text=str1;
                        str = str.Remove(0, m + 1);
                        n = str.IndexOf("02");
                        if (n == 0)
                        {
                            m = str.IndexOf("e");
                            str2 = str.Substring(2, m - 2);
                            label9.Text = str2;
                            str = str.Remove(0, m + 1);

                            save_str[i] = str1 +" " + str2;
                            dataQueue1.Dequeue();
                            dataQueue2.Dequeue();
                            dataQueue1.Enqueue(Convert.ToInt32(str1));
                            dataQueue2.Enqueue(Convert.ToInt32(str2));
                            if(i==4)
                            {
                                i = -1;
                                string fname = "C:\\Users\\18800\\Desktop\\" + 
                                        DateTime.Now.ToLongDateString()+ ".txt";
                                StreamWriter sw = new StreamWriter(fname,true);
                                for (int j=0;j<5;j++)
                                {
                                    sw.WriteLine(save_str[j]);
                                }
                                sw.Close();
                            }
                            i++;
                        }
                        

                    }
                    else
                        textBox_receive.AppendText(str+"\r\n");
                }
                   )
                );

            }
            catch (Exception ex)
            {
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(ex.Message);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    //串口处于开启状态，将发送区文本发送
                    serialPort1.Write("00000002");
                }
            }
            catch (Exception ex)
            {
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button1.Text = "打开串口";
                button1.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    //串口处于开启状态，将发送区文本发送
                    serialPort1.Write("00000003");
                }
            }
            catch (Exception ex)
            {
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button1.Text = "打开串口";
                button1.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.chart1.Series["Series1"].Points.Clear();
            this.chart2.Series["Series1"].Points.Clear();
            for (int i = 0;i < 20;i++)
            {
                this.chart1.Series["Series1"].Points.AddY(dataQueue1.ElementAt(i));
                this.chart2.Series["Series1"].Points.AddY(dataQueue2.ElementAt(i));
            }
           
        }
    }
}
