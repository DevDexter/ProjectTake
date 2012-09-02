using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using PrjTake.Communications.Tcp;
using PrjTake.Storage;
using System.Data;

namespace PrjTake.Messages.Requests
{
    public partial class Packet : ServerMessage
    {
        private NetworkStream clientStream = NewSocket.clientStream;
        private string Username;

        /// <summary>
        /// 22 - "VERSIONCHECK"
        /// </summary>
        public void VERSIONCHECK()
        {
            Init("ENCRYPTION_OFF");
            Send(clientStream);

            Init("SECRET_KEY");
            AppendInteger(1337);
            Send(clientStream);
        }

        /// <summary>
        /// 16 - "APPROVENAME"
        /// </summary>
        public void APPROVENAME()
        {
            DatabaseClient dbClient = Program.Manager.GetClient();
            dbClient.AddParamWithValue("name", NewSocket.split[2]);
            bool Row = dbClient.ReadBoolean("SELECT * FROM users WHERE username = @name;");

            if (Row)
            {
                Init("BADNAME");
                Send(clientStream);
            }
        }

        /// <summary>
        /// 15 - "LOGIN"
        /// </summary>
        public void LOGIN()
        {
            string username = NewSocket.split[2];
            string password = NewSocket.split[3];

            using (DatabaseClient dbClient = Program.Manager.GetClient())
            {
                dbClient.AddParamWithValue("username", username);
                dbClient.AddParamWithValue("password", password);

                try
                {
                    string checkdata = dbClient.ReadString("SELECT * FROM users WHERE username = @username AND password = @password");

                    if (checkdata != null)
                    {
                        DataRow dbRow = dbClient.ReadDataRow("SELECT * FROM users WHERE username = @username;");
                        Username = (String)dbRow["username"];

                        base.Init("USEROBJECT");
                        base.AppendString("name=" + (String)dbRow["username"]);
                        base.AppendString("figure=" + (String)dbRow["figure"]);
                        base.AppendString("birthday=" + (String)dbRow["dob"]);
                        base.AppendString("phonenumber=");
                        base.AppendString("customData=" + (String)dbRow["motto"]);
                        base.AppendString("had_read_agreement=1");
                        base.AppendString("sex=" + (String)dbRow["gender"]);
                        base.AppendString("country=en-UK");
                        base.AppendString("has_special_rights=0");
                        base.AppendString("badge_type=1");
                        base.Send(clientStream);
                    }
                    else
                    {
                        NewSocket.Alert("Wrong username/password!", clientStream);
                    }
                }
                catch
                {
                    NewSocket.Alert("Wrong username/password!", clientStream);
                }
            }
        }

        public void INFORETRIEVE()
        {
            string username = NewSocket.split[2];
            string password = NewSocket.split[3];

            using (DatabaseClient dbClient = Program.Manager.GetClient())
            {
                dbClient.AddParamWithValue("username", username);
                dbClient.AddParamWithValue("password", password);

                try
                {
                    string checkdata = dbClient.ReadString("SELECT * FROM users WHERE username = @username AND password = @password");

                    if (checkdata != null)
                    {
                        DataRow dbRow = dbClient.ReadDataRow("SELECT * FROM users WHERE username = @username;");
                        Username = (String)dbRow["username"];

                        ServerMessage fuseMessage = new ServerMessage();
                        fuseMessage.Init("USEROBJECT");
                        fuseMessage.AppendString("name=" + (String)dbRow["username"]);
                        fuseMessage.AppendString("figure=" + (String)dbRow["figure"]);
                        fuseMessage.AppendString("birthday=" + (String)dbRow["dob"]);
                        fuseMessage.AppendString("phonenumber=");
                        fuseMessage.AppendString("customData=" + (String)dbRow["motto"]);
                        fuseMessage.AppendString("had_read_agreement=1");
                        fuseMessage.AppendString("sex=" + (String)dbRow["gender"]);
                        fuseMessage.AppendString("country=nl");
                        fuseMessage.AppendString("has_special_rights=0");
                        fuseMessage.AppendString("badge_type=1");
                        NewSocket.SendData(clientStream, fuseMessage);
                    }
                    else
                    {
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 10 - "GETCREDITS"
        /// </summary>
        public void GETCREDITS()
        {
            base.Init("WALLETBALANCE");
            base.AppendString("1337.0");
            base.Send(clientStream);

            base.Init("MESSENGERREADY");
            base.AppendChar(13);
            base.Send(clientStream);

            base.Init("MESSENGERSMSACCOUNT");
            base.AppendString("I am a BOT:D");
            base.Send(clientStream);
        }
    }
}
