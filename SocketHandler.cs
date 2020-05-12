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
using System.Runtime.InteropServices;

namespace HMCU_Sim
{
    using DWORD = System.UInt32;
    using WORD = System.UInt16;

    public class EtherHandler : CommHandler
    {

        private Socket csocketHandler;
        private AsyncCallback m_callback;

        public EtherHandler()
        {
            m_callback = null;
        }

        public EtherHandler(AsyncCallback callback)
        {
            m_callback = callback;
        }

        /// <summary>
        /// SocketHandler 설정
        /// </summary>
        public Socket CSocketHandler
        {
            get
            {
                return (csocketHandler);
            }
            set
            {
                csocketHandler = value;
            }
        }

        /// <summary>
        /// 송신을 재정의한다.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len"></param>
        public override void Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
            m_callback = callback;
            csocketHandler.BeginSend(buffer, offset, size, socketFlags,
                m_callback, (state == null) ? csocketHandler : state);
        }

        public override void Send(byte[] buffer, int size)
        {
            csocketHandler.BeginSend(buffer, 0, size, 0,
                m_callback, csocketHandler);
        }

        public override void Close()
        {
            if(csocketHandler != null)
            {
                csocketHandler.Shutdown(SocketShutdown.Both);
                csocketHandler.Close();
                csocketHandler.Dispose();
                csocketHandler = null;
            }
            
        }
        /// <summary>
        /// 소켓이 유효한지를 나타낸다.
        /// </summary>
        /// <returns></returns>
        public override bool IsRun()
        {
            bool status = false;

            if(csocketHandler != null)
            {
                status = true;
            }

            return status;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return 0;
        }

