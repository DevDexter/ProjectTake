using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PrjTake.Messages.Requests;
using System.Reflection;
using PrjTake.Messages;
using PrjTake.Storage;

namespace PrjTake.Communications.Tcp
{
    class NewSocket
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        public static TcpClient tcpClient;
        public static NetworkStream clientStream;
        public static string pktData;
        public static string[] split;
        public static string[] slash;

        public void Server()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 30000);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();

            Console.WriteLine("Server is listening on port 30000.\n");
        }

        public void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();

                while (true)
                {
                    TcpClient client = this.tcpListener.AcceptTcpClient();

                    Console.WriteLine("Open connection [{0}]\n", client.Client.RemoteEndPoint.ToString().Split(':')[0]);
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);
                }
            }
            catch
            {
                Console.WriteLine("SOME FUCKING PROGRAM IS USING PORT 90! RAWR!");
            }
        }

        public void HandleClientComm(object client)
        {
            tcpClient = (TcpClient)client;
            clientStream = tcpClient.GetStream();

            SendData(clientStream, "HELLO");

            byte[] message = new byte[4000];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4000);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                {
                    break;
                }

                ASCIIEncoding encoder = new ASCIIEncoding();
                string NewData = encoder.GetString(message, 0, bytesRead);
                pktData = NewData.Substring(3);
                split = NewSocket.pktData.Split(' ');
                slash = NewSocket.pktData.Split('/');
                string[] split13 = NewData.Split((Char)13);
                string[] RawSplit = NewData.Split(' ');

                /* Register:
                figure=sd=001/0&hr=017/151,109,62&hd=001/216,176,126&ey=002/0&fc=001/216,176,126
                &bd=001/216,176,126&lh=001/216,176,126&rh=001/216,176,126&ch=002/255,179,215&ls=
                country=legreement=1of ur business!lg=005/255,255,255&sh=002/175,220,223
                 */

                if (pktData.Contains("figure="))
                {

                    string name = split13[0].Split(' ')[2].Replace("name=", ""); ;
                    string password = split13[1].Replace("password=", "");
                    string email = split13[2].Replace("email=", "");
                    string figure = split13[3].Replace("figure=", "");
                    string bday = split13[5].Replace("birthday=", "");
                    string phone = split13[6].Replace("phonenumber=", "");
                    string motto = split13[7].Replace("customData=", "");
                    int hasReadAgreement = Convert.ToInt32(split13[8].Replace("has_read_agreement=", ""));
                    string sex = split13[9].Replace("sex=", "");

                    using (DatabaseClient dbClient = Program.Manager.GetClient())
                    {
                        dbClient.AddParamWithValue("username", name);
                        Boolean dbRow = dbClient.ReadBoolean("SELECT * FROM users WHERE username = @username;");

                        if (dbRow == false)
                        {
                            dbClient.AddParamWithValue("name", name);
                            dbClient.AddParamWithValue("pass", password);
                            dbClient.AddParamWithValue("email", email);
                            dbClient.AddParamWithValue("figure", figure);
                            dbClient.AddParamWithValue("motto", motto);
                            dbClient.AddParamWithValue("sex", sex);
                            dbClient.AddParamWithValue("bday", bday);
                            dbClient.ExecuteQuery("INSERT INTO users (username, password, figure, motto, dob, gender, email) VALUES (@name, @pass, @figure, @motto, @bday, @sex, @email)");
                        }
                        else
                        {
                        }
                    }

                    return;
                }

                try
                {
                    Invoke<Packet>(NewData.Split(' ')[2]);
                }
                catch { Console.WriteLine(NewData); }
            }

            Console.WriteLine("Close connection [" + tcpClient.Client.RemoteEndPoint.ToString().Split(':')[0] + "]");
            //tcpClient.Client.Close();
        }

        public void Invoke<T>(string methodName) where T : new()
        {
            T instance = new T();
            MethodInfo method = typeof(T).GetMethod(methodName);
            method.Invoke(instance, null);
        }

        public static void SendData(NetworkStream clientStream, string Data)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();

            byte[] buffer = encoder.GetBytes("#" + Data + (char)13 + "##");
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
        public static void SendData(NetworkStream clientStream, ServerMessage Data)
        {
            NewSocket.SendData(clientStream, Data.toString());
        }

        public static void Alert(string Alert, NetworkStream str)
        {
            ServerMessage Message = new ServerMessage();
            Message.Init("BROADCASTMESSAGE");
            Message.AppendString(Alert);
            SendData(str, Message);
        }
    }
}