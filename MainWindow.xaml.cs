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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;
using System.IO.Ports;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace HMCU_Sim
{

    public enum CommMethod
    {
        Ethernet,
        Serial,
    }

    // State object for receiving data from remote device.
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public Socket g_listener = null;

         

        ConfigManager m_cfg;
        public ConfigManager GetCfgManager() => m_cfg;

        public CommMethod comm;

        public CommHandler commHandler;

        public FrameHeader frameHeader;

        /// <summary>
        /// 이더넷 방식인지 여부
        /// </summary>
        public bool IsEther
        {
            get
            {
                return (comm == CommMethod.Ethernet);
            }
            set
            {
                comm = value ? CommMethod.Ethernet : CommMethod.Serial;
                OnPropertyChanged("IsEther");
            }
        }
        /// <summary>
        /// 시리얼 통신인지 여부
        /// </summary>
        public bool IsSerial
        {
            get
            {
                return (comm == CommMethod.Serial);
            }
            set
            {
                comm = value ? CommMethod.Serial : CommMethod.Ethernet;
                OnPropertyChanged("IsSerial");
            }
        }

        private string svrport;
        public string Svrport
        {
            get
            {
                return svrport;
            }
            set
            {
                svrport = value;
            }
        }
        /// <summary>
        /// 서버 IP
        /// </summary>
        private string svrIP;
        public string SvrIP
        {
            get
            {
                return svrIP;
            }
            set
            {
                svrIP = value;
            }
        }

        public RecvUserControl recvTabUsrCtrl;
        public SendUserControl sndTabUsrCtrl;
        public OtherUserControl othTabUsrCtrl;

        delegate void SerialRecvDelegate();

        private string[] ports;

        private string port;
        /**
         *  COM 포트 설정
         */
        public string Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }

        private string speed;
        /**
         *  COM Speed 설정
         * */
        public string Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }
        private string[] speeds = new string[] { "2400", "4800", "9600", "19200", "22800", "38400", "57600", "115200" };

        private bool _isRuning = false;
        public bool isRuning
        {
            get { return _isRuning; }
            set
            {
                _isRuning = value;
                OnPropertyChanged("isRuning");
            }
        }

        public RecvBufferStruct recvBuff;

        public MainWindow()
        {
            InitializeComponent();

            /// MainWindow과  데이터 동기화를 하기 위해서는 아래 문장을 실행 시켜 준다.
            DataContext = this;

            recvTabUsrCtrl = (RecvUserControl)rcvTabCtrl.Content;
            sndTabUsrCtrl = (SendUserControl)sndTabCtrl.Content;
            othTabUsrCtrl = (OtherUserControl)othTabCtrl.Content;

            Form = this;

            m_cfg = new ConfigManager(this);
            GetCfgManager().setFileName(System.AppDomain.CurrentDomain.BaseDirectory + @"HMCUSIM.ini");
            GetCfgManager().Load();

           

            recvTab = recvTabUsrCtrl;
            sndTab = sndTabUsrCtrl;
            othTab = othTabUsrCtrl;

            recvTabUsrCtrl.ethIP.Text = SvrIP;
            recvTabUsrCtrl.ethPort.Text = svrport;

            /// 시리얼 통신이면 초기화 과정을 수행한다.
            if(comm == CommMethod.Serial)
            {
                this.Loaded += new RoutedEventHandler(InitSerialPort);
                commHandler = new SerialHandler();
                frameHeader = new SerialHeader();
            }
            else
            {
                //commHandler = new EtherHandler();
                commHandler = new EtherHandler(new AsyncCallback(SendCallback));
                frameHeader = new EthHeader();
            }

            isRuning = false;

            recvBuff = new RecvBufferStruct();

        }
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            try
            {

                runServer = false;

                if (commHandler != null)
                {
                    commHandler.Close();
                }

                /// 서버 종료시 처리
                if (g_listener != null)
                {
                    g_listener.Close();
                    g_listener.Dispose();
                }




            }
            catch (NullReferenceException ne)
            {
            }
            catch (Exception)
            {
            }


        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            GetCfgManager().Load();
            recvTabUsrCtrl.ethPort.Text = svrport;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            svrport = recvTabUsrCtrl.ethPort.Text;
            svrIP = recvTabUsrCtrl.ethIP.Text;
            GetCfgManager().Save();
        }

        public Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null) return null;
            if (element.GetType() == type) return element;
            Visual foundElement = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }

        static string[] car_class = 
      { "가","나","다","라","마","바","사","아","자","차","카","타","파","하",
        "거","너","더","러","머","버","서","어","저","처","커","터","퍼","허",
        "고","노","도","로","모","보","소","오","조","초","코","토","포","호",
        "구","누","두","루","무","부","수","우","주","추","쿠","투","푸","후",
        "그","느","드","르","므","브","스","으","즈","츠","크","트","프","흐",
        "기","니","디","리","미","비","시","이","지","치","키","티","피","히",
        "육","해","공","국","합","_","배","외"};
        /// <summary>
        /// string으로 입력된 번호판을 bcd로 변환하여 준다.
        /// </summary>
        /// <param name="plateNum"></param>
        /// <returns></returns>
        static public byte[] SetPlateNum(string plateNum)
        {
            byte[] bcdPlate = new byte[5];
            string region = plateNum.Substring(0, 2);
            string region1 = plateNum.Substring(0, 1);
            int index = 0;
            int b_index = 0;
            switch (region)
            {
                case "서울":
                    bcdPlate[0] = 0x00;
                    index += 2;
                    break;
                case "부산":
                    bcdPlate[0] = 0x01;
                    index += 2;
                    break;
                case "인천":
                    bcdPlate[0] = 0x02;
                    index += 2;
                    break;
                case "대구":
                    bcdPlate[0] = 0x03;
                    index += 2;
                    break;
                case "광주":
                    bcdPlate[0] = 0x04;
                    index += 2;
                    break;
                case "대전":
                    bcdPlate[0] = 0x05;
                    index += 2;
                    break;
                case "경기":
                    bcdPlate[0] = 0x06;
                    index += 2;
                    break;
                case "강원":
                    bcdPlate[0] = 0x07;
                    index += 2;
                    break;
                case "충북":
                    bcdPlate[0] = 0x08;
                    index += 2;
                    break;
                case "충남":
                    bcdPlate[0] = 0x09;
                    index += 2;
                    break;
                case "전북":
                    bcdPlate[0] = 0x10;
                    index += 2;
                    break;
                case "전남":
                    bcdPlate[0] = 0x11;
                    index += 2;
                    break;
                case "경북":
                    bcdPlate[0] = 0x12;
                    index += 2;
                    break;
                case "경남":
                    bcdPlate[0] = 0x13;
                    index += 2;
                    break;
                case "제주":
                    bcdPlate[0] = 0x14;
                    index += 2;
                    break;
                case "울산":
                    bcdPlate[0] = 0x15;
                    index += 2;
                    break;
                case "외교":
                    bcdPlate[0] = 0x98;
                    index += 2;
                    break;
                default:
                    switch(region1)
                    {
                        case "0":
                            bcdPlate[0] = 0x80;
                            index += 1;
                            break;
                        case "1":
                            bcdPlate[0] = 0x81;
                            index += 1;
                            break;
                        case "2":
                            bcdPlate[0] = 0x82;
                            index += 1;
                            break;
                        case "3":
                            bcdPlate[0] = 0x83;
                            index += 1;
                            break;
                        case "4":
                            bcdPlate[0] = 0x84;
                            index += 1;
                            break;
                        case "5":
                            bcdPlate[0] = 0x85;
                            index += 1;
                            break;
                        case "6":
                            bcdPlate[0] = 0x86;
                            index += 1;
                            break;
                        case "7":
                            bcdPlate[0] = 0x87;
                            index += 1;
                            break;
                        case "8":
                            bcdPlate[0] = 0x88;
                            index += 1;
                            break;
                        case "9":
                            bcdPlate[0] = 0x89;
                            index += 1;
                            break;
                        default:
                            bcdPlate[0] = 0x99;
                            index += 2;
                            break;
                    }
                    break;
            }
            b_index += 1;
            ///상위번호
            string sNum = plateNum.Substring(index, 2);
            string tmpStr = Regex.Replace(sNum, @"\D", "");  ///숫자만 추출
            int stInt = int.Parse(tmpStr);

            byte[] bsNum = ((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(stInt);
            Buffer.BlockCopy(bsNum, 0, bcdPlate, b_index, Marshal.SizeOf(typeof(byte)));
            if(stInt < 10)
            {
                index += Marshal.SizeOf(typeof(byte));
            }
            else
            {
                index += Marshal.SizeOf(typeof(short));
            }
            b_index += 1;

            //차분류
            string cClass = plateNum.Substring(index, 1);
            int cIndex = 0;
            foreach( string s in car_class)
            {

                if(s.CompareTo(cClass) == 0)
                {
                    break;
                }
                cIndex++;
            }

            byte[] bClass = ((MainWindow)System.Windows.Application.Current.MainWindow).ByteToBCD(cIndex);
            Buffer.BlockCopy(bClass, 0, bcdPlate, b_index, Marshal.SizeOf(typeof(byte)));
            index += Marshal.SizeOf(typeof(byte));
            b_index += 1;

            //번호판번호
            string szNum = plateNum.Substring(index, 4);
            int nIntNum = int.Parse(szNum);
            byte[] bmNum = ((MainWindow)System.Windows.Application.Current.MainWindow).IntToBCD(nIntNum);
            Buffer.BlockCopy(bmNum, 0, bcdPlate, b_index, Marshal.SizeOf(typeof(short)));
            index += Marshal.SizeOf(typeof(short));
            b_index += 1;
            return bcdPlate;
        }
        static public string GetPlateNum(byte[] bcd)
        {
            bool isYong = false;  //영자 존재 여부
            StringBuilder sb = new StringBuilder();
            switch(bcd[0])
            {
                case 0:
                    sb.Append("서울");
                    break;
                case 1:
                    sb.Append("부산");
                    break;
                case 2:
                    sb.Append("인천");
                    break;
                case 3:
                    sb.Append("대구");
                    break;
                case 4:
                    sb.Append("광주");
                    break;
                case 5:
                    sb.Append("대전");
                    break;
                case 6:
                    sb.Append("경기");
                    break;
                case 7:
                    sb.Append("강원");
                    break;
                case 8:
                    sb.Append("충북");
                    break;
                case 9:
                    sb.Append("충남");
                    break;
                case 0x10:
                    sb.Append("전북");
                    break;
                case 0x11:
                    sb.Append("전남");
                    break;
                case 0x12:
                    sb.Append("경북");
                    break;
                case 0x13:
                    sb.Append("경남");
                    break;
                case 0x14:
                    sb.Append("제주");
                    break;
                case 0x15:
                    sb.Append("울산");
                    break;
                case 0x16:
                    sb.Append("세종");
                    break;
                case 0x40:
                    sb.Append("영서울");
                    break;
                case 0x41:
                    sb.Append("영부산");
                    break;
                case 0x42:
                    sb.Append("영인천");
                    break;
                case 0x43:
                    sb.Append("영대구");
                    break;
                case 0x44:
                    sb.Append("영광주");
                    break;
                case 0x45:
                    sb.Append("영대전");
                    break;
                case 0x46:
                    sb.Append("영경기");
                    break;
                case 0x47:
                    sb.Append("영강원");
                    break;
                case 0x48:
                    sb.Append("영충북");
                    break;
                case 0x49:
                    sb.Append("영충남");
                    break;
                case 0x50:
                    sb.Append("영전북");
                    break;
                case 0x51:
                    sb.Append("영전남");
                    break;
                case 0x52:
                    sb.Append("영경북");
                    break;
                case 0x53:
                    sb.Append("영경남");
                    break;
                case 0x54:
                    sb.Append("영제주");
                    break;
                case 0x55:
                    sb.Append("영울산");
                    break;
                case 0x56:
                    sb.Append("영세종");
                    break;
                case 0x80:
                    sb.Append("0");
                    break;
                case 0x81:
                    sb.Append("1");
                    break;
                case 0x82:
                    sb.Append("2");
                    break;
                case 0x83:
                    sb.Append("3");
                    break;
                case 0x84:
                    sb.Append("4");
                    break;
                case 0x85:
                    sb.Append("5");
                    break;
                case 0x86:
                    sb.Append("6");
                    break;
                case 0x87:
                    sb.Append("7");
                    break;
                case 0x88:
                    sb.Append("8");
                    break;
                case 0x89:
                    sb.Append("9");
                    break;
                case 0x98:
                    sb.Append("외교");
                    break;
                case 0x99:;
                    break;
                default:
                    break;

            }
            //small number
            sb.Append(bcd[1].ToString("X2"));

            int carclass = (int)(bcd[2] >> 4) * 10 + (int)(bcd[2] & 0x0F);

            if(carclass <  car_class.Length)
            {
                sb.Append(car_class[carclass]);
            }

            sb.Append(bcd[3].ToString("X2"));
            sb.Append(bcd[4].ToString("X2"));


            return sb.ToString();
        }

        private void IsEtherSerial_Checked(object sender, RoutedEventArgs e)
        {
            if (comm == CommMethod.Serial)
            {
                this.Loaded += new RoutedEventHandler(InitSerialPort);
                commHandler = new SerialHandler();
                frameHeader = new SerialHeader();
            }
            else
            {
                //commHandler = new EtherHandler();
                commHandler = new EtherHandler(new AsyncCallback(SendCallback));
                frameHeader = new EthHeader();
            }
        }

    }
}
