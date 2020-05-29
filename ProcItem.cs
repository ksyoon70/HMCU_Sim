using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMCU_Sim
{
    public class ProcItem
    {
       public UInt32 [] ProcNum;         ///처리번호
       public Byte seq;               ///전송연번
       public ushort vioNum;         ///위반번호 위반촬영장치 부여
       public bool sndVioReq;        ///위반확인보냄
       public int sndImgCfm;        ///영상확인보냄  
       public UInt32 ProcNumCnt;    ///보낼 처리번호 갯수 ProcNum 갯수 
        public UInt32 curProcNumCnt;    ///현재 보낸 처리번호 갯수
        public ProcItem()
        {
            ProcNum = new UInt32[4];  //처리번호 저장소
            for(int i = 0; i < 4; i++)
            {
                ProcNum[i] = 0;
            }
            seq = 0;
            vioNum = 0;
            sndVioReq = false;
            sndImgCfm = 0;
            ProcNumCnt = 0;
            curProcNumCnt = 0;
        }
    }
}
