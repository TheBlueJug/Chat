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

        

        private List<string> messages = new List<string>();

        public int get_messages_quantity()
        {
            return messages.Count;
        }

        public List<string> get_messages_list()
        {
            return messages;
        }
        



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

                    
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    messages.Add(message);
                    
                }
                catch (Exception ex)
                {
                    
                    break;
                }
            }
        }

        public void send_message(string message_)
        {
            byte[] data = Encoding.UTF8.GetBytes(message_);
            clientSocket.Send(data);

        }

        public void close_connect()
        {
            instance = null;
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            
        }

        ~Client()
        {
            close_connect();
        }


    }

}

