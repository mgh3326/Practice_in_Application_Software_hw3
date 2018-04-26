using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketLibrary;
namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private TcpClient m_client;
        private byte[] sendBuffer = new byte[1024 * 4];
        private byte[] readBuffer = new byte[1024 * 4];
        private bool m_bConnect = false;
        public Initialize m_initializeClass;
        public Login m_loginClass;
        private NetworkStream m_networkstream;
        public Error m_errorClass;
        public Search m_searchClass;
        public Upload m_uploadClass;
        public void Send()
        {
            this.m_networkstream.Write(this.sendBuffer, 0, this.sendBuffer.Length);
            this.m_networkstream.Flush();
            for (int i = 0; i < 1024 * 4; i++)
                this.sendBuffer[i] = 0;
        }


        private void pictureBoxHome_Click(object sender, EventArgs e)
        {
            panelMypage.Visible = false;

            panelSearch.Visible = false;
            panelUpload.Visible = false;
            panelHome.Visible = true;
            pictureBoxSerach.BackColor = SystemColors.Control;
            pictureBoxMypage.BackColor = SystemColors.ActiveCaption;
            pictureBoxUpload.BackColor = SystemColors.Control;
            pictureBoxHome.BackColor = SystemColors.Control;
        }

        private void pictureBoxSerach_Click(object sender, EventArgs e)
        {
            pictureBoxSerach.BackColor = SystemColors.ActiveCaption;
            pictureBoxMypage.BackColor = SystemColors.Control;
            pictureBoxUpload.BackColor = SystemColors.Control;
            pictureBoxHome.BackColor = SystemColors.Control;

            panelUpload.Visible = false;
            panelHome.Visible = false;
            panelMypage.Visible = false;
            panelSearch.Visible = true;
            if (!this.m_bConnect)
                return;
            Search searchClass = new Search();
            searchClass.Type = (int)PacketType.조회;
            searchClass.m_strID = this.textBoxID.Text;
            //searchClass.m_strPassword = this.textBoxPassword.Text;
            //MessageBox.Show("어디서 뒤졋지222");

            Packet.Serialize(searchClass).CopyTo(this.sendBuffer, 0);
            this.Send();
            int nRead = 0;

            nRead = 0;
            //MessageBox.Show("AAAA");

            nRead = this.m_networkstream.Read(readBuffer, 0, 1024 * 4);//여기서 멈춰있나

            Packet packet = (Packet)Packet.Desserialize(this.readBuffer);//이거 까지 올려야되나
            switch ((int)packet.Type)
            {
                case (int)PacketType.조회:
                    {
                        this.m_searchClass = (Search)Packet.Desserialize(this.readBuffer);
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            ListBoxSearch.Items.Clear();

                            foreach (string ohho in m_searchClass.m_list)
                            {
                                ListBoxSearch.Items.Add(ohho);
                            }
                        }));
                        break;
                    }
                    //case (int)PacketSendERROR.정상:
                    //    {
                    //        this.m_errorClass = (Error)Packet.Desserialize(this.readBuffer);
                    //        this.Invoke(new MethodInvoker(delegate ()
                    //        {
                    //            //MessageBox.Show("가입 성공");
                    //            //errorClass.str = "이미 사용중인 ID입니다.";


                    //        }));
                    //        break;

                    //    }
                    //case (int)PacketSendERROR.에러:
                    //    {
                    //        this.m_errorClass = (Error)Packet.Desserialize(this.readBuffer);
                    //        this.Invoke(new MethodInvoker(delegate ()
                    //        {
                    //            //MessageBox.Show(this.m_errorClass.str);
                    //            //ListBoxSearch.Items.Add("예시로 일단 이거를 넣어보자");
                    //        }));
                    //        break;

                    //    }

            }
        }

        private void pictureBoxUpload_Click(object sender, EventArgs e)
        {
            panelSearch.Visible = false;
            panelHome.Visible = false;
            panelMypage.Visible = false;

            panelUpload.Visible = true;
            pictureBoxSerach.BackColor = SystemColors.Control;
            pictureBoxMypage.BackColor = SystemColors.Control;
            pictureBoxUpload.BackColor = SystemColors.ActiveCaption;
            pictureBoxHome.BackColor = SystemColors.Control;
            if (!this.m_bConnect)
                return;
            Upload uploadClass = new Upload();
            uploadClass.Type = (int)PacketType.업로드;
            uploadClass.m_strID = this.textBoxID.Text;
            //searchClass.m_strPassword = this.textBoxPassword.Text;
            //MessageBox.Show("어디서 뒤졋지222");

            Packet.Serialize(uploadClass).CopyTo(this.sendBuffer, 0);
            this.Send();
            int nRead = 0;

            nRead = 0;
            //MessageBox.Show("AAAA");

            nRead = this.m_networkstream.Read(readBuffer, 0, 1024 * 4);//여기서 멈춰있나

            Packet packet = (Packet)Packet.Desserialize(this.readBuffer);//이거 까지 올려야되나
            switch ((int)packet.Type)
            {
                case (int)PacketType.조회:
                    {
                        this.m_searchClass = (Search)Packet.Desserialize(this.readBuffer);
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            ListBoxSearch.Items.Clear();

                            foreach (string ohho in m_searchClass.m_list)
                            {
                                ListBoxSearch.Items.Add(ohho);
                            }
                        }));
                        break;
                    }
            }
        }

        private void pictureBoxMypage_Click(object sender, EventArgs e)
        {
            panelSearch.Visible = false;
            panelHome.Visible = false;
            panelUpload.Visible = false;
            panelMypage.Visible = true;
            pictureBoxSerach.BackColor = SystemColors.Control;
            pictureBoxMypage.BackColor = SystemColors.ActiveCaption;
            pictureBoxUpload.BackColor = SystemColors.Control;
            pictureBoxHome.BackColor = SystemColors.Control;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (buttonConnect.Text == "Connect")//연결
            {//다른 버튼들도 활성화 시켜야 겠다. 이거를 짧게 만들어주는게 좋을까??

                //TcpClient client = null;
                this.m_client = new TcpClient();


                try
                {
                    //client = new TcpClient();
                    IPAddress locAddr = IPAddress.Parse(textBoxIP.Text); int port = Int32.Parse(textBoxPort.Text);
                    m_client.Connect(locAddr, port);


                }
                catch (SocketException se)
                {
                    MessageBox.Show("서버 연결중에 오류 발생!");
                    return;
                }
                buttonConnect.Text = "Disconnect";//버튼 변경을 여기서 해주면 될까?
                buttonConnect.ForeColor = Color.Red;
                textBoxID.ReadOnly = false;
                textBoxPassword.ReadOnly = false;
                buttonLogIn.Enabled = true;
                buttonJoin.Enabled = true;
                //                MessageBox.Show("여기는 오니?");
                this.m_bConnect = true;
                this.m_networkstream = this.m_client.GetStream();
            }
            else//연결 해제
            {
                buttonConnect.Text = "Connect";
                buttonConnect.ForeColor = Color.Black;
                textBoxID.ReadOnly = true;
                textBoxPassword.ReadOnly = true;
                buttonLogIn.Enabled = false;
                buttonJoin.Enabled = false;
                this.m_client.Close();
                this.m_networkstream.Close();
                MessageBox.Show("ohohoh");

            }

        }

        private void buttonJoin_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("어디서 뒤졋지111");

            if (!this.m_bConnect)
                return;
            Join joinClass = new Join();
            joinClass.Type = (int)PacketType.회원가입;
            joinClass.m_strID = this.textBoxID.Text;
            joinClass.m_strPassword = this.textBoxPassword.Text;
            //MessageBox.Show("어디서 뒤졋지222");

            Packet.Serialize(joinClass).CopyTo(this.sendBuffer, 0);
            this.Send();
            int nRead = 0;

            nRead = 0;
            //MessageBox.Show("AAAA");

            nRead = this.m_networkstream.Read(readBuffer, 0, 1024 * 4);//여기서 멈춰있나

            Packet packet = (Packet)Packet.Desserialize(this.readBuffer);//이거 까지 올려야되나
            switch ((int)packet.Type)
            {

                case (int)PacketSendERROR.정상:
                    {
                        this.m_errorClass = (Error)Packet.Desserialize(this.readBuffer);
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            //MessageBox.Show("가입 성공");
                            //errorClass.str = "이미 사용중인 ID입니다.";


                        }));
                        break;

                    }
                case (int)PacketSendERROR.에러:
                    {
                        this.m_errorClass = (Error)Packet.Desserialize(this.readBuffer);
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            MessageBox.Show(this.m_errorClass.str);

                        }));
                        break;

                    }

            }
            //MessageBox.Show(textBoxID.Text + "랑" + textBoxPassword.Text + "이걸 보내줘야되네");
            //패킷 수신 해줘야겠네
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            if (!this.m_bConnect)
            {
                return;
            }

            Login login = new Login();
            login.Type = (int)PacketType.로그인;
            login.m_strID = this.textBoxID.Text;
            login.m_strPassword = this.textBoxPassword.Text;

            Packet.Serialize(login).CopyTo(this.sendBuffer, 0);
            this.Send();
            int nRead = 0;

            nRead = 0;
            //MessageBox.Show("AAAA");

            nRead = this.m_networkstream.Read(readBuffer, 0, 1024 * 4);//여기서 멈춰있나

            Packet packet = (Packet)Packet.Desserialize(this.readBuffer);//이거 까지 올려야되나
            switch ((int)packet.Type)
            {

                case (int)PacketSendERROR.정상://정상 로그인
                    {
                        this.m_errorClass = (Error)Packet.Desserialize(this.readBuffer);
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            //MessageBox.Show("가입 성공");
                            //errorClass.str = "이미 사용중인 ID입니다.";
                            if (buttonLogIn.Text == "로그인")
                            {
                                buttonLogIn.ForeColor = Color.Red;
                                buttonLogIn.Text = "로그아웃";

                            }
                            else
                            {
                                buttonLogIn.ForeColor = Color.Black;
                                buttonLogIn.Text = "로그인";
                            }
                        }));
                        break;

                    }
                case (int)PacketSendERROR.에러://로그인 못함
                    {
                        this.m_errorClass = (Error)Packet.Desserialize(this.readBuffer);
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            MessageBox.Show(this.m_errorClass.str);

                        }));
                        break;

                    }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            ListBoxSearch.Items.Clear();
            foreach (string ohoh in m_searchClass.m_list)
            {

                if (ohoh.StartsWith(textBoxSearch.Text) == true)
                {
                    ListBoxSearch.Items.Add(ohoh);
                }
            }

        }
    }
}