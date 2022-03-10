using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GramAIO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void usernameTextBox_Click(object sender, EventArgs e)
        {
            usernameTextBox.Text = "";
        }

        private void passwordTextBox_Click(object sender, EventArgs e)
        {
            passwordTextBox.Text = "";
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if(usernameTextBox.Text == "123" && passwordTextBox.Text == "123")
            {
                new GramAIO().Show();
                this.Hide();               
            }
            else
            {
                MessageBox.Show("Invalid login!");
            }
        }
    }
}
