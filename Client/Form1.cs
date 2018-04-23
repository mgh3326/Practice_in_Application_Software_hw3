using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
