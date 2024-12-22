using System.Collections.Generic;

namespace Assets.Scripts
{
    public class PacketQueue
    {
        public static PacketQueue Instance { get; } = new PacketQueue();

        Queue<IPacket> m_PacketQueue = new Queue<IPacket>();
        object m_Lock = new object();

        public void Push(IPacket packet)
        {
            lock (m_Lock)
            {
                m_PacketQueue.Enqueue(packet);
            }
        }

        public IPacket Pop()
        {
            lock (m_Lock)
            {
                if (m_PacketQueue.Count == 0)
                    return null;

                return m_PacketQueue.Dequeue();
            }
        }
    }
}
