﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;   //class 추가
using System.Reflection;

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
            int index;
            StringBuilder temp = new StringBuilder(255);

            GetPrivateProfileString("ETHERNET", "IP", "127.0.0.1", temp, 255, PROGRAM_INI_FULLPATH);
            /// 이더넷 IP 설정               
            m_form.SvrIP = temp.ToString();

            GetPrivateProfileString("ETHERNET", "PORT", "20355", temp, 255, PROGRAM_INI_FULLPATH);
            /// 이더넷 포트 설정                 
            m_form.Svrport = temp.ToString();

            GetPrivateProfileString("SYSTEM", "PROTOCOL", "1", temp, 255, PROGRAM_INI_FULLPATH);
            /// 프로토콜 설정
            index = Convert.ToInt32(temp.ToString());
            m_form.recvTabUsrCtrl.protoComboBox.SelectedIndex = index;

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
            index = Convert.ToInt32(temp.ToString());
            m_form.sndTabUsrCtrl.wkComboBox.SelectedIndex = index;

            //처리 갯수
            GetPrivateProfileString("SYSTEM", "PROCESS_COUNT", "0", temp, 255, PROGRAM_INI_FULLPATH);
            count = Convert.ToInt32(temp.ToString());
            m_form.sndTabUsrCtrl.pcComboBox.SelectedIndex = count;


            //처리1
            GetPrivateProfileString("SYSTEM", "PROCESS_NUM1", "1", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.ProcNumber1 = uint.Parse(temp.ToString());
            //위반형태1
            GetPrivateProfileString("SYSTEM", "VIO_TYPE1","0", temp, 255, PROGRAM_INI_FULLPATH);
            count = Convert.ToInt32(temp.ToString());
            m_form.sndTabUsrCtrl.vioType1.SelectedIndex = count;
            //코드1
            GetPrivateProfileString("SYSTEM", "VIO_CODE1","0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.VioCode1= temp.ToString();
            //OBU1 번호
            GetPrivateProfileString("SYSTEM", "OBU_NUM1", "12345671", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.ObuNum1 = temp.ToString();
            //OBU1 종류
            GetPrivateProfileString("SYSTEM", "OBU_TYPE1", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.obuType1.SelectedIndex = Convert.ToInt32(temp.ToString());
            //OBU1 차종
            GetPrivateProfileString("SYSTEM", "OBU_CAR_CLASS1", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.obuCarClass1.SelectedIndex = Convert.ToInt32(temp.ToString());

            //처리2
            GetPrivateProfileString("SYSTEM", "PROCESS_NUM2", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.ProcNumber2 = uint.Parse(temp.ToString());
            //위반형태2
            GetPrivateProfileString("SYSTEM", "VIO_TYPE2", "0", temp, 255, PROGRAM_INI_FULLPATH);
            count = Convert.ToInt32(temp.ToString());
            m_form.sndTabUsrCtrl.vioType2.SelectedIndex = count;
            //코드2
            GetPrivateProfileString("SYSTEM", "VIO_CODE2", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.VioCode2 = temp.ToString();
            //OBU2 번호
            GetPrivateProfileString("SYSTEM", "OBU_NUM2", "12345672", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.ObuNum2 = temp.ToString();
            //OBU2 종류
            GetPrivateProfileString("SYSTEM", "OBU_TYPE2", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.obuType2.SelectedIndex = Convert.ToInt32(temp.ToString());
            //OBU2 차종
            GetPrivateProfileString("SYSTEM", "OBU_CAR_CLASS2", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.obuCarClass2.SelectedIndex = Convert.ToInt32(temp.ToString());


            //처리3
            GetPrivateProfileString("SYSTEM", "PROCESS_NUM3", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.ProcNumber3 = uint.Parse(temp.ToString());
            //위반형태3
            GetPrivateProfileString("SYSTEM", "VIO_TYPE3", "0", temp, 255, PROGRAM_INI_FULLPATH);
            count = Convert.ToInt32(temp.ToString());
            m_form.sndTabUsrCtrl.vioType3.SelectedIndex = count;
            //코드3
            GetPrivateProfileString("SYSTEM", "VIO_CODE3", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.VioCode3 = temp.ToString();
            //OBU3 번호
            GetPrivateProfileString("SYSTEM", "OBU_NUM3", "12345673", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.ObuNum3 = temp.ToString();
            //OBU3 종류
            GetPrivateProfileString("SYSTEM", "OBU_TYPE3", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.obuType3.SelectedIndex = Convert.ToInt32(temp.ToString());
            //OBU3 차종
            GetPrivateProfileString("SYSTEM", "OBU_CAR_CLASS3", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.obuCarClass3.SelectedIndex = Convert.ToInt32(temp.ToString());


            //처리4
            GetPrivateProfileString("SYSTEM", "PROCESS_NUM4", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.ProcNumber4 = uint.Parse(temp.ToString());
            //위반형태4
            GetPrivateProfileString("SYSTEM", "VIO_TYPE4", "0", temp, 255, PROGRAM_INI_FULLPATH);
            count = Convert.ToInt32(temp.ToString());
            m_form.sndTabUsrCtrl.vioType4.SelectedIndex = count;
            //코드4
            GetPrivateProfileString("SYSTEM", "VIO_CODE4", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.VioCode4 = temp.ToString();
            //OBU4 번호
            GetPrivateProfileString("SYSTEM", "OBU_NUM4", "12345674", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.ObuNum4 = temp.ToString();
            //OBU4 종류
            GetPrivateProfileString("SYSTEM", "OBU_TYPE4", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.obuType4.SelectedIndex = Convert.ToInt32(temp.ToString());
            //OBU4 차종
            GetPrivateProfileString("SYSTEM", "OBU_CAR_CLASS4", "0", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.obuCarClass4.SelectedIndex = Convert.ToInt32(temp.ToString());


            //확정 위치
            GetPrivateProfileString("SYSTEM", "CONF_LOC", "0", temp, 255, PROGRAM_INI_FULLPATH);
            count = Convert.ToInt32(temp.ToString());
            m_form.othTabUsrCtrl.cnfComboBox.SelectedIndex = count;

            //위반확인응답 종류
            GetPrivateProfileString("SYSTEM", "CONF_RES_TYPE", "0", temp, 255, PROGRAM_INI_FULLPATH);
            count = Convert.ToInt32(temp.ToString());
            m_form.othTabUsrCtrl.cnfResTypeComboBox.SelectedIndex = count;

            //영상확정시점
            GetPrivateProfileString("SYSTEM", "CONF_TIME", "0", temp, 255, PROGRAM_INI_FULLPATH);
            count = Convert.ToInt32(temp.ToString());
            m_form.sndTabUsrCtrl.cftComboBox.SelectedIndex = count;

            //바이트 오더
            GetPrivateProfileString("SYSTEM", "BYTE_ORDER", "0", temp, 255, PROGRAM_INI_FULLPATH);
            count = Convert.ToInt32(temp.ToString());
            m_form.sndTabUsrCtrl.ByteOrder.SelectedIndex = count;

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

            // 위반확인자동응답 체크 저장
            GetPrivateProfileString("ETC", "AUTO_SEND_VIORESPONSE", "false" ,temp, 255, PROGRAM_INI_FULLPATH);
            m_form.othTabUsrCtrl.autoVioSendCheck.IsChecked = (temp.ToString() == "1") ? true : false;

            // 영상확정자동전송 체크 저장
            GetPrivateProfileString("ETC", "AUTO_SEND_CONFIRM", "false", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.othTabUsrCtrl.autoConfirmSendCheck.IsChecked = (temp.ToString() == "1") ? true : false;

            // 자동응답 체크
            GetPrivateProfileString("SYSTEM", "AUTO_RESPONSE", "false", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.recvTabUsrCtrl.autoSendCheck.IsChecked = (temp.ToString() == "1") ? true : false;

            //자동상태요구
            GetPrivateProfileString("SYSTEM", "AUTO_STATUS_RESQ", "false", temp, 255, PROGRAM_INI_FULLPATH);
            m_form.sndTabUsrCtrl.autoSendStatusCheck.IsChecked = (temp.ToString() == "1") ? true : false;
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

            /// 프로토콜 저장
            WritePrivateProfileString("SYSTEM", "PROTOCOL", m_form.recvTabUsrCtrl.protoComboBox.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
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
            //처리 갯수
            WritePrivateProfileString("SYSTEM", "PROCESS_COUNT", m_form.sndTabUsrCtrl.pcComboBox.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //처리1 번호
            WritePrivateProfileString("SYSTEM", "PROCESS_NUM1", m_form.sndTabUsrCtrl.ProcNumber1.ToString(), PROGRAM_INI_FULLPATH);
            //처리1 위반형태
            WritePrivateProfileString("SYSTEM", "VIO_TYPE1", m_form.sndTabUsrCtrl.vioType1.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //처리1 위반코드
            WritePrivateProfileString("SYSTEM", "VIO_CODE1", m_form.sndTabUsrCtrl.VioCode1, PROGRAM_INI_FULLPATH);
            //OBU1 번호
            WritePrivateProfileString("SYSTEM", "OBU_NUM1", m_form.sndTabUsrCtrl.ObuNum1, PROGRAM_INI_FULLPATH);
            //OBU1 종류
            WritePrivateProfileString("SYSTEM", "OBU_TYPE1", m_form.sndTabUsrCtrl.obuType1.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //OBU1 차종
            WritePrivateProfileString("SYSTEM", "OBU_CAR_CLASS1", m_form.sndTabUsrCtrl.obuCarClass1.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //처리2 번호
            WritePrivateProfileString("SYSTEM", "PROCESS_NUM2", m_form.sndTabUsrCtrl.ProcNumber2.ToString(), PROGRAM_INI_FULLPATH);
            //처리2 위반형태
            WritePrivateProfileString("SYSTEM", "VIO_TYPE2", m_form.sndTabUsrCtrl.vioType2.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //처리2 위반코드
            WritePrivateProfileString("SYSTEM", "VIO_CODE2", m_form.sndTabUsrCtrl.VioCode2, PROGRAM_INI_FULLPATH);
            //OBU2 번호
            WritePrivateProfileString("SYSTEM", "OBU_NUM2", m_form.sndTabUsrCtrl.ObuNum2, PROGRAM_INI_FULLPATH);
            //OBU2 종류
            WritePrivateProfileString("SYSTEM", "OBU_TYPE2", m_form.sndTabUsrCtrl.obuType2.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //OBU2 차종
            WritePrivateProfileString("SYSTEM", "OBU_CAR_CLASS2", m_form.sndTabUsrCtrl.obuCarClass2.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //처리3 번호
            WritePrivateProfileString("SYSTEM", "PROCESS_NUM3", m_form.sndTabUsrCtrl.ProcNumber3.ToString(), PROGRAM_INI_FULLPATH);
            //처리3 위반형태
            WritePrivateProfileString("SYSTEM", "VIO_TYPE3", m_form.sndTabUsrCtrl.vioType3.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //처리3 위반코드
            WritePrivateProfileString("SYSTEM", "VIO_CODE3", m_form.sndTabUsrCtrl.VioCode3, PROGRAM_INI_FULLPATH);
            //OBU3 번호
            WritePrivateProfileString("SYSTEM", "OBU_NUM3", m_form.sndTabUsrCtrl.ObuNum3, PROGRAM_INI_FULLPATH);
            //OBU3 종류
            WritePrivateProfileString("SYSTEM", "OBU_TYPE3", m_form.sndTabUsrCtrl.obuType3.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //OBU3 차종
            WritePrivateProfileString("SYSTEM", "OBU_CAR_CLASS3", m_form.sndTabUsrCtrl.obuCarClass3.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //처리4 번호
            WritePrivateProfileString("SYSTEM", "PROCESS_NUM4", m_form.sndTabUsrCtrl.ProcNumber4.ToString(), PROGRAM_INI_FULLPATH);
            //처리4 위반형태
            WritePrivateProfileString("SYSTEM", "VIO_TYPE4", m_form.sndTabUsrCtrl.vioType4.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //처리4 위반코드
            WritePrivateProfileString("SYSTEM", "VIO_CODE4", m_form.sndTabUsrCtrl.VioCode4, PROGRAM_INI_FULLPATH);
            //OBU4 번호
            WritePrivateProfileString("SYSTEM", "OBU_NUM4", m_form.sndTabUsrCtrl.ObuNum4, PROGRAM_INI_FULLPATH);
            //OBU4 종류
            WritePrivateProfileString("SYSTEM", "OBU_TYPE4", m_form.sndTabUsrCtrl.obuType4.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //OBU4 차종
            WritePrivateProfileString("SYSTEM", "OBU_CAR_CLASS4", m_form.sndTabUsrCtrl.obuCarClass4.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            // 확정 위치
            WritePrivateProfileString("SYSTEM", "CONF_LOC", m_form.othTabUsrCtrl.cnfComboBox.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //위반확인응답 타입
            WritePrivateProfileString("SYSTEM", "CONF_RES_TYPE", m_form.othTabUsrCtrl.cnfResTypeComboBox.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //영상확정시점
            WritePrivateProfileString("SYSTEM", "CONF_TIME", m_form.sndTabUsrCtrl.cftComboBox.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);
            //바이트 오더
            WritePrivateProfileString("SYSTEM", "BYTE_ORDER", m_form.sndTabUsrCtrl.ByteOrder.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);

            // 싱크 방식
            WritePrivateProfileString("SYSTEM", "SYNC_METHOD", m_form.sndTabUsrCtrl.syncMethod.SelectedIndex.ToString(), PROGRAM_INI_FULLPATH);

            // 시리얼 포트 저장
            WritePrivateProfileString("SERIAL", "PORT", m_form.Port, PROGRAM_INI_FULLPATH);
            // 시리얼 속도 저장
            WritePrivateProfileString("SERIAL", "SPEED", m_form.Speed, PROGRAM_INI_FULLPATH);
            /// 통신방식 저장
            WritePrivateProfileString("SYSTEM", "COMMETHOD", m_form.comm.ToString(), PROGRAM_INI_FULLPATH);

            // 위반확인자동응답 체크 저장
            int value = (m_form.othTabUsrCtrl.autoVioSendCheck.IsChecked == true) ? 1 : 0;
            WritePrivateProfileString("ETC", "AUTO_SEND_VIORESPONSE", value.ToString(), PROGRAM_INI_FULLPATH);

            // 영상확정자동전송 체크 저장
            value = (m_form.othTabUsrCtrl.autoConfirmSendCheck.IsChecked == true) ? 1 : 0;
            WritePrivateProfileString("ETC", "AUTO_SEND_CONFIRM", value.ToString(), PROGRAM_INI_FULLPATH);
            //자동응답 CHECK
            value = (m_form.recvTabUsrCtrl.autoSendCheck.IsChecked == true) ? 1 : 0;
            WritePrivateProfileString("SYSTEM", "AUTO_RESPONSE", value.ToString(), PROGRAM_INI_FULLPATH);

            //자동상태요구
            value =  (m_form.sndTabUsrCtrl.autoSendStatusCheck.IsChecked == true) ? 1 : 0;
            WritePrivateProfileString("SYSTEM", "AUTO_STATUS_RESQ", value.ToString(), PROGRAM_INI_FULLPATH);
        }

        /// <summary>
        /// INI 파일이름 설정
        /// </summary>
        public void setFileName(string fileName) => PROGRAM_INI_FULLPATH = fileName;
    }
 }
