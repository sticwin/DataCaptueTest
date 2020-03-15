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
    public partial class btnTest : Form
    {
        public static string strSelectedPorts = "";


        public int iCommand = 0;
        public bool bShowHex, bClosing, bDataReceived;
        public static string[] byteToHexStr = new string[] { "00 ", "01 ", "02 ", "03 ", "04 ", "05 ", "06 ", "07 ", "08 ", "09 ", "0A ", "0B ", "0C ", "0D ", "0E ", "0F ", 
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
        public byte[] byteSerialData = new byte[2024];
        public int byteRecvSum;
        string strShow = null;
        int iRead = 0;
        int iKeyHint = 0xFF;

        string strSerialRecv = "";

        public bool bShowEnable = true;

        public bool bAutoLine = false;

        public int iBenching = 0;   //0 未发送任何数据， 1发送了数据， 2发送准备结束的命令，等待发折弯次数减一的数据

        public int iAutoLineNum = 0;
        double dBenchLenght, dBenchHeight;
        double dStartBenchLength, dCurMaterialLength;
        string strBenchSendData;

        public btnTest()
        {
            InitializeComponent();
        }

        private void 串口1_Load(object sender, EventArgs e)
        {
            InitCombBoxItem();
            bShowHex = true;

            if (dataGridView1.Rows.Count == 1)
            {
                dataGridView1.Rows[0].Cells[0].Value = "8";
                dataGridView1.Rows[0].Cells[1].Value = "1";
                dataGridView1.Rows[0].Cells[2].Value = "350";
                dataGridView1.Rows[0].Cells[3].Value = "450";
            }
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
                    btnOpenClose.Text = "关闭";
                    btnOpenClose.BackColor = Color.White;
                    while (bDataReceived) { Application.DoEvents(); }
                    SerialPort1.Close();
                    btnOpenClose.UseWaitCursor = false;
                }
                else if (btnOpenClose.Text == "关闭")
                {
                    btnOpenClose.Text = "打开";
                    btnOpenClose.BackColor = Color.Green;
                    bClosing = false;
                    SerialPort1.Close();    //更改串口号前， 先关闭串口
                    SerialPort1.PortName = cbbxPort.Text;
                    SerialPort1.BaudRate = int.Parse(cbbxBaudRate.Text);
                    SerialPort1.DataBits = int.Parse(cbbxDataBits.Text);
                    SerialPort1.Parity = (Parity)cbbxParity.SelectedIndex;
                    SerialPort1.StopBits = (StopBits)int.Parse(cbbxStopBits.Text);

                    SerialPort1.Open();

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

                    btnOpenClose.Text = "打开";
                    btnOpenClose.BackColor = Color.Green;
                    bClosing = false;
                    SerialPort1.Close();    //更改串口号前， 先关闭串口
                    SerialPort1.PortName = cbbxPort.Text;
                    SerialPort1.BaudRate = int.Parse(cbbxBaudRate.Text);
                    SerialPort1.DataBits = int.Parse(cbbxDataBits.Text);
                    SerialPort1.Parity = (Parity)cbbxParity.SelectedIndex;
                    SerialPort1.StopBits = (StopBits)int.Parse(cbbxStopBits.Text);

                    SerialPort1.Open();
                }
                

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            if (false)
            {
                try
                {
                    bClosing = true;
                    btnOpenClose.UseWaitCursor = true;
                    //while (bDataReceived) { Application.DoEvents(); }
                    SerialPort1.Close();
                    btnOpenClose.UseWaitCursor = false;

                    SerialPort1.PortName = cbbxPort.Text;
                    SerialPort1.BaudRate = int.Parse(cbbxBaudRate.Text);
                    SerialPort1.DataBits = int.Parse(cbbxDataBits.Text);
                    SerialPort1.Parity = (Parity)cbbxParity.SelectedIndex;
                    SerialPort1.StopBits = (StopBits)int.Parse(cbbxStopBits.Text);

                    SerialPort1.Open();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    cbbxPort.SelectedIndex = 0;
                    btnOpenClose.Text = "关闭";
                    btnOpenClose.BackColor = Color.White;
                }

            }

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

        private byte[] strToHex(string strInput)
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

        public void ManipulateData()
        {
            if (strSerialRecv.Length < 2) return;

            byte[] byteRecvData = GetByteArray(strSerialRecv);
            //if (byteRecvData.Length > 100)
            //{
            //    strSerialRecv = strSerialRecv.Substring();
            //}

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
                        {
                            analysisData(byteRecvData, i, iDataFrameLength);
                            i += iDataFrameLength + 4; //i 后面iDataLength的数据不用再校验， 加6 是应为 01 00 FF XX（长度） 04 06总共6个都可以跳过
                            iStrToDeleteSum += (i - iLastLRC_Index) * 3;
                            iLastLRC_Index = i;
                            
                            //strSerialRecv = strSerialRecv.Substring(3 * (iDataFrameLength + 4));
                            //  MessageBox.Show("收到满足LRC校验的数据");
                        }
                    }
                }
            }
            strSerialRecv = strSerialRecv.Substring(iStrToDeleteSum);

        }

        //byteData 数据， iStartIndex 起始位置， length长度
        public void analysisData(byte[] byteData, int iStartIndex, int length)
        {

            if (byteData[iStartIndex + 3] == 0x1C && byteData[iStartIndex + 4] == 0x21)
            { //获取余料长度
                byte[] byteLength = new byte[4];

                byteLength[0] = byteData[iStartIndex + 28];
                byteLength[1] = byteData[iStartIndex + 29];
                byteLength[2] = byteData[iStartIndex + 30];
                byteLength[3] = byteData[iStartIndex + 31];
                byteSwicth(byteLength);
                float fLength = BitConverter.ToSingle(byteLength,0);
                textBox2.Text = fLength.ToString();

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
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                dCurMaterialLength = float.Parse(textBox2.Text); 
            }

            switch ((iCommand++) %2)
            {
                case 0:
                    if (SerialPort1.IsOpen)
                    {
                        strToHex("06 01 00 FF 03 21 00 01 DC 04");
                    }
                    break;
                case 1:

                    break;
                default:
                    break;
            }

            if (iBenching == 1)
            {
                double dCurRectLength;
                if (dStartBenchLength < dCurMaterialLength )
                {
                    dCurRectLength = dStartBenchLength + 5000 - dCurMaterialLength;
                }
                else
                {
                    dCurRectLength = dStartBenchLength - dCurMaterialLength;
                }
                double dTargetRectLengthSum = 2 * (dBenchHeight + dBenchLenght) - (6 + Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value))*8 -1.6;  //
                if (Math.Abs(dTargetRectLengthSum - dCurRectLength) < 2)
                {//折弯结束
                    strToHex("06 01 00 FF 06 31 00 01 11 05 13 A0 04");  //先发送的第一条命令
                    iBenching = 2;  //达到折弯长度，等待发送 折弯次数减一的数据
                }
            }
            else if (iBenching == 2)
            {
                int iClueDepth = Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value);
                int iNum = Convert.ToInt32(dataGridView1.Rows[0].Cells[1].Value);
                int iLength = Convert.ToInt32(dataGridView1.Rows[0].Cells[2].Value);
                int iHeight = Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value);
                string strBenchData = "06 01 00 FF 22 18 00 0E 06 00 C8 "; //启动折弯胶条深度、数量、长度、宽度的模板数据
                string strTemp = FloatToString((float)iClueDepth);
                strBenchData += strTemp + " ";
                strTemp = FloatToString((float)(iNum-1));
                strBenchData += strTemp + " ";
                strTemp = FloatToString((float)iLength);
                strBenchData += strTemp + " ";
                strTemp = FloatToString((float)iHeight);
                strBenchData += strTemp + " 00 00 00 00 00 00 00 00 00 00 00 00";


                List<byte> byteLsit = new List<byte>();
                byte byteTemp;
                for (int i = 0; i < strBenchData.Length; i += 3)  //把字符串改成byte数据，用list存储
                {
                    if ((i + 1) < strBenchData.Length)
                    {
                        byteTemp = (byte)(HexCharToValue(strBenchData[i]) * 16);
                        byteTemp += HexCharToValue(strBenchData[i + 1]);
                    }
                    else
                    {
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


                iBenching = 0;  //不处于折弯过程了
            }

            
            ManipulateData();  //解析余料长度
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

        private void timer3_Tick(object sender, EventArgs e)
        {
            byte[] sendData = new byte[] { 0x06, 0x01, 0x00, 0xFF, 0x1C, 0x21, 0x00, 0x00, 
                                           0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x18, 
                                           0xBD, 0xA4, 0x4C, 0x00, 0x01, 0xA4, 0x4C, 0x00, 
                                           0x01, 0x40, 0x00, 0x45, 0x9C, 0x1B, 0x76, 0x45, 0x56, 0xA8, 0x04 };
            if (iKeyHint == -1)
            {//向下键按下
                float fLength = float.Parse(textBox4.Text);
                fLength -= 0.7F;
                textBox4.Text = fLength.ToString();
            } else if(iKeyHint == 1) {
                //向上键按下
                float fLength = float.Parse(textBox4.Text);
                fLength += 0.7F;
                textBox4.Text = fLength.ToString();
            }

            if (checkBox3.Checked && !string.IsNullOrWhiteSpace(textBox4.Text))  
            {//自动发送余料长度被选中
               
                float fLength = float.Parse(textBox4.Text);
                byte[] byteLength = BitConverter.GetBytes(fLength);

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
            if (!SerialPort1.IsOpen)
            {
                return;
            }

            string strBenchData = "06 01 00 FF 22 18 00 0E 06 00 C8 "; //启动折弯胶条深度、数量、长度、宽度的模板数据
            string strConform = "06 01 00 FF 06 31 00 01 11 05 13 A0 04";  //用于说明，本次折弯要结束了，前置命令
            string strEndBenchData = "06 01 00 FF 22 18 00 0E 06 00 C8 00 00 41 00 00 00 3F 80 00 00 43 AF 00 00 43 B9 00 00 00 00 00 00 00 00 00 00 00 00 FD 04";
            string strBenchStart = "06 01 00 FF 06 30 00 01 11 05 02 B2 04";  //设置好折弯数据后， 发送此命令

            int iClueDepth = Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value);
            int iNum = Convert.ToInt32(dataGridView1.Rows[0].Cells[1].Value);
            int iLength = Convert.ToInt32(dataGridView1.Rows[0].Cells[2].Value);
            int iHeight = Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value);

            dBenchHeight = iHeight;
            dBenchLenght = iLength;

            string strTemp = FloatToString((float)iClueDepth);
            strBenchData += strTemp + " ";
            if (iNum <1)
            {
                return; //折弯次数小于1， 直接退出
            }
            strTemp = FloatToString((float)iNum);
            strBenchData += strTemp + " ";
            strTemp = FloatToString((float)iLength);
            strBenchData += strTemp + " ";
            strTemp = FloatToString((float)iHeight);
            strBenchData += strTemp + " 00 00 00 00 00 00 00 00 00 00 00 00";

            List <byte> byteLsit = new List<byte>();
            byte byteTemp ;
            for (int i = 0; i < strBenchData.Length; i += 3)
            {

                if ((i + 1) < strBenchData.Length)
                {
                    byteTemp = (byte)(HexCharToValue(strBenchData[i]) * 16);
                    byteTemp += HexCharToValue(strBenchData[i + 1]);
                }
                else
                {
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

            byte[] tempByteArray = byteLsit.ToArray();
            strBenchSendData = "";
            for (int i = 0; i < byteLsit.Count; i++)
            {
                strBenchSendData += byteToHexStr[tempByteArray[i]];
            }


                Delay(150);

            strToHex("06 01 00 FF 06 30 00 01 11 05 02 B2 04");  //设置间断， 即启动折弯

            iBenching = 1;  //f发送了折弯数据，并启动了折弯


            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                dStartBenchLength = float.Parse(textBox2.Text);
            }

        
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

    }
}
