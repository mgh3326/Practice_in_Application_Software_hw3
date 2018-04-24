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

                TcpClient client = null;


                try
                {
                    client = new TcpClient();
                    IPAddress locAddr = IPAddress.Parse(textBoxIP.Text); int port = Int32.Parse(textBoxPort.Text);
                    client.Connect(locAddr, port);


                    buttonConnect.Text = "Disconnect";//버튼 변경을 여기서 해주면 될까?
                    buttonConnect.ForeColor = Color.Red;
                    textBoxID.ReadOnly = false;
                    textBoxPassword.ReadOnly = false;
                    buttonLogIn.Enabled = true;
                    buttonJoin.Enabled = true;


                    NetworkStream stream = client.GetStream();
                    byte[] readBuffer = new byte[sizeof(int)];

                    //read bufferSize
                    stream.Read(readBuffer, 0, readBuffer.Length);
                    int bufferSize = BitConverter.ToInt32(readBuffer, 0);
                    Console.WriteLine("Received: {0}", bufferSize);

                    //read buffer
                    readBuffer = new byte[bufferSize];
                    int bytes = stream.Read(readBuffer, 0, readBuffer.Length);
                    string message = Encoding.UTF8.GetString(readBuffer, 0, bytes);
                    Console.WriteLine("Received: {0}", message);

                    stream.Close();
                    client.Close();
                    //listening loop
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException:{0}", se);
                    MessageBox.Show("서버 연결중에 오류 발생!");
                }
                finally
                {
                    client.Close();
                }
                Console.WriteLine("Client Exit");
            }
            else//연결 해제
            {
                buttonConnect.Text = "Connect";
                buttonConnect.ForeColor = Color.Black;
                textBoxID.ReadOnly = true;
                textBoxPassword.ReadOnly = true;
                buttonLogIn.Enabled = false;
                buttonJoin.Enabled = false;
            }

        }

        private void buttonJoin_Click(object sender, EventArgs e)
        {
            MessageBox.Show(textBoxID.Text + "랑" + textBoxPassword.Text + "이걸 보내줘야되네");
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(textBoxID.Text + "랑" + textBoxPassword.Text + "이걸 보내줘야되네");
        }
    }
}
