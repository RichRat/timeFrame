using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace timeframeServer
{
    class Connection : AbstrConnection
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected static int MSG_INFOLENGTH = 1;
        protected static int MSG_FULL = 0x10;
        protected static int MSG_DELTA = 0x20;
        protected static int MSG_CHAT = 0x80;
        protected static int MSG_RAW = 0x01;
        protected static int MSG_STR = 0x02;

        public Connection(Socket s) : base(s) {}

        override protected void evalMsg(byte[] msg)
        {
            byte infoB = msg[0];
            if ((infoB & MSG_STR) == MSG_STR)
                evalStrMsg(msg, MSG_INFOLENGTH);
        }

        protected void evalStrMsg(byte[] msg, int offset)
        {
            String str = System.Text.Encoding.UTF8.GetString(msg, offset, msg.Length - offset);
            log.Info("Str Msg recieved   >>   " + str);
        }
    }
}
