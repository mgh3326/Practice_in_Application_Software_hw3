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
using System.Net.NetworkInformation;
using System.IO;
using PacketLibrary;
namespace Server
{
    public partial class Form1 : Form
    {
        public static string GetPhysicalIPAdress()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                if (addr != null && !addr.Address.ToString().Equals("0.0.0.0"))
                {
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            return String.Empty;
        }
        public Form1()
        {
            InitializeComponent();
            //MessageBox.Show(Get_MyIP());

        }
        private NetworkStream m_networkstream;
        private TcpListener m_listener;
        private byte[] sendBuffer = new byte[1024 * 4];
        private byte[] readBuffer = new byte[1024 * 4];
        private bool m_bClientOn = false;
        private Thread m_thread;
        public Initialize m_initializeClass;
        public Join m_joinClass;

        public Login m_loginClass;
        //TcpListener server;

        public void RUN()
        {
            //MessageBox.Show("TTT");

            //this.m_listener = new TcpListener(7777);
            //this.m_listener.Start();

            //m_listener = null;
            IPAddress locAddr = IPAddress.Parse(textBox_IP.Text);/* int port = 13000;*/

            m_listener = new TcpListener(locAddr, Int32.Parse(textBox_PortNumber.Text));
            //server = new TcpListener(IPAddress.Parse("127.0.0.1"), 13002);
            m_listener.Start();
            if (!this.m_bClientOn)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    TextBox_ServerLog.Text = ("Server - Start\n");
                    TextBox_ServerLog.AppendText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\n");

                }));

            }
            //MessageBox.Show("저 여기서 기다리고 있씁니다.");
            TextBox_ServerLog.AppendText("Waiting for a connection...\n");
            TcpClient client = m_listener.AcceptTcpClient();

            if (client.Connected)
            {
                this.m_bClientOn = true;
                this.Invoke(new MethodInvoker(delegate ()
                {
                    TextBox_ServerLog.AppendText("Client Access!!\n");
                }));
                m_networkstream = client.GetStream();
            }
            //listening loop // 여기 무한 루프라서 안 되네
            int nRead = 0;
            //MessageBox.Show("여기는 오니?");
            while (this.m_bClientOn)
            {

                //MessageBox.Show("TTTTT");

                try
                {
                    nRead = 0;
                    //MessageBox.Show("AAAA");

                    nRead = this.m_networkstream.Read(readBuffer, 0, 1024 * 4);//여기서 멈춰있나

                    Packet packet = (Packet)Packet.Desserialize(this.readBuffer);//이거 까지 올려야되나
                    switch ((int)packet.Type)
                    {
                        case (int)PacketType.초기화:
                            {
                                this.m_initializeClass = (Initialize)Packet.Desserialize(this.readBuffer);
                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    MessageBox.Show("초기화 버튼을 눌려부렸어");
                                    this.TextBox_ServerLog.AppendText("패킷 전송 성공. " + "Initialize Class Data is" + this.m_initializeClass.Data + "\n");

                                }));
                                break;

                            }
                        case (int)PacketType.회원가입:
                            {
                                //MessageBox.Show("어디서 뒤졋지0000");

                                this.m_joinClass = (Join)Packet.Desserialize(this.readBuffer);
                                //MessageBox.Show(MemberList.Items[0].SubItems[1].Text);
                                int a = 0;
                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    for (int i = 0; i < MemberList.Items.Count; i++)//같은거 확인하려고 했는데 안되네
                                    {
                                        if (m_joinClass.m_strID.Equals(MemberList.Items[i].SubItems[1].Text))
                                        {
                                            MessageBox.Show("이미 회원가입이 되어있습니다.");
                                            a = 1;
                                            break;
                                        }
                                    }
                                    if(a!=1)
                                    {
                                        ListViewItem item;
                                        item = MemberList.Items.Add((MemberList.Items.Count + 1).ToString());//오예 추가된다 기모띠
                                        item.SubItems.Add(this.m_joinClass.m_strID);
                                        item.SubItems.Add(this.m_joinClass.m_strPassword);
                                        a = 0;

                                    }


                                    //MessageBox.Show("index = "+MemberList.Items.Count.ToString());



                                    //this.TextBox_ServerLog.AppendText(MemberList.Items[1].SubItems.ToString());


                                }));
                                break;
                            }
                        case (int)PacketType.로그인:
                            {
                                this.m_loginClass = (Login)Packet.Desserialize(this.readBuffer);
                                int a = 0;

                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    for (int i = 0; i < MemberList.Items.Count; i++)//가입 확인하고 로그인 시켜주자
                                    {
                                        if (m_loginClass.m_strID.Equals(MemberList.Items[i].SubItems[1].Text))
                                        {
                                            MessageBox.Show(this.m_loginClass.m_strID +"가 로그인을 하였습니다.");
                                            a = 1;
                                            break;
                                        }
                                    }
                                    if (a != 1)
                                    {
                                        MessageBox.Show(this.m_loginClass.m_strID + "은 가입이 안되있소 가입을 먼저 부탁하오");
                                        a = 0;

                                    }
                                }));
                                break;
                            }

                    }

                }


                catch
                {
                    this.m_bClientOn = false;
                    this.m_networkstream = null;
                    MessageBox.Show("클라이언트가 먼저 뒤진거 같습니다");
                    break;//오 이러니까 되는거 같다. 개꿀 또 안되네 뭐지
                }

            }


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox_IP.Text = GetPhysicalIPAdress();//자신의 IP 받아오기

        }
        private void button_Start_Click(object sender, EventArgs e)
        {

            if (button_Start.Text == "Start")
            {
                if (textBox_IP.Text == "" || textBox_PortNumber.Text == "")
                {
                    MessageBox.Show("IP와 Port를 입력하고 Start를 눌러주세요");
                    return;
                }

                button_Start.Text = "Stop";
                button_Start.ForeColor = Color.Red;
                TextBox_ServerLog.Text = ("Server 기다리는 중..\n");

                this.m_thread = new Thread(new ThreadStart(RUN));
                this.m_thread.Start();
                //MessageBox.Show("왜 아무것도 안나오는거야");
            }
            else//Stop일 경우
            {
                button_Start.Text = "Start";
                button_Start.ForeColor = Color.Black;
                m_listener.Stop();
                //this.m_networkstream.Close();
                this.m_thread.Abort();
            }
        }
    }
}
