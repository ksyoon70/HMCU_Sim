using System;
using System.Collections.Generic;
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

    static class EthHeader
    {
        public const byte FLen = 10;
        public const byte TimeLen = 14;
        public const byte TLen = 48;
        public const byte HeartBeatLen = 9;
        public const byte ConfirmLen = 29;
        public const byte ConfirmNewLen = 41;
        public const byte WorkStartLen = 16;
        public const byte WorkEndLen = 16;
        public const byte VioNumberSync = 4;
        public const byte ImageConfirmLen = 15;         //영상확정 길이
        public const byte AckLen = 2;
        public const byte NackLen = 2;
        public const byte extraLen = 3;     //STX, LEN, ETX 갯수
    }
}
