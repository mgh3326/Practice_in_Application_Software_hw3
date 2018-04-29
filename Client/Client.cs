using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ofd.Filter = "Images Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png|" + "All files (*.*)|*.*";
            ofd.Title = "My Tet Browser";
            ofd.FileName = "";
        }
        private TcpClient m_client;
        private byte[] sendBuffer = new byte[1024 * 1024 * 1 * 4];
        private byte[] readBuffer = new byte[1024 * 1024 * 1 * 4];
        private bool m_bConnect = false;
        public Initialize m_initializeClass;
        public Login m_loginClass;
        private NetworkStream m_networkstream;
        public Error m_errorClass;
        public Search m_searchClass;
        public Upload m_uploadClass;
        public FileStream m_fileStream;
        private Thread m_thread;
        public Join m_joinClass;
        public string ohoh_filename;
        public byte[] ohoh_array;
        private StreamReader f_reader = null;
        private StreamWriter f_writer = null;
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
            textBoxIP.ReadOnly = true;
            textBoxPort.ReadOnly = true;
            //textBox_PortNumber.ReadOnly = true;
            buttonLogIn.Enabled = true;
            buttonJoin.Enabled = true;
            //                MessageBox.Show("여기는 오니?");
            this.m_bConnect = true;
            this.m_networkstream = this.m_client.GetStream();
            int nRead = 0;

            while (this.m_bConnect)
            {

                nRead = 0;

                nRead = this.m_networkstream.Read(readBuffer, 0, 1024 * 1024 * 4);//여기서 멈춰있나


                Packet packet = (Packet)Packet.Desserialize(this.readBuffer);//이거 까지 올려야되나
                try
                {
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
                                //MessageBox.Show("받았쪄");
                                break;
                            }
                        case (int)PacketType.로그인://정상 로그인
                            {

                                this.m_loginClass = (Login)Packet.Desserialize(this.readBuffer);
                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    if (m_loginClass.Data == 0)
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
                                    }
                                    else
                                    {
                                        MessageBox.Show(this.m_loginClass.str);

                                    }
                                }));
                                break;
                            }
                        case (int)PacketType.회원가입:
                            {
                                this.m_joinClass = (Join)Packet.Desserialize(this.readBuffer);
                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    if (m_joinClass.Data == 1)
                                    {
                                        MessageBox.Show(this.m_joinClass.str);
                                    }


                                    //MessageBox.Show("가입 성공");
                                    //errorClass.str = "이미 사용중인 ID입니다.";
                                }));

                                break;

                            }
                    }
                }
                catch
                {
                    this.m_bConnect = false;
                    this.m_networkstream = null;
                    MessageBox.Show("클라이언트 종료");
                    break;//오 이러니까 되는거 같다. 개꿀 또 안되네 뭐지
                }
            }


        }


        private void pictureBoxHome_Click(object sender, EventArgs e)
        {
            panelMypage.Visible = false;

            panelSearch.Visible = false;
            panelUpload.Visible = false;
            panelHome.Visible = true;
            pictureBoxSerach.BackColor = SystemColors.Control;
            pictureBoxMypage.BackColor = SystemColors.Control;
            pictureBoxUpload.BackColor = SystemColors.Control;
            pictureBoxHome.BackColor = SystemColors.ActiveCaption;
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
            if (textBoxPort.Text == "" || textBoxIP.Text == "")
                MessageBox.Show("IP와 Port 번호를 채우고 눌러주세요");
            if (buttonConnect.Text == "Connect")//연결
            {//다른 버튼들도 활성화 시켜야 겠다. 이거를 짧게 만들어주는게 좋을까??
                this.m_thread = new Thread(new ThreadStart(RUN));
                this.m_thread.Start();
                //TcpClient client = null;
            }
            else//연결 해제
            {
                buttonConnect.Text = "Connect";
                buttonConnect.ForeColor = Color.Black;
                textBoxID.ReadOnly = true;
                textBoxPassword.ReadOnly = true;
                buttonLogIn.Enabled = false;
                buttonJoin.Enabled = false;
                textBoxIP.ReadOnly = true;
                textBoxPort.ReadOnly = true;
                this.m_client.Close();
                this.m_networkstream.Close();
                this.m_thread.Abort();
                //MessageBox.Show("ohohoh");
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


        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            ListBoxSearch.Items.Clear();
            foreach (string ohoh in m_searchClass.m_list)
            {

                if (ohoh.Contains(textBoxSearch.Text) == true)
                {
                    ListBoxSearch.Items.Add(ohoh);
                }
            }

        }

        private void buttonFindPicture_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show(ofd.FileName);
                m_fileStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                ohoh_filename = ofd.SafeFileName;
                textBoxPicturepath.Text = ofd.FileName;
                //ohoh_array.Initialize();
                ohoh_array = File.ReadAllBytes(ofd.FileName);
                pictureBoxUploadPicture.Image = System.Drawing.Image.FromFile(ofd.FileName);
                //Image img = System.Drawing.Image.FromStream(m_fileStream);
                //MessageBox.Show(ofd.SafeFileName);
                //img.Save(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\myImage");
                ////byte[] buff = System.IO.File.ReadAllBytes(ofd.FileName);
                ////System.IO.MemoryStream ms = new System.IO.MemoryStream(buff);
                //MessageBox.Show(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\myImage.Jpeg");

                ofd.FileName = "";
            }
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            //textBoxUpload.Text;


            if (!this.m_bConnect)
                return;
            if (textBoxPicturepath.Text == "")
            {
                MessageBox.Show("사진을 선택해주세요");
                return;
            }

            Upload uploadClass = new Upload();
            uploadClass.Type = (int)PacketType.업로드;
            uploadClass.m_strID = this.textBoxID.Text;
            uploadClass.m_message = this.textBoxUpload.Text;
            //uploadClass.m_file = this.m_fileStream;
            uploadClass.m_filename = ohoh_filename;
            ohoh_filename = "";
            uploadClass.m_byte = ohoh_array;
            PictureBox pictureBox1 = new PictureBox();
            //pictureBox1.Location = new System.Drawing.Point(79, 40);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Image = Image.FromFile(textBoxPicturepath.Text);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            panelImage.Controls.Add(pictureBox1);


            //panelImage.BackgroundImage = Image.FromFile(textBoxPicturepath.Text);
            labelPostCounting.Text = (Int32.Parse(labelPostCounting.Text) + 1).ToString();

            //searchClass.m_strPassword = this.textBoxPassword.Text;
            //MessageBox.Show("어디서 뒤졋지222");

            Packet.Serialize(uploadClass).CopyTo(this.sendBuffer, 0);
            this.Send();
            for (int i = 0; i < uploadClass.m_byte.Length; i++)
                ohoh_array[i] = 0;


			//MessageBox.Show("AAAA");

			//nRead = this.m_networkstream.Read(readBuffer, 0, 1024*1024 * 4);//여기서 멈춰있나
			MessageBox.Show("전송 완료!");
			//Packet packet = (Packet)Packet.Desserialize(this.readBuffer);//이거 까지 올려야되나
			//switch ((int)packet.Type)
			//{
			//    case (int)PacketType.업로드:
			//        {
			//            this.m_searchClass = (Search)Packet.Desserialize(this.readBuffer);
			//            this.Invoke(new MethodInvoker(delegate ()
			//            {
			//                ListBoxSearch.Items.Clear();

			//                foreach (string ohho in m_searchClass.m_list)
			//                {
			//                    ListBoxSearch.Items.Add(ohho);
			//                }
			//            }));
			//            break;
			//        }
			//}
		}

        private void buttonProfileEdit_Click(object sender, EventArgs e)
        {
			MessageBox.Show("프로필이 수정되었습니다.");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.m_client.Close();
            this.m_networkstream.Close();
            this.m_thread.Abort();
        }

		private void pictureBoxProfileImage_Click(object sender, EventArgs e)
		{
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				//MessageBox.Show(ofd.FileName);
				m_fileStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
				pictureBoxProfileImage.Image = System.Drawing.Image.FromFile(ofd.FileName);

				//Image img = System.Drawing.Image.FromStream(m_fileStream);
				//MessageBox.Show(ofd.SafeFileName);
				//img.Save(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\myImage");
				////byte[] buff = System.IO.File.ReadAllBytes(ofd.FileName);
				////System.IO.MemoryStream ms = new System.IO.MemoryStream(buff);
				//MessageBox.Show(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\myImage.Jpeg");

				ofd.FileName = "";
			}
		}
	}
}