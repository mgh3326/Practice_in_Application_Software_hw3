using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            //MessageBox.Show(Get_MyIP());

        }
        public string Get_MyIP()
        {
            IPHostEntry host = Dns.GetHostByName(Dns.GetHostName());
            string myip = host.AddressList[0].ToString();
            return myip;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox_IP.Text = Get_MyIP();

        }
        TcpListener server;
        private Thread m_thread;

        public void RUN()
        {
            MessageBox.Show("TTT");

            //this.m_listener = new TcpListener(7777);
            //this.m_listener.Start();

            server = null;
            IPAddress locAddr = IPAddress.Parse(textBox_IP.Text);/* int port = 13000;*/
            try
            {
                 server = new TcpListener(locAddr, Int32.Parse(textBox_PortNumber.Text));
                //server = new TcpListener(IPAddress.Parse("127.0.0.1"), 13002);
                server.Start();
                TextBox_ServerLog.Text += "\nConnected";

                //listening loop // 여기 무한 루프라서 안 되네
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    MessageBox.Show("저 여기서 기다리고 있씁니다.");

                    TcpClient client = server.AcceptTcpClient();
                    MessageBox.Show("여기서 기다릴라나");

                    Console.WriteLine("Conneted!");

                    DateTime t = DateTime.Now;
                    // string to byte
                    string message = string.Format("서버에서 보내는 메세지 {0}", t.ToString("yyyy-MM-dd hh:mm:ss"));
                    byte[] wrtieBuffer = Encoding.UTF8.GetBytes(message);

                    //int to byte
                    int bytes = wrtieBuffer.Length;
                    byte[] wrtieBufferSize = BitConverter.GetBytes(bytes);

                    //send to client
                    NetworkStream stream = client.GetStream();
                    //send Buffer
                    stream.Write(wrtieBufferSize, 0, wrtieBufferSize.Length);
                    Console.WriteLine("Sent: {0}", message);
                    stream.Close();
                    client.Close();
                    Console.WriteLine();
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException:{0}", se);
            }
            finally
            {
                server.Stop();
            }
            Console.WriteLine("\n서버가 종료됩니다.");

        }
        private void button_Start_Click(object sender, EventArgs e)
        {
            ListViewItem item;
            if (button_Start.Text == "Start")
            {
                if (textBox_IP.Text == "" || textBox_PortNumber.Text == "")
                {
                    MessageBox.Show("IP와 Port를 입력하고 Start를 눌러주세요");
                    return;
                }

                button_Start.Text = "Stop";
                button_Start.ForeColor = Color.Red;
                TextBox_ServerLog.Text = "Server 기다리는 중..";
                item = MemberList.Items.Add("1");//여기서 파일 받아오면 될것 같다.
                item.SubItems.Add("root");
                item.SubItems.Add("password");
                this.m_thread = new Thread(new ThreadStart(RUN));
                this.m_thread.Start();
                MessageBox.Show("왜 아무것도 안나오는거야");
                
            }
            else//Stop일 경우
            {
                button_Start.Text = "Start";
                button_Start.ForeColor = Color.Black;
                server.Stop();
                //this.m_networkstream.Close();
                this.m_thread.Abort();
            }


        }
    }
}
