﻿using System;
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

                //처리 갯수
                GetPrivateProfileString("SYSTEM", "PROCESS_COUNT", "0", temp, 255, PROGRAM_INI_FULLPATH);
                count = Convert.ToInt32(temp.ToString());
                m_form.sndTabUsrCtrl.pcComboBox.SelectedIndex = index;


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


                //처리2
                GetPrivateProfileString("SYSTEM", "PROCESS_NUM2", "0", temp, 255, PROGRAM_INI_FULLPATH);
                m_form.sndTabUsrCtrl.ProcNumber2 = uint.Parse(temp.ToString());
                //위반형태
                GetPrivateProfileString("SYSTEM", "VIO_TYPE2", "0", temp, 255, PROGRAM_INI_FULLPATH);
                count = Convert.ToInt32(temp.ToString());
                m_form.sndTabUsrCtrl.vioType2.SelectedIndex = count;
                //처리
                GetPrivateProfileString("SYSTEM", "VIO_CODE2", "0", temp, 255, PROGRAM_INI_FULLPATH);
                m_form.sndTabUsrCtrl.VioCode2 = temp.ToString();

                //처리
                GetPrivateProfileString("SYSTEM", "PROCESS_NUM3", "0", temp, 255, PROGRAM_INI_FULLPATH);
                m_form.sndTabUsrCtrl.ProcNumber3 = uint.Parse(temp.ToString());

                //처리
                GetPrivateProfileString("SYSTEM", "PROCESS_NUM4", "0", temp, 255, PROGRAM_INI_FULLPATH);
                m_form.sndTabUsrCtrl.ProcNumber4 = uint.Parse(temp.ToString());

                //위반형태
                GetPrivateProfileString("SYSTEM", "VIO_TYPE3", "0", temp, 255, PROGRAM_INI_FULLPATH);
                count = Convert.ToInt32(temp.ToString());
                m_form.sndTabUsrCtrl.vioType3.SelectedIndex = count;
                //처리
                GetPrivateProfileString("SYSTEM", "VIO_CODE3", "0", temp, 255, PROGRAM_INI_FULLPATH);
                m_form.sndTabUsrCtrl.VioCode3 = temp.ToString();

                //위반형태
                GetPrivateProfileString("SYSTEM", "VIO_TYPE4", "0", temp, 255, PROGRAM_INI_FULLPATH);
                count = Convert.ToInt32(temp.ToString());
                m_form.sndTabUsrCtrl.vioType4.SelectedIndex = count;
                //처리
                GetPrivateProfileString("SYSTEM", "VIO_CODE4", "0", temp, 255, PROGRAM_INI_FULLPATH);
                m_form.sndTabUsrCtrl.VioCode4 = temp.ToString();

                //확정 위치
                GetPrivateProfileString("SYSTEM", "CONF_LOC", "0", temp, 255, PROGRAM_INI_FULLPATH);
                count = Convert.ToInt32(temp.ToString());
                m_form.sndTabUsrCtrl.cnfComboBox.SelectedIndex = count;

                //영상확정시점
                GetPrivateProfileString("SYSTEM", "CONF_TIME", "0", temp, 255, PROGRAM_INI_FULLPATH);
                count = Convert.ToInt32(temp.ToString());
                m_form.sndTabUsrCtrl.cftComboBox.SelectedIndex = count;

                // 싱크 방식
                GetPrivateProfileString("SYSTEM", "SYNC_METHOD", "0", temp, 255, PROGRAM_INI_FULLPATH);
                count = Convert.ToInt32(temp.ToString());
                m_form.sndTabUsrCtrl.syncMethod.SelectedIndex = count;

                // 시리얼 포트 읽기
                GetPrivateProfileString("SERIAL", "PORT", "COM1", temp, 255, PROGRAM_INI_FULLPATH);
                m_form.Port = temp.ToString();
                /// 시리얼 BPS 읽기
                GetPrivateProfileString("SERIAL", "SPEED", "9600", temp, 255, PROGRAM_INI_FULLPATH);
                m_form.Speed = temp.ToString();

                /// 통신방식 설정
                GetPrivateProfileString("SYSTEM", "COMMETHOD", CommMethod.Serial.ToString(), temp, 255, PROGRAM_INI_FULLPATH);
                m_form.comm = (CommMethod)Enum.Parse(typeof(CommMethod), temp.ToString());
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
                
                //처리 갯수
                WritePrivateProfileString("SYSTEM", "PROCESS_COUNT", m_form.sndTabUsrCtrl.pcComboBox.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);

                //처리1 위반형태
                WritePrivateProfileString("SYSTEM", "VIO_TYPE1", m_form.sndTabUsrCtrl.vioType1.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);

                //처리1 위반코드
                WritePrivateProfileString("SYSTEM", "VIO_CODE1", m_form.sndTabUsrCtrl.VioCode1, PROGRAM_INI_FULLPATH);
                //처리2 번호
                WritePrivateProfileString("SYSTEM", "PROCESS_NUM2", m_form.sndTabUsrCtrl.ProcNumber2.ToString(), PROGRAM_INI_FULLPATH);
                //처리3 번호
                WritePrivateProfileString("SYSTEM", "PROCESS_NUM3", m_form.sndTabUsrCtrl.ProcNumber3.ToString(), PROGRAM_INI_FULLPATH);
                //처리4 번호
                WritePrivateProfileString("SYSTEM", "PROCESS_NUM4", m_form.sndTabUsrCtrl.ProcNumber4.ToString(), PROGRAM_INI_FULLPATH);

                // 확정 위치
                WritePrivateProfileString("SYSTEM", "CONF_LOC", m_form.sndTabUsrCtrl.cnfComboBox.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);

                //영상확정시점
                WritePrivateProfileString("SYSTEM", "CONF_TIME", m_form.sndTabUsrCtrl.cftComboBox.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);

                // 싱크 방식
                WritePrivateProfileString("SYSTEM", "SYNC_METHOD", m_form.sndTabUsrCtrl.syncMethod.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);

                // 시리얼 포트 저장
                WritePrivateProfileString("SERIAL", "PORT", m_form.Port, PROGRAM_INI_FULLPATH);
                // 시리얼 속도 저장
                WritePrivateProfileString("SERIAL", "SPEED", m_form.Speed, PROGRAM_INI_FULLPATH);
                /// 통신방식 저장
                WritePrivateProfileString("SYSTEM", "COMMETHOD", m_form.comm.ToString(), PROGRAM_INI_FULLPATH);
            }

            /// <summary>
            /// INI 파일이름 설정
            /// </summary>
            public void setFileName(string fileName) => PROGRAM_INI_FULLPATH = fileName;
        }
 }
