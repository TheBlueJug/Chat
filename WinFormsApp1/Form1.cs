namespace WinFormsApp1
{
    using Microsoft.VisualBasic.Devices;
    using System.Net.Sockets;
    using System.Net;
    using System.Text;

    public partial class Form1 : Form
    {
        Form2 frm2;
        public Form1()
        {
            InitializeComponent();
        }
        private string user_name;
        private string ip;
        private int port;

        private void Destroy()
        {
            this.Controls.Clear();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            user_name = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ip = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            port = Convert.ToInt16(textBox3.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataBank.user_name = user_name;
            DataBank.port = port;
            DataBank.ip = ip;

            frm2 = new Form2();
            frm2.Show();

            
        }
    }

    

    
}
