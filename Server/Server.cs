﻿using System;
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
        private byte[] sendBuffer = new byte[1024 * 1024 * 1 * 4];
        private byte[] readBuffer = new byte[1024 * 1024 * 1 * 4];
        private bool m_bClientOn = false;
        private Thread m_thread;
        public Initialize m_initializeClass;
        public Join m_joinClass;
        public Search m_searchClass;
        public Upload m_uploadClass;
        public Login m_loginClass;
        private StreamReader m_file_reader = null;
        private StreamWriter m_file_writer = null;
        //TcpListener server;
        public void Send()
        {
            this.m_networkstream.Write(this.sendBuffer, 0, this.sendBuffer.Length);
            this.m_networkstream.Flush();
            for (int i = 0; i < 1024 * 1024 * 4; i++)
                this.sendBuffer[i] = 0;
        }
        public void RUN()
        {
            CheckForIllegalCrossThreadCalls = false;    // cross thread false

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
                    TextBox_ServerLog.AppendText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\test" + "\n");//저장 파일 폴더
                    System.IO.Directory.CreateDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\test");// 없다면 생성 되도록 함
                    if (this.m_file_writer == null)
                        this.m_file_writer = new StreamWriter(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\test" + "\\member.txt", true);
                    m_file_writer.Flush();
                    m_file_writer.Close();
                    m_file_writer = null;
                    this.m_file_reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\test" + "\\member.txt");
                    ////처음으로 돌립니다.
                    //if (this.m_file_reader.Peek() < 0) this.m_file_reader.BaseStream.Seek(0, SeekOrigin.Begin);

                    //리스트뷰에 넣기
                    int i = 1;
                    while (m_file_reader.Peek() >= 0)
                    {
                        ListViewItem item = new ListViewItem(i.ToString());
                        foreach (string ohohstring in m_file_reader.ReadLine().Split(new char[] { ',' }))
                        {
                            item.SubItems.Add(ohohstring);
                        }
                        MemberList.Items.Add(item);
                        i++;
                    }
                    this.m_file_reader.Close();
                    this.m_file_reader = null;
                    //MessageBox.Show("멤버 파일을 모두 읽어 왔습니다.");
                    //this.m_file_writer = File.AppendText("log.txt");
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

                    nRead = this.m_networkstream.Read(readBuffer, 0, 1024 * 1024 * 4);//여기서 멈춰있나

                    Packet packet = (Packet)Packet.Desserialize(this.readBuffer);//이거 까지 올려야되나
                    switch ((int)packet.Type)
                    {

                        case (int)PacketType.회원가입:
                            {
                                this.m_joinClass = (Join)Packet.Desserialize(this.readBuffer);
                                int a = 0;
                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    for (int i = 0; i < MemberList.Items.Count; i++)//같은거 확인하려고 했는데 안되네
                                    {
                                        if (m_joinClass.m_strID.Equals(MemberList.Items[i].SubItems[1].Text))
                                        {
                                            Join joinClass = new Join();
                                            joinClass.Type = (int)PacketType.회원가입;
                                            joinClass.Data = 1;
                                            joinClass.str = "이미 사용중인 ID입니다.";
                                            Packet.Serialize(joinClass).CopyTo(this.sendBuffer, 0);
                                            this.Send();
                                            a = 1;
                                            break;
                                        }
                                    }
                                    if (a != 1)
                                    {
                                        ListViewItem item;
                                        item = MemberList.Items.Add((MemberList.Items.Count + 1).ToString());//오예 추가된다 기모띠
                                        item.SubItems.Add(this.m_joinClass.m_strID);
                                        item.SubItems.Add(this.m_joinClass.m_strPassword);//회원 가입 완료
                                        this.TextBox_ServerLog.AppendText(this.m_joinClass.m_strID + " >> Join\n");
                                        if (this.m_file_writer == null)
                                            this.m_file_writer = new StreamWriter(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\test" + "\\member.txt", true);
                                        System.IO.Directory.CreateDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\test\\"+this.m_joinClass.m_strID);// 개인 폴더 생성

                                        m_file_writer.WriteLine(this.m_joinClass.m_strID +","+ this.m_joinClass.m_strPassword);
                                        m_file_writer.Flush();
                                        m_file_writer.Close();
                                        m_file_writer = null;
                                        //System.IO.Directory.CreateDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\test\\"+ this.m_joinClass.m_strID);// 없다면 생성 되도록 함

                                        Join joinClass = new Join();
                                        joinClass.Type = (int)PacketType.회원가입;
                                        joinClass.Data = 0;
                                        Packet.Serialize(joinClass).CopyTo(this.sendBuffer, 0);
                                        this.Send();
                                    }
                                }));
                                break;
                            }
                        case (int)PacketType.로그인:
                            {
                                this.m_loginClass = (Login)Packet.Desserialize(this.readBuffer);
                                int a = 0;
                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    //MessageBox.Show(MemberList.Items.Count.ToString());//Test
                                    for (int i = 0; i < MemberList.Items.Count; i++)//가입 확인하고 로그인 시켜주자
                                    {
                                        //MessageBox.Show(MemberList.Items[i].SubItems[1].Text);
                                        if (m_loginClass.m_strID.Equals(MemberList.Items[i].SubItems[1].Text))//왜 이게 안되지??
                                        {
                                            this.TextBox_ServerLog.AppendText(this.m_loginClass.m_strID + " >> Login\n");
                                            Login loginClass = new Login();
                                            loginClass.Type = (int)PacketType.로그인;
                                            loginClass.Data = 0;
                                            Packet.Serialize(loginClass).CopyTo(this.sendBuffer, 0);
                                            this.Send();
                                            a = 1;
                                            break;
                                        }
                                    }
                                    if (a != 1)
                                    {
                                        a = 0;
                                        Login loginClass = new Login();
                                        loginClass.Type = (int)PacketType.로그인;
                                        loginClass.Data = 1;
                                        loginClass.str = "ID 또는 PW가 잘못되었습니다.\n계정이 없다면 회원가입 버튼을 통해 계정을 만드십시오!";
                                        Packet.Serialize(loginClass).CopyTo(this.sendBuffer, 0);
                                        this.Send();
                                    }
                                }));
                                break;
                            }
                        case (int)PacketType.조회:
                            {
                                this.m_searchClass = (Search)Packet.Desserialize(this.readBuffer);
                                int a = 0;
                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    List<String> parts = new List<String>();
                                    for (int i = 0; i < MemberList.Items.Count; i++)//같은거 확인하려고 했는데 안되네
                                    {
                                        if (m_searchClass.m_strID.Equals(MemberList.Items[i].SubItems[1].Text))
                                        {
                                            continue;
                                        }
                                        parts.Add(MemberList.Items[i].SubItems[1].Text);
                                        //MessageBox.Show(parts[i]);

                                    }
                                    Search searchClass = new Search();
                                    searchClass.Type = (int)PacketType.조회;
                                    searchClass.Data = 0;
                                    searchClass.m_list = parts;
                                    Packet.Serialize(searchClass).CopyTo(this.sendBuffer, 0);
                                    this.Send();
                                    //MessageBox.Show("보냈쪄");
                                }));
                                break;
                            }
                        case (int)PacketType.업로드:
                            {

                                this.m_uploadClass = (Upload)Packet.Desserialize(this.readBuffer);
                                int a = 0;
                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    const string fileName = "Test#@@#.dat";
                                    using (FileStream
            fileStream = new FileStream(fileName, FileMode.Create))
                                    {
                                        for (int i = 0; i < m_uploadClass.m_byte.Length; i++)
                                        {
                                            fileStream.WriteByte(m_uploadClass.m_byte[i]);
                                        }
                                        Image img = System.Drawing.Image.FromStream(fileStream);
										
										img.Save(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\test\\" + this.m_loginClass.m_strID + "\\" + m_uploadClass.m_filename);
									}
                                    MessageBox.Show("수신 완료\n경로 : " + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\test\\" + this.m_loginClass.m_strID + "\\" + m_uploadClass.m_filename);
									//if (this.m_file_writer == null)
									//	this.m_file_writer = new StreamWriter(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\test" + this.m_loginClass.m_strID + "\\" +"message.txt", true);
									////System.IO.Directory.CreateDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\test\\" + this.m_joinClass.m_strID);// 개인 폴더 생성

									//m_file_writer.WriteLine(m_uploadClass.m_message);
									//m_file_writer.Flush();
									//m_file_writer.Close();
									//m_file_writer = null;

									//MessageBox.Show();


									//Search searchClass = new Search();
									//searchClass.Type = (int)PacketType.조회;
									//searchClass.Data = 0;
									//searchClass.m_list = parts;
									//Packet.Serialize(searchClass).CopyTo(this.sendBuffer, 0);
									//this.Send();
								}));
                                break;
                            }
                    }
                }
                catch
                {
                    this.m_bClientOn = false;
                    this.m_networkstream = null;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_listener.Stop();
            //this.m_networkstream.Close();
            this.m_thread.Abort();
        }
    }
}
