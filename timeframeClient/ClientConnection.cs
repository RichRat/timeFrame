using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using timeframeServer;

namespace timeframeClient
{
    class ClientConnection : AbstrConnection
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ClientConnection(Socket s) : base(s) 
        {
        }

        public static ClientConnection createConnectionTo(string host, int port) 
        {
            try
            {
                IPHostEntry ipHostEntry = Dns.GetHostEntry(host);
                IPEndPoint remoteEP = new IPEndPoint(ipHostEntry.AddressList[0], port);

                // Create a TCP/IP  socket.  
                Socket sock = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream, 
                ProtocolType.Tcp);
            
                sock.Connect(remoteEP);
                new ClientConnection(sock);
            }
            catch (SocketException e)
            {
                log.Error("Socket Error while connecting!", e);
            }
            catch (ArgumentException e)
            {
                log.Error("Arg Error while connecting!!", e);
            }
            catch (Exception e)
            {
                log.Error("Error connecting!", e);
            }

            return null;
        }
    }


}
