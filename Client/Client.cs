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
        private NetworkStream m_networkstream;
        private TcpClient m_client;
        private byte[] sendBuffer = new byte[1024 * 4];
        private byte[] readBuffer = new byte[1024 * 4];
        private bool m_bConnect = false;
        public Initialize m_initializeClass;
        public Login m_loginClass;
        public void Send()
        {
            this.m_networkstream.Write(this.sendBuffer, 0, this.sendBuffer.Length);
            this.m_networkstream.Flush();
            for (int i = 0; i < 1024 * 4; i++)
                this.sendBuffer[i] = 0;
        }
        private void pictureBoxHome_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxHome.BackColor = SystemColors.ActiveCaption;
        }

        private void pictureBoxHome_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxHome.BackColor = SystemColors.Control;

        }

        private void pictureBoxSerach_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxSerach.BackColor = SystemColors.ActiveCaption;
        }

        private void pictureBoxSerach_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxSerach.BackColor = SystemColors.Control;

        }

        private void pictureBoxUpload_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxUpload.BackColor = SystemColors.ActiveCaption;

        }

        private void pictureBoxUpload_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxUpload.BackColor = SystemColors.Control;

        }

        private void pictureBoxMypage_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxMypage.BackColor = SystemColors.ActiveCaption;

        }

        private void pictureBoxMypage_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxMypage.BackColor = SystemColors.Control;

        }
        private void pictureBoxHome_Click(object sender, EventArgs e)
        {
            panelMypage.Visible = false;

            panelSearch.Visible = false;
            panelUpload.Visible = false;
            panelHome.Visible = true;

        }

        private void pictureBoxSerach_Click(object sender, EventArgs e)
        {
            panelUpload.Visible = false;
            panelHome.Visible = false;
            panelMypage.Visible = false;

            panelSearch.Visible = true;
            //MessageBox.Show("Aaaa");

        }

        private void pictureBoxUpload_Click(object sender, EventArgs e)
        {
            panelSearch.Visible = false;
            panelHome.Visible = false;
            panelMypage.Visible = false;

            panelUpload.Visible = true;
        }

        private void pictureBoxMypage_Click(object sender, EventArgs e)
        {
            panelSearch.Visible = false;
            panelHome.Visible = false;
            panelUpload.Visible = false;
            panelMypage.Visible = true;
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
            //MessageBox.Show(textBoxID.Text + "랑" + textBoxPassword.Text + "이걸 보내줘야되네");
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
        }
    }
}