        public override bool IsConnected()
        {
            return csocketHandler.Connected;
        }
    }

    public partial class MainWindow : Window
    {

        public enum Order
        {
            SLAVE,
            MASTER,
        }

        public static MainWindow Form;
        public static RecvUserControl recvTab;
        public static SendUserControl sndTab;

        private WORD msgCnt;

        private bool runServer;

        public Order or = Order.MASTER;
        private WORD devNum = 0x100;    //장비 번호


        // Get local IP
        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    return localIP;
                }
            }
            return "127.0.0.1";
        }
        private void SocketServer_Click(object sender, RoutedEventArgs e)
        {
            if(comm == CommMethod.Ethernet)
            {
                if (string.Compare(SvrBtnText.Text, "서버 종료") == 0)
                {
                    try
                    {
                        /// 서버 종료시 처리
                        //ConnectionBtn.Content = "서버 시작";
                        SvrBtnText.Text = "서버 시작";
                        isRuning = false;

                        runServer = false;

                        if (commHandler.IsRun())
                        {
                            commHandler.Close();
                            //csocketHandler.Dispose();
                            //csocketHandler.Close();

                        }

                        if (g_listener != null)
                        {
                            g_listener.Dispose();
                            g_listener.Close();

                        }

                        DisplayText(recvTabUsrCtrl.CommRxList, "서버가 종료 되었습니다.");

                    }
                    catch (Exception ex)
                    {
                        DisplayText(recvTabUsrCtrl.CommRxList, ex.Message);
                    }

                }
                else
                {
                    //ConnectionBtn.Content = "서버 종료";
                    SvrBtnText.Text = "서버 종료";
                    isRuning = true;


                    runServer = true;

                    byte[] tmp = new byte[4];
                    // Establish the local endpoint for the socket.  
                    // The DNS name of the computer  
                    // running the listener is "host.contoso.com".  
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];
                    // tmp = vdu1ip.GetAddressBytes();

                    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(recvTabUsrCtrl.ethIP.Text), Int32.Parse(recvTabUsrCtrl.ethPort.Text));
                    //IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(LocalIPAddress()), Int32.Parse(recvTabUsrCtrl.ethPort.Text));


                    // Create a TCP/IP socket.  
                    // g_listener = new Socket(ipAddress.AddressFamily,
                    //     SocketType.Stream, ProtocolType.Tcp);
                    g_listener = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

                    int option = 1; ///SO_RESUADDR의 옵션 값을 TRUE로
                    g_listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, option);

                    // Bind the socket to the local endpoint and listen for incoming connections.  
                    try
                    {
                        g_listener.Bind(localEndPoint);
                        g_listener.Listen(100);
                        new Thread(delegate ()
                        {
                            while (runServer)
                            {
                                // Set the event to nonsignaled state.  
                                allDone.Reset();

                                // Start an asynchronous socket to listen for connections.  
                                Console.WriteLine("Waiting for a connection...");
                                g_listener.BeginAccept(
                                    new AsyncCallback(AcceptCallback),
                                    g_listener);

                                // Wait until a connection is made before continuing.  
                                allDone.WaitOne();

                            }
                        }).Start();

                        DisplayText(recvTabUsrCtrl.CommRxList, "서버가 시작 되었습니다.");


                    }
                    catch (Exception ex)
                    {
                        DisplayText(recvTabUsrCtrl.CommRxList, ex.Message);
                    }

                }
            }
            else
            {
                 SerialConnet_Click(this, new RoutedEventArgs());
            }

            

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Signal the main thread to continue.  
                allDone.Set();

                if (Form.runServer != true)
                    return;

                // Get the socket that handles the client request.  
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = handler;
                EtherHandler eh = (EtherHandler)Form.commHandler;
                eh.CSocketHandler = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                if (handler.Connected)
                    Form.Dispatcher.Invoke(new UpdateTextDelegate(Form.DisplayText), recvTab.CommRxList, "클라이언트 연결!!");
            }
            catch(SocketException ex)
            {
                //Form.Dispatcher.Invoke(new UpdateTextDelegate(Form.DisplayText), Form.SocketRxList, ex.Message);
            }
           
        }

        private delegate void UpdateTextDelegate(ListBox listBox,string str);
        private delegate void UpdateButtonTextDelegate(string str);

        private void UpdateButtonText(string str)
        {
            //ConnectionBtn.Content = str;
            SvrBtnText.Text = str;
        }

        private void DisplayText(ListBox listBox, string str)
        {
            
            listBox.Items.Add(str);
            if (listBox.Items.Count > 100)
            {
                listBox.Items.Clear();
            }
            listBox.SelectedIndex = recvTabUsrCtrl.CommRxList.Items.Count - 1;
            ScrollViewer _listboxScrollViewer = GetDescendantByType(listBox, typeof(ScrollViewer)) as ScrollViewer;
            _listboxScrollViewer.ScrollToEnd();
            return;
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            EtherHandler eh = (EtherHandler)Form.commHandler;
            eh.CSocketHandler = handler;

            try
            {
                if (handler != null)
                {
                    if (handler.Connected)
                    {
                        // Read data from the client socket.   
                        int bytesRead = handler.EndReceive(ar);

                        if (bytesRead > 0)
                        {
                            // There  might be more data, so store the data received so far.  
                            //state.sb.Append(Encoding.ASCII.GetString(
                            //  state.buffer, 0, bytesRead));

                            // Check for end-of-file tag. If it is not there, read   
                            // more data.  

                            string str = string.Empty;
                            int cnt = 0;
                            foreach (byte b in state.buffer)
                            {
                                str += string.Format("[" + "{0:x2}" + "]", b);
                                cnt++;
                                if (cnt == bytesRead)
                                    break;
                            }

                            Form.Dispatcher.Invoke(() =>
                            {
                                //code 로 메시지 확인
                                switch(state.buffer[Frame.Code])
                                {
                                    
                                    case Code.ACK:
                                        recvTab.SeqNum = (int)state.buffer[Frame.Seq];  ///전송연번 업데이트
                                        break;
                                    case Code.NACK:
                                        recvTab.SeqNum = (int)state.buffer[Frame.Seq];  ///전송연번 업데이트
                                        //추후 재전송 로직 추가
                                        break;
                                    case Code.STATUS_RES:  ///상태정보 수신
                                        {
                                            recvTab.SeqNum = (int)state.buffer[Frame.Seq];  ///전송연번 업데이트
                                            //ACK를 보내줌.
                                            sndTab.MakeFrame(Code.ACK, out byte[] data, Form.comm);
                                            Send(data);
                                        }
                                        break;
                                    case Code.VIO_CONFIRM_REQ:   ///위반확인요구 수신
                                        {
                                            recvTab.SeqNum = (int)state.buffer[Frame.Seq];  ///전송연번 업데이트
                                            //ACK를 보내줌.
                                            //sndTab.MakeEtherFrame(Code.ACK, out byte[] data);
                                            //Send(handler, data);

                                            int nCopy = Marshal.SizeOf(typeof(PACKET_VIO_REQUEST));
                                            byte[] _cpyArray = new byte[nCopy];

                                            Array.Copy(state.buffer, Frame.Data, _cpyArray, 0,nCopy);

                                            //위반확인응답을 보내줌.
                                            PACKET_VIO_REQUEST pVioReq = (PACKET_VIO_REQUEST)PacketMethods.ByteToStructure(_cpyArray, typeof(PACKET_VIO_REQUEST));
                                            if(pVioReq.imgStatus ==  0x00)
                                            {
                                                recvTab.triggerStatus.Text = "정상";
                                            }
                                            else
                                            {
                                                recvTab.triggerStatus.Text = "비정상";
                                            }

                                            ProcItem pItem = new ProcItem();
                                            pItem.seq = state.buffer[Frame.Seq];
                                            pItem.vioNum = pVioReq.imagNum;
                                            sndTab.procList.Add(pItem);
                                            /// 영상번호 업데이트
                                            recvTab.imageNum.Text = pVioReq.imagNum.ToString();
                                            if(sndTab.syncMethod.SelectedIndex == 1)
                                            {
                                                sndTab.VioNumber = pVioReq.imagNum;
                                            }
                                            

                                            //sndTab.MakeEtherFrame(Code.VIO_CONFIRM_RES, out byte[] data);
                                            //Send(handler, data);

                                        }
                                        break;
                                    case Code.PLATE_RECOG_NOTIFY:
                                        {
                                            recvTab.SeqNum = (int)state.buffer[Frame.Seq];  ///전송연번 업데이트
                                        }
                                        break;
                                    default:
                                        break;
                                }

                            }
                            );

                            try
                            {
                                Form.Dispatcher.Invoke(new UpdateTextDelegate(Form.DisplayText), recvTab.CommRxList, str);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }



                            if (content.IndexOf("<EOF>") > -1)
                            {
                                // All the data has been read from the   
                                // client. Display it on the console.  
                                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                                    content.Length, content);
                                // Echo the data back to the client.  
                                Send(content);
                            }
                            else
                            {
                                // Not all data received. Get more.  
                                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                            }
                        }
                        else
                        {
                            //소켓을 끊을 때.
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                            try
                            {
                                Form.Dispatcher.Invoke(new UpdateTextDelegate(Form.DisplayText), recvTab.CommRxList, "클라이언트 연결 끊김!!");
                                if (Form.runServer == false)
                                {
                                    Form.Dispatcher.Invoke(new UpdateButtonTextDelegate(Form.UpdateButtonText), "서버 시작");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }

                    }
                    else
                    {
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                        Form.Dispatcher.Invoke(new UpdateTextDelegate(Form.DisplayText), recvTab.CommRxList, "강제로 소켓 끊김");
                        Form.Dispatcher.Invoke(new UpdateButtonTextDelegate(Form.UpdateButtonText), "서버 시작");
                    }
                }
                else
                {
                    Form.Dispatcher.Invoke(new UpdateTextDelegate(Form.DisplayText), recvTab.CommRxList, "Client 소켓이 유효하지 않음");
                }
            }
            catch(Exception e)
            {

                if (Form.runServer == false)
                {
                    //사람이 서버를 종료한 경우에 한하여 버튼을 바꾼다.
                    Form.Dispatcher.Invoke(new UpdateButtonTextDelegate(Form.UpdateButtonText), "서버 시작");
                }
                Form.Dispatcher.Invoke(new UpdateTextDelegate(Form.DisplayText), recvTab.CommRxList, "소켓 종료됨");
            }

            
            
        }

        private static void Send( String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  

            //Form.commHandler.Send(byteData, 0, byteData.Length, 0,
            //                new AsyncCallback(SendCallback), null);
            Form.commHandler.Send(byteData, byteData.Length);

            string str = string.Empty;

            foreach (byte b in byteData)
            {
                str += string.Format("[" + "{0:x2}" + "]", b);
            }

            try
            {
                Form.Dispatcher.Invoke(new UpdateTextDelegate(Form.DisplayText), sndTab.CommTxList, str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static void Send( byte[] byteData)
        {
            try
            {
                if (Form.commHandler.IsRun() == true)
                {
                    if (Form.commHandler.IsConnected())
                    {
                        // Begin sending the data to the remote device.  
                        //Form.commHandler.Send(byteData, 0, byteData.Length,0,
                        //    new AsyncCallback(SendCallback), null);
                        Form.commHandler.Send(byteData, byteData.Length);

                        StringBuilder sb = new StringBuilder();

                        foreach (byte b in byteData)
                        {
                            sb.Append(string.Format("[" + "{0:x2}" + "]", b));
                        }

                        try
                        {
                            Form.Dispatcher.Invoke(new UpdateTextDelegate(Form.DisplayText), sndTab.CommTxList, sb.ToString());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Form.Dispatcher.Invoke(new UpdateTextDelegate(Form.DisplayText), sndTab.CommTxList, e.ToString());
            }
            
            
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// 이더넷 헤더를 만든는 함수
        /// </summary>
        /// <param name="Datalen">데이터 크기</param>
        /// <param name="retryCnt">재시도 횟수</param>
        /// <param name="devSerial">장비명</param>
        /// <param name="header">출력</param>
        private void MakeEthHeader(int Datalen, byte retryCnt, byte[] devCode, out byte [] header)
        {
            EthHeader ethHeader = new EthHeader();

            header = new byte[ethHeader.FLen];
            header[0] = Protocols.STX;
            byte[] bytes = BitConverter.GetBytes(msgCnt);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Buffer.BlockCopy(bytes, 0, header, 1, 2);
            header[3] = retryCnt;  //재 시도 횟수 최대 3회

            byte[] bDataLen = BitConverter.GetBytes((WORD)Datalen);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bDataLen);
            Buffer.BlockCopy(bDataLen, 0, header, 4, 2);
            header[6] = devCode[2];
            header[7] = devCode[3];
            header[8] = 0;  /// 6, 7은 예비
            header[9] = 0;
            msgCnt++;

        }

        public void SendMatchInfo(byte [] data, int len)
        {
            int device = ((int)or << 7) | devNum;

            byte [] bdevCode = BitConverter.GetBytes(device);
            Array.Reverse(bdevCode);

            EthHeader ethHeader = new EthHeader();

            MakeEthHeader(ethHeader.TLen, 0, bdevCode, out byte[] header);
            byte[] frame = new byte[header.Length + len + 1];
            Buffer.BlockCopy(header, 0, frame, 0, header.Length);
            Buffer.BlockCopy(data, 0, frame, header.Length, len);
            frame[frame.Length - 1] = Protocols.ETX;

            Send(frame);
        }

        public void SendTimeSync(byte[] data, int len)
        {
            int device = ((int)or << 7) | devNum;

            byte[] bdevCode = BitConverter.GetBytes(device);
            Array.Reverse(bdevCode);

            EthHeader ethHeader = new EthHeader();

            MakeEthHeader(ethHeader.TimeLen, 0, bdevCode, out byte[] header);
            byte[] frame = new byte[header.Length + len + 1];
            Buffer.BlockCopy(header, 0, frame, 0, header.Length);
            Buffer.BlockCopy(data, 0, frame, header.Length, len);
            frame[frame.Length - 1] = Protocols.ETX;
            Send(frame);
        }

        public void SendHeartBeat(byte[] data, int len)
        {
            Send(data);
        }

        public void SendEtherData(byte[] data, int len)
        {
            Send(data);
        }

        public byte[] ByteToBCD(int input)
        {
            if (input > 99 || input < 0)
                throw new ArgumentOutOfRangeException("input");

            int tens = input / 10;
            int ones = (input -= tens * 10);

            byte[] bcd = new byte[] {
                (byte)(tens << 4 | ones)
             };

            return bcd;
        }

        public byte[] IntToBCD(int input)
        {
            if (input > 9999 || input < 0)
                throw new ArgumentOutOfRangeException("input");

            int thousands = input / 1000;
            int hundreds = (input -= thousands * 1000) / 100;
            int tens = (input -= hundreds * 100) / 10;
            int ones = (input -= tens * 10);

            byte[] bcd = new byte[] {
                (byte)(thousands << 4 | hundreds),
                (byte)(tens << 4 | ones)
             };

            return bcd;
        }

        public byte[] Int32ToBCD(Int32 input)
        {
            if (input > 99999999 || input < 0)
                throw new ArgumentOutOfRangeException("input");


            int h8 = input / 10000000;
            int h7 = (input -= h8 * 10000000) / 1000000;
            int h6 = (input -= h7 * 1000000) / 100000;
            int h5 = (input -= h6 * 100000) / 10000;

            int h4 = (input -= h5 * 10000) / 1000;
            int h3 = (input -= h4 * 1000) / 100;
            int h2 = (input -= h3 * 100) / 10;
            int h1 = (input -= h2 * 10);

            byte[] bcd = new byte[] {
                (byte)(h8 << 4 | h7),
                (byte)(h6 << 4 | h5),
                (byte)(h4 << 4 | h3),
                (byte)(h2 << 4 | h1)
             };

            return bcd;
        }

        private int BCDToInt(byte bcd)
        {
            int result = 0;
            result += (int)(10 * (bcd >> 4));
            result += (int)(bcd & 0xf);
            return result;
        }


        static public WORD BCDToWORD(byte[] bcd)
        {
            WORD result = 0;
            result += (WORD)(1000 * (bcd[0] >> 4));
            result += (WORD)(100 *(bcd[0] & 0xf));
            result += (WORD)(10 * (bcd[1] >> 4));
            result += (WORD)(bcd[1] & 0xf);
            return result;
        }
        /// <summary>
        /// bcd 값을 int 형으로 바꾸는 함수이다. type 형을 넣어 주면 그것 크기만큼만 변환한다.
        /// </summary>
        /// <param name="bcd">bcd 배열</param>
        /// <param name="param">타입형</param>
        /// <returns></returns>
        static public int BCDToInt(byte[] bcd, Type param)
        {
            int size = Marshal.SizeOf(param);
            int result = 0;
            int j = 0;
            for(int i = 0; i< size; i++)
            {
                result += (int)(Math.Pow(10, 2*size - 1 - j) * (bcd[i] >> 4));
                j++;
                result += (int)(Math.Pow(10, 2*size - 1 - j) * (bcd[i] & 0x0f));
                j++;
            }
            return result;
        }

    }
}
