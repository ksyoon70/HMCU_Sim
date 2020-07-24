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

namespace HMCU_Sim
{
    /// <summary>
    /// OtherUserControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OtherUserControl : UserControl
    {
        private string[] confLocation = new string[] { "전면", "후면" };
        private string[] confResType = new string[] { "위반확인응답", "위반확인응답신규" };

        public OtherUserControl()
        {
            InitializeComponent();

            // 확정위치
            foreach (string loc in confLocation)
            {
                cnfComboBox.Items.Add(loc);
            }
            //위반확인응답 종류
            foreach(string type in confResType)
            {
                cnfResTypeComboBox.Items.Add(type);
            }
        }
    }
}
