namespace DataCaptueTest
{
    partial class btnTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbSerialOne = new System.Windows.Forms.TextBox();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.btnOpenClose = new System.Windows.Forms.Button();
            this.cbbxStopBits = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbbxDataBits = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbbxParity = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbbxBaudRate = new System.Windows.Forms.ComboBox();
            this.cbbxPort = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SerialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tbSendText = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnClearSendTxt = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnCut = new System.Windows.Forms.Button();
            this.btnContinually = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tbFloatNum = new System.Windows.Forms.TextBox();
            this.tbHexValue = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.胶深 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.长度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.宽度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button6 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbSerialOne
            // 
            this.tbSerialOne.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSerialOne.Location = new System.Drawing.Point(632, 80);
            this.tbSerialOne.Multiline = true;
            this.tbSerialOne.Name = "tbSerialOne";
            this.tbSerialOne.ReadOnly = true;
            this.tbSerialOne.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSerialOne.Size = new System.Drawing.Size(705, 294);
            this.tbSerialOne.TabIndex = 1;
            this.tbSerialOne.TextChanged += new System.EventHandler(this.tbSerialOne_TextChanged);
            // 
            // btnSaveData
            // 
            this.btnSaveData.Location = new System.Drawing.Point(119, 41);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(75, 23);
            this.btnSaveData.TabIndex = 28;
            this.btnSaveData.Text = "保存数据";
            this.btnSaveData.UseVisualStyleBackColor = true;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // btnOpenClose
            // 
            this.btnOpenClose.Location = new System.Drawing.Point(21, 41);
            this.btnOpenClose.Name = "btnOpenClose";
            this.btnOpenClose.Size = new System.Drawing.Size(75, 23);
            this.btnOpenClose.TabIndex = 27;
            this.btnOpenClose.Text = "关闭";
            this.btnOpenClose.UseVisualStyleBackColor = true;
            this.btnOpenClose.Click += new System.EventHandler(this.btnOpenClose_Click);
            // 
            // cbbxStopBits
            // 
            this.cbbxStopBits.FormattingEnabled = true;
            this.cbbxStopBits.Location = new System.Drawing.Point(685, 5);
            this.cbbxStopBits.Name = "cbbxStopBits";
            this.cbbxStopBits.Size = new System.Drawing.Size(82, 20);
            this.cbbxStopBits.TabIndex = 25;
            this.cbbxStopBits.SelectedIndexChanged += new System.EventHandler(this.cbbxStopBits_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(639, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 26;
            this.label5.Text = "停止位：";
            // 
            // cbbxDataBits
            // 
            this.cbbxDataBits.FormattingEnabled = true;
            this.cbbxDataBits.Location = new System.Drawing.Point(380, 5);
            this.cbbxDataBits.Name = "cbbxDataBits";
            this.cbbxDataBits.Size = new System.Drawing.Size(82, 20);
            this.cbbxDataBits.TabIndex = 23;
            this.cbbxDataBits.SelectedIndexChanged += new System.EventHandler(this.cbbxDataBits_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(333, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 24;
            this.label4.Text = "数据位：";
            // 
            // cbbxParity
            // 
            this.cbbxParity.FormattingEnabled = true;
            this.cbbxParity.Location = new System.Drawing.Point(526, 5);
            this.cbbxParity.Name = "cbbxParity";
            this.cbbxParity.Size = new System.Drawing.Size(82, 20);
            this.cbbxParity.TabIndex = 21;
            this.cbbxParity.SelectedIndexChanged += new System.EventHandler(this.cbbxParity_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(487, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "校验：";
            // 
            // cbbxBaudRate
            // 
            this.cbbxBaudRate.FormattingEnabled = true;
            this.cbbxBaudRate.Location = new System.Drawing.Point(215, 5);
            this.cbbxBaudRate.Name = "cbbxBaudRate";
            this.cbbxBaudRate.Size = new System.Drawing.Size(82, 20);
            this.cbbxBaudRate.TabIndex = 19;
            this.cbbxBaudRate.SelectedIndexChanged += new System.EventHandler(this.cbbxBaudRate_SelectedIndexChanged);
            // 
            // cbbxPort
            // 
            this.cbbxPort.FormattingEnabled = true;
            this.cbbxPort.Location = new System.Drawing.Point(67, 5);
            this.cbbxPort.Name = "cbbxPort";
            this.cbbxPort.Size = new System.Drawing.Size(82, 20);
            this.cbbxPort.TabIndex = 17;
            this.cbbxPort.SelectedIndexChanged += new System.EventHandler(this.cbbxPort_SelectedIndexChanged);
            this.cbbxPort.Click += new System.EventHandler(this.cbbxPort_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(169, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "波特率：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "串口号：";
            // 
            // SerialPort1
            // 
            this.SerialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(218, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 33;
            this.button1.Text = "Hex显示";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(314, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 34;
            this.button2.Text = "清空接收";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(417, 40);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 35;
            this.button3.Text = "手动规整";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(508, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 36;
            this.textBox1.Text = "04 06";
            // 
            // tbSendText
            // 
            this.tbSendText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSendText.Location = new System.Drawing.Point(632, 383);
            this.tbSendText.Multiline = true;
            this.tbSendText.Name = "tbSendText";
            this.tbSendText.Size = new System.Drawing.Size(705, 135);
            this.tbSendText.TabIndex = 37;
            this.tbSendText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSendText_KeyPress);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(8, 490);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 38;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnClearSendTxt
            // 
            this.btnClearSendTxt.Location = new System.Drawing.Point(8, 433);
            this.btnClearSendTxt.Name = "btnClearSendTxt";
            this.btnClearSendTxt.Size = new System.Drawing.Size(75, 23);
            this.btnClearSendTxt.TabIndex = 39;
            this.btnClearSendTxt.Text = "清空发送";
            this.btnClearSendTxt.UseVisualStyleBackColor = true;
            this.btnClearSendTxt.Click += new System.EventHandler(this.btnClearSendTxt_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(8, 462);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 40;
            this.button4.Text = "Hex发送";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(12, 405);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(43, 21);
            this.textBox3.TabIndex = 41;
            this.textBox3.Text = "100";
            this.textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox3_KeyPress);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoCheck = false;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(8, 384);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 42;
            this.checkBox1.Text = "定时发送";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(61, 410);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 43;
            this.label7.Text = "ms";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(632, 48);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(72, 16);
            this.checkBox2.TabIndex = 44;
            this.checkBox2.Text = "自动规整";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point(1, 250);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(50, 23);
            this.btnForward.TabIndex = 45;
            this.btnForward.Text = "向前";
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnForward_MouseDown);
            this.btnForward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnForward_MouseUp);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(56, 250);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(50, 23);
            this.btnBack.TabIndex = 46;
            this.btnBack.Text = "向后";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            this.btnBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBack_MouseDown);
            this.btnBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBack_MouseUp);
            // 
            // btnCut
            // 
            this.btnCut.Location = new System.Drawing.Point(1, 278);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(50, 23);
            this.btnCut.TabIndex = 47;
            this.btnCut.Text = "断料";
            this.btnCut.UseVisualStyleBackColor = true;
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // btnContinually
            // 
            this.btnContinually.Location = new System.Drawing.Point(56, 278);
            this.btnContinually.Name = "btnContinually";
            this.btnContinually.Size = new System.Drawing.Size(50, 23);
            this.btnContinually.TabIndex = 48;
            this.btnContinually.Text = "连续";
            this.btnContinually.UseVisualStyleBackColor = true;
            this.btnContinually.Click += new System.EventHandler(this.btnContinually_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(2, 307);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(50, 23);
            this.btnReset.TabIndex = 49;
            this.btnReset.Text = "复位";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 50;
            this.label6.Text = "余料长度：mm";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 94);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(88, 21);
            this.textBox2.TabIndex = 51;
            // 
            // tbFloatNum
            // 
            this.tbFloatNum.Location = new System.Drawing.Point(9, 136);
            this.tbFloatNum.Name = "tbFloatNum";
            this.tbFloatNum.Size = new System.Drawing.Size(88, 21);
            this.tbFloatNum.TabIndex = 52;
            this.tbFloatNum.Text = "1.0";
            // 
            // tbHexValue
            // 
            this.tbHexValue.Location = new System.Drawing.Point(9, 209);
            this.tbHexValue.Name = "tbHexValue";
            this.tbHexValue.Size = new System.Drawing.Size(88, 21);
            this.tbHexValue.TabIndex = 53;
            this.tbHexValue.Text = "00 00 3F 80";
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(13, 168);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(38, 23);
            this.btnConvert.TabIndex = 54;
            this.btnConvert.Text = "↑";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 55;
            this.label8.Text = "浮点数";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 194);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 12);
            this.label9.TabIndex = 56;
            this.label9.Text = "Hex";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(-1, 233);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 12);
            this.label10.TabIndex = 57;
            this.label10.Text = "1.0 = 00 00 3F 80";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(63, 168);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(38, 23);
            this.button5.TabIndex = 58;
            this.button5.Text = "↓";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(5, 354);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(88, 21);
            this.textBox4.TabIndex = 59;
            this.textBox4.Text = "1.0";
            this.textBox4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox4_KeyDown);
            this.textBox4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox4_KeyUp);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(5, 336);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(96, 16);
            this.checkBox3.TabIndex = 61;
            this.checkBox3.Text = "发送余料长度";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // timer3
            // 
            this.timer3.Enabled = true;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.胶深,
            this.数量,
            this.长度,
            this.宽度});
            this.dataGridView1.Location = new System.Drawing.Point(131, 94);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(443, 136);
            this.dataGridView1.TabIndex = 62;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // 胶深
            // 
            this.胶深.HeaderText = "胶深";
            this.胶深.Name = "胶深";
            // 
            // 数量
            // 
            this.数量.HeaderText = "数量";
            this.数量.Name = "数量";
            // 
            // 长度
            // 
            this.长度.HeaderText = "长度";
            this.长度.Name = "长度";
            // 
            // 宽度
            // 
            this.宽度.HeaderText = "宽度";
            this.宽度.Name = "宽度";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(171, 278);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(91, 23);
            this.button6.TabIndex = 63;
            this.button6.Text = "测试";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // btnTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1340, 520);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.tbHexValue);
            this.Controls.Add(this.tbFloatNum);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnContinually);
            this.Controls.Add(this.btnCut);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnForward);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.btnClearSendTxt);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbSendText);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSaveData);
            this.Controls.Add(this.btnOpenClose);
            this.Controls.Add(this.cbbxStopBits);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbbxDataBits);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbbxParity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbbxBaudRate);
            this.Controls.Add(this.cbbxPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbSerialOne);
            this.Name = "btnTest";
            this.Text = "Steven串口助手";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.串口1_FormClosed);
            this.Load += new System.EventHandler(this.串口1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSerialOne;
        private System.Windows.Forms.Button btnSaveData;
        private System.Windows.Forms.Button btnOpenClose;
        private System.Windows.Forms.ComboBox cbbxStopBits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbbxDataBits;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbbxParity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbbxBaudRate;
        private System.Windows.Forms.ComboBox cbbxPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.IO.Ports.SerialPort SerialPort1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox tbSendText;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnClearSendTxt;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnCut;
        private System.Windows.Forms.Button btnContinually;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox tbFloatNum;
        private System.Windows.Forms.TextBox tbHexValue;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 胶深;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 长度;
        private System.Windows.Forms.DataGridViewTextBoxColumn 宽度;
        private System.Windows.Forms.Button button6;
    }
}