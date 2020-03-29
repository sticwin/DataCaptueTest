using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;



namespace DataCaptueTest
{
    public partial class MainForm : Form
    {

#if _Bend_Length_Record
        struct cutLength
        {
            public double startLength;
            public double endLength;
            public double targetRectLength;
        }
#endif


        struct bendData
        {
            /// <summary>
            /// 胶深
            /// </summary>
            public double dGlueDepth;
            /// <summary>
            /// 目标开料次数
            /// </summary>
            public int iTaskNum;
            /// <summary>
            /// 已折弯的次数
            /// </summary>
            public int iBentNum;
            /// <summary>
            /// 长度1、长度2
            /// </summary>
            public double dLength_One, dLength_Two;
            /// <summary>
            /// 本数据所属datagridview的行号，0开始
            /// </summary>
            public int iRowIndex;

            #if _Bend_Length_Record
            /// <summary>
            /// 记录第i次折弯，有效的起始余料长度和结束余料长度，调试用
            /// </summary>
            public List<cutLength> iBentCutLength;  //记录第i次折弯，有效的起始余料长度和结束余料长度，调试用
            #endif
        }

        List<bendData> BendTaskList = new List<bendData>();
        /// <summary>
        /// 折弯任务链表所在的下标
        /// </summary>
        public int iBendListIndex;
        /// <summary>
        /// 当前剩余的折弯任务数
        /// </summary>
       // public int iBendListTaskCout;  
        public static string strSelectedPorts = "";

        int iSensorSatus = 0x0000;

        public int iCommand = 0;
        public bool bShowHex, bClosing, bDataReceived;
        public static string[] byteToHexStr = new string[] 
                                                { "00 ", "01 ", "02 ", "03 ", "04 ", "05 ", "06 ", "07 ", "08 ", "09 ", "0A ", "0B ", "0C ", "0D ", "0E ", "0F ", 
                                                  "10 ", "11 ", "12 ", "13 ", "14 ", "15 ", "16 ", "17 ", "18 ", "19 ", "1A ", "1B ", "1C ", "1D ", "1E ", "1F ",
                                                  "20 ", "21 ", "22 ", "23 ", "24 ", "25 ", "26 ", "27 ", "28 ", "29 ", "2A ", "2B ", "2C ", "2D ", "2E ", "2F ",
                                                  "30 ", "31 ", "32 ", "33 ", "34 ", "35 ", "36 ", "37 ", "38 ", "39 ", "3A ", "3B ", "3C ", "3D ", "3E ", "3F ",
                                                  "40 ", "41 ", "42 ", "43 ", "44 ", "45 ", "46 ", "47 ", "48 ", "49 ", "4A ", "4B ", "4C ", "4D ", "4E ", "4F ",
                                                  "50 ", "51 ", "52 ", "53 ", "54 ", "55 ", "56 ", "57 ", "58 ", "59 ", "5A ", "5B ", "5C ", "5D ", "5E ", "5F ",
                                                  "60 ", "61 ", "62 ", "63 ", "64 ", "65 ", "66 ", "67 ", "68 ", "69 ", "6A ", "6B ", "6C ", "6D ", "6E ", "6F ",
                                                  "70 ", "71 ", "72 ", "73 ", "74 ", "75 ", "76 ", "77 ", "78 ", "79 ", "7A ", "7B ", "7C ", "7D ", "7E ", "7F ",
                                                  "80 ", "81 ", "82 ", "83 ", "84 ", "85 ", "86 ", "87 ", "88 ", "89 ", "8A ", "8B ", "8C ", "8D ", "8E ", "8F ",
                                                  "90 ", "91 ", "92 ", "93 ", "94 ", "95 ", "96 ", "97 ", "98 ", "99 ", "9A ", "9B ", "9C ", "9D ", "9E ", "9F ",
                                                  "A0 ", "A1 ", "A2 ", "A3 ", "A4 ", "A5 ", "A6 ", "A7 ", "A8 ", "A9 ", "AA ", "AB ", "AC ", "AD ", "AE ", "AF ",
                                                  "B0 ", "B1 ", "B2 ", "B3 ", "B4 ", "B5 ", "B6 ", "B7 ", "B8 ", "B9 ", "BA ", "BB ", "BC ", "BD ", "BE ", "BF ",
                                                  "C0 ", "C1 ", "C2 ", "C3 ", "C4 ", "C5 ", "C6 ", "C7 ", "C8 ", "C9 ", "CA ", "CB ", "CC ", "CD ", "CE ", "CF ",
                                                  "D0 ", "D1 ", "D2 ", "D3 ", "D4 ", "D5 ", "D6 ", "D7 ", "D8 ", "D9 ", "DA ", "DB ", "DC ", "DD ", "DE ", "DF ",
                                                  "E0 ", "E1 ", "E2 ", "E3 ", "E4 ", "E5 ", "E6 ", "E7 ", "E8 ", "E9 ", "EA ", "EB ", "EC ", "ED ", "EE ", "EF ",
                                                  "F0 ", "F1 ", "F2 ", "F3 ", "F4 ", "F5 ", "F6 ", "F7 ", "F8 ", "F9 ", "FA ", "FB ", "FC ", "FD ", "FE ", "FF "};


        public string[] NumToHex4bit = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
        public byte[] byteSerialData = new byte[20240];
        public int byteRecvSum;
        string strShow = null;
        //int iRead = 0;
        int iKeyHint = 0xFF;

        string strSerialRecv = "";

        public bool bShowEnable = true;

        public bool bAutoLine = false;

        /// <summary>
        /// 0 未发送任何数据， 1发送了数据， 2发送准备结束的命令，等待发折弯次数减一的数据
        /// </summary>
        public int iBendStatus = 0;
        /// <summary>
        /// 0表示当前无cut， 开始切割时，状态依次为0x06、0x0E、0x14
        /// </summary>
        public int iCutStatus = 0, iLastCutStatus = 0;

        double dCurRectLength;  //记录当前已经伸出的矩形长度
        double dTargetRectLengthSum;

        public int iAutoLineNum = 0;
        /// <summary>
        /// 当前折弯尺寸，用于计算矩形周长使用
        /// </summary>
        double dBendLenght, dBendHeight;
        double dStartBendLength, dCurMaterialLength;


        public MainForm()
        {
            InitializeComponent();
        }

        private void 串口1_Load(object sender, EventArgs e)
        {
            InitCombBoxItem();
            bShowHex = true;

            dataGridView1.TopLeftHeaderCell.Value = "序号";

            for (int i = 0; i < 50; i++ )
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString() ;
                dataGridView1.Rows[i].Cells[0].Value = false;
                dataGridView1.Rows[i].Cells[1].Value = 8;
                dataGridView1.Rows[i].Cells[2].Value = i % 4 +1 ;
                dataGridView1.Rows[i].Cells[3].Value = 300+10*i;
                dataGridView1.Rows[i].Cells[4].Value = 400+10*i;
            }

            this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);

        }

        private void InitCombBoxItem()
        {
            //查询当前可用串口号
            string[] ports = SerialPort.GetPortNames();
            int iSysHasPort = 0;    // 0表示没有有效的port

            cbbxPort.Items.Clear();
            cbbxParity.Items.Clear();
            cbbxDataBits.Items.Clear();
            cbbxStopBits.Items.Clear();
            cbbxBaudRate.Items.Clear();
            //初始化串口下拉框的内容
            //将全部可用串口号，名称写入combox
            foreach (string str in ports)
            {
                cbbxPort.Items.Add(str);
                iSysHasPort++;
            }

            //设置波特率选择下拉框
            string[] tab_Baud = new string[] { "110", "300", "600", "1200",
                                            "2400", "4800", "9600", 
                                            "14400", "19200", "38400", 
                                            "57600", "115200", "128000", "256000", };
            foreach (string baud in tab_Baud)
            {
                cbbxBaudRate.Items.Add(baud);
            }

            //设置数据位下拉选项
            string[] tab_data = new string[] { "5", "6", "7", "8" };
            foreach (string dataBit in tab_data)
            {
                cbbxDataBits.Items.Add(dataBit);
            }

            //设置停止位下拉选项
            string[] tab_stop = new string[] { "1", "2" };
            foreach (string str in tab_stop)
            {
                cbbxStopBits.Items.Add(str);
            }

            //设置校验位下拉框
            string[] tab_parity = new string[] { "None", "Odd", "Even", "Mark", "Space" };
            foreach (string str in tab_parity)
            {
                cbbxParity.Items.Add(str);
            }

            if (iSysHasPort > 0)
            {
                if (string.IsNullOrEmpty(strSelectedPorts))
                {
                    cbbxPort.Text = ports[0];
                }
                else
                {
                    cbbxPort.Text = strSelectedPorts;
                }
            }
            else
            {
                cbbxPort.Text = null;
            }
            cbbxBaudRate.Text = "115200";
            cbbxDataBits.Text = "8";
            cbbxParity.Text = "Even";
            cbbxStopBits.Text = "1";

            if (SerialPort1.IsOpen)
            {
                btnOpenClose.Text = "打开";
                btnOpenClose.BackColor = Color.Green;
            } else
            {
                btnOpenClose.Text = "关闭";
                btnOpenClose.BackColor = Color.White;
                
            }

        }

        private void btnOpenClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnOpenClose.Text == "打开")
                {
                    bClosing = true;
                    btnOpenClose.UseWaitCursor = true;
                    while (bDataReceived) { Application.DoEvents(); }
                    SerialPort1.Close();
                    btnOpenClose.Text = "关闭";
                    btnOpenClose.BackColor = Color.White;
                    btnOpenClose.UseWaitCursor = false;
                }
                else if (btnOpenClose.Text == "关闭")
                {
                    bClosing = false;
                    SerialPort1.Close();    //更改串口号前， 先关闭串口
                    SerialPort1.PortName = cbbxPort.Text;
                    SerialPort1.BaudRate = int.Parse(cbbxBaudRate.Text);
                    SerialPort1.DataBits = int.Parse(cbbxDataBits.Text);
                    SerialPort1.Parity = (Parity)cbbxParity.SelectedIndex;
                    SerialPort1.StopBits = (StopBits)int.Parse(cbbxStopBits.Text);

                    SerialPort1.Open();
                    btnOpenClose.Text = "打开";
                    btnOpenClose.BackColor = Color.Green;

                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "字符显示")
            {
                button1.Text = "Hex显示";
                bShowHex = true;
            }
            else if(button1.Text == "Hex显示")
            {
                button1.Text = "字符显示";
                bShowHex = false;
            }
        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            strShow = "";
            if (bClosing == true) return;  //UI要关闭串口，尽快释放串口线程
            try
            {

                bDataReceived = true;
                if (bShowHex)
                {
                    //hex 显示
                    byteRecvSum = SerialPort1.BytesToRead; //获取需要读取字节的个数
                    SerialPort1.Read(byteSerialData, 0, byteRecvSum);
                    SerialPort1.DiscardInBuffer();  //将串口接收的数据读空

                    if (byteRecvSum == 0) return;


                    //将接收到的字符转换成hex 每个8bit的hex 用空格隔开
                    for (int i = 0; i < byteRecvSum; i++)
                    {
                        strShow += byteToHexStr[byteSerialData[i]];
                        strSerialRecv += byteToHexStr[byteSerialData[i]];

                    }

                    //if (byteRecvSum > 1 && bAutoLine)
                    //{
                    //    if (byteSerialData[0] == 0x06 && byteSerialData[byteRecvSum-1] == 0x04)
                    //    {
                    //        strShow += "\r\n";
                    //    }
                    //}

                }
                else
                {
                    strShow = SerialPort1.ReadExisting();  //将串口接收的数据读空
                }

                SerialShow();

                //BeginInvoke(
                //   new MethodInvoker(
                //        delegate()
                //        {
                //            //Control.CheckForIllegalCrossThreadCalls = false;
                //            //tbSerialOne.Text += strShow;
                //            tbSerialOne.AppendText(strShow);  //不会闪屏
                //            if (bShowHex && bAutoLine)
                //            {
                //                AutoLine(tbSerialOne.Text,iAutoLineNum );
                //            }

                //            //Control.CheckForIllegalCrossThreadCalls = true;
                //        }
                //   )
                //);
            }
            finally
            {
                bDataReceived = false;
            }
        }

        void SerialShow()
        {
            BeginInvoke(
                      new MethodInvoker(
                           delegate()
                           {
                               //Control.CheckForIllegalCrossThreadCalls = false;
                               //tbSerialOne.Text += strShow;
                               if (tbSerialOne.Text.Length == 0)
                               {
                                   tbSerialOne.Text = strShow;
                               } 
                               else
                               {
                                   tbSerialOne.AppendText(strShow);  //不会闪屏
                               }

                               if (bShowHex && bAutoLine)
                               {
                                   AutoLine(tbSerialOne.Text, iAutoLineNum);
                               }

                               //Control.CheckForIllegalCrossThreadCalls = true;
                           }
                      )
                   );

        }

        private void AutoLine(string strShowed, int startIndex)
        {
            string strTemp = strShowed;
            string strNew = null;

            //if (bShowHex == false)
            //{
            //    string strTohex=null;
            //    for (int i = 0; i < strTemp.Length; i++ )
            //    {
            //        strTohex += NumToHex[Convert.ToByte(strTemp[i])];
            //    }
            //    strTemp = strTohex;
            //}

            //if (string.IsNullOrWhiteSpace(textBox1.Text))
            //{
            //    return;
            //}

            

            for (int i=0; i < startIndex; i++ )
            {
                strNew += strTemp[i];
            }

            for (int i = startIndex; i < strTemp.Length; i++)
            {
                strNew += strTemp[i];
                if (i > 3)
                {
                    string strCheck = strTemp.Substring(i - textBox1.Text.Length + 1, textBox1.Text.Length);
                    if (strCheck == textBox1.Text)
                    {
                        strNew += "\r\n";
                        iAutoLineNum = i;
                        i++; // 06后面的空格不要了
                        
                    }
                }
            }
            tbSerialOne.Text = strNew;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tbSerialOne.Text = null;
            iAutoLineNum = 0;
 
        }

        private void tbSerialOne_TextChanged(object sender, EventArgs e)
        {
            //tbSerialOne.SelectionStart = tbSerialOne.TextLength;//文本框选中的起始点在最后
            //tbSerialOne.ScrollToCaret();//将控件内容滚动到当前插入符号位置
        }

        private void cbbxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateSerialPort();
        }

        void updateSerialPort()
        {

            try
            {
                if (btnOpenClose.Text == "打开")
                {
                    bClosing = true;
                    btnOpenClose.UseWaitCursor = true;
                    btnOpenClose.Text = "关闭";
                    btnOpenClose.BackColor = Color.White;
                    while (bDataReceived) { Application.DoEvents(); }
                    SerialPort1.Close();
                    btnOpenClose.UseWaitCursor = false;

                    //btnOpenClose.Text = "打开";
                    //btnOpenClose.BackColor = Color.Green;
                    bClosing = false;
                    SerialPort1.Close();    //更改串口号前， 先关闭串口
                    SerialPort1.PortName = cbbxPort.Text;
                    SerialPort1.BaudRate = int.Parse(cbbxBaudRate.Text);
                    SerialPort1.DataBits = int.Parse(cbbxDataBits.Text);
                    SerialPort1.Parity = (Parity)cbbxParity.SelectedIndex;
                    SerialPort1.StopBits = (StopBits)int.Parse(cbbxStopBits.Text);

                   // SerialPort1.Open();
                }
                

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            //if (false)
            //{
            //    try
            //    {
            //        bClosing = true;
            //        btnOpenClose.UseWaitCursor = true;
            //        //while (bDataReceived) { Application.DoEvents(); }
            //        SerialPort1.Close();
            //        btnOpenClose.UseWaitCursor = false;

            //        SerialPort1.PortName = cbbxPort.Text;
            //        SerialPort1.BaudRate = int.Parse(cbbxBaudRate.Text);
            //        SerialPort1.DataBits = int.Parse(cbbxDataBits.Text);
            //        SerialPort1.Parity = (Parity)cbbxParity.SelectedIndex;
            //        SerialPort1.StopBits = (StopBits)int.Parse(cbbxStopBits.Text);

            //        SerialPort1.Open();
            //    }
            //    catch (System.Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString());
            //        cbbxPort.SelectedIndex = 0;
            //        btnOpenClose.Text = "关闭";
            //        btnOpenClose.BackColor = Color.White;
            //    }

            //}

        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            string FileName = null;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "文本文档(*.txt)|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileName = sfd.FileName;
            }

            if (!string.IsNullOrWhiteSpace(FileName))
            {
                writtxt(tbSerialOne.Text, FileName);
            }

            
        }

        public  void writtxt(string strContent, string FilePath)
        {
            FileStream fileStream = new FileStream(FilePath , FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default);
            streamWriter.Write(strContent);
            streamWriter.Flush();
            streamWriter.Close();
           
            fileStream.Close();
            return ;
        }

        private void 串口1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //System.Environment.Exit(System.Environment.ExitCode);
            //this.Dispose();
            //this.Close();
            //Form1.SerialForm1.Close();
        }

        private void cbbxPort_Click(object sender, EventArgs e)
        {
            try
            {
                string CurName = SerialPort1.PortName;

                //查询当前可用串口号
                string[] ports = SerialPort.GetPortNames();
                cbbxPort.Items.Clear();
                foreach (string str in ports)
                {
                    cbbxPort.Items.Add(str);
                }
                cbbxPort.Text = CurName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string strTemp = tbSerialOne.Text;
            string strNew = null;

            //if (bShowHex == false)
            //{
            //    string strTohex=null;
            //    for (int i = 0; i < strTemp.Length; i++ )
            //    {
            //        strTohex += NumToHex[Convert.ToByte(strTemp[i])];
            //    }
            //    strTemp = strTohex;
            //}

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                return;
            }

            for (int i = 0; i < strTemp.Length; i++ )
            {
                strNew += strTemp[i];
                if (i > 3)
                {
                    string strCheck = strTemp.Substring(i - textBox1.Text.Length+1, textBox1.Text.Length);
                    if (strCheck == textBox1.Text)
                    {
                        strNew += "\r\n";
                        i++; // 06后面的空格不要了
                    }
                }
            }
            tbSerialOne.Text = strNew;
        }

        private void btnClearSendTxt_Click(object sender, EventArgs e)
        {
            tbSendText.Text = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "Hex发送")
            {
                button4.Text = "字符发送";
            }
            else if (button4.Text == "字符发送")
            {
                button4.Text = "Hex发送";
            }
        }

        private byte HexCharToValue(char cInput)
        {
            byte byteValue = 0;
            if (cInput>= 0x30 && cInput<=0x39) 
            {
                byteValue = (byte)(cInput - 0x30);
            }
            if (cInput >= 0x41 && cInput <= 0x46)
            {
                byteValue = (byte)(cInput - 0x41 + 10);
            }
            if (cInput >= 0x61 && cInput <= 0x66)
            {
                byteValue = (byte)(cInput - 0x61+10);
            }
            

            return byteValue;
        }

        private byte[] strToHex(string strInput)  //将string字符串，以byte形式发送
        {
            byte[] byteToReturn = new byte[1000];
            int j = 0;

            for (int i=0; i<strInput.Length; i++)
            {
                if (i%3 == 2) //如果不是空格分隔hex数据，就报错，不发送数据
                {
                    if (strInput[i] != 0x20)
                    {
                        MessageBox.Show("输入的16进制数据，空格位置不对，发送失败！");
                        return null;
                    }
                }
            }


            byte temp = 0;
            for (int i = 0; i < strInput.Length; i+=3 )
            {
                
                if ((i+1)<strInput.Length)
                {
                    temp = (byte)(HexCharToValue(strInput[i]) * 16);
                    temp += HexCharToValue(strInput[i + 1]);
                }
                else
                {
                    temp = HexCharToValue(strInput[i]);
                }
                
                byteToReturn[j++] = temp;
            }
            if (SerialPort1.IsOpen)
            {
                SerialPort1.Write(byteToReturn, 0, j);
            }
            else
            {
                MessageBox.Show("请先打开串口！");
            }
            

            return byteToReturn;
        }

        private bool SendCheck()
        {

            if (string.IsNullOrEmpty(tbSendText.Text)) { MessageBox.Show("请先输入内容"); return false; }

            if (SerialPort1.IsOpen)
            {
                if (button4.Text == "Hex发送")
                {
                    string strInput = tbSendText.Text;
                    for (int i = 0; i < strInput.Length; i++)
                    {
                        if (i % 3 == 2) //如果不是空格分隔hex数据，就报错，不发送数据
                        {
                            if (strInput[i] != 0x20)
                            {
                                MessageBox.Show("输入的16进制数据，空格位置不对，发送失败！");
                                return false;
                            }
                        }
                    }

                    foreach (char cText in tbSendText.Text)
                    {

                        if ((cText < 0x30 || (cText > 0x39 && cText < 0x41) || (cText > 0x46 && cText < 0x61) || cText > 0x66) && cText != 0x20)
                        {
                            MessageBox.Show("输入了非16进制数据, 发送失败");
                            return false;
                        }
                    }
                    return true;


                }
                else if (button4.Text == "字符发送")
                {
                    return true;
                }
            }

            MessageBox.Show("请先打开串口");
            return false;

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbSendText.Text))   { MessageBox.Show("请先输入内容"); return; }

            if (SerialPort1.IsOpen)
            {
                if (button4.Text == "Hex发送")
                {
                    string strTempReplace = tbSendText.Text;

                    strTempReplace = strTempReplace.Replace("\r", "");
                    tbSendText.Text = strTempReplace.Replace("\n", "");
                    foreach (char cText in tbSendText.Text)
                    {
               
                        if ((cText < 0x30 || (cText > 0x39 && cText < 0x41) || (cText > 0x46 && cText < 0x61) || cText > 0x66 )&& cText != 0x20)
                        {
                            MessageBox.Show("输入了非16进制数据, 发送失败");
                            return;
                        }
                    }
                    strToHex(tbSendText.Text);


                }
                else if (button4.Text == "字符发送")
                {
                    SerialPort1.Write(tbSendText.Text);
                }
            }
            else
            {
                MessageBox.Show("请先打开串口");
            }
        }

        private void tbSendText_KeyPress(object sender, KeyPressEventArgs e)
        {
            #region //只允许输入数字
            //if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键
            ////if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数
            //if (e.KeyChar > 0x20)
            //{
            //    try
            //    {
            //        double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());  //只要有数字，才不会报错
            //    }
            //    catch
            //    {
            //        e.KeyChar = (char)0;   //处理非法字符
            //    }
            //}
            #endregion

            //改到发送前检查数据合法性
            //if (e.KeyChar == 0x20 || e.KeyChar == 0x08) return;  // 不论什么模式，都允许输入 空格键 和删除键
            //if (button4.Text == "Hex发送")
            //{
            //    if ((e.KeyChar >= 0x30 && e.KeyChar <= 0x39) || (e.KeyChar >= 0x41 && e.KeyChar <= 0x46) || (e.KeyChar >= 0x61 && e.KeyChar <= 0x66))
            //    {
            //        return;  //允许输入0-9， A-F， a-f
            //    }
            //    else
            //    { // hex模式下， 其余字符设为非法字符， 不允许输入
            //        e.KeyChar = (char)0;
            //    }

            //}

            if (bShowHex)
            {
                tbSendText.Text.Replace("\r", "");
                tbSendText.Text.Replace("\n","");
            }



        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            btnSend_Click(null,null);


        }


        //解析数据
        public void ManipulateData()
        {
            if (strSerialRecv.Length < 2) return;

            byte[] byteRecvData = GetByteArray(strSerialRecv);
            //if (byteRecvData.Length > 100)
            //{
            //    strSerialRecv = strSerialRecv.Substring();
            //}

            #region //搜索接收到的命令中是否有符合的命令
            int iStrToDeleteSum=0, iLastLRC_Index=0;
            for (int i = 0; i < byteRecvData.Length - 5; i++)  //先搜索01 00 FF这个帧头
            {
                if (byteRecvData[i]== 0x01 && byteRecvData[i+1]==0x00 && byteRecvData[i+2]==0xFF)
                {//找到帧头
                    int iDataFrameLength = byteRecvData[i + 3];  //读取本帧数据长度，不包含校验和尾部0x04
                    int iFrameCode = byteRecvData[i + 4]; //帧命令码
                    //检查数据校验是否准确
                    if (i + iDataFrameLength + 4 < byteRecvData.Length)
                    {
                        if (byteRecvData[i + iDataFrameLength + 4] == GetLRC_Result(byteRecvData, i, iDataFrameLength + 4))
                        {  //找到符合LRC校验的数据
                            analysisData(byteRecvData, i, iDataFrameLength);  //解析整包数据
                            i += iDataFrameLength + 4; //i 后面iDataLength的数据不用再校验， 加6 是应为 01 00 FF XX（长度） 04 06总共6个都可以跳过
                            iStrToDeleteSum += (i - iLastLRC_Index) * 3;
                            iLastLRC_Index = i;
                        }
                    }
                }
            }
            #endregion
            strSerialRecv = strSerialRecv.Substring(iStrToDeleteSum);  //从制定的下标开始，删减接收到的字符串

        }

        //byteData 数据， iStartIndex 起始位置， length长度
        public void analysisData(byte[] byteData, int iStartIndex, int length)
        {

            if (byteData[iStartIndex + 3] == 0x1C && byteData[iStartIndex + 4] == 0x21)  //1C 21是关于余料长度 和 传感器状态的
            { //获取余料长度
                byte[] byteLength = new byte[4];
                byte[] byteStatus = new byte[8];

                byteLength[0] = byteData[iStartIndex + 28];
                byteLength[1] = byteData[iStartIndex + 29];
                byteLength[2] = byteData[iStartIndex + 30];
                byteLength[3] = byteData[iStartIndex + 31];
                byteSwicth(byteLength);
                float fLength = BitConverter.ToSingle(byteLength,0);
                textBox2.Text = fLength.ToString();

                #region  //读取状态字节，判断切割状态
                for (int i = 0; i < 8; i++ )  {
                    byteStatus[i] = byteData[iStartIndex + 8 + i];
                }

                switch (byteStatus[1])
                {
                    case 0:
                        iLastCutStatus = iCutStatus;
                        iCutStatus = 0;
                        break;
                    case 0x06:  //开始启动切割了
                        iLastCutStatus = iCutStatus;
                        iCutStatus = 0x06;
                        break;
                    case 0x0E: //切割中
                        iLastCutStatus = iCutStatus;
                        iCutStatus = 0x0E;
                        break;
                    case 0x14://切割完成了
                        iLastCutStatus = iCutStatus;
                        iCutStatus = 0x14;
                        break;
                    default:
                        break;
                }

                iSensorSatus = (((int)byteStatus[6]) << 8)|((int)byteStatus[7]);

                #endregion 

                textBox7.Text = byteToHexStr[iCutStatus];

                string tempStr="";
                for (int i = 0; i < 8; i++ ){
                    tempStr += byteToHexStr[byteStatus[i]];
                }
                textBox6.Text = tempStr;
            }
        }

        //byteData:计算的数据， iStartIndex开始LRC计算的位置， 参与LRC计算的个数
        public byte GetLRC_Result(byte[] byteData, int iStartIndex, int length)  
        {
            byte byteSum=0;
            for (int i = 0; i < length; i++ )
            {
                byteSum += byteData[iStartIndex+i];
            }
            byte byteTemp = (byte)(256 - byteSum + 1);
            return byteTemp;

        }


        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键
            if (e.KeyChar == 0x08) return;   //允许backspace
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());  //只要有数字，才不会报错
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符
                }
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                if (false == SendCheck())
                {
                    checkBox1.Checked = false;
                    return;
                }
                if (string.IsNullOrEmpty(textBox3.Text))
                {
                    checkBox1.Checked = false;
                    MessageBox.Show("请输入定时间隔时间ms");
                    return;
                }
                checkBox1.Checked = true;
                timer1.Interval = Convert.ToInt16(textBox3.Text);
                timer1.Enabled = true;
            }
            else
            {
                checkBox1.Checked = false;
                timer1.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                bAutoLine = true;
            } 
            else
            {
                bAutoLine = false;
            }
        }

        private void cbbxBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateSerialPort();
        }

        private void cbbxDataBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateSerialPort();
        }

        private void cbbxParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateSerialPort();
        }

        private void cbbxStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateSerialPort();
        }

        //ms级别的延时
        public static bool Delay(int delayTime)
        {
            DateTime now = DateTime.Now;
            int s = 0;
            do
            {
                TimeSpan spand = DateTime.Now - now;
                s = spand.Milliseconds;

            }

            while (s < delayTime);
            return true;


        }

        private void btnForward_MouseDown(object sender, MouseEventArgs e)
        {
            strToHex("06 01 00 FF 06 30 00 01 11 03 21 95 04");
        }

        private void btnForward_MouseUp(object sender, MouseEventArgs e)
        {
            strToHex("06 01 00 FF 06 31 00 01 11 03 21 94 04");
        }

        private void btnBack_MouseDown(object sender, MouseEventArgs e)
        {
            strToHex("06 01 00 FF 06 30 00 01 11 03 22 94 04");
        }

        private void btnBack_MouseUp(object sender, MouseEventArgs e)
        {
            strToHex("06 01 00 FF 06 31 00 01 11 03 22 93 04");
        }

        private void btnCut_Click(object sender, EventArgs e)
        {
            strToHex("06 01 00 FF 06 30 00 01 11 02 00 B7 04");
            Delay(150);
            strToHex("06 01 00 FF 06 31 00 01 11 02 00 B6 04");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            strToHex("06 01 00 FF 06 30 00 01 11 03 20 96 04");
            Delay(150);
            strToHex("06 01 00 FF 06 31 00 01 11 03 20 95 04");
        }

        private void btnContinually_Click(object sender, EventArgs e)
        {
            if (btnContinually.Text == "连续")
            {
                btnContinually.Text = "间断";
            } else if (btnContinually.Text == "间断")
            {
                btnContinually.Text = "连续";
            }

            strToHex("06 01 00 FF 06 30 00 01 11 05 02 B2 04");
            Delay(150);
            strToHex("06 01 00 FF 06 31 00 01 11 05 02 B1 04");
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            string strTemp = tbHexValue.Text;
            char[] aHex = strTemp.ToCharArray();

            byte[] aByte = GetByteArray(tbHexValue.Text);

            //一个字中，转换为高位在先
            byte byteTemp = aByte[0];
            aByte[0] = aByte[1];  //交换0、1一个字中 两个字节的顺序
            aByte[1] = byteTemp;

            byteTemp = aByte[2];  //交换2、3，后一个字中，两个字节的顺序
            aByte[2] = aByte[3];
            aByte[3] = byteTemp;

            float dValue = BitConverter.ToSingle(aByte, 0);

            tbFloatNum.Text = dValue.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            float dValue = float.Parse( tbFloatNum.Text);
            byte[] aByte = BitConverter.GetBytes(dValue);

            //一个字中，转换为高位在先
            byte byteTemp = aByte[0];
            aByte[0] = aByte[1];  //交换0、1一个字中 两个字节的顺序
            aByte[1] = byteTemp;

            byteTemp = aByte[2];  //交换2、3，后一个字中，两个字节的顺序
            aByte[2] = aByte[3];
            aByte[3] = byteTemp;

            string strTemp =  BitConverter.ToString(aByte);
            //strTemp.Replace("-", " ");
            tbHexValue.Text = strTemp.Replace('-', ' ');
            
        }


        //十六进制格式的字符串转换成byte值， 用空格割开
        public byte[] GetByteArray(string strHex)
        {
            string[] strArray = strHex.Split(' ');
            List<byte> byteList = new List<byte>();
            foreach (var s in strArray)
            {
                if (s == "")
                {
                    continue;
                }
                byteList.Add(Convert.ToByte(s, 16));
            }
            return byteList.ToArray();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            ManipulateData();  //解析余料长度, 并更新到textBox2中

            if (!SerialPort1.IsOpen)
            {
                lbl_Tips.Text = "串口未打开";  
                return;
            }

            lbl_Tips.Text = "";

            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                dCurMaterialLength = float.Parse(textBox2.Text);   //跟新当前的余料长度
            }

            switch ((iCommand++) %2)
            {
                case 0:
                    if (SerialPort1.IsOpen)
                    {
                        strToHex("06 01 00 FF 03 21 00 01 DC 04");   //查询长度和状态
                    }
                    break;
                case 1:  //这样不会在一个周期内同时发出折弯结束 和 折弯任务减1的命令
                    #region    //状态2，说明完成了一次折弯（4个边）
                    if (iBendStatus == 2)
                    {
                        bendData tempBendData = BendTaskList[iBendListIndex];

                        //先结束本次运行， 等R8空闲按，不为零才发新任务
                        GetBendSendStrData(tempBendData.dGlueDepth,
                                               0,
                                               tempBendData.dLength_One,
                                               tempBendData.dLength_Two);
                         
                        if ((iSensorSatus & 0x100) != 0x100)
                        {//R8空闲了，说明可以发送新的折弯
                            tempBendData.iBentNum += 1;
                            BendTaskList[iBendListIndex] = tempBendData;
                            if (tempBendData.iTaskNum > tempBendData.iBentNum)
                            { //本次任务还未结束
                                GetBendSendStrData(tempBendData.dGlueDepth,
                                                    tempBendData.iTaskNum - tempBendData.iBentNum,
                                                    tempBendData.dLength_One,
                                                    tempBendData.dLength_Two);
                            }
                            else
                            {  //本次任务结束，切换到下一个index
                                iBendListIndex += 1;
                            }

                            iBendStatus = 0;
                            iCutStatus = 0;
                        }


                    }
                    #endregion


                    #region //如果在状态1，判断是否完成了一次折弯，并切断了料， 如果是，则发送字符串完成折弯的命令， 并进入下一状态（当前折弯任务减1，到0位置）
                    if (iBendStatus == 1)
                    { //状态1， 说明已经启动了折弯， 等待并判断折弯结束型号， 0x06
                        //double dCurRectLength;  //记录当前已经伸出的矩形长度
                        //double dTargetRectLengthSum;
                        if (dStartBendLength < dCurMaterialLength) { //续接了一根料
                            dCurRectLength = dStartBendLength + 5000 - dCurMaterialLength;
                        } else {
                            dCurRectLength = dStartBendLength - dCurMaterialLength;
                        }

                        dTargetRectLengthSum = 2 * (dBendHeight + dBendLenght) - (6 + Convert.ToDouble(BendTaskList[iBendListIndex].dGlueDepth)) * 8 - 1.6;  //计算当前的目标切割长度
                        if ((Math.Abs(dTargetRectLengthSum - dCurRectLength) < 200) && (iCutStatus == 0x0E || iCutStatus == 0x14))   //
                        {//折弯结束
                            strToHex("06 01 00 FF 06 31 00 01 11 05 13 A0 04");  //先发送的第一条命令
                            iBendStatus = 2;  //达到折弯长度，等待发送 折弯次数减一的数据
                            iCutStatus = 0;

                            #if _Bend_Length_Record
                            cutLength CutData = new cutLength();
                            CutData.startLength = dStartBendLength;
                            CutData.endLength = dCurMaterialLength;
                            CutData.targetRectLength = dTargetRectLengthSum;
                            BendTaskList[iBendListIndex].iBentCutLength.Add(CutData);

                            //FileMode.Append为不覆盖文件效果.create为覆盖
                            FileStream fs = new FileStream("record.txt", FileMode.Append);
                            //"起始长度\t"+"结束长度\t"+"目标矩形长度\r\n"
                            byte[] data = System.Text.Encoding.Default.GetBytes(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "\t" + 
                                                                                dStartBendLength.ToString() + "\t" + 
                                                                                dCurMaterialLength.ToString() + "\t" + dTargetRectLengthSum.ToString() + "\r\n");
                            //开始写入
                            fs.Write(data, 0, data.Length);
                            //清空缓冲区、关闭流
                            fs.Flush();
                            fs.Close();

                            #endif
                        }

                        if (iCutStatus == 0x14)
                        { //断料了
                            if (!string.IsNullOrWhiteSpace(textBox2.Text))
                            {
                                if (Math.Abs(dCurRectLength - dTargetRectLengthSum) > 800)  //有断料，且距离目标长度偏差大，重置其实料长
                                {
                                    //testi++;
                                    dStartBendLength = float.Parse(textBox2.Text);  //记录折弯开始的余料长度
                                }

                            }
                        }
                    }
                    #endregion


                    #region  //状态0，  如果有任务则发送任务
                    if (iBendStatus == 0)
                    {
                        if (iBendListIndex < BendTaskList.Count)  //下标从0开始
                        {
                            double dLengthOne = BendTaskList[iBendListIndex].dLength_One;
                            double dLengthTwo = BendTaskList[iBendListIndex].dLength_Two;
                            double dTotalLength = 2 * (dLengthOne + dLengthTwo);

                            //if

                            //double[] dArrLength = new double[5];
                            //dArrLength[0] = 80;
                            //dArrLength[1] = dArrLength[0] + dLengthOne;
                            //dArrLength[2] = dArrLength[1] + dLengthTwo;
                            //dArrLength[3] = dArrLength[2] + dLengthOne;

                            startBend(BendTaskList[iBendListIndex].dGlueDepth,
                                        BendTaskList[iBendListIndex].iTaskNum - BendTaskList[iBendListIndex].iBentNum,
                                        BendTaskList[iBendListIndex].dLength_One,
                                        BendTaskList[iBendListIndex].dLength_Two);  // 发送胶深、次数、长度1、长度2
                            iBendStatus = 1;  //进入下一个状态
                        }
                    }
                    #endregion

                    break;
                default:
                    break;
            }

            if (iBendStatus == 0)
            {
                //iCutStatus = 0;
  

            }

#region 

            //if (iBendStatus == 1)
            //{//已经发送了折弯数据，并设置为连续模式
                //double dCurRectLength;  //记录当前已经伸出的矩形长度
                //double dTargetRectLengthSum;
                //if (dStartBendLength < dCurMaterialLength) { //续接了一根料
                //    dCurRectLength = dStartBendLength + 5000 - dCurMaterialLength;
                //} else {
                //    dCurRectLength = dStartBendLength - dCurMaterialLength;
                //}


                //if (iBendListIndex == 0xFFFF)
                //{ //测试模式， 默认胶深8，次数1， L1：300， L2：450mm
                //    dTargetRectLengthSum = 2 * (dBendHeight + dBendLenght) - (6 + 8) * 8 - 1.6;
                //    if (Math.Abs(dTargetRectLengthSum - dCurRectLength) < 2 && iCutStatus == 0x0E)
                //    {//折弯结束
                //        strToHex("06 01 00 FF 06 31 00 01 11 05 13 A0 04");  //先发送的第一条命令
                //        iBendStatus = 2;  //达到折弯长度，等待发送 折弯次数减一的数据
                //       // iCutStatus = 0;
                //    }
                //} 
                //else
                //{
                //    dTargetRectLengthSum = 2 * (dBendHeight + dBendLenght) - (6 + Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value)) * 8 - 1.6;  //
                //    if (Math.Abs(dTargetRectLengthSum - dCurRectLength) < 2)
                //    {//折弯结束
                //        strToHex("06 01 00 FF 06 31 00 01 11 05 13 A0 04");  //先发送的第一条命令
                //        iBendStatus = 2;  //达到折弯长度，等待发送 折弯次数减一的数据
                //    }
                //}



            //}
            //else if (iBendStatus == 2)
            //{
            //    if (iBendListIndex == 0xFFFF)
            //    {//仅仅做测试用
            //        GetBendSendStrData(8, 0, 300, 450);  //测试数据，将切割次数清零
            //        iBendListIndex = 0;
            //    } else {
            //        int iGlueDepth = Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value);
            //        int iNum = Convert.ToInt32(dataGridView1.Rows[0].Cells[1].Value);
            //        int iLength = Convert.ToInt32(dataGridView1.Rows[0].Cells[2].Value);
            //        int iHeight = Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value);
            //        GetBendSendStrData(iGlueDepth, iNum, iLength, iHeight);
            //    }




            //    //iCutStatus = 0;
            //    iBendStatus = 0;  //不处于折弯过程了
            //}


