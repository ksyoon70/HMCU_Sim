using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMCU_Sim
{
    public class ProcItem
    {
        public UInt32[] ProcNum;        ///처리번호
        public Byte seq;                ///전송연번
        public ushort vioNum;            ///위반번호 위반촬영장치 부여
        public bool sndVioReq;          ///위반확인보냄
        public UInt32 curCfmCnt;        ///영상확인보냄  
        public UInt32 resNumCnt;        ///위반응답 처리 갯수 갯수 
        public UInt32 procNumTotal;     //현재 아이템의 처리번호 갯수
        public int tickCount;
        public ProcItem()
        {
            ProcNum = new UInt32[4];  //처리번호 저장소
            for (int i = 0; i < 4; i++)
            {
                ProcNum[i] = 0;
            }
            seq = 0;
            vioNum = 0;
            sndVioReq = false;
            curCfmCnt = 0;
            resNumCnt = 0;
            procNumTotal = 1;
            tickCount = Environment.TickCount;
        }
        public ProcItem(UInt32 param )
        {
            ProcNum = new UInt32[4];  //처리번호 저장소
            for (int i = 0; i < 4; i++)
            {
                ProcNum[i] = 0;
            }
            seq = 0;
            vioNum = 0;
            sndVioReq = false;
            curCfmCnt = 0;
            resNumCnt = 0;
            procNumTotal = param;
            tickCount = Environment.TickCount;
        }
    }
}
