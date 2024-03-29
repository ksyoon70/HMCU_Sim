﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace HMCU_Sim
{
    /// <summary>
    /// SendUserControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SendUserControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string[] vdss = new string[] { "VDS#1", "VDS#2" };
        private string[] lanes = new string[] { "0", "1", "2", "3", "4", "5" };
        private string[] procCounts = new string[] { "1", "2", "3", "4" };   //처리번호 갯수
        private string[] voTypes = new string[] { "정보없음", "근무중위반", "면제할인", "폐쇄차로위반","테스트모드","통행권미수취","선불출퇴근추가할인","선불심야할인"
        ,"후불정상","후불출퇴근추가할인","후불심야할인","감면할인"};
        private string[] obuTypes = new string[] { "없음", "할인", "면제"};
        private string[] obuCarClass = new string[] { "1종", "2종", "3종","4종","5종","6종" };
        private string[] confLocation = new string[] { "전면", "후면" };
        private string[] syncMethods = new string[] { "전송연번", "위반번호" };
        private string[] confTime = new string[] { "위반응답", "영상확정" };
        private string[] endian = new string[] { "little Endian" , "Big Endian" };

        public int cycleNum; // 처리갯수

        public List<ProcItem> procList;


        public class WorkType
        {

            private int _id;

            public int Id
            {
                get { return _id; }
                set { _id = value; }
            }
            private string _name;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
        }

        private ObservableCollection<WorkType> _workTypes;

        public ObservableCollection<WorkType> Worktypes  //collection을 반환
        {
            get { return _workTypes; }
            set { _workTypes = value; }
        }
        
        private WorkType _workType;

        public WorkType Worktype  //개별 아이템을 반환
        {
            get { return _workType; }
            set
            {
                _workType = value;
                OnPropertyChanged("Worktype");
            }
        }

        private string vds;
        /**
        *  VDS설정
        * */
        public string Vds
        {
            get
            {
                return vds;
            }
            set
            {
                vds = value;
            }
        }

        private string ln;
        /**
        *  lane 설정
        * */
        public string Ln
        {
            get
            {
                return ln;
            }
            set
            {
                ln = value;
            }
        }

        private string procCount;
        /**
        *  process count 설정
        * */
        public string ProcCount
        {
            get
            {
                return procCount;
            }
            set
            {
                procCount = value;
            }
        }

        private UInt32 procNumber1;
        /**
        *  process num 1설정
        * */
        public UInt32 ProcNumber1
        {
            get
            {
                return procNumber1;
            }
            set
            {
                procNumber1 = value;
                OnPropertyChanged("ProcNumber1");
            }
        }
        private UInt32 procNumber2;
        /**
        *  process num 2설정
        * */
        public UInt32 ProcNumber2
        {
            get
            {
                return procNumber2;
            }
            set
            {
                procNumber2 = value;
                OnPropertyChanged("ProcNumber2");
            }
        }

        private UInt32 procNumber3;
        /**
        *  process num 3설정
        * */
        public UInt32 ProcNumber3
        {
            get
            {
                return procNumber3;
            }
            set
            {
                procNumber3 = value;
                OnPropertyChanged("ProcNumber3");
            }
        }

        private UInt32 procNumber4;
        /**
        *  process num 4설정
        * */
        public UInt32 ProcNumber4
        {
            get
            {
                return procNumber4;
            }
            set
            {
                procNumber4 = value;
                OnPropertyChanged("ProcNumber4");
            }
        }


        /**
        *  vio code1설정
        * */
        private string _vioCode1;
        public string VioCode1
        {
            get
            {
                return _vioCode1;
            }
            set
            {
                _vioCode1 = value;
                OnPropertyChanged("VioCode1");
            }
        }

        /**
        *  vio code2설정
        * */
        private string _vioCode2;
        public string VioCode2
        {
            get
            {
                return _vioCode2;
            }
            set
            {
                _vioCode2 = value;
                OnPropertyChanged("VioCode2");
            }
        }

        /**
        *  vio code3설정
        * */
        private string _vioCode3;
        public string VioCode3
        {
            get
            {
                return _vioCode3;
            }
            set
            {
                _vioCode3 = value;
                OnPropertyChanged("VioCode3");
            }
        }

        /**
        *  vio code4설정
        * */
        private string _vioCode4;
        public string VioCode4
        {
            get
            {
                return _vioCode4;
            }
            set
            {
                _vioCode4 = value;
                OnPropertyChanged("VioCode4");
            }
        }

        /**
       *  obu num1설정
       * */
        private string _obuNum1;
        public string ObuNum1
        {
            get
            {
                return _obuNum1;
            }
            set
            {
                _obuNum1 = value;
                OnPropertyChanged("ObuNum1");
            }
        }

        /**
       *  obu num2설정
       * */
        private string _obuNum2;
        public string ObuNum2
        {
            get
            {
                return _obuNum2;
            }
            set
            {
                _obuNum2 = value;
                OnPropertyChanged("ObuNum2");
            }
        }

        /**
       *  obu num3설정
       * */
        private string _obuNum3;
        public string ObuNum3
        {
            get
            {
                return _obuNum3;
            }
            set
            {
                _obuNum3 = value;
                OnPropertyChanged("ObuNum3");
            }
        }

        /**
       *  obu num4설정
       * */
        private string _obuNum4;
        public string ObuNum4
        {
            get
            {
                return _obuNum4;
            }
            set
            {
                _obuNum4 = value;
                OnPropertyChanged("ObuNum4");
            }
        }

        private string officeNumber;
        /**
        *  office num 설정
        * */
        public string OfficeNumber
        {
            get
            {
                return officeNumber;
            }
            set
            {
                officeNumber = value;
                OnPropertyChanged("OfficeNumber");
            }
        }
        private string laneNumber;
        /**
        *  lane num 설정
        * */
        public string LaneNumber
        {
            get
            {
                return laneNumber;
            }
            set
            {
                laneNumber = value;
                OnPropertyChanged("LaneNumber");
            }
        }

        private string workNumber;
        /**
        *  Work num 설정
        * */
        public string WorkNumber
        {
            get
            {
                return workNumber;
            }
            set
            {
                workNumber = value;
                OnPropertyChanged("WorkNumber");
            }
        }


        private int seqNum;
        /**
        *  전송연번 설정
        * */
        public int SeqNum
        {
            get
            {
                return seqNum;
            }
            set
            {
                seqNum = value;
                OnPropertyChanged("SeqNum");
            }
        }

        private int vioNumber;
        /**
        *  위반번호 (통합차로 제어기 부여)
        * */
        public int VioNumber
        {
            get
            {
                return vioNumber;
            }
            set
            {
                vioNumber = value;
                OnPropertyChanged("VioNumber");
            }
        }

        public OtherUserControl othTabUsrCtrl;
        //타이머 시작
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer statusReqTimer = new DispatcherTimer();
        public SendUserControl()
        {
            InitializeComponent();
            /// MainWindow과  데이터 동기화를 하기 위해서는 아래 문장을 실행 시켜 준다.
            DataContext = this;

            SeqNum = 1;
            VioNumber = 1;

            Worktypes = new ObservableCollection<WorkType>()
            {
                new WorkType(){ Id = 0, Name ="정상근무" },new WorkType(){ Id = 1, Name ="보수근무" }
            };

            workTime.Text = DateTime.Now.ToString("yyyyMMdd");

            /// 처리일시
            procTime.Text = DateTime.Now.ToString("yyyyMMddHHmmss");

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            dispatcherTimer.Start();

            statusReqTimer.Tick += new EventHandler(statusReqTimer_Tick);

            statusReqTimer.Interval = new TimeSpan(0, 0, 60);

            statusReqTimer.Start();

            ///처리번호갯수
            foreach (string pc in procCounts)
            {
                pcComboBox.Items.Add(pc);
            }
            ///처리번호

            /// 위반형태
            foreach (string vt in voTypes)
            {
                vioType1.Items.Add(vt);
                vioType2.Items.Add(vt);
                vioType3.Items.Add(vt);
                vioType4.Items.Add(vt);
            }

            //OBU 타입
            foreach (string ot in obuTypes)
            {
                obuType1.Items.Add(ot);
                obuType2.Items.Add(ot);
                obuType3.Items.Add(ot);
                obuType4.Items.Add(ot);
            }
            //OBU 차종
            foreach (string oc in obuCarClass)
            {
                obuCarClass1.Items.Add(oc);
                obuCarClass2.Items.Add(oc);
                obuCarClass3.Items.Add(oc);
                obuCarClass4.Items.Add(oc);
            }
            ///바이트오더
            foreach (string ed in endian)
            {
                ByteOrder.Items.Add(ed);
            }

           //동기방식
           foreach(string synm in syncMethods)
            {
                syncMethod.Items.Add(synm);
            }

            // 영상확정시점
            foreach (string ct in confTime)
            {
                cftComboBox.Items.Add(ct);
            }

            procList = new List<ProcItem>();
                        
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)

        {
            //타이머 이벤트 실행히 실행될 코드

            //아래는 델리게이트 사용법 : 쓰레드 타이머를 사용하였기 때문에 UI Thread와 다른 Thread라서 델리게이트를 사용해야 함

            Dispatcher.Invoke(DispatcherPriority.Background, (Action)delegate ()

            {
                procTime.Text = DateTime.Now.ToString("yyyyMMddHHmmss");
            });



        }

        private void statusReqTimer_Tick(object sender, EventArgs e)
        {
            if(autoSendStatusCheck.IsChecked == true)
            {
                ProcItem item = null;
                MakeFrame(Code.STATUS_REQ, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item);
                ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);
            }
        }

        private void SocketTxClear_Click(object sender, RoutedEventArgs e)
        {
            CommTxList.Items.Clear();
        }

        private void MakeTimeData(out byte[] data)
        {
            EthHeader ethHeader = new EthHeader();
            data = new byte[ethHeader.TimeLen];
            Array.Clear(data, 0, data.Length); //array clear
            byte[] bytes = BitConverter.GetBytes((Int16)0x1003);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Buffer.BlockCopy(bytes, 0, data, 0, 2);  //MsgID

            DateTime dt = DateTime.Now;
            int year = dt.Year;
            int month = dt.Month;
            int day = dt.Day;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int sec = dt.Second;
            int ms = dt.Millisecond * 10;

            int index = 2;

            byte[] bYear = ((MainWindow)System.Windows.Application.Current.MainWindow).IntToBCD(year);

            Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
            index += Marshal.SizeOf(typeof(short));

            Buffer.BlockCopy(((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(month), 0, data, index, Marshal.SizeOf(typeof(Byte)));
            index += Marshal.SizeOf(typeof(Byte));

            Buffer.BlockCopy(((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(day), 0, data, index, Marshal.SizeOf(typeof(Byte)));
            index += Marshal.SizeOf(typeof(Byte));

            Buffer.BlockCopy(((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(hour), 0, data, index, Marshal.SizeOf(typeof(Byte)));
            index += Marshal.SizeOf(typeof(Byte));

            Buffer.BlockCopy(((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(minute), 0, data, index, Marshal.SizeOf(typeof(Byte)));
            index += Marshal.SizeOf(typeof(Byte));

            Buffer.BlockCopy(((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(sec), 0, data, index, Marshal.SizeOf(typeof(Byte)));
            index += Marshal.SizeOf(typeof(Byte));

            byte[] bMs = ((MainWindow)System.Windows.Application.Current.MainWindow).IntToBCD(ms);
 

            Buffer.BlockCopy(bMs, 0, data, index, Marshal.SizeOf(typeof(short)));
            index += Marshal.SizeOf(typeof(short));

            //Reserved
            data[index] = 0x00;
            index += Marshal.SizeOf(typeof(Byte));
            data[index] = 0x00;
            index += Marshal.SizeOf(typeof(Byte));
            data[index] = 0x00;
            index += Marshal.SizeOf(typeof(Byte));

        }

        private void TimeSync_Click(object sender, RoutedEventArgs e)
        {
            MakeTimeData(out byte[] data);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendTimeSync(data, data.Length);
        }

        private void MakeHeartBeat(out byte[] data, CommMethod method)
        {
            int index = 0;
            FrameHeader fheader;

            if (method == CommMethod.Ethernet)
            {
                fheader = new EthHeader();
            }
            else
            {
                fheader = new SerialHeader();
            }

            data = new byte[fheader.HeartBeatLen];
            data[0] = Protocols.STX;
            data[1] = fheader.HeartBeatLen;

            index += fheader.HeartBeatLen;

        }
        public bool MakeFrame(int code, out byte[] data, CommMethod method, ref ProcItem pItem)
        {

            DateTime dt = DateTime.Now;
            int year = dt.Year;
            int month = dt.Month;
            int day = dt.Day;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int sec = dt.Second;
            int intValue;


            FrameHeader fheader;

            if(method == CommMethod.Ethernet)
            {
                fheader = new EthHeader();
            }
            else
            {
                fheader = new SerialHeader();
            }

            byte[] bYear = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN(year, 2);

            switch (code)
            {
                case Code.ACK:
                    data = new byte[fheader.AckLen + fheader.ExtraLen];
                    Array.Clear(data, 0, data.Length);
                    data[fheader.LenPos] = fheader.AckLen;
                    break;
                case Code.NACK:
                    data = new byte[fheader.NackLen + fheader.ExtraLen];
                    Array.Clear(data, 0, data.Length);
                    data[fheader.LenPos] = fheader.NackLen;
                    break;
                case Code.VIO_NUMBER_SYNC:
                    data = new byte[fheader.VioNumberSync + fheader.ExtraLen];
                    Array.Clear(data, 0, data.Length);
                    data[fheader.LenPos] = fheader.VioNumberSync;
                    break;
                case Code.WORK_START:
                    data = new byte[fheader.WorkStartLen + fheader.ExtraLen];
                    Array.Clear(data, 0, data.Length);
                    data[fheader.LenPos] = fheader.WorkStartLen;
                    break;
                case Code.WORK_END:
                    data = new byte[fheader.WorkEndLen + fheader.ExtraLen];
                    Array.Clear(data, 0, data.Length);
                    data[fheader.LenPos] = fheader.WorkEndLen;
                    break;
                case Code.STATUS_REQ:
                    data = new byte[fheader.HeartBeatLen + fheader.ExtraLen];
                    Array.Clear(data, 0, data.Length);
                    data[fheader.LenPos] = fheader.HeartBeatLen;
                    break;
                case Code.VIO_CONFIRM_RES:
                    data = new byte[fheader.ConfirmLen + fheader.ExtraLen];
                    Array.Clear(data, 0, data.Length);
                    data[fheader.LenPos] = fheader.ConfirmLen;
                    break;
                case Code.VIO_CONFIRM_RES_N:
                    data = new byte[fheader.ConfirmNewLen + fheader.ExtraLen];
                    Array.Clear(data, 0, data.Length);
                    data[fheader.LenPos] = fheader.ConfirmNewLen;
                    break;
                case Code.IMAGE_CONFIRM:
                    data = new byte[fheader.ImageConfirmLen + fheader.ExtraLen];
                    Array.Clear(data, 0, data.Length);
                    data[fheader.LenPos] = fheader.ImageConfirmLen;
                    break;
                case Code.PROCESS_RESULT:
                    data = new byte[fheader.ProcResultLen + fheader.ExtraLen];
                    Array.Clear(data, 0, data.Length);
                    data[fheader.LenPos] = fheader.ProcResultLen;
                    break;
                case Code.CONFIRM_INFO:
                    data = new byte[fheader.Confirm22Len + fheader.ExtraLen];
                    Array.Clear(data, 0, data.Length);
                    data[fheader.LenPos] = fheader.Confirm22Len;
                    break;
                default:
                    data = new byte[100 + fheader.ExtraLen];
                    break;
            }

            if (method == CommMethod.Serial)
            {
                ///시리얼 전송일 때만 DLE를 추가한다.
                data[fheader.SDLEPos] = Protocols.DLE;
            }
            data[fheader.StxPos] = Protocols.STX;
            data[fheader.CodePos] = (byte)code;  //CODE
            if(code == Code.ACK || code == Code.NACK)
            {
                int seq = ((MainWindow)System.Windows.Application.Current.MainWindow).recvTabUsrCtrl.SeqNum;
                data[fheader.SeqPos] = (byte)seq;
            }
            else
            {
                if(syncMethod.SelectedIndex == 0 && ((code == Code.VIO_CONFIRM_RES) || (code == Code.VIO_CONFIRM_RES_N)))
                {
                    
                }
                else
                {
                    data[fheader.SeqPos] = (byte)SeqNum;
                    SeqNum = SeqNum + 1;
                    if (SeqNum == 0x100)
                    {
                        SeqNum = 1;
                    }
                }
                
            }

            if (method == CommMethod.Ethernet)
            {
                data[data.Length - 1] = Protocols.ETX;
            }
            else
            {

            }
            
            int index = fheader.DataPos;

            switch (code)
            {
                case Code.ACK:
                    {

                    }
                    break;
                case Code.NACK:
                    {

                    }
                    break;
                case Code.VIO_NUMBER_SYNC:
                    {
                        byte[] intBytes = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN(VioNumber, 2);

                        Buffer.BlockCopy(intBytes, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));
                        ///위반번호 증가
                        VioNumber = VioNumber + 1;
                        if (VioNumber == 0xFFFF)
                        {
                            VioNumber = 1;
                        }
                    }
                    break;
                case Code.WORK_START:
                    {
                        ///근무개시시간
                        Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));

                        data[index] = (byte)month;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)day;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)hour;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)minute;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)sec;
                        index += Marshal.SizeOf(typeof(Byte));

                        /// 근무형태
                        data[index] = (byte)(wkComboBox.SelectedIndex & 0xFF);
                        index += Marshal.SizeOf(typeof(Byte));

                        ///근무번호
                        byte[] bWorkNum = ((MainWindow)System.Windows.Application.Current.MainWindow).IntToBCD(Convert.ToInt32(WorkNumber));
                        Buffer.BlockCopy(bWorkNum, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));

                        ///근무일자
                        Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));

                        Buffer.BlockCopy(((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(month), 0, data, index, Marshal.SizeOf(typeof(Byte)));
                        index += Marshal.SizeOf(typeof(Byte));

                        Buffer.BlockCopy(((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(day), 0, data, index, Marshal.SizeOf(typeof(Byte)));
                        index += Marshal.SizeOf(typeof(Byte));
                    }
                    break;
                case Code.WORK_END:
                    {
                        ///근무종료시간
                        Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));

                        data[index] = (byte)month;
                        index += Marshal.SizeOf(typeof(Byte));
                        data[index] = (byte)day;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)hour;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)minute;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)sec;
                        index += Marshal.SizeOf(typeof(Byte));

                        /// 근무형태
                        data[index] = (byte)(wkComboBox.SelectedIndex & 0xFF);
                        index += Marshal.SizeOf(typeof(Byte));

                        ///근무번호
                        byte[] bWorkNum = ((MainWindow)System.Windows.Application.Current.MainWindow).IntToBCD(Convert.ToInt32(WorkNumber));
                        Buffer.BlockCopy(bWorkNum, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));

                        ///근무일자
                        Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));

                        Buffer.BlockCopy(((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(month), 0, data, index, Marshal.SizeOf(typeof(Byte)));
                        index += Marshal.SizeOf(typeof(Byte));

                        Buffer.BlockCopy(((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(day), 0, data, index, Marshal.SizeOf(typeof(Byte)));
                        index += Marshal.SizeOf(typeof(Byte));
                    }
                    break;
                case Code.STATUS_REQ:
                    {

                        Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));

                        data[index] = (byte)month;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)day;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)hour;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)minute;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)sec;
                        index += Marshal.SizeOf(typeof(Byte));
                    }
                    break;
                case Code.VIO_CONFIRM_RES:
                    {

                        int vioTypeIndex;

                        // 위반확인요청이 없으면 종료한다.
                        if (procList.Count == 0)
                        {
                            MessageBox.Show("수신 item 0입니다. 수신된 위반확인 요구가 없습니다.");
                            return false;
                        }

                        /// 위반번호
                        ///
                        bool findList = false;
                        byte[] intBytes;
                        if (syncMethod.SelectedIndex == 1)
                        {//영상번호(위반번호)로 싱크를 맞추면..

                            if (pItem.sndVioReq == false)
                            {
                                //뒤짚어서 보내면 안됨
                                intBytes = BitConverter.GetBytes(pItem.vioNum);
                                Buffer.BlockCopy(intBytes, 0, data, index, Marshal.SizeOf(typeof(short)));
                                findList = true;
                            }

                        }
                        else
                        {
                            intBytes = BitConverter.GetBytes(VioNumber); //UI에 있는 자동으로 증가되는 번호
                            Buffer.BlockCopy(intBytes, 0, data, index, Marshal.SizeOf(typeof(short)));
                        }

                        if(syncMethod.SelectedIndex == 1 && findList == false)
                        {
                            MessageBox.Show("이미 위반 확인응답을 보냈습니다 (1)");
                            return false;
                        }
                        
                        index += Marshal.SizeOf(typeof(short));                       


                        ///위반일시 생성
                        Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));

                        data[index] = (byte)month;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)day;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)hour;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)minute;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)sec;
                        index += Marshal.SizeOf(typeof(Byte));
                        
                        //VioType은 나중에 할것이므로 기억만 만하고 있다가 나중에 처리
                        vioTypeIndex = index;
                        index += Marshal.SizeOf(typeof(Byte));

                        //근무번호
                        data[index] = Convert.ToByte(LaneNumber);
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = Convert.ToByte(WorkNumber);
                        index += Marshal.SizeOf(typeof(Byte));
                        
                        ///근무일자
                        Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));

                        data[index] = (byte)month;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)day;
                        index += Marshal.SizeOf(typeof(Byte));


                        if(pItem != null)
                        {
                            switch (pItem.resNumCnt)
                            {
                                case 0:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType1.SelectedIndex;

                                        /// 처리번호 (통합차로제어기 부여)
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber1, 4); 

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[0] = ProcNumber1;
                                            pItem.resNumCnt++;
                                            //전송연번으로 동기화 할때.
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber1 == 0xFFFFFFFF)
                                        {
                                            ProcNumber1 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber1++;
                                        }
                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode1);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                                case 1:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType2.SelectedIndex;
                                        /// 처리번호 (통합차로제어기 부여)
                                        ProcNumber2 = ProcNumber1;
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber2, 4);

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[1] = ProcNumber2;
                                            pItem.resNumCnt++;
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber2 == 0xFFFFFFFF)
                                        {
                                            ProcNumber2 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber2++;
                                        }

                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode2);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                                case 2:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType3.SelectedIndex;
                                        /// 처리번호 (통합차로제어기 부여)
                                        ProcNumber3 = ProcNumber2;
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber3, 4);

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[2] = ProcNumber3;
                                            pItem.resNumCnt++;
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber3 == 0xFFFFFFFF)
                                        {
                                            ProcNumber3 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber3++;
                                        }
                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode3);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                                case 3:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType4.SelectedIndex;
                                        /// 처리번호 (통합차로제어기 부여)
                                        ProcNumber4 = ProcNumber3;
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber4, 4);

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[3] = ProcNumber4;
                                            pItem.resNumCnt++;
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber4 == 0xFFFFFFFF)
                                        {
                                            ProcNumber4 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber4++;
                                        }
                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode4);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                            }
                        }
    
                        /// 차량번호
                        Buffer.BlockCopy(MainWindow.SetPlateNum(plateNum.Text), 0, data, index, 5);
                        index += 5;
                        /// 근무형태
                        data[index] = (byte)(wkComboBox.SelectedIndex & 0xFF);
                        index += Marshal.SizeOf(typeof(Byte));
                    }
                    break;
                case Code.VIO_CONFIRM_RES_N:
                    {
                        int vioTypeIndex;

                        // 위반확인요청이 없으면 종료한다.
                        if (procList.Count == 0)
                        {
                            MessageBox.Show("수신된 위반확인 요구가 없습니다.");
                            return false;
                        }

                        /// 위반번호
                        /// 
                        bool findList = false;
                        byte[] intBytes;
                        if (syncMethod.SelectedIndex == 1)
                        {//영상번호(위반번호)로 싱크를 맞추면..

                            if (pItem.sndVioReq == false)
                            {
                                //뒤짚어서 보내면 안됨
                                intBytes = BitConverter.GetBytes(pItem.vioNum);
                                Buffer.BlockCopy(intBytes, 0, data, index, Marshal.SizeOf(typeof(short)));
                                findList = true;
                            }

                        }
                        else
                        {
                            intBytes = BitConverter.GetBytes(VioNumber);
                            Buffer.BlockCopy(intBytes, 0, data, index, Marshal.SizeOf(typeof(short)));
                        }

                        if (syncMethod.SelectedIndex == 1 && findList == false)
                        {
                            MessageBox.Show("이미 위반 확인응답을 보냈습니다 (1)");
                            return false;
                        }

                        index += Marshal.SizeOf(typeof(short));

                        ///위반일시 생성
                        Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));
                        data[index] = (byte)month;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)day;
                        index += Marshal.SizeOf(typeof(Byte));
                        data[index] = (byte)hour;
                        index += Marshal.SizeOf(typeof(Byte));
                        data[index] = (byte)minute;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)sec;
                        index += Marshal.SizeOf(typeof(Byte));

                        //VioType은 나중에 할것이므로 기억만 만하고 있다가 나중에 처리
                        vioTypeIndex = index;
                        index += Marshal.SizeOf(typeof(Byte));

                        ///근무번호
                        data[index] = Convert.ToByte(LaneNumber);
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = Convert.ToByte(WorkNumber);
                        index += Marshal.SizeOf(typeof(Byte));

                        ///근무일자
                        Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));
                        data[index] = (byte)month;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)day;
                        index += Marshal.SizeOf(typeof(Byte));

                        if(pItem != null)
                        {
                            switch (pItem.resNumCnt)
                            {
                                case 0:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType1.SelectedIndex;
                                        /// 처리번호 (통합차로제어기 부여)
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber1, 4);

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[0] = ProcNumber1;
                                            pItem.resNumCnt++;
                                            //전송연번으로 동기화 할때.
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber1 == 0xFFFFFFFF)
                                        {
                                            ProcNumber1 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber1++;
                                        }
                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode1);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                                case 1:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType2.SelectedIndex;
                                        /// 처리번호 (통합차로제어기 부여)
                                        ProcNumber2 = ProcNumber1;
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber2, 4);

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[1] = ProcNumber2;
                                            pItem.resNumCnt++;      //보낸 갯수 증가
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber2 == 0xFFFFFFFF)
                                        {
                                            ProcNumber2 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber2++;
                                        }

                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode2);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                                case 2:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType3.SelectedIndex;
                                        /// 처리번호 (통합차로제어기 부여)
                                        ProcNumber3 = ProcNumber2;
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber3, 4);

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[2] = ProcNumber3;
                                            pItem.resNumCnt++;
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber3 == 0xFFFFFFFF)
                                        {
                                            ProcNumber3 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber3++;
                                        }
                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode3);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                                case 3:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType4.SelectedIndex;

                                        /// 처리번호 (통합차로제어기 부여)
                                        ProcNumber4 = ProcNumber3;
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber4, 4);

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[3] = ProcNumber4;
                                            pItem.resNumCnt++;
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber4 == 0xFFFFFFFF)
                                        {
                                            ProcNumber4 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber4++;
                                        }
                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode4);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                            }
                        }


                        /// 차량번호
                        Buffer.BlockCopy(MainWindow.SetPlateNum(plateNum.Text), 0, data, index, 5);
                        index += 5;
                        /// 근무형태
                        data[index] = (byte)(wkComboBox.SelectedIndex & 0xFF);
                        index += Marshal.SizeOf(typeof(Byte));
                        /// 영업소 번호
                        intValue = Convert.ToInt32(OfficeNumber);
                        // byte[] boffice = ((MainWindow)System.Windows.Application.Current.MainWindow).IntToBCD(intValue);
                        byte[] boffice = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN(intValue, 2);
                        Buffer.BlockCopy(boffice, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));
                        index += 10; ///reserved
                    }
                    break;
                case Code.IMAGE_CONFIRM:
                    {
                        /// procList에서 item의 갯수를 구한다.
                        /// 

                        if(pItem.curCfmCnt < pItem.procNumTotal)
                        {
                            /// 영업소 번호
                            intValue = Convert.ToInt32(OfficeNumber);
                            byte[] boffice = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN(intValue, 2);
                            Buffer.BlockCopy(boffice, 0, data, index, Marshal.SizeOf(typeof(short)));
                            index += Marshal.SizeOf(typeof(short));
                            ///근무일자
                            Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                            index += Marshal.SizeOf(typeof(short));
                            data[index] = (byte)month;
                            index += Marshal.SizeOf(typeof(Byte));

                            data[index] = (byte)day;
                            index += Marshal.SizeOf(typeof(Byte));

                            ///근무번호
                            data[index] = Convert.ToByte(LaneNumber);
                            index += Marshal.SizeOf(typeof(Byte));

                            data[index] = Convert.ToByte(WorkNumber);
                            index += Marshal.SizeOf(typeof(Byte));

                            /// 처리번호 (통합차로제어기 부여)
                            byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)pItem.ProcNum[pItem.curCfmCnt], 4);
                            pItem.curCfmCnt++;
                            Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                            index += Marshal.SizeOf(typeof(UInt32));

                            //확정위치
                            data[index] = (byte)(othTabUsrCtrl.cnfComboBox.SelectedIndex + 1);  // 1이면 전면 2이면 후면.
                        }
                        else
                        {
                            MessageBox.Show("영상 확정을 보낼 것이 없습니다.(1)");
                        }

                    }
                    break;
                case Code.PROCESS_RESULT:
                    {
                        int vioTypeIndex;

                        // 위반확인요청이 없으면 종료한다.
                        if (procList.Count == 0)
                        {
                            MessageBox.Show("수신된 번호인식결과가 없습니다.");
                            return false;
                        }

                        /// 위반번호
                        /// 
                        bool findList = false;
                        byte[] intBytes;

                        if (pItem.sndVioReq == false)
                        {
                            //뒤짚어서 보내면 안됨
                            //intBytes = BitConverter.GetBytes(pItem.vioNum);
                            intBytes = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)pItem.vioNum, 2);
                            Buffer.BlockCopy(intBytes, 0, data, index, Marshal.SizeOf(typeof(short)));
                            findList = true;
                        }

                        if (syncMethod.SelectedIndex == 1 && findList == false)
                        {
                            MessageBox.Show("이미 차량처리결과를 보냈습니다 (1)");
                            return false;
                        }

                        index += Marshal.SizeOf(typeof(short));

                        ///위반일시 생성
                        Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));
                        data[index] = (byte)month;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)day;
                        index += Marshal.SizeOf(typeof(Byte));
                        data[index] = (byte)hour;
                        index += Marshal.SizeOf(typeof(Byte));
                        data[index] = (byte)minute;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)sec;
                        index += Marshal.SizeOf(typeof(Byte));

                        //VioType은 나중에 할것이므로 기억만 만하고 있다가 나중에 처리
                        vioTypeIndex = index;
                        index += Marshal.SizeOf(typeof(Byte));

                        ///근무번호
                        data[index] = Convert.ToByte(LaneNumber);
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = Convert.ToByte(WorkNumber);
                        index += Marshal.SizeOf(typeof(Byte));

                        ///근무일자
                        Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));
                        data[index] = (byte)month;
                        index += Marshal.SizeOf(typeof(Byte));

                        data[index] = (byte)day;
                        index += Marshal.SizeOf(typeof(Byte));

                        if (pItem != null)
                        {
                            switch (pItem.resNumCnt)
                            {
                                case 0:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType1.SelectedIndex;
                                        /// 처리번호 (통합차로제어기 부여)
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber1, 4);

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[0] = ProcNumber1;
                                            
                                            //전송연번으로 동기화 할때.
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber1 == 0xFFFFFFFF)
                                        {
                                            ProcNumber1 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber1++;
                                        }
                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode1);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                                case 1:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType2.SelectedIndex;
                                        /// 처리번호 (통합차로제어기 부여)
                                        ProcNumber2 = ProcNumber1;
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber2, 4);

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[1] = ProcNumber2;
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber2 == 0xFFFFFFFF)
                                        {
                                            ProcNumber2 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber2++;
                                        }

                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode2);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                                case 2:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType3.SelectedIndex;
                                        /// 처리번호 (통합차로제어기 부여)
                                        ProcNumber3 = ProcNumber2;
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber3, 4);

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[2] = ProcNumber3;
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber3 == 0xFFFFFFFF)
                                        {
                                            ProcNumber3 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber3++;
                                        }
                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode3);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                                case 3:
                                    {
                                        ///위반형태
                                        data[vioTypeIndex] = (byte)vioType4.SelectedIndex;

                                        /// 처리번호 (통합차로제어기 부여)
                                        ProcNumber4 = ProcNumber3;
                                        byte[] bProcNum = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)ProcNumber4, 4);

                                        if (pItem.sndVioReq == false)
                                        {
                                            pItem.ProcNum[3] = ProcNumber4;
                                            int seq = pItem.seq;
                                            data[fheader.SeqPos] = (byte)seq;
                                        }

                                        if (ProcNumber4 == 0xFFFFFFFF)
                                        {
                                            ProcNumber4 = 1;
                                        }
                                        else
                                        {
                                            ProcNumber4++;
                                        }
                                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                                        index += Marshal.SizeOf(typeof(UInt32));
                                        /// 위반코드
                                        data[index] = Convert.ToByte(VioCode4);
                                        index += Marshal.SizeOf(typeof(Byte));

                                        break;
                                    }
                            }
                        }


                        /// 차량번호
                        Buffer.BlockCopy(MainWindow.SetPlateNum(plateNum.Text), 0, data, index, 5);
                        index += 5;
                        /// 근무형태
                        data[index] = (byte)(wkComboBox.SelectedIndex & 0xFF);
                        index += Marshal.SizeOf(typeof(Byte));
                        /// 영업소 번호
                        intValue = Convert.ToInt32(OfficeNumber);
                        byte[] boffice = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN(intValue, 2);
                        Buffer.BlockCopy(boffice, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));
                        switch (pItem.resNumCnt)
                        {
                            case 0:
                                {
                                    //OBU 제조번호
                                    int obuNumber = Convert.ToInt32(obuNum1.Text);
                                    string str = obuNumber.ToString("D8");
                                    byte[] digits = str.ToString().Select(c => byte.Parse(c.ToString())).ToArray();
                                    Buffer.BlockCopy(digits, 0, data, index, digits.Length);
                                    index += digits.Length;
                                    //OBU 종류
                                    data[index] = (byte)(obuType1.SelectedIndex & 0xFF);
                                    index += Marshal.SizeOf(typeof(Byte));
                                    //OBU 차종
                                    data[index] = (byte)(obuCarClass1.SelectedIndex + 1 & 0xFF);
                                    index += Marshal.SizeOf(typeof(Byte));
                                }
                                break;
                            case 1:
                                {
                                    int obuNumber = Convert.ToInt32(obuNum2.Text);
                                    string str = obuNumber.ToString("D8");
                                    byte[] digits = str.ToString().Select(c => byte.Parse(c.ToString())).ToArray();
                                    Buffer.BlockCopy(digits, 0, data, index, digits.Length);
                                    index += digits.Length;
                                    //OBU 종류
                                    data[index] = (byte)(obuType2.SelectedIndex & 0xFF);
                                    index += Marshal.SizeOf(typeof(Byte));
                                    //OBU 차종
                                    data[index] = (byte)(obuCarClass2.SelectedIndex + 1 & 0xFF);
                                    index += Marshal.SizeOf(typeof(Byte));
                                }
                                break;
                            case 2:
                                {
                                    int obuNumber = Convert.ToInt32(obuNum3.Text);
                                    string str = obuNumber.ToString("D8");
                                    byte[] digits = str.ToString().Select(c => byte.Parse(c.ToString())).ToArray();
                                    Buffer.BlockCopy(digits, 0, data, index, digits.Length);
                                    index += digits.Length;
                                    //OBU 종류
                                    data[index] = (byte)(obuType3.SelectedIndex & 0xFF);
                                    index += Marshal.SizeOf(typeof(Byte));
                                    //OBU 차종
                                    data[index] = (byte)(obuCarClass3.SelectedIndex + 1 & 0xFF);
                                    index += Marshal.SizeOf(typeof(Byte));
                                }
                                break;
                            case 3:
                                {
                                    int obuNumber = Convert.ToInt32(obuNum4.Text);
                                    string str = obuNumber.ToString("D8");
                                    byte[] digits = str.ToString().Select(c => byte.Parse(c.ToString())).ToArray();
                                    Buffer.BlockCopy(digits, 0, data, index, digits.Length);
                                    index += digits.Length;
                                    //OBU 종류
                                    data[index] = (byte)(obuType4.SelectedIndex & 0xFF);
                                    index += Marshal.SizeOf(typeof(Byte));
                                    //OBU 차종
                                    data[index] = (byte)(obuCarClass4.SelectedIndex + 1 & 0xFF);
                                    index += Marshal.SizeOf(typeof(Byte));
                                }
                                break;
                            default:
                                break;
                        }

                        index += 5; ///reserved
                        pItem.resNumCnt++;

                    }
                    break;
                case Code.CONFIRM_INFO:
                    {
                        //처리번호
                        byte[] intBytes = ((MainWindow)System.Windows.Application.Current.MainWindow).INT2ENDIAN((int)pItem.vioNum, 2);
                        Buffer.BlockCopy(intBytes, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += 2;
                        //확정위치
                        data[index] = (byte)(othTabUsrCtrl.cnfComboBox.SelectedIndex + 1);  // 1이면 전면 2이면 후면.
                        index += 1;
                        index += 5; ///reserved
                    }
                    break;
                default:
                    break;
            }

            
            if (method == CommMethod.Serial)
            {
                //BCC계산.
                byte[] bccData = new byte[data.Length - fheader.ExtraLen]; //DLE,STX, LEN은 빼고...
                Array.Copy(data, fheader.CodePos, bccData, 0, bccData.Length);
                byte calBcc = MainWindow.CalBCC(bccData, bccData.Length);
                //DLE 추가
                byte[] dleData = new byte[data.Length*2];  //충분한 버퍼 확보
                int dleSize = data.Length - fheader.ExtraLen + 1;
                Array.Copy(data, fheader.LenPos, dleData, 0, dleSize);
                int dleDataLen = MainWindow.AddDLE(ref dleData, dleSize);
                Array.Resize(ref data, dleDataLen + fheader.ExtraLen -1);

                Array.Copy(dleData, 0, data, fheader.LenPos, dleDataLen);

                data[data.Length - 3] = Protocols.DLE;
                data[data.Length - 2] = Protocols.ETX;
                data[data.Length - 1] = calBcc;
            }

            return true;

        }
        /// <summary>
        /// 상태정보 요청 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeartBeat_Click(object sender, RoutedEventArgs e)
        {
            ProcItem item = null;
            MakeFrame(Code.STATUS_REQ, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// 위반확인응답 전송
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendVioConfirmResponse_Click(object sender, RoutedEventArgs e)
        {

            ///위반확인응답프레임 만들기.
            int maxLoop = pcComboBox.SelectedIndex + 1;
            uint saveProcNum = ProcNumber1;

            int ProtocolIndex = ((MainWindow)System.Windows.Application.Current.MainWindow).recvTabUsrCtrl.protoComboBox.SelectedIndex;

            if (ProtocolIndex == 0)
            {
                for (int k = 0; k < procList.Count; k++)
                {
                    for (int i = 0; i < procList[k].procNumTotal; i++)
                    {
                        ProcItem item = (ProcItem)procList[k];
                        if (MakeFrame(Code.VIO_CONFIRM_RES, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item) == true)
                        {
                            ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);
                        }
                    }
                    if (procList[k].sndVioReq == false)
                    {
                        procList[k].sndVioReq = true;

                        ///싱크 번호가 이미지 번호가 아니면 그냥 MCU Sim에서 번호를 증가 한다.
                        if (syncMethod.SelectedIndex != 1)
                        {
                            ///위반번호 증가
                            VioNumber = VioNumber + 1;
                            if (VioNumber == 0xFFFF)
                            {
                                VioNumber = 1;
                            }
                        }

                        ProcNumber1 = saveProcNum;
                        ProcNumber1 += (uint)maxLoop;
                        ProcNumber2 = ProcNumber1 + 1;
                        ProcNumber3 = ProcNumber2 + 1;
                        ProcNumber4 = ProcNumber3 + 1;

                        saveProcNum = ProcNumber1;
                    }

                }
                if (cftComboBox.SelectedIndex == 0)
                {
                    for (int k = 0; k < procList.Count; k++)
                    {
                        if (procList[k].sndVioReq == true)
                        {
                            //위반확인에서 영상확정이고, 위반확인을 보내면 item 삭제
                            procList.RemoveAt(k);
                        }
                    }
                }
            }
            else if(ProtocolIndex == 1)
            {
                for (int k = 0; k < procList.Count; k++)
                {
                    for (int i = 0; i < procList[k].procNumTotal; i++)
                    {
                        ProcItem item = (ProcItem)procList[k];
                        if (MakeFrame(Code.VIO_CONFIRM_RES_N, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item) == true)
                        {
                            ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);
                        }
                    }
                    if (procList[k].sndVioReq == false)
                    {
                        procList[k].sndVioReq = true;

                        ///싱크 번호가 이미지 번호가 아니면 그냥 MCU Sim에서 번호를 증가 한다.
                        if (syncMethod.SelectedIndex != 1)
                        {
                            ///위반번호 증가
                            VioNumber = VioNumber + 1;
                            if (VioNumber == 0xFFFF)
                            {
                                VioNumber = 1;
                            }
                        }

                        ProcNumber1 = saveProcNum;
                        ProcNumber1 += (uint)maxLoop;
                        ProcNumber2 = ProcNumber1 + 1;
                        ProcNumber3 = ProcNumber2 + 1;
                        ProcNumber4 = ProcNumber3 + 1;
                        saveProcNum = ProcNumber1;
                    }

                }
                if (cftComboBox.SelectedIndex == 0)
                {
                    for (int k = 0; k < procList.Count; k++)
                    {
                        if (procList[k].sndVioReq == true)
                        {
                            //위반확인에서 영상확정이고, 위반확인을 보내면 item 삭제
                            procList.RemoveAt(k);
                        }
                    }
                }
            }
            else
            {
                //22년 기능개선 프로토콜
                for (int k = 0; k < procList.Count; k++)
                {
                    for (int i = 0; i < procList[k].procNumTotal; i++)
                    {
                        ProcItem item = (ProcItem)procList[k];
                        if (MakeFrame(Code.PROCESS_RESULT, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item) == true)
                        {
                            ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);
                        }
                    }
                    if (procList[k].sndVioReq == false)
                    {
                        procList[k].sndVioReq = true;

                        ///싱크 번호가 이미지 번호가 아니면 그냥 MCU Sim에서 번호를 증가 한다.
                        if (syncMethod.SelectedIndex != 1)
                        {
                            ///위반번호 증가
                            VioNumber = VioNumber + 1;
                            if (VioNumber == 0xFFFF)
                            {
                                VioNumber = 1;
                            }
                        }

                        ProcNumber1 = saveProcNum;
                        ProcNumber1 += (uint)maxLoop;
                        ProcNumber2 = ProcNumber1 + 1;
                        ProcNumber3 = ProcNumber2 + 1;
                        ProcNumber4 = ProcNumber3 + 1;
                        saveProcNum = ProcNumber1;
                    }

                }

            }

        }
        /// <summary>
        /// 근무개시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkStart_Click(object sender, RoutedEventArgs e)
        {
            ProcItem item = null; 
            MakeFrame(Code.WORK_START, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);
            //((MainWindow)System.Windows.Application.Current.MainWindow).commHandler.Send(data, data.Length);
        }
        /// <summary>
        /// 근무종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkEnd_Click(object sender, RoutedEventArgs e)
        {
            ProcItem item = null; 
            MakeFrame(Code.WORK_END, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);
            //((MainWindow)System.Windows.Application.Current.MainWindow).commHandler.Send(data, data.Length);
        }
        /// <summary>
        /// Sync Frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncFrame_Click(object sender, RoutedEventArgs e)
        {
            ProcItem item = null;

            MakeFrame(Code.VIO_NUMBER_SYNC, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);
        }
        /// <summary>
        /// 영상확정 처리기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageConfirm_Click(object sender, RoutedEventArgs e)
        {
            int procNum = procList.Count;
            int ProtocolIndex = ((MainWindow)System.Windows.Application.Current.MainWindow).recvTabUsrCtrl.protoComboBox.SelectedIndex;

            if (procList.Count > 0)
            {
                if (ProtocolIndex == 0 || ProtocolIndex == 1)
                {
                    for (int i = 0; i < procList.Count; i++)
                    {
                        if (procList[i].sndVioReq == true)
                        {

                            for (int j = 0; j < procList[i].procNumTotal; j++)
                            {
                                ProcItem item = (ProcItem)procList[i];
                                MakeFrame(Code.IMAGE_CONFIRM, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item);
                                ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);

                            }
                            if (procList[i].procNumTotal == procList[i].curCfmCnt)
                            {
                                procList.RemoveAt(i);  //영상확정을 보내면 삭제한다.
                            }
                            break;
                        }
                    }
                }
                else
                {
                    //22년 기능개선 확정인 경우
                    for (int i = 0; i < procList.Count; i++)
                    {
                        if (procList[i].sndVioReq == true)
                        {

                            for (int j = 0; j < procList[i].procNumTotal; j++)
                            {
                                ProcItem item = (ProcItem)procList[i];
                                MakeFrame(Code.CONFIRM_INFO, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item);
                                ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);

                            }
                            if (procList[i].procNumTotal == procList[i].curCfmCnt)
                            {
                                procList.RemoveAt(i);  //영상확정을 보내면 삭제한다.
                            }
                            break;
                        }
                    }
                }
               
            }
            else
            {
                MessageBox.Show("영상 확정을 보낼 것이 없습니다 (2)");
            }
           
        }

        private void Nack_Click(object sender, RoutedEventArgs e)
        {
            ProcItem item = null;
            MakeFrame(Code.NACK, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);
        }

        private void Ack_Click(object sender, RoutedEventArgs e)
        {
            ProcItem item = null;
            MakeFrame(Code.ACK, out byte[] data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref item);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(data, data.Length);
        }

        private void cftComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //영상확정 변경 때...

            if(cftComboBox.SelectedIndex == 0)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).othTabUsrCtrl.autoConfirmSendCheck.IsChecked = false;
            }
        }
    }
}
