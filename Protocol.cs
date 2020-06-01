using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMCU_Sim
{
    static class Protocols
    {
        public const byte STX = 0x02;
        public const byte ETX = 0x03;
        public const byte DLE = 0x10;

        public const UInt16 ACK = 0x01;
        public const UInt16 NACK = 0x02;
        public const UInt16 HEATBEAT = 0x03;
        public const UInt16 STATE = 0x04;
        public const UInt16 IMG_INFO = 0x4101;
        public const UInt16 IMG_MATCH_INFO = 0x1401;

        public const int MAX_BUF_SIZE = 128;
    }

    static class Code
    {
        public const byte ACK = 0x06;
        public const byte NACK = 0x15;
        public const byte STATUS_REQ = 0x11;        //상태요청                   
        public const byte STATUS_RES = 0x21;        //상태응답
        public const byte WORK_START = 0x31;        //근무시작
        public const byte WORK_END = 0x32;          //근무종료
        public const byte VIO_CONFIRM_REQ = 0x41;   //위반확인요구
        public const byte VIO_CONFIRM_RES = 0x42;   //위반확인응답
        public const byte VIO_CONFIRM_RES_N = 0x43;   //위반확인응답
        public const byte PLATE_RECOG_NOTIFY = 0x44;       //번호판인식결과 통보 
        public const byte IMAGE_CONFIRM = 0x45;       //영상확정 
        public const byte VIO_NUMBER_SYNC = 0x25;          //위반번호 Sync
        public const byte IN_SPECIAL_ISSUE_START_NOTIFY = 0x51;     //입구특별발행개시 통보
        public const byte IN_SPECIAL_ISSUE_END_NOTIFY = 0x52;     //입구특별발행종료 통보
    }

    static class Frame
    {
        public const byte Len = 0x01;
        public const byte Code = 0x02;
        public const byte Seq = 0x03;
        public const byte Data = 0x04;
    }


    public abstract class FrameHeader
    {
        public byte FLen = 10;
        public byte TimeLen = 14;
        public byte TLen = 48;
        public byte HeartBeatLen = 9;
        public byte ConfirmLen = 29;
        public byte ConfirmNewLen = 41;
        public byte WorkStartLen = 16;
        public byte WorkEndLen = 16;
        public byte VioNumberSync = 4;
        public byte ImageConfirmLen = 15;         //영상확정 길이
        public byte AckLen = 2;
        public byte NackLen = 2;
        private byte extraLen = 3;     //STX, LEN, ETX 갯수
        public  virtual byte ExtraLen
        {
            get
            {
                return extraLen;
            }
        }
        private byte minFrameLen = 8;
        public virtual byte MinFrameLen
        {
            get
            {
                return minFrameLen;
            }
        }
        private byte sDLEPos = 0;    // Start DLE 위치
        public virtual byte SDLEPos
        {
            get
            {
                return sDLEPos;
            }
        }

        private byte stxPos = 0;    // STX 위치
        public virtual byte StxPos
        {
            get
            {
                return stxPos;
            }
        }

        private byte lenPos = 1;    // LEN 위치
        public virtual byte LenPos
        {
            get
            {
                return lenPos;
            }
        }
        private byte codePos = 2;    // Code 위치
        public virtual byte CodePos
        {
            get
            {
                return codePos;
            }
        }
        private byte seqPos = 3;    // Sequence 위치
        public virtual byte SeqPos
        {
            get
            {
                return seqPos;
            }
        }
        private byte dataPos = 4;    // Data 위치
        public virtual byte DataPos
        {
            get
            {
                return dataPos;
            }
        }
    }

    public class EthHeader : FrameHeader
    {
        private  byte extraLen = 3;     //STX, LEN, ETX 갯수
        public override byte ExtraLen
        {
            get
            {
                return extraLen;
            }
        }

    }

    public class SerialHeader : FrameHeader
    {
        private  byte extraLen = 6;     //STX, LEN, ETX 갯수
        public override byte ExtraLen
        {
            get
            {
                return extraLen;
            }
        }

        private byte stxPos = 1;    // STX 위치
        public override byte StxPos
        {
            get
            {
                return stxPos;
            }
        }

        private byte lenPos = 2;    // LEN 위치
        public override byte LenPos
        {
            get
            {
                return lenPos;
            }
        }
        private byte codePos = 3;    // Code 위치
        public override byte CodePos
        {
            get
            {
                return codePos;
            }
        }
        private byte seqPos = 4;    // Sequence 위치
        public override byte SeqPos
        {
            get
            {
                return seqPos;
            }
        }
        private byte dataPos = 5;    // Data 위치
        public override byte DataPos
        {
            get
            {
                return dataPos;
            }
        }
    }
}
