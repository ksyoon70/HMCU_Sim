using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMCU_Sim
{
    public class RecvBufferStruct
    {
        public byte[] buff = new byte[1024];
        public int buffLen;

        public RecvBufferStruct()
        {
            init();
        }
        public void init()
        {
            buff = new byte[1024];
            buffLen = 0;
        }

        public void reset()
        {
            buffLen = 0;
        }
    }
}
