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

namespace HMCU_Sim
{
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
                if (m_Port.IsOpen)
                {
                    SvrBtnText.Text = "서버 시작";
                    isRuning = false;
                    m_Port.Close();
                    m_Port.DataReceived -= new SerialDataReceivedEventHandler(Serial_DataReceived);
                    recvTabUsrCtrl.CommRxList.Items.Add("종료 되었습니다.");

                }
                else
                {
                    //ConnectionBtn.Content = "시리얼 종료";
                    SvrBtnText.Text = "시리얼 종료";
                    isRuning = true;

                    try
                    {
                        m_Port.PortName = port;
                        m_Port.BaudRate = int.Parse(speed.Trim());
                        m_Port.DataBits = 8;
                        m_Port.StopBits = StopBits.One;
                        m_Port.Parity = Parity.None;
                        m_Port.Open();
                        m_Port.DataReceived += new SerialDataReceivedEventHandler(Serial_DataReceived);

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
            int len = m_Port.Read(array, 0, array.Length);

            for (int i = 0; i < len; i++)
            {
                str += string.Format("[" + "{0:x2}" + "]", array[i]);
            }

            SerialRecvDelegate srdel = delegate ()
            {
                recvTabUsrCtrl.CommRxList.Items.Add(str);
                if (recvTabUsrCtrl.CommRxList.Items.Count > 100)
                {
                    recvTabUsrCtrl.CommRxList.Items.Clear();
                }
                recvTabUsrCtrl.CommRxList.ScrollIntoView(recvTabUsrCtrl.CommRxList.SelectedItem);
            };
            this.Dispatcher.Invoke(srdel);
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
