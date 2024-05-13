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
        private string text_port;
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
            text_port = textBox3.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = true;
            if (user_name == null) {
                flag = false;
                DialogResult result = MessageBox.Show("Не оставляйте ваше имя пустым", "Ошибка");
            }
            int numeric_value;
            if (int.TryParse(text_port, out numeric_value))
            {
                port = Convert.ToInt16(text_port);
            }
            else
            {
                flag = false;
                DialogResult result = MessageBox.Show("Порт должен быть целым числом", "Ошибка");
            }

            if (flag)
            {
                DataBank.user_name = user_name;
                DataBank.port = port;
                DataBank.ip = ip;
                this.Hide();
                frm2 = new Form2();
                frm2.Show();
            }
            
        }
    }

    

    
}
