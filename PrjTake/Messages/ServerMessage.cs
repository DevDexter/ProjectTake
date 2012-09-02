using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using PrjTake.Communications.Tcp;

namespace PrjTake.Messages
{
    public class ServerMessage
    {
        internal StringBuilder stringBuilder;

        internal void Init(string stringB)
        {
            stringBuilder = new StringBuilder();
            stringBuilder.Append(stringB);
        }

        internal void AppendString(string stringB)
        {
            stringBuilder.Append((Char)13);
            stringBuilder.Append(stringB);
        }

        internal void AppendInteger(int intB)
        {
            stringBuilder.Append((Char)13);
            stringBuilder.Append(intB);
        }

        internal void Append(string stringB)
        {
            stringBuilder.Append(stringB);
        }

        internal void Append(int intB)
        {
            stringBuilder.Append(intB);
        }

        internal void AppendChar(int i)
        {
            stringBuilder.Append((Char)i);
        }

        internal string toString()
        {
            return stringBuilder.ToString();
        }

        internal void Send(NetworkStream clientStream)
        {
            NewSocket.SendData(clientStream, this);
        }
    }
}
