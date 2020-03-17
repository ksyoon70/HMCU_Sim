using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HMCU_Sim
{
    class PacketMethods
    {
        //byte 배열을 구조체로
        public static object ByteToStructure(byte[] data, Type type)
        {
            IntPtr buff = Marshal.AllocHGlobal(data.Length); // 배열의 크기만큼 비관리 메모리 영역에 메모리를 할당한다.
            Marshal.Copy(data, 0, buff, data.Length); // 배열에 저장된 데이터를 위에서 할당한 메모리 영역에 복사한다.
            object obj = Marshal.PtrToStructure(buff, type); // 복사된 데이터를 구조체 객체로 변환한다.
            Marshal.FreeHGlobal(buff); // 비관리 메모리 영역에 할당했던 메모리를 해제함

            //if (Marshal.SizeOf(obj) != data.Length)// (((PACKET_DATA)obj).TotalBytes != data.Length) // 구조체와 원래의 데이터의 크기 비교
            // {
            //     return null; // 크기가 다르면 null 리턴
            //}
            return obj; // 구조체 리턴
        }


        // 구조체를 byte 배열로
        public static byte[] StructureToByte(object obj)
        {
            int datasize = Marshal.SizeOf(obj);//((PACKET_DATA)obj).TotalBytes; // 구조체에 할당된 메모리의 크기를 구한다.
            IntPtr buff = Marshal.AllocHGlobal(datasize); // 비관리 메모리 영역에 구조체 크기만큼의 메모리를 할당한다.
            Marshal.StructureToPtr(obj, buff, false); // 할당된 구조체 객체의 주소를 구한다.
            byte[] data = new byte[datasize]; // 구조체가 복사될 배열
            Marshal.Copy(buff, data, 0, datasize); // 구조체 객체를 배열에 복사
            Marshal.FreeHGlobal(buff); // 비관리 메모리 영역에 할당했던 메모리를 해제함
            return data; // 배열을 리턴
        }
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    unsafe struct HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte[] seq;            //sequance
        public Byte retry;            //retry count
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte [] len;            //length
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte [] deviceCode;            //device code
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte [] dummy;            //reserved
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    unsafe struct BCDTIME
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte [] year;            //year
        public Byte month;            //month
        public Byte day;            //day
        public Byte hour;            //hour
        public Byte min;            //minute
        public Byte sec;            //second
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte [] ms;            //mili second

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    unsafe struct BCDTIME_DAY
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte[] year;            //year
        public Byte month;            //month
        public Byte day;            //day
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    unsafe struct BCDTIME_DAY_SEC
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte[] year;            //year
        public Byte month;            //month
        public Byte day;            //day
        public Byte hour;            //hour
        public Byte min;            //minute
        public Byte sec;            //second
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    unsafe struct PACKET_IMAGE_INFO_DATA
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte [] mgsID;            //메시지 ID
        public Byte location;           // 차량 위치
        public BCDTIME time;
        public Byte vdsID;              //VDS ID
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Byte [] trigger;          //트리거 번호
        public Byte lane1Code;             //차로코드 #1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Byte [] plateNum1;            //인식번호
        public Byte lane2Code;             //차로코드 #1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Byte [] plateNum2;            //인식번호
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)] 
        public Byte[] reserved;  //예약
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    unsafe struct PACKET_IMAGE_INFO
    {
        public Byte stx;
        public HEADER header;
        public PACKET_IMAGE_INFO_DATA infoData;
        public Byte etx;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    unsafe struct OBU_INFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Byte [] num;
        public Byte vio_type;
        public Byte vio_code;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    unsafe struct PACKET_MATCH_INFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte[] hmsgID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Byte[] htrigger1;          //트리거1 번호
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Byte[] htrigger2;          //트리거1 번호
        public Byte hlocation;           //영상선택위치
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte [] boffce;              //오피스번호
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte [] bworkNum;             //근무번호
        public BCDTIME_DAY timeDay;        //근무일자
        public Byte hworkType;            //근무형태
        public BCDTIME_DAY_SEC time;      //처리일시
        public Byte hprocCount;             //처리갯수
        public OBU_INFO obu1;
        public OBU_INFO obu2;
        public OBU_INFO obu3;
        public OBU_INFO obu4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] res;
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    unsafe struct PACKET_VIO_REQUEST
    {       
        public Byte imgStatus;           //트리거 상태
        public Int16 imagNum;            //영상번호
    }
}
