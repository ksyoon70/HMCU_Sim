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

        public OtherUserControl()
        {
            InitializeComponent();

            // 확정위치
            foreach (string loc in confLocation)
            {
                cnfComboBox.Items.Add(loc);
            }
        }
    }
}
