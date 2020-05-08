using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HMCU_Sim
{
    public abstract class CommHandler
    {
        /// <summary>
        /// 송신을 담당한다.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len"></param>
        public abstract void Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state);
        public abstract void Send(byte[] data, int len);
        /// <summary>
        /// 통신을 닫는다.
        /// </summary>
        public abstract void Close();
        /// <summary>
        /// 동작여부를 확인한다.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsRun();

        public abstract int Read(byte[] buffer, int offset, int count);
        /// <summary>
        /// 통신연결 여부를 나타낸다.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsConnected();
    }
}