#endregion
        }



        void sendToSerialPort(string strSend)
        {
            if (SerialPort1.IsOpen)
            {
                SerialPort1.Write(strSend);
            }
            else
            {
                MessageBox.Show("串口未打开");
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }


        //将string字符串转化为byte数组
        public byte[] StrToByteArray(string strInput)
        {
            byte[] byteToReturn = new byte[1000];
            List<byte> byteList = new List<byte>();
            //int j = 0;
            for (int i = 0; i < strInput.Length; i++)
            {
                if (i % 3 == 2) //如果不是空格分隔hex数据，就报错，不发送数据
                {
                    if (strInput[i] != 0x20)
                    {
                        MessageBox.Show("输入的16进制数据，空格位置不对，发送失败！");
                        return null;
                    }
                }
            }

            byte temp = 0;
            for (int i = 0; i < strInput.Length; i += 3)
            {

                if ((i + 1) < strInput.Length)
                {
                    temp = (byte)(HexCharToValue(strInput[i]) * 16);
                    temp += HexCharToValue(strInput[i + 1]);
                }
                else
                {
                    temp = HexCharToValue(strInput[i]);
                }
                byteList.Add(temp);
                
            }

            return byteList.ToArray();


        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            byte[] sendData = new byte[] { 0x06, 0x01, 0x00, 0xFF, 0x1C, 0x21, 0x00, 0x00, 
                                           0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x18, 
                                           0xBD, 0xA4, 0x4C, 0x00, 0x01, 0xA4, 0x4C, 0x00, 
                                           0x01, 0x40, 0x00, 0x45, 0x9C, 0x1B, 0x76, 0x45, 0x56, 0xA8, 0x04 };
            if (iKeyHint == -1)
            {//向下键按下
                float fLength = float.Parse(textBox4.Text);
                fLength -= 1F;
                textBox4.Text = fLength.ToString();
            } else if(iKeyHint == 1) {
                //向上键按下
                float fLength = float.Parse(textBox4.Text);
                fLength += 1F;
                textBox4.Text = fLength.ToString();
            }

            if (checkBox3.Checked && !string.IsNullOrWhiteSpace(textBox4.Text))  
            {//自动发送余料长度被选中
               
                float fLength = float.Parse(textBox4.Text);
                byte[] byteLength = BitConverter.GetBytes(fLength);

                if (checkBox4.Checked == true)
                { //更新8个字节的数据
                    string tempStr = textBox5.Text;
                    byte[] tempByteArray = StrToByteArray(tempStr);

                    for (int i = 0; i < 8; i++) {
                        sendData[9 + i] = tempByteArray[i];
                    }
                }

                //更新余料长度数据
                byteSwicth(byteLength);  //将byte的0、1对调， 2、3对调
                for (int i=0; i < 4; i++ )
                {
                    sendData[29 + i] = byteLength[i];
                }

                byte byteLRC_Result = GetLRC_Result(sendData, 1, 32);
                sendData[33] = byteLRC_Result;



                if (SerialPort1.IsOpen)
                {
                    SerialPort1.Write(sendData, 0, 35);
                }

            }
        }


        //abyte长度为4， 将0、1对调， 2、3对调
        public void byteSwicth(byte[] aByte)
        {
            //一个字中，转换为高位在先
            byte byteTemp = aByte[0];
            aByte[0] = aByte[1];  //交换0、1一个字中 两个字节的顺序
            aByte[1] = byteTemp;

            byteTemp = aByte[2];  //交换2、3，后一个字中，两个字节的顺序
            aByte[2] = aByte[3];
            aByte[3] = byteTemp;
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData  == Keys.Down)
            {
                iKeyHint = -1; //向下键按下

            } else if (e.KeyData == Keys.Up)
            {
                iKeyHint = 1; //向上键按下
            }
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            iKeyHint = 0xFF;  //无按键按下
        }

        private void btnBack_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void button6_Click(object sender, EventArgs e)
        {
            if (iBendStatus != 0 && BendTaskList.Count != 0) {
                MessageBox.Show("当前还有未完成折弯"); return;
            }

            //startBend(8, 1,  300, 450);  //测试折弯数据胶深8， 次数1， L1：300， L2：450mm
            //iBendListIndex = 0xFFFF;  //表示测试状态
            BendTaskList.Clear();
            bendData TempBendData = new bendData();
            TempBendData.dGlueDepth = 8;
            TempBendData.iTaskNum = 1;
            TempBendData.dLength_One = 300;
            TempBendData.dLength_Two = 450;
            TempBendData.iBentNum = 0;
            TempBendData.iRowIndex = 0;

            BendTaskList.Add(TempBendData);
        
        }

        //传入胶深、 折弯次数、折弯长度1、折弯长度2
        public void startBend(double iClueDepth,int iNum, double iLength, double iHeight)
        {
            if (!SerialPort1.IsOpen) {
                MessageBox.Show("串口未打开");
                return;
            }

   
            //string strBenchData = "06 01 00 FF 22 18 00 0E 06 00 C8 "; //启动折弯胶条深度、数量、长度、宽度的模板数据，将要发送的数据依次往后追加

            //string strTemp = FloatToString((float)iClueDepth);
            //strBenchData += strTemp + " ";
            //if (iNum < 1)
            //{
            //    MessageBox.Show("错误，折弯次数小于1");
            //    return; //折弯次数小于1， 直接退出
            //}
            //strTemp = FloatToString((float)iNum);
            //strBenchData += strTemp + " ";
            //strTemp = FloatToString((float)iLength);
            //strBenchData += strTemp + " ";
            //strTemp = FloatToString((float)iHeight);
            //strBenchData += strTemp + " 00 00 00 00 00 00 00 00 00 00 00 00";

            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                dStartBendLength = float.Parse(textBox2.Text);  //记录折弯开始的余料长度
            }

            GetBendSendStrData(iClueDepth, iNum, iLength, iHeight);


            #region
            //List<byte> byteLsit = new List<byte>();
            //byte byteTemp;
            //for (int i = 0; i < strBenchData.Length; i += 3)
            //{

            //    if ((i + 1) < strBenchData.Length)
            //    {
            //        byteTemp = (byte)(HexCharToValue(strBenchData[i]) * 16);
            //        byteTemp += HexCharToValue(strBenchData[i + 1]);
            //    }
            //    else
            //    {
            //        byteTemp = HexCharToValue(strBenchData[i]);
            //    }

            //    byteLsit.Add(byteTemp);
            //}

            //byte[] byteDataArray = byteLsit.ToArray();
            //byteLsit.Add(GetLRC_Result(byteDataArray, 1, 38));
            //byteLsit.Add(04);
            //byte[] SendByteArray = byteLsit.ToArray();
            //if (SerialPort1.IsOpen)
            //{
            //    SerialPort1.Write(SendByteArray, 0, byteLsit.Count);  //根据内容发送胶深、数量、长度、宽度
            //}

            //byte[] tempByteArray = byteLsit.ToArray();
            //strBendSendData = "";
            //for (int i = 0; i < byteLsit.Count; i++)
            //{
            //    strBendSendData += byteToHexStr[tempByteArray[i]];
            //}
            #endregion

            Delay(150);

            strToHex("06 01 00 FF 06 30 00 01 11 05 02 B2 04");  //设置连续模式， 即启动折弯

            //iBendStatus = 1;  //发送了折弯数据，并启动了折弯


        }

        /// <summary>
        /// 根据胶条深度、数量、长度、宽度，追加LRC，然后发送
        /// </summary>
        /// <param name="iClueDepth"></param>
        /// <param name="iNum"></param>
        /// <param name="iLength"></param>
        /// <param name="iHeight"></param>
        public void GetBendSendStrData(double iClueDepth, int iNum, double iLength, double iHeight)  //根据胶条深度、数量、长度、宽度，追加LRC，然后发送
        {
            string strBenchData = "06 01 00 FF 22 18 00 0E 06 00 C8 "; //启动折弯胶条深度、数量、长度、宽度的模板数据，将要发送的数据依次往后追加
            string strTemp = FloatToString((float)iClueDepth);
            strBenchData += strTemp + " ";
            strTemp = FloatToString((float)iNum);
            strBenchData += strTemp + " ";
            strTemp = FloatToString((float)iLength);
            strBenchData += strTemp + " ";
            strTemp = FloatToString((float)iHeight);
            strBenchData += strTemp + " 00 00 00 00 00 00 00 00 00 00 00 00";

            dBendHeight = iHeight;  //更新当前折弯的尺寸
            dBendLenght = iLength;

            AddLRC_ByteSend(strBenchData);  //追加LRC校验，然后改成以字节发送
        }

        public void AddLRC_ByteSend(string strBenchData)  //给string追加LRC校验， 然后以字节形式发出去
        {
            List<byte> byteLsit = new List<byte>();
            byte byteTemp;
            for (int i = 0; i < strBenchData.Length; i += 3)
            {
                if ((i + 1) < strBenchData.Length)  {
                    byteTemp = (byte)(HexCharToValue(strBenchData[i]) * 16);
                    byteTemp += HexCharToValue(strBenchData[i + 1]);
                } else {
                    byteTemp = HexCharToValue(strBenchData[i]);
                }

                byteLsit.Add(byteTemp);
            }

            byte[] byteDataArray = byteLsit.ToArray();
            byteLsit.Add(GetLRC_Result(byteDataArray, 1, 38));
            byteLsit.Add(04);
            byte[] SendByteArray = byteLsit.ToArray();
            if (SerialPort1.IsOpen)
            {
                SerialPort1.Write(SendByteArray, 0, byteLsit.Count);  //根据内容发送胶深、数量、长度、宽度
            }

            byteLsit.Clear();
        }

        string FloatToString(double dValueNum)
        {
            float dValue = (float)dValueNum;
            byte[] aByte = BitConverter.GetBytes(dValue);

            //一个字中，转换为高位在先
            byte byteTemp = aByte[0];
            aByte[0] = aByte[1];  //交换0、1一个字中 两个字节的顺序
            aByte[1] = byteTemp;

            byteTemp = aByte[2];  //交换2、3，后一个字中，两个字节的顺序
            aByte[2] = aByte[3];
            aByte[3] = byteTemp;

            string strTemp = BitConverter.ToString(aByte);
            //strTemp.Replace("-", " ");
            return strTemp.Replace('-', ' ');

        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 0)
            //{
            //    int iRowIndex = e.RowIndex;
            //    bool tempValue = (bool)dataGridView1.Rows[iRowIndex].Cells[0].Value;
            //    dataGridView1.Rows[iRowIndex].Cells[0].Value = !tempValue;

            //    dataGridView1.Update();
            //}
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count <1)
            {
                return;
            }
            //int iRowIndex = e.RowIndex;
            //bool tempValue = (bool)dataGridView1.Rows[iRowIndex].Cells[0].Value;
            //dataGridView1.Rows[iRowIndex].Cells[0].Value = !tempValue;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) //checkbox列
            {
                if (Convert.ToInt32(dataGridView1.CurrentRow.Cells[2].Value) <= 0 && (bool)dataGridView1.CurrentRow.Cells[0].Value)
                {
                    dataGridView1.CurrentRow.Cells[0].Value = false;

                    //lbl_Tips.Text = "折弯次数错误";

                    //MessageBox.Show("折弯次数错误");
                }
                else
                {
                    lbl_Tips.Text = "";
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (iBendStatus != 0 && BendTaskList.Count !=0)
            {
                MessageBox.Show("请等待折弯结束，再尝试");
                return;
            }
            //检查折弯尺寸， 并更新折弯任务
            bendData TempBendData = new bendData();
            BendTaskList.Clear();
            for (int i = 0; i < dataGridView1.Rows.Count; i++ )
            {
               
                if ((bool)dataGridView1.Rows[i].Cells[0].Value)
                {//checkbox选中了
                    #region //判断任务中的数据是否OK
                    TempBendData.dGlueDepth = double.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    TempBendData.iTaskNum = int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    TempBendData.dLength_One = double.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    TempBendData.dLength_Two = double.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    TempBendData.iBentNum = 0;
                    TempBendData.iRowIndex = i;
                    TempBendData.iBentCutLength = new List<cutLength>();

                    if (TempBendData.iTaskNum < 1) { MessageBox.Show("第" + (i + 1).ToString() + "行，折弯次数错误。\r\n" + "执行失败"); return; }
                    if (TempBendData.dLength_One < 300) { MessageBox.Show("第" + (i + 1).ToString() + "行，折弯尺寸太小\r\n" + "执行失败"); return; }
                    if (TempBendData.dLength_Two < 300) { MessageBox.Show("第" + (i + 1).ToString() + "行，折弯尺寸太小\r\n" + "执行失败"); return; }
                #endregion
                    BendTaskList.Add(TempBendData);
                }

            }
            ;
            
            if (BendTaskList.Count < 1)
            {
                MessageBox.Show("未选中数据");
                return;
            }
            iBendListIndex = 0;
        }



        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {


        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                e.KeyChar = (char)0;
                MessageBox.Show("发送状态，不可编辑");
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键
            //if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //iBendStatus = 0;
            //bendData tempBendData = BendTaskList[0];
            strToHex("06 01 00 FF 06 31 00 01 11 05 13 A0 04");  //先发送的第一条命令
            GetBendSendStrData(8, 0,  500,  500);
            BendTaskList.Clear();
            iBendStatus = 0;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            iBendStatus = 0;
        }

    }
}
