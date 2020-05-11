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
            m_Port  = new SerialPort();
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
            if(m_recvHandler != null)
            {
                m_Port.DataReceived -= m_recvHandler;
                m_recvHandler = null;
            }
        }

        public override bool IsRun()
        {
            bool status = false;

            if(m_Port.IsOpen)
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
            if(comm == CommMethod.Serial)
            {
                if (commHandler.IsRun())
                {
                    SvrBtnText.Text = "서버 시작";
                    isRuning = false;

                    SerialHandler sh = (SerialHandler)commHandler;
                    commHandler.Close();
                    recvTabUsrCtrl.CommRxList.Items.Add("종료 되었습니다.");

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
            byte[] array = new byte[1024];
            string str = string.Empty;
            //int len = commHandler.Read(array, 0, array.Length);
            int len = commHandler.Read(recvBuff.buff, recvBuff.buffLen, recvBuff.buff.Length);
           // Array.Copy(recvBuff.buff, recvBuff.buffLen, array, 0, len);
            recvBuff.buffLen += len;

            if(recvBuff.buffLen <= frameHeader.ExtraLen)
            {
                return;
            }

            if (recvBuff.buff[recvBuff.buffLen - 2] != Protocols.ETX || recvBuff.buff[recvBuff.buffLen - 3] != Protocols.DLE)
            {
                return;
            }

            if (recvBuff.buffLen >= 1024)
            {
                recvBuff.reset();
            }

            for (int i = 0; i < recvBuff.buffLen; i++)
            {
                str += string.Format("[" + "{0:x2}" + "]", recvBuff.buff[i]);
            }

            byte revBcc = recvBuff.buff[recvBuff.buffLen - 1];  //BCC 저장


            Array.Copy(recvBuff.buff,2, array, 0, recvBuff.buffLen - 5); // DLE STX ~ DLE ETX BCC 를 뺌.
            int validSize = DelDLE(ref array, recvBuff.buffLen - 5);
            Array.Copy(array, 0, recvBuff.buff, frameHeader.LenPos, validSize);

            byte [] bccData = new byte[validSize - 1]; //LEN이 빠진 데이터 길이.

            Array.Copy(array, 1, bccData, 0, bccData.Length);

            byte calBcc = MainWindow.CalBCC(bccData, bccData.Length);

            if(revBcc != calBcc)
            {
                recvBuff.reset();  //BCC 오류
            }
            else
            {
                recvBuff.buffLen = 5 + validSize;
                recvBuff.buff[recvBuff.buffLen - 3] = Protocols.DLE;
                recvBuff.buff[recvBuff.buffLen - 2] = Protocols.ETX;
                recvBuff.buff[recvBuff.buffLen - 1] = revBcc;
            }

            SerialRecvDelegate srdel = delegate ()
            {
                switch(recvBuff.buff[frameHeader.CodePos])
                {
                    case Code.ACK:
                        recvTab.SeqNum = (int)recvBuff.buff[frameHeader.SeqPos];  ///전송연번 업데이트
                        break;
                    case Code.NACK:
                        recvTab.SeqNum = (int)recvBuff.buff[frameHeader.SeqPos];  ///전송연번 업데이트
                        //추후 재전송 로직 추가
                        break;
                    case Code.STATUS_RES:  ///상태정보 수신
                    {
                        recvTab.SeqNum = (int)recvBuff.buff[frameHeader.SeqPos];  ///전송연번 업데이트
                            //ACK를 보내줌.
                            sndTab.MakeFrame(Code.ACK, out byte[] data, comm);
                        //sndTab.MakeSerialFrame(Code.ACK, out byte[] data);
                            commHandler.Send(data,data.Length);
                    }
                        break;
                    case Code.VIO_CONFIRM_REQ:   ///위반확인요구 수신
                    {
                        recvTab.SeqNum = (int)recvBuff.buff[frameHeader.SeqPos];  ///전송연번 업데이트
                        //ACK를 보내줌.
                        int nCopy = Marshal.SizeOf(typeof(PACKET_VIO_REQUEST));
                         byte[] _cpyArray = new byte[nCopy];
                        Array.Copy(recvBuff.buff, 5, _cpyArray, 0, nCopy);

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

                        ProcItem pItem = new ProcItem();
                        pItem.seq = recvBuff.buff[frameHeader.SeqPos];
                        pItem.vioNum = pVioReq.imagNum;
                        sndTab.procList.Add(pItem);
                        /// 영상번호 업데이트
                        recvTab.imageNum.Text = pVioReq.imagNum.ToString();
                        if (sndTab.syncMethod.SelectedIndex == 1)
                        {
                            sndTab.VioNumber = pVioReq.imagNum;
                        }
                    }
                    break;
                    case Code.PLATE_RECOG_NOTIFY:
                    {
                        recvTab.SeqNum = (int)recvBuff.buff[frameHeader.SeqPos]; ///전송연번 업데이트
                    }
                    break;
                    default:
                    break;
                 }

                recvTabUsrCtrl.CommRxList.Items.Add(str);
                if (recvTabUsrCtrl.CommRxList.Items.Count > 100)
                {
                    recvTabUsrCtrl.CommRxList.Items.Clear();
                }
                recvTabUsrCtrl.CommRxList.ScrollIntoView(recvTabUsrCtrl.CommRxList.SelectedItem);
            };
            this.Dispatcher.Invoke(srdel);

            recvBuff.reset();
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

            if (bcc <= 0x20)
            {
                bcc += 20;
            }

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
