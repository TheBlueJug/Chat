using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ChatCore;
using System.Collections;
namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            
            Client.initialize(DataBank.ip, DataBank.port);


            Client.Instance.connect();
            Client.Instance.nickname_send(DataBank.user_name);

            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 10; // 1 секунда
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();

            Client.Instance.start_recive();

        }

        

        private System.Windows.Forms.Timer updateTimer;

        private string message;
        private string priv_message;
        private string message_to_send;


        private int messages_lenght;
        private int prev_messages_lenght = 0;





        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // Код для получения новых элементов для ListBox
            if (Client.Instance != null)
            {
                

                List<string> messages_list = Client.Instance.get_messages_list();
                

                List<string> listBoxItems = listBox1.Items.Cast<string>().ToList();

                

                if (!messages_list.SequenceEqual(listBoxItems))
                {
                    

                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(messages_list.ToArray());


                }
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            message_to_send = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (message_to_send != null)
            {
                Client.Instance.send_message(message_to_send);
            }
            else
            {
                DialogResult result = MessageBox.Show("Не оставляйте текст своего сообщения пустым", "Ошибка");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /*private Form1 GetForm1()
        {
            return Application.OpenForms.OfType<Form1>().FirstOrDefault();
        }*/
        
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            //Client.Instance.close_connect();

            /*Form1 form1 = GetForm1();
            
            form1.Show();*/

            Form frm1 = Application.OpenForms[0];
            frm1.Show();
            Client.Instance.close_connect();
            prev_messages_lenght = 0;

            
        }
    }
}
