using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;   //class 추가

namespace HMCU_Sim
{

        public class ConfigManager
         {
            static string PROGRAM_INI_FULLPATH;

            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
                                                            int size, string filePath);
            #region Members
            /// <summary>
            /// Saves the desired connection mode
            /// </summary>
            MainWindow m_form;
            #endregion

            public ConfigManager(MainWindow parent)
            {
                m_form = parent;
            }

            /// <summary>
            /// INI 파일의 설정  값 읽어오기
            /// </summary>
            public void Load()
            {
                 int count;
                StringBuilder temp = new StringBuilder(255);

                GetPrivateProfileString("ETHERNET", "IP", "127.0.0.1", temp, 255, PROGRAM_INI_FULLPATH);
                /// 이더넷 IP 설정               
                m_form.SvrIP = temp.ToString();

                GetPrivateProfileString("ETHERNET", "PORT", "20355", temp, 255, PROGRAM_INI_FULLPATH);
                /// 이더넷 포트 설정                 
                m_form.Svrport = temp.ToString();

            

                GetPrivateProfileString("SYSTEM", "OPFFICE_NUM", "1234", temp, 255, PROGRAM_INI_FULLPATH);
                /// 영업소번호
                m_form.sndTabUsrCtrl.OfficeNumber = temp.ToString();

                GetPrivateProfileString("SYSTEM", "LANE_NUM", "1", temp, 255, PROGRAM_INI_FULLPATH);
                /// 차선번호
                m_form.sndTabUsrCtrl.LaneNumber = temp.ToString();

                GetPrivateProfileString("SYSTEM", "WORK_NUM", "1", temp, 255, PROGRAM_INI_FULLPATH);
                /// 근무번호
                m_form.sndTabUsrCtrl.WorkNumber = temp.ToString();

                GetPrivateProfileString("SYSTEM", "PLATE_NUM", "", temp, 255, PROGRAM_INI_FULLPATH);
                /// 자동차번호
                m_form.sndTabUsrCtrl.plateNum.Text = temp.ToString();

                GetPrivateProfileString("SYSTEM", "WORK_TYPE", "0", temp, 255, PROGRAM_INI_FULLPATH);
                /// 근무타입
                int index = Convert.ToInt32(temp.ToString());
                m_form.sndTabUsrCtrl.wkComboBox.SelectedIndex = index;


                //처리1
                GetPrivateProfileString("SYSTEM", "PROCESS_NUM1", "1", temp, 255, PROGRAM_INI_FULLPATH);
                m_form.sndTabUsrCtrl.ProcNumber1 = uint.Parse(temp.ToString());
                //위반형태
                GetPrivateProfileString("SYSTEM", "VIO_TYPE1","0", temp, 255, PROGRAM_INI_FULLPATH);
                count = Convert.ToInt32(temp.ToString());
                m_form.sndTabUsrCtrl.vioType1.SelectedIndex = count;
                //처리
                GetPrivateProfileString("SYSTEM", "VIO_CODE1","0", temp, 255, PROGRAM_INI_FULLPATH);
                m_form.sndTabUsrCtrl.VioCode1= temp.ToString();

             }

            /// <summary>
            /// INI 파일의 설정  값 쓰기
            /// </summary>
            public void Save()
            {
            //mode 저장
                WritePrivateProfileString("ETHERNET", "IP", m_form.SvrIP, PROGRAM_INI_FULLPATH);
                //IP저장
                WritePrivateProfileString("ETHERNET", "PORT", m_form.Svrport, PROGRAM_INI_FULLPATH);
                //포트저장
                WritePrivateProfileString("SYSTEM", "OPFFICE_NUM", m_form.sndTabUsrCtrl.OfficeNumber, PROGRAM_INI_FULLPATH);
                //차선번호
                WritePrivateProfileString("SYSTEM", "LANE_NUM", m_form.sndTabUsrCtrl.LaneNumber, PROGRAM_INI_FULLPATH);
                //근무번호
                WritePrivateProfileString("SYSTEM", "WORK_NUM", m_form.sndTabUsrCtrl.WorkNumber, PROGRAM_INI_FULLPATH);

                //자동차번호
                WritePrivateProfileString("SYSTEM", "PLATE_NUM", m_form.sndTabUsrCtrl.plateNum.Text, PROGRAM_INI_FULLPATH);
                    //근무타입
                WritePrivateProfileString("SYSTEM", "WORK_TYPE", m_form.sndTabUsrCtrl.wkComboBox.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);

                //처리1 번호
                WritePrivateProfileString("SYSTEM", "PROCESS_NUM1", m_form.sndTabUsrCtrl.ProcNumber1.ToString(), PROGRAM_INI_FULLPATH);


                //처리1 위반형태
                WritePrivateProfileString("SYSTEM", "VIO_TYPE1", m_form.sndTabUsrCtrl.vioType1.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);

                //처리1 위반코드
                WritePrivateProfileString("SYSTEM", "VIO_CODE1", m_form.sndTabUsrCtrl.VioCode1, PROGRAM_INI_FULLPATH);

            }

            /// <summary>
            /// INI 파일이름 설정
            /// </summary>
            public void setFileName(string fileName) => PROGRAM_INI_FULLPATH = fileName;
        }
 }
