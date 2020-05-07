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
using System.Windows.Shapes;

namespace HMCU_Sim
{
    /// <summary>
    /// SerialCfgDlg.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SerialCfgDlg : Window
    {
        private string selport;
        public string Selport
        {
            get
            {
                return selport;
            }
        }

        private string selspeed;
        public string Selspeed
        {
            get
            {
                return selspeed;
            }
        }
        
        public SerialCfgDlg()
        {
            InitializeComponent();
        }

        public SerialCfgDlg(string[] ports, string curPort, string [] speeds, string curSpeed)
        {
            InitializeComponent();

            bool findComPort = false;
            bool findSpeed = false;

            foreach (string port in ports)
            {
                comports.Items.Add(port);
            }

            foreach (string port in ports)
            {
                if(string.Compare(port,curPort)==0)
                {
                    findComPort = true;
                    break;
                }
            }

            if(findComPort == true)
            {
                comports.SelectedItem = curPort;
            }
            else
            {
                comports.SelectedIndex = 0;
            }

            foreach (string speed in speeds)
            {
                comspeeds.Items.Add(speed);
            }

            foreach (string speed in speeds)
            {
                if (string.Compare(speed, curSpeed) == 0)
                {
                    findSpeed = true;
                    break;
                }
            }

            if (findSpeed == true)
            {
                comspeeds.SelectedItem = curSpeed;
            }
            else
            {
                comspeeds.SelectedIndex = 0;
            }


        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            selport = (string)comports.SelectedItem;
            selspeed = (string)comspeeds.SelectedItem;

            this.DialogResult = true;
        }
    }
}
