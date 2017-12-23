using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using log4net;
using System.Net.Sockets;

namespace timeframeServer
{
    class Serv
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private int m_port;
        private bool m_running = true;
        private List<AbstrConnection> m_sessions = new List<AbstrConnection>();

        public Serv(int port)
        {
            m_port = port;
        }

        public void listen() 
        {
            log.Info("Starting Socket Server");
            TcpListener servSocket = new TcpListener(IPAddress.Any, m_port);
            servSocket.Server.ReceiveTimeout = 5000;
            servSocket.Start();
            log.Info("Listiening to ingoing connections ...");

            while (m_running)
            {
                Socket sock = servSocket.AcceptSocket();
                log.Info("ingoing conection >> " + sock.RemoteEndPoint.ToString());
                AbstrConnection s = new Connection(sock);
                s.OnClose += onSessionClosed;
                m_sessions.Add(s.start());
            }

            log.Info("Shutting down!");

            while (m_sessions.Count > 0)
            {
                m_sessions[0].MainThread.Join();
            }

            log.Info("Shut down complete!");
        }

        public void onSessionClosed(AbstrConnection s)
        {
            log.Info("session ended >> " + s.ToString());
            m_sessions.Remove(s);
        }

        public void close()
        {
            m_running = false;
        }
    }
}
