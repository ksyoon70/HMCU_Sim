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
using static HMCU_Sim.SendUserControl;
using static HMCU_Sim.RecvUserControl;

namespace HMCU_Sim
{
    /// <summary>
    /// RecvUserControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RecvUserControl : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public class ProtoType
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

        private ObservableCollection<ProtoType> _protoTypes;

        public ObservableCollection<ProtoType> ProtoTypes  //collection을 반환
        {
            get { return _protoTypes; }
            set { _protoTypes = value; }
        }

        private ProtoType _protoType;

        public ProtoType Prototype  //개별 아이템을 반환
        {
            get { return _protoType; }
            set
            {
                _protoType = value;
                OnPropertyChanged("Prototype");
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

        public RecvUserControl()
        {
            InitializeComponent();
            /// MainWindow과  데이터 동기화를 하기 위해서는 아래 문장을 실행 시켜 준다.
            DataContext = this;
            SeqNum = 1;

            ProtoTypes = new ObservableCollection<ProtoType>()
            {
                new ProtoType(){ Id = 0, Name ="구형" },new ProtoType(){ Id = 1, Name ="신형" },new ProtoType(){ Id = 2, Name ="신형22" }
            };
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
