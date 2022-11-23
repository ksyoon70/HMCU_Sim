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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HMCU_Sim
{

    public class SerialHandler : CommHandler
    {

        public SerialPort m_Port;
        private SerialDataReceivedEventHandler m_recvHandler;

        public SerialHandler()
        {
            m_Port = new SerialPort();
            m_recvHandler = null;
        }

        /// <summary>
        /// 전송을 재정의 한다.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len"></param>
        public override void Send(byte[] data, int len)
        {
            m_Port.Write(data, 0, len);
        }

        public override void Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
        }

        public override void Close()
        {
            m_Port.Close();
            if (m_recvHandler != null)
            {
                m_Port.DataReceived -= m_recvHandler;
                m_recvHandler = null;
            }
        }

        public override bool IsRun()
        {
            bool status = false;

            if (m_Port.IsOpen)
            {
                status = true;
            }

            return status;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_Port.Read(buffer, offset, count - offset);
        }

        public void Init(string port, int BaudRate, SerialDataReceivedEventHandler recvHandler)
        {
            m_Port.PortName = port;
            m_Port.BaudRate = BaudRate; //int.Parse(speed.Trim());
            m_Port.DataBits = 8;
            m_Port.StopBits = StopBits.One;
            m_Port.Parity = Parity.None;
            m_Port.Open();
            m_Port.DataReceived += recvHandler;
            m_recvHandler = recvHandler;
        }

        public override bool IsConnected()
        {
            return true;
        }
    }
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// 시리얼 연결 함수.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SerialConnet_Click(object sender, RoutedEventArgs e)
        {
            if (comm == CommMethod.Serial)
            {
                if (commHandler.IsRun())
                {
                    SvrBtnText.Text = "서버 시작";
                    isRuning = false;

                    SerialHandler sh = (SerialHandler)commHandler;
                    commHandler.Close();
                    recvTabUsrCtrl.CommRxList.Items.Add("종료 되었습니다.");

                }
                else if(SvrBtnText.Text == "서버 종료")
                {
                    // 소켓이 연결되어있을 경우 소켓 종료
                    try
                    {
                        /// 서버 종료시 처리
                        SvrBtnText.Text = "서버 시작";
                        isRuning = false;

                        runServer = false;

                        if (commHandler.IsRun())
                        {
                            commHandler.Close();
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
                    //ConnectionBtn.Content = "시리얼 종료";
                    SvrBtnText.Text = "시리얼 종료";
                    isRuning = true;

                    try
                    {
                        SerialHandler sh = (SerialHandler)commHandler;
                        sh.Init(port, int.Parse(speed.Trim()), new SerialDataReceivedEventHandler(Serial_DataReceived));
                        recvTabUsrCtrl.CommRxList.Items.Add(port + " " + speed + "bps" + "로 연결 되었습니다.");
                        recvTabUsrCtrl.CommRxList.ScrollIntoView(recvTabUsrCtrl.CommRxList.SelectedItem);
                    }
                    catch (Exception err)
                    {
                        recvTabUsrCtrl.CommRxList.Items.Add(err.ToString());
                        recvTabUsrCtrl.CommRxList.ScrollIntoView(recvTabUsrCtrl.CommRxList.SelectedItem);
                    }

                }
            }
            else
            {
                MessageBox.Show("통신방식이 Serial이 아닙니다.");
            }


        }
        void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            int len = commHandler.Read(dataBuf.buff, dataBuf.buffLen, dataBuf.buff.Length);
            dataBuf.buffLen += len;

            if (dataBuf.buffLen <= frameHeader.MinFrameLen)
            {
                return;
            }

            if (dataBuf.buff[dataBuf.buffLen - 2] != Protocols.ETX || dataBuf.buff[dataBuf.buffLen - 3] != Protocols.DLE)
            {
                return;
            }


            if (recvBuff.buffLen >= 256)
            {
                recvBuff.reset();
            }


            Array.Copy(dataBuf.buff,0, recvBuff.buff, recvBuff.buffLen, dataBuf.buffLen);
            recvBuff.buffLen += dataBuf.buffLen;
            dataBuf.reset();


            SerialRecvDelegate srdel = delegate ()
            {
                byte[] array = new byte[256];
                string str = string.Empty;
                StringBuilder sb = new StringBuilder();

                bool findFrame = false;

                while (recvBuff.buffLen > 0)
                {
                    findFrame = false;
                    sb.Clear();

                    int recvLen = recvBuff.buffLen;
                    for(int i = frameHeader.MinFrameLen; i < recvLen; i++)
                    {
                        if (recvBuff.buff[0] == Protocols.DLE && recvBuff.buff[1] == Protocols.STX && recvBuff.buff[i - 1] == Protocols.ETX && recvBuff.buff[i - 2] == Protocols.DLE)
                        {
                            frameBuf.buffLen = i + 1;
                            recvBuff.buffLen = i + 1;
                            Array.Copy(recvBuff.buff, 0, frameBuf.buff, 0, recvLen);
                            findFrame = true;
                            break;
                        }
                    }
                    

                    if (findFrame == false)
                          return;

                    byte revBcc = frameBuf.buff[frameBuf.buffLen - 1];  //BCC 저장

                    Array.Copy(frameBuf.buff, 4, array, 0, frameBuf.buffLen - 7); // DLE STX LEN CODE ~ DLE ETX BCC 를 뺌.
                    int validSize = DelDLE(ref array, frameBuf.buffLen - 7);
                    int delLen = frameBuf.buffLen;
                    Array.Copy(array, 0, frameBuf.buff, 4, validSize);

                    byte[] bccData = new byte[validSize + 1]; //LEN이 빠진 데이터 길이. Code + Seq + Data 까지검사
                    bccData[0] = frameBuf.buff[3]; // code
                    Array.Copy(array, 0, bccData, 1, validSize);

                    byte calBcc = MainWindow.CalBCC(bccData, bccData.Length);

                    if (revBcc != calBcc)
                    {
                        
                        Array.Clear(frameBuf.buff, 0, frameBuf.buff.Length);
                        sb.Append("BCC 오류");
                        frameBuf.reset();  //BCC 오류
                        recvBuff.buffLen = 0;
                        return;
                    }
                    else
                    {
                        //frameBuf.buffLen = 5 + validSize;
                        frameBuf.buff[frameBuf.buffLen - 3] = Protocols.DLE;
                        frameBuf.buff[frameBuf.buffLen - 2] = Protocols.ETX;
                        frameBuf.buff[frameBuf.buffLen - 1] = revBcc;
                    }

                    recvBuff.buffLen -= delLen;

                    switch (frameBuf.buff[frameHeader.CodePos])
                    {
                        case Code.ACK:
                            recvTab.SeqNum = (int)frameBuf.buff[frameHeader.SeqPos];  ///전송연번 업데이트
                            break;
                        case Code.NACK:
                            recvTab.SeqNum = (int)frameBuf.buff[frameHeader.SeqPos];  ///전송연번 업데이트
                            //추후 재전송 로직 추가
                            break;
                        case Code.STATUS_RES:  ///상태정보 수신
                            {
                                recvTab.SeqNum = (int)frameBuf.buff[frameHeader.SeqPos];  ///전송연번 업데이트
                                //ACK를 보내줌.
                                ProcItem item = null;
                                sndTab.MakeFrame(Code.ACK, out byte[] data, comm, ref item);
                                data[frameHeader.SeqPos] = frameBuf.buff[frameHeader.SeqPos];
                                //commHandler.Send(data,data.Length);
                                MainWindow.Send(data);
                            }
                            break;
                        case Code.VIO_CONFIRM_REQ:   ///위반확인요구 수신
                            {
                                ProcItem item = null;
                                recvTab.SeqNum = (int)frameBuf.buff[frameHeader.SeqPos];  ///전송연번 업데이트
                                sndTab.MakeFrame(Code.ACK, out byte[] data, comm, ref item);
                                data[frameHeader.SeqPos] = frameBuf.buff[frameHeader.SeqPos];
                                //commHandler.Send(data, data.Length);
                                MainWindow.Send(data);
                                //ACK를 보내줌.
                                int nCopy = Marshal.SizeOf(typeof(PACKET_VIO_REQUEST));
                                byte[] _cpyArray = new byte[nCopy];
                                Array.Copy(frameBuf.buff, 5, _cpyArray, 0, nCopy);

                                //위반확인응답을 보내줌.
                                PACKET_VIO_REQUEST pVioReq = (PACKET_VIO_REQUEST)PacketMethods.ByteToStructure(_cpyArray, typeof(PACKET_VIO_REQUEST));
                                if (pVioReq.imgStatus == 0x00)
                                {
                                    recvTab.triggerStatus.Text = "정상";
                                }
                                else
                                {
                                    recvTab.triggerStatus.Text = "비정상";
                                }

                                ProcItem pItem = new ProcItem((uint)sndTab.pcComboBox.SelectedIndex + 1);
                                pItem.seq = frameBuf.buff[frameHeader.SeqPos];
                                pItem.vioNum = pVioReq.imagNum;
                                sndTab.procList.Add(pItem);
                                /// 영상번호 업데이트
                                recvTab.imageNum.Text = pVioReq.imagNum.ToString();
                                if (sndTab.syncMethod.SelectedIndex == 1)
                                {
                                    sndTab.VioNumber = pVioReq.imagNum;
                                }

                                //위반확인자동응답 체크 시 전송을 수행함.
                                if (othTab.autoVioSendCheck.IsChecked == true)
                                {
                                    int maxLoop = sndTab.pcComboBox.SelectedIndex + 1;
                                    uint saveProcNum = sndTab.ProcNumber1;
                                    for (sndTab.cycleNum = 1; sndTab.cycleNum <= maxLoop; sndTab.cycleNum++)
                                    {
                                        if (othTab.cnfResTypeComboBox.SelectedIndex == 0)
                                        {
                                            if (sndTab.MakeFrame(Code.VIO_CONFIRM_RES, out byte[] auto_data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref pItem) == true)
                                            {
                                                ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(auto_data, auto_data.Length);
                                            }
                                        }
                                        else
                                        {
                                            //신규응답으로 보낸다.
                                            if (sndTab.MakeFrame(Code.VIO_CONFIRM_RES_N, out byte[] auto_data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref pItem) == true)
                                            {
                                                ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(auto_data, auto_data.Length);
                                            }
                                        }
                                            
                                    }

                                    if (pItem.sndVioReq == false)
                                    {
                                        //위반확인응답을 보냄.
                                        pItem.sndVioReq = true;
                                    }

                                    ///싱크 번호가 이미지 번호가 아니면 그냥 MCU Sim에서 번호를 증가 한다.
                                    if (sndTab.syncMethod.SelectedIndex != 1)
                                    {
                                        ///위반번호 증가
                                        sndTab.VioNumber = sndTab.VioNumber + 1;
                                        if (sndTab.VioNumber == 0xFFFF)
                                        {
                                            sndTab.VioNumber = 1;
                                        }
                                    }

                                    sndTab.ProcNumber1 = saveProcNum;
                                    sndTab.ProcNumber1 += (uint)maxLoop;
                                    sndTab.ProcNumber2 = sndTab.ProcNumber1 + 1;
                                    sndTab.ProcNumber3 = sndTab.ProcNumber2 + 1;
                                    sndTab.ProcNumber4 = sndTab.ProcNumber3 + 1;

                                }
                            }
                            break;
                        case Code.PLATE_RECOG_NOTIFY:
                            {
                                ProcItem item = null;
                                recvTab.SeqNum = (int)frameBuf.buff[frameHeader.SeqPos]; ///전송연번 업데이트
                                sndTab.MakeFrame(Code.ACK, out byte[] data, comm, ref item);
                                data[frameHeader.SeqPos] = frameBuf.buff[frameHeader.SeqPos];
                                MainWindow.Send(data);
                                //임시저장소 생성
                                byte[] bVioNum = new byte[2];
                                Array.Copy(frameBuf.buff, frameHeader.SeqPos + 1, bVioNum, 0, sizeof(ushort));

                                ushort vioNum = BitConverter.ToUInt16(bVioNum, 0);  //영상번호 

                                //영상확장자동전송 체크 시 전송을 수행함.
                                if (othTab.autoConfirmSendCheck.IsChecked == true && sndTab.cftComboBox.SelectedIndex == 1)
                                {
                                    int procNum = sndTab.procList.Count;
                                    bool findOk = false;
                                    if (sndTab.procList.Count > 0)
                                    {
                                        for (int i = 0; i < sndTab.procList.Count; i++)
                                        {
                                            if (sndTab.procList[i].sndVioReq == true)
                                            {
                                                if (vioNum == sndTab.procList[i].vioNum)
                                                {
                                                    uint j = sndTab.procList[i].curCfmCnt;
                                                    for ( ; j < sndTab.procList[i].procNumTotal; j++)
                                                    {
                                                        ProcItem pItem = (ProcItem)sndTab.procList[i];
                                                        sndTab.MakeFrame(Code.IMAGE_CONFIRM, out byte[] auto_data, ((MainWindow)System.Windows.Application.Current.MainWindow).comm, ref pItem);
                                                        ((MainWindow)System.Windows.Application.Current.MainWindow).SendData(auto_data, auto_data.Length);
                                                        findOk = true;
                                                        break;
                                                    }
                                                    if (sndTab.procList[i].procNumTotal == sndTab.procList[i].curCfmCnt)
                                                    {
                                                        sndTab.procList.RemoveAt(i);  //영상확정을 보내면 삭제한다.
                                                    }
                                                    if(findOk == true)
                                                    {
                                                        break;
                                                    }

                                                }
                                            }
                                        }
                                        if (findOk == false)
                                        {
                                            sb.Append("영상확정 오류 !해당 영상번호가 없음\r\n");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("영상 확정을 보낼 것이 없습니다 (3)");
                                    }
                                }
                                else
                                {
                                    ///차량번호 통보를 받았으면 리스트에 있는 것을 삭제한다.
                                    if(sndTab.cftComboBox.SelectedIndex == 0)
                                    {
                                        if (sndTab.procList.Count > 0)
                                        {
                                            for (int i = 0; i < sndTab.procList.Count; i++)
                                            {
                                                if (sndTab.procList[i].sndVioReq == true)
                                                {
                                                    if (vioNum == sndTab.procList[i].vioNum)
                                                    {

                                                        //처리번호의 갯수와 전송 갯수가 같으면... 삭제
                                                        sndTab.procList.RemoveAt(i);

                                                        break;
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    else if(sndTab.cftComboBox.SelectedIndex == 1)
                                    {

                                    }
                                    else
                                    {
                                        MessageBox.Show("영상 확정을 보낼 것이 없습니다 (4)");
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    for (int i = 0; i < frameBuf.buffLen; i++)
                    {
                        sb.Append(string.Format("[" + "{0:x2}" + "]", frameBuf.buff[i]));
                    }

                    recvTabUsrCtrl.CommRxList.Items.Add(sb.ToString());
                    if (recvTabUsrCtrl.CommRxList.Items.Count > 50)
                    {
                        recvTabUsrCtrl.CommRxList.Items.Clear();
                    }
                    recvTabUsrCtrl.CommRxList.ScrollIntoView(recvTabUsrCtrl.CommRxList.SelectedItem);

                    /*if (recvBuff.buffLen > frameBuf.buffLen)
                    {
                        Array.ConstrainedCopy(recvBuff.buff, frameBuf.buffLen, recvBuff.buff, 0, (recvBuff.buffLen - frameBuf.buffLen));
                    }*/
                    
                    frameBuf.reset();
                    Array.Clear(frameBuf.buff, 0,frameBuf.buff.Length);
                    if(recvBuff.buffLen < 0)
                    {
                        MessageBox.Show("수신 메시지 이상\r\n");
                        recvBuff.buffLen = 0;
                    }

                }//while
                
            };
            this.Dispatcher.Invoke(srdel);
           // recvBuff.reset();
        }
        /// <summary>
        /// DLE를 삭제한다.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int DelDLE(ref byte[] buf, int size)
        {
            byte[] tmp = new byte[Protocols.MAX_BUF_SIZE];
            int i;
            int index = 0;
            Debug.Assert(size <= Protocols.MAX_BUF_SIZE);
            Array.Copy(buf, tmp, size);
            for (i = 0; i < size; i++)
            {
                buf[index++] = tmp[i];
                if (tmp[i] == Protocols.DLE && tmp[i + 1] == Protocols.DLE)
                {
                    i++;
                }
            }
            return index;
        }
        /// <summary>
        /// DLE를 추가하는 함수이다.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int AddDLE(ref byte[] buf, int size)
        {
            byte[] tmp = new byte[Protocols.MAX_BUF_SIZE];
            int i;
            int index = 0;
            Debug.Assert(size <= Protocols.MAX_BUF_SIZE);
            Array.Copy(buf, tmp, size);
            for (i = 0; i < size; i++)
            {
                buf[index++] = tmp[i];
                if (tmp[i] == Protocols.DLE)
                {
                    buf[index++] = Protocols.DLE;
                }
            }
            return index;
        }
        /// <summary>
        /// BCC를 size까지 계산함.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte CalBCC(byte[] buf, int size)
        {
            byte bcc = 0x00;
            byte nextValue;

            for (int i = 0; i < size; i++)
            {
                nextValue = buf[i];
                bcc = (byte)(bcc ^ nextValue);
            }
            /*
            if (bcc <= 0x20)
            {
                bcc += 20;
            }
            */
            return bcc;
        }


        private void ComConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            SerialCfgDlg dlg = new SerialCfgDlg(ports, port, speeds, speed);

            dlg.ShowDialog();

            if (dlg.DialogResult.Value)
            {
                port = dlg.Selport;
                speed = dlg.Selspeed;
                recvTabUsrCtrl.serialPort.Text = port;
            }
            else
            {
            }
        }
        private void InitSerialPort(object sender, RoutedEventArgs e)
        {
            bool findPort = false;

            ports = SerialPort.GetPortNames();

            foreach (string pt in ports)
            {
                if (string.Compare(port, pt) == 0)
                {
                    findPort = true;
                    break;
                }
            }

            if (findPort == false)
            {
                if (ports.Length == 0)
                {
                    //포트를 찾지 못하면...
                    string msg = "COM 포트 이상! COM 포트가 없습니다. USB 시리얼을 삽입하세요.";
                    string cap = "에러";
                    System.Windows.MessageBox.Show(msg, cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    //포트를 찾지 못하면...
                    string msg = "COM 포트 이상! COM 포트를 설정 하십시오";
                    string cap = "에러";
                    System.Windows.MessageBox.Show(msg, cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    SerialCfgDlg dlg = new SerialCfgDlg(ports, port, speeds, speed);

                    dlg.ShowDialog();

                    if (dlg.DialogResult.Value)
                    {
                        port = dlg.Selport;
                        speed = dlg.Selspeed;
                        recvTabUsrCtrl.serialPort.Text = port;
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                recvTabUsrCtrl.serialPort.Text = port;
            }

        }

        private void SeiralTxClear_Click(object sender, RoutedEventArgs e)
        {
            sndTabUsrCtrl.CommTxList.Items.Clear();
        }


        private void SeiralRxClear_Click(object sender, RoutedEventArgs e)
        {
            sndTabUsrCtrl.CommTxList.Items.Clear();
        }
    }
}
