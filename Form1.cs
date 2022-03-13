using System;
using System.Drawing;
using System.Windows.Forms;

namespace GramAIO
{
    public partial class Form1 : Form
    {
        bool mouseDown;
        private Point offset;
        public Form1()
        {
            InitializeComponent();
            passwordTextBox.PasswordChar = '*';
        }

        private void usernameTextBox_Click(object sender, EventArgs e)
        {
            if (usernameTextBox.Text == "Username")
            {
                usernameTextBox.Text = "";
            }

        }

        private void passwordTextBox_Click(object sender, EventArgs e)
        {
            if (passwordTextBox.Text == "Password")
            {
                passwordTextBox.Text = "";
            }
        }

        void loginHandle()
        {
            if (usernameTextBox.Text != "Username" && passwordTextBox.Text != "Password" && usernameTextBox.Text != null && passwordTextBox.Text != null)
            {
                Properties.Settings.Default.username = usernameTextBox.Text;
                Properties.Settings.Default.password = passwordTextBox.Text;
                Properties.Settings.Default.Save();
                new GramAIO().Show();
                this.Hide();
            }
            else
            {
                loginBtn.FlatAppearance.BorderColor = Color.Red;
                loginBtn.Text = "Bad Login";
            }
        }
        private void loginBtn_Click(object sender, EventArgs e)
        {
            loginHandle();
        }

        private void usernameTextBox_Leave(object sender, EventArgs e)
        {
            if (usernameTextBox.Text == "")
            {
                usernameTextBox.Text = "Username";
            }
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            if (passwordTextBox.Text == "")
            {
                passwordTextBox.Text = "Password";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mouseDown = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void usernameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                loginHandle();
            }
        }

        private void passwordTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                loginHandle();
            }
        }

        private void usernameTextBox_TextChanged(object sender, EventArgs e)
        {
            loginBtn.FlatAppearance.BorderColor = Color.FromArgb(251, 173, 80);
            loginBtn.Text = "Login";
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            loginBtn.FlatAppearance.BorderColor = Color.FromArgb(251, 173, 80);
            loginBtn.Text = "Login";
        }
    }
}
