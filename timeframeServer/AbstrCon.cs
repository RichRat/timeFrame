using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace timeframeServer
{
    public abstract class AbstrConnection
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static const int BUFFRSIZE = 1024;
        private static int s_sessionIDCounter = 0;
        public event Action<AbstrConnection> OnClose;

        private static const byte FRAME_START = 0x2A;
        private static const byte FRAME_LEN_LENGTH = 4;

        private readonly int m_sessionID;
        private Socket m_sock;
        private bool m_running = true;

        public Thread MainThread {
            get;
            private set;
        }

        public AbstrConnection(Socket sock)
        {
            m_sock = sock;

            lock (log)
            {
                m_sessionID = s_sessionIDCounter++;
            }
        }

        public AbstrConnection start()
        {
            MainThread = new Thread(listen);

            return this;
        }

        private void listen()
        {
            byte[] buf = new byte[BUFFRSIZE];
            while (m_running)
            {
                int len = m_sock.Receive(buf);
                
            }
        }

        private void evalFrame(byte[] buf, int len) 
        {
            for (int offset = 0; offset < len; offset++)
            {
                int fstart = offset;
                if (buf[offset] != FRAME_START)
                {
                    log.Debug("invalid start byte!");
                    continue;
                }

                offset++;
                int contentLength = 0;
                for (int i = 12; i >= 0; i += 4)
                    contentLength += (int)buf[offset++] << i;

                if (contentLength <= 0 || contentLength > BUFFRSIZE)
                {
                    log.Debug("invalid contentLength!");
                    return;
                }

                byte[] message = new byte[contentLength];
                Buffer.BlockCopy(buf, offset, message, 0, contentLength);
                offset += contentLength;
                byte checksm = buf[offset];
                byte sum = 0;
                for (int i = fstart; i < offset; i++)
                    sum += buf[i];

                if (sum != checksm)
                {
                    log.Error("checksums dont match!");
                    return;
                }

                evalMsg(message);
            }
        }

        protected void evalMsg(byte[] msg);

        private void invokeOnCloseEvent()
        {
            if (OnClose != null)
                OnClose.Invoke(this);
        }

        public void stop() 
        {
            m_running = false;
        }
    }
}
