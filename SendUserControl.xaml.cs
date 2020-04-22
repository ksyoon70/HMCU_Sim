using System;
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
        private string[] confLocation = new string[] { "전면", "후면" };
        private string[] syncMethods = new string[] { "전송연번", "위반번호" };
        private string[] confTime = new string[] { "위반응답", "영상확정" };

        private int cycleNum; // 처리갯수

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
        *  vio code1설정
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
        *  vio code1설정
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
        //타이머 시작
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
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

            workTime.Text = DateTime.Now.ToString("yyMMdd");

            /// 처리일시
            procTime.Text = DateTime.Now.ToString("yyMMddHHmmss");

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            dispatcherTimer.Start();

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

            // 확정위치
            foreach (string loc in confLocation)
            {
                cnfComboBox.Items.Add(loc);
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
                procTime.Text = DateTime.Now.ToString("yyMMddHHmmss");
            });

        }

        private void SocketTxClear_Click(object sender, RoutedEventArgs e)
        {
            SocketTxList.Items.Clear();
        }

        private void MakeTimeData(out byte[] data)
        {
            data = new byte[EthHeader.TimeLen];
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

        private void MakeHeartBeat(out byte[] data)
        {
            int index = 0;
            data = new byte[EthHeader.HeartBeatLen];
            data[0] = Protocols.STX;
            data[1] = EthHeader.HeartBeatLen;

            index += EthHeader.HeartBeatLen;

        }
        public bool MakeEtherFrame(int code, out byte[] data)
        {

            DateTime dt = DateTime.Now;
            int year = dt.Year;
            int month = dt.Month;
            int day = dt.Day;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int sec = dt.Second;
            int intValue;

            byte[] bYear = ((MainWindow)System.Windows.Application.Current.MainWindow).IntToBCD(year);

            switch (code)
            {
                case Code.ACK:
                    data = new byte[EthHeader.AckLen + EthHeader.extraLen];
                    Array.Clear(data, 0, data.Length);
                    data[1] = EthHeader.AckLen;
                    break;
                case Code.NACK:
                    data = new byte[EthHeader.NackLen + EthHeader.extraLen];
                    Array.Clear(data, 0, data.Length);
                    data[1] = EthHeader.NackLen;
                    break;
                case Code.VIO_NUMBER_SYNC:
                    data = new byte[EthHeader.VioNumberSync + EthHeader.extraLen];
                    Array.Clear(data, 0, data.Length);
                    data[1] = EthHeader.VioNumberSync;
                    break;
                case Code.WORK_START:
                    data = new byte[EthHeader.WorkStartLen + EthHeader.extraLen];
                    Array.Clear(data, 0, data.Length);
                    data[1] = EthHeader.WorkStartLen;
                    break;
                case Code.WORK_END:
                    data = new byte[EthHeader.WorkEndLen + EthHeader.extraLen];
                    Array.Clear(data, 0, data.Length);
                    data[1] = EthHeader.WorkEndLen;
                    break;
                case Code.STATUS_REQ:
                    data = new byte[EthHeader.HeartBeatLen + EthHeader.extraLen];
                    Array.Clear(data, 0, data.Length);
                    data[1] = EthHeader.HeartBeatLen;
                    break;
                case Code.VIO_CONFIRM_RES:
                    data = new byte[EthHeader.ConfirmLen + EthHeader.extraLen];
                    Array.Clear(data, 0, data.Length);
                    data[1] = EthHeader.ConfirmLen;
                    break;
                case Code.VIO_CONFIRM_RES_N:
                    data = new byte[EthHeader.ConfirmNewLen + EthHeader.extraLen];
                    Array.Clear(data, 0, data.Length);
                    data[1] = EthHeader.ConfirmNewLen;
                    break;
                case Code.IMAGE_CONFIRM:
                    data = new byte[EthHeader.ImageConfirmLen + EthHeader.extraLen];
                    Array.Clear(data, 0, data.Length);
                    data[1] = EthHeader.ImageConfirmLen;
                    break;
                default:
                    data = new byte[100 + EthHeader.extraLen];
                    break;
            }

           
            data[0] = Protocols.STX;
            data[2] = (byte)code;  //CODE
            if(code == Code.ACK || code == Code.NACK)
            {
                int seq = ((MainWindow)System.Windows.Application.Current.MainWindow).recvTabUsrCtrl.SeqNum;
                data[3] = (byte)seq;
            }
            else
            {
                if(syncMethod.SelectedIndex == 0 && ((code == Code.VIO_CONFIRM_RES) || (code == Code.VIO_CONFIRM_RES_N)))
                {
                    
                }
                else
                {
                    data[3] = (byte)SeqNum;
                    SeqNum = SeqNum + 1;
                    if (SeqNum == 0x100)
                    {
                        SeqNum = 1;
                    }
                }
                
            }
           

            data[data.Length - 1] = Protocols.ETX;
            int index = 4;

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
                        byte[] intBytes = BitConverter.GetBytes(VioNumber);

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
                case Code.STATUS_REQ:  //
                    {

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
                    }
                    break;
                case Code.VIO_CONFIRM_RES:
                    {

                        // 위반확인요청이 없으면 종료한다.
                        if (procList.Count == 0)
                        {
                            MessageBox.Show("수신된 위반확인 요구가 없습니다.");
                            return false;
                        }

                        /// 위반번호
                        /// 
                        byte[] intBytes = BitConverter.GetBytes(VioNumber);

                        Buffer.BlockCopy(intBytes, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));
                        ///위반번호 증가
                        if (syncMethod.SelectedIndex == 1)
                        {
                            VioNumber = VioNumber + 1;
                            if (VioNumber == 0xFFFF)
                            {
                                VioNumber = 1;
                            }
                        }


                        ///위반일시 생성
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

                       

                        switch (cycleNum)
                        {
                            case 1:
                                { 
                                    ///위반형태
                                    data[index] = (byte)vioType1.SelectedIndex;
                                    index += Marshal.SizeOf(typeof(Byte));
                                    /// 처리번호 (통합차로제어기 부여)
                                    byte[] bProcNum = BitConverter.GetBytes(ProcNumber1);
                                    
                                    
                                    bool find = false;
                                    for(int k = 0; k < procList.Count; k++)
                                    {
                                        if(procList[k].sndVioReq == false)
                                        {
                                            procList[k].ProcNum[0] = ProcNumber1;
                                            procList[k].ProcNumCnt++;
                                            find = true;
                                            //전송연번으로 동기화 할때.
                                            int seq = procList[k].seq;
                                            data[3] = (byte)seq;
                                            break;
                                        }
                                    }

                                    if(find == false)
                                    {
                                        MessageBox.Show("이미 위반 확인응답을 보냈습니다 (1)");
                                        return false;
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
                            case 2:
                                {
                                    ///위반형태
                                    data[index] = (byte)vioType2.SelectedIndex;
                                    index += Marshal.SizeOf(typeof(Byte));
                                    /// 처리번호 (통합차로제어기 부여)
                                    ProcNumber2 = ProcNumber1;
                                    byte[] bProcNum = BitConverter.GetBytes(ProcNumber2);

                                    bool find = false;
                                    for (int k = 0; k < procList.Count; k++)
                                    {
                                        if (procList[k].sndVioReq == false)
                                        {
                                            procList[k].ProcNum[1] = ProcNumber2;
                                            procList[k].ProcNumCnt++;
                                            int seq = procList[k].seq;
                                            data[3] = (byte)seq;
                                            find = true;
                                            break;
                                        }
                                    }

                                    if (find == false)
                                    {
                                        MessageBox.Show("이미 위반 확인응답을 보냈습니다 (2)");
                                        return false;
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
                            case 3:
                                {
                                    ///위반형태
                                    data[index] = (byte)vioType3.SelectedIndex;
                                    index += Marshal.SizeOf(typeof(Byte));
                                    /// 처리번호 (통합차로제어기 부여)
                                    ProcNumber3 = ProcNumber2;
                                    byte[] bProcNum = BitConverter.GetBytes(ProcNumber3);

                                    bool find = false;
                                    for (int k = 0; k < procList.Count; k++)
                                    {
                                        if (procList[k].sndVioReq == false)
                                        {
                                            procList[k].ProcNum[2] = ProcNumber3;
                                            procList[k].ProcNumCnt++;
                                            int seq = procList[k].seq;
                                            data[3] = (byte)seq;
                                            find = true;
                                            break;
                                        }
                                    }

                                    if (find == false)
                                    {
                                        MessageBox.Show("이미 위반 확인응답을 보냈습니다 (3)");
                                        return false;
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
                            case 4:
                                {
                                    ///위반형태
                                    data[index] = (byte)vioType4.SelectedIndex;
                                    index += Marshal.SizeOf(typeof(Byte));
                                    /// 처리번호 (통합차로제어기 부여)
                                    ProcNumber4 = ProcNumber3;
                                    byte[] bProcNum = BitConverter.GetBytes(ProcNumber4);

                                    bool find = false;
                                    for (int k = 0; k < procList.Count; k++)
                                    {
                                        if (procList[k].sndVioReq == false)
                                        {
                                            procList[k].ProcNum[3] = ProcNumber4;
                                            procList[k].ProcNumCnt++;
                                            int seq = procList[k].seq;
                                            data[3] = (byte)seq;
                                            find = true;
                                            break;
                                        }
                                    }

                                    if (find == false)
                                    {
                                        MessageBox.Show("이미 위반 확인응답을 보냈습니다 (4)");
                                        return false;
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
                        /// 위반번호
                        /// 
                        byte[] intBytes = BitConverter.GetBytes(VioNumber);

                        Buffer.BlockCopy(intBytes, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));
                        ///위반번호 증가
                        if (syncMethod.SelectedIndex == 1)
                        {
                            VioNumber = VioNumber + 1;
                            if (VioNumber == 0xFFFF)
                            {
                                VioNumber = 1;
                            }
                        }

                        ///위반일시 생성
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

                        ///위반형태
                        data[index] = (byte)vioType1.SelectedIndex;
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
                        /// 처리번호 (통합차로제어기 부여)
                        byte[] bProcNum = BitConverter.GetBytes(ProcNumber1);
                        Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                        index += Marshal.SizeOf(typeof(UInt32));
                        /// 위반코드
                        data[index] = Convert.ToByte(VioCode1);
                        index += Marshal.SizeOf(typeof(Byte));
                        /// 차량번호
                        Buffer.BlockCopy(MainWindow.SetPlateNum(plateNum.Text), 0, data, index, 5);
                        index += 5;
                        /// 근무형태
                        data[index] = (byte)(wkComboBox.SelectedIndex & 0xFF);
                        index += Marshal.SizeOf(typeof(Byte));
                        /// 영업소 번호
                        intValue = Convert.ToInt32(OfficeNumber);
                        byte[] boffice = ((MainWindow)System.Windows.Application.Current.MainWindow).IntToBCD(intValue);
                        Buffer.BlockCopy(boffice, 0, data, index, Marshal.SizeOf(typeof(short)));
                        index += Marshal.SizeOf(typeof(short));
                        index += 10; ///reserved
                    }
                    break;
                case Code.IMAGE_CONFIRM:
                    {
                        /// procList에서 item의 갯수를 구한다.
                        /// 

                        if(procList[0].curProcNumCnt < procList[0].ProcNumCnt)
                        {
                            /// 영업소 번호
                            intValue = Convert.ToInt32(OfficeNumber);
                            byte[] boffice = ((MainWindow)System.Windows.Application.Current.MainWindow).IntToBCD(intValue);
                            Buffer.BlockCopy(boffice, 0, data, index, Marshal.SizeOf(typeof(short)));
                            index += Marshal.SizeOf(typeof(short));
                            ///근무일자
                            Buffer.BlockCopy(bYear, 0, data, index, Marshal.SizeOf(typeof(short)));
                            index += Marshal.SizeOf(typeof(short));

                            Buffer.BlockCopy(((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(month), 0, data, index, Marshal.SizeOf(typeof(Byte)));
                            index += Marshal.SizeOf(typeof(Byte));

                            Buffer.BlockCopy(((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(day), 0, data, index, Marshal.SizeOf(typeof(Byte)));
                            index += Marshal.SizeOf(typeof(Byte));

                            ///근무번호
                            byte[] bWorkNum = ((MainWindow)System.Windows.Application.Current.MainWindow).IntToBCD(Convert.ToInt32(WorkNumber));
                            Buffer.BlockCopy(bWorkNum, 0, data, index, Marshal.SizeOf(typeof(short)));
                            index += Marshal.SizeOf(typeof(short));

                            /// 처리번호 (통합차로제어기 부여)
                            byte[] bProcNum = BitConverter.GetBytes(procList[0].ProcNum[procList[0].curProcNumCnt]);
                            procList[0].curProcNumCnt++;
                            Buffer.BlockCopy(bProcNum, 0, data, index, Marshal.SizeOf(typeof(UInt32)));
                            index += Marshal.SizeOf(typeof(UInt32));

                            //확정위치
                            data[index] = (byte)(cnfComboBox.SelectedIndex + 1);  // 1이면 전면 2이면 후면.
                        }
                        else
                        {
                            MessageBox.Show("영상확정을 보낼 것이 없습니다.");
                        }

                    }
                    break;
                default:
                    break;
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
            MakeEtherFrame(Code.STATUS_REQ, out byte[] data);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendEtherData(data, data.Length);
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
            for (cycleNum = 1; cycleNum <= maxLoop; cycleNum++)
            {
                if (MakeEtherFrame(Code.VIO_CONFIRM_RES, out byte[] data) == true)
                {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).SendEtherData(data, data.Length);                   
                }
            }

            for (int k = 0; k < procList.Count; k++)
            {
                if (procList[k].sndVioReq == false && procList[k].ProcNumCnt > 0)
                {
                    //위반확인응답을 보냄.
                    procList[k].sndVioReq = true;
                    break;
                }
            }

            ProcNumber1 = saveProcNum;
            ProcNumber1 += (uint)maxLoop;
            ProcNumber2 = ProcNumber1 + 1;
            ProcNumber3 = ProcNumber2 + 1;
            ProcNumber4 = ProcNumber3 + 1;

            for(int k= 0; k < procList.Count; k++)
            {
                if(procList[k].sndVioReq == true && cftComboBox.SelectedIndex == 0)
                {
                    //위반확인에서 영상확정이고, 위반확인을 보내면 item 삭제
                    procList.RemoveAt(k);
                }
            }
        }

        private void SendVioConfirmResponseNew_Click(object sender, RoutedEventArgs e)
        {
            ///위반확인응답프레임-New 만들기.
            MakeEtherFrame(Code.VIO_CONFIRM_RES_N, out byte[] data);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendEtherData(data, data.Length);
        }
        /// <summary>
        /// 근무개시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkStart_Click(object sender, RoutedEventArgs e)
        {
            MakeEtherFrame(Code.WORK_START, out byte[] data);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendEtherData(data, data.Length);
        }
        /// <summary>
        /// 근무종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkEnd_Click(object sender, RoutedEventArgs e)
        {
            MakeEtherFrame(Code.WORK_END, out byte[] data);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendEtherData(data, data.Length);
        }
        /// <summary>
        /// Sync Frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncFrame_Click(object sender, RoutedEventArgs e)
        {
            MakeEtherFrame(Code.VIO_NUMBER_SYNC, out byte[] data);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendEtherData(data, data.Length);
        }
        /// <summary>
        /// 영상확정 처리기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageConfirm_Click(object sender, RoutedEventArgs e)
        {
            int procNum = procList.Count;
            if (procList.Count > 0)
            {
                for(int i = 0; i < procList.Count; i++)
                {
                    if(procList[i].sndVioReq == true && procList[i].sndImgCfm == false)
                    {
                       
                        for(int j = 0; j < procList[i].ProcNumCnt;  j++)
                        {
                            MakeEtherFrame(Code.IMAGE_CONFIRM, out byte[] data);
                            ((MainWindow)System.Windows.Application.Current.MainWindow).SendEtherData(data, data.Length);
                            
                        }
                        procList[i].sndImgCfm = true;
                        procList.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("영상 확정을 보낼 것이 없습니다");
            }
           
        }

        private void Nack_Click(object sender, RoutedEventArgs e)
        {
            MakeEtherFrame(Code.NACK, out byte[] data);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendEtherData(data, data.Length);
        }

        private void Ack_Click(object sender, RoutedEventArgs e)
        {
            MakeEtherFrame(Code.ACK, out byte[] data);
            ((MainWindow)System.Windows.Application.Current.MainWindow).SendEtherData(data, data.Length);
        }
    }
}
