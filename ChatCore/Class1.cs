using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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




        private byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC; // Указываем режим CBC
                aesAlg.Padding = PaddingMode.PKCS7; // Указываем заполнение PKCS7
                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC; // Указываем режим CBC
                aesAlg.Padding = PaddingMode.PKCS7; // Указываем заполнение PKCS7
                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;

        }



        private byte[] key;
        private byte[] IV;

        private RSACryptoServiceProvider rsa;
        private byte[] publicKey;
        
        private Client(string ip, int port)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            rsa = new RSACryptoServiceProvider();
            //publicKey = rsa.ToXmlString(false);
            publicKey = rsa.ExportCspBlob(false);
            //privateKey = rsa.ToXmlString(true);
        }

        public void publicKey_send()
        {
            //byte[] publicKey_data = Encoding.UTF8.GetBytes(publicKey);

            //byte[] sizeData = BitConverter.GetBytes(publicKey.Length);
            //clientSocket.Send(sizeData);
            clientSocket.Send(publicKey);
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

        public void recieve_key()
        {
            byte[] buffer = new byte[128];
            int bytesReceived = clientSocket.Receive(buffer);
            key = rsa.Decrypt(buffer, false);
            //key = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            
        }

        public void recieve_IV()
        {
            byte[] buffer = new byte[128];
            int bytesReceived = clientSocket.Receive(buffer);
            IV = rsa.Decrypt(buffer, false);
            //IV = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
        }
        private void ReceiveMessages(object obj)
        {
            Socket clientSocket = (Socket)obj;
            byte[] buffer = new byte[8192];
            int bytesReceived;

            while (true)
            {
                try
                {
                    // Получаем сообщение от сервера
                    bytesReceived = clientSocket.Receive(buffer);
                    if (bytesReceived == 0)
                        break;

                    string recieve_message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    byte[] recieve_message_bytes = Encoding.UTF8.GetBytes(recieve_message);
                    string decrypt_recieve_message = DecryptStringFromBytes_Aes(recieve_message_bytes, key, IV);

                    /*string encryptedMessage = Convert.ToBase64String(buffer);


                    byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(encryptedMessage), false);
                    string decryptedMessage = Encoding.UTF8.GetString(decryptedData);*/
                    messages.Add(decrypt_recieve_message);
                    
                }
                catch (Exception ex)
                {
                    
                    break;
                }
            }
        }

        public void send_message(string message_)
        {
            //byte[] data = Encoding.UTF8.GetBytes(message_);
            byte[] encrypted = EncryptStringToBytes_Aes(message_, key, IV);

            //byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(message_), false);
            clientSocket.Send(encrypted);

        }

        public void close_connect()
        {
            instance = null;
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            
        }

        


    }

}

