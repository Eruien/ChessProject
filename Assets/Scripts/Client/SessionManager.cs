using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts
{
    class SessionManager
    {
        static SessionManager m_SessionMgr = new SessionManager();
        public static SessionManager Instance { get { return m_SessionMgr; } }

        object m_Lock = new object();

        ServerSession m_ServerSession = null;

        List<ServerSession> m_SessionList = new List<ServerSession>();
        
        public ServerSession Create()
        {
            lock (m_Lock)
            {
                m_ServerSession = new ServerSession();
               
                return m_ServerSession;
            }
        }

        public ServerSession GetServerSession()
        {
            lock (m_Lock)
            {
                return m_ServerSession;
            }
        }
    }
}
