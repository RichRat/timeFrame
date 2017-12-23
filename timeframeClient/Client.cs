using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timeframeClient
{
    class Client
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected ClientConnection m_con = null;

        public Client() { }

        public void run()
        {
            m_con = ClientConnection.createConnectionTo("localhost", 4200);
            if (m_con != null)
                m_con.start();
        }
    }
}
