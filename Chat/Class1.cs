

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Network
{
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
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    Console.WriteLine(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при получении сообщения: {ex.Message}");
                    break;
                }
            }
        }

        private void send_message(string message)
        {
            while (true)
            {

                
                byte[] data = Encoding.UTF8.GetBytes(message);
                clientSocket.Send(data);
            }
        }


        ~Client()
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }


    }
}
