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
    /// RecvUserControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RecvUserControl : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

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

        public RecvUserControl()
        {
            InitializeComponent();
            /// MainWindow과  데이터 동기화를 하기 위해서는 아래 문장을 실행 시켜 준다.
            DataContext = this;
            SeqNum = 1;
        }

        private void SocketRxClear_Click(object sender, RoutedEventArgs e)
        {
            CommRxList.Items.Clear();
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
