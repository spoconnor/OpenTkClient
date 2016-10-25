using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenTkClient
{
    public partial class IntroForm : Form
    {
        public string ServerName { get; set; }

        public IntroForm()
        {
            ServerName = Dns.GetHostName();
            InitializeComponent();
            serverTextBox.Text = ServerName;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            ServerName = serverTextBox.Text;
            this.Close();
        }
    }
}
