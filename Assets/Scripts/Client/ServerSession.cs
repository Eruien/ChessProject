using ServerCore;
using System;

namespace Assets.Scripts
{
    class ServerSession : PacketSession
    {
        public override void OnConnect()
        {
            Console.WriteLine("연결됬을때 하는 행동");
        }

        public override void OnDisconnect()
        {
            throw new NotImplementedException();
        }

        public override int OnRecvPacket(ArraySegment<byte> buffer)
        {
            return PacketManager.Instance.OnRecvPacket(this, buffer, (s, p) => PacketQueue.Instance.Push(p));
        }

        public override void OnSend(int numOfBytes)
        {
            throw new NotImplementedException();
        }
    }
}
