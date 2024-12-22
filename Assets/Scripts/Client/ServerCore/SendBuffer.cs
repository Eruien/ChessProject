using System;
using System.Threading;

namespace ServerCore
{
    public class SendBufferHelper
    {
        static ThreadLocal<SendBuffer> CurrentBuffer = new ThreadLocal<SendBuffer>(()=> { return null; });

        static int ChunkSize { get; set; } = 65535 * 100;

        public static ArraySegment<byte> Open(int reserveSize)
        {
            if (CurrentBuffer.Value == null)
                CurrentBuffer.Value = new SendBuffer(ChunkSize);

            if (CurrentBuffer.Value.FreeSize < reserveSize)
                CurrentBuffer.Value = new SendBuffer(ChunkSize);

            return CurrentBuffer.Value.Open(reserveSize);
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            return CurrentBuffer.Value.Close(usedSize);
        }
    }

    public class SendBuffer
    {
        ArraySegment<byte> m_Buffer;
        int m_UsedSize = 0;

        public int FreeSize { get { return m_Buffer.Count - m_UsedSize; } }

        public SendBuffer(int bufferSize)
        {
            m_Buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        public ArraySegment<byte> Open(int reserveSize)
        {
            if (reserveSize > FreeSize)
                return null;

            return new ArraySegment<byte>(m_Buffer.Array, m_Buffer.Offset + m_UsedSize, reserveSize);
        }

        public ArraySegment<byte> Close(int usedSize)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(m_Buffer.Array, m_Buffer.Offset + m_UsedSize, usedSize);
            m_UsedSize += usedSize;
            return segment;
        }
    }
}
