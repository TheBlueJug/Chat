using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Runtime.CompilerServices;

namespace ChatCore
{

    public class Client
    {
        
        private static Client instance;

        private string port;
        private int ip;

        public static void initialize(string _ip, int _port)
        {
            if(instance == null)
            {
                instance = new Client(_ip, _port);
             
            }
        }

        public static Client Instance
        {
            get 
            {
                if (instance == null)
                {
                    return null;
                }
                return instance;
            }
        }
        private Socket clientSocket;
        IPEndPoint serverEndPoint;

        /*private string message;
        private string prev_message;
        private string message_to_send;*/

        private List<string> messages = new List<string>();

        public int get_messages_quantity()
        {
            return messages.Count;
        }

        public List<string> get_messages_list()
        {
            return messages;
        }
        /*private int messages_lenght;
        private int prev_messages_lenght;*/





        /*public string get_message() { return message; }
        public void set_message(string message) { this.message = message; }

        public string get_prev_message() { return prev_message; }
        public void set_prev_message(string prev_message) { this.prev_message = prev_message; }

        public string get_message_to_send() {  return message_to_send; }
        public void set_message_to_send(string message_to_send) { this.message_to_send = message_to_send; }

        public int get_messages_length() { return messages_lenght; }
        public void set_messages_length(int messages_lenght) { this.messages_lenght =  messages_lenght; }*/



        private Client(string ip, int port)
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
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
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


    }

}

