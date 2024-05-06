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
            /*Client client = new Client(DataBank.ip, DataBank.port);
            client.connect();
            client.nickname_send(DataBank.user_name);*/

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
        private int prev_messages_lenght;

        
        /*private Client client;
        static private string message;
        private string priv_message;
        private string message_to_send;

        static private List<string> messages = new List<string>();
        private int messages_lenght;
        private int prev_messages_lenght;
        class Client
        {
            private Socket clientSocket;
            IPEndPoint serverEndPoint;


            public Client(string ip, int port)
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            }
            public void connect()
            {
                clientSocket.Connect(serverEndPoint);
            }

            public void nickname_send(string nickname)
            {

                byte[] nickname_data = Encoding.UTF8.GetBytes(nickname);
                clientSocket.Send(nickname_data);
            }

            public void start_recive()
            {
                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.Start(clientSocket);
            }


            private void ReceiveMessages(object obj)
            {
                Socket clientSocket = (Socket)obj;
                byte[] buffer = new byte[1024];
                int bytesReceived;

                while (true)
                {
                    try
                    {
                        // Получаем сообщение от сервера
                        bytesReceived = clientSocket.Receive(buffer);
                        if (bytesReceived == 0)
                            break;

                        // Выводим сообщение в консоль
                        message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                        messages.Add(message);
                        //Console.WriteLine(message);
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine($"Ошибка при получении сообщения: {ex.Message}");
                        break;
                    }
                }
            }

            public void send_message(string message_)
            {
                byte[] data = Encoding.UTF8.GetBytes(message_);
                clientSocket.Send(data);

            }


            ~Client()
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }


        }*/



        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // Код для получения новых элементов для ListBox
            List<string> messages_list = Client.Instance.get_messages_list();
            messages_lenght = messages_list.Count();
            if (messages_lenght != prev_messages_lenght)
            {
                listBox1.Items.Add(messages_list[messages_lenght-1]);
            }
            prev_messages_lenght = messages_lenght;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            message_to_send = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Client.Instance.send_message(message_to_send);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
