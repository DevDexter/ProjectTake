using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrjTake.Communications.Tcp;

namespace PrjTake.Messages.Requests
{
    public partial class Packet
    {
        /// <summary>
        /// 16 - "INITUNITLISTENER"
        /// </summary>
        public void INITUNITLISTENER()
        {
            base.Init("ALLUNITS");
            base.AppendString("Test,0,25,127.0.0.1/127.0.0.1,30000,Test" + (char)9 + "map_test,0,25,notmuch" + (char)13);
            base.Send(clientStream);
        }
        
        /// <summary>
        /// 24 - "SEARCHFLATFORUSER"
        /// </summary>
        public void SEARCHFLATFORUSER()
        {
            NewSocket.SendData(clientStream, "BUSY_FLAT_RESULTS 1" + (char)13 + "1/Test/JoshZ/0//0.0/127.0.0.1/127.0.0.1/30000/0/null/ForMeOnly");
        }

        public void SEARCHBUSYFLATS()
        {
            NewSocket.SendData(clientStream, "BUSY_FLAT_RESULTS 1" + (char)13 + "1/Test/JoshZ/0//0.0/127.0.0.1/127.0.0.1/30000/0/null/ForMeOnly");
        }
    }
}
