namespace MyCalc
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.answerBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.MultyParamsFunc = new System.Windows.Forms.CheckBox();
            this.roundDigits = new System.Windows.Forms.ComboBox();
            this.History = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(420, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 14);
            this.label1.TabIndex = 9;
            this.label1.Text = "=";
            // 
            // answerBox
            // 
            this.answerBox.Location = new System.Drawing.Point(439, 32);
            this.answerBox.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.answerBox.Name = "answerBox";
            this.answerBox.ReadOnly = true;
            this.answerBox.Size = new System.Drawing.Size(108, 21);
            this.answerBox.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 61);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 22);
            this.button1.TabIndex = 6;
            this.button1.Text = "Кнопочка";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 32);
            this.textBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(404, 21);
            this.textBox1.TabIndex = 5;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // MultyParamsFunc
            // 
            this.MultyParamsFunc.AutoSize = true;
            this.MultyParamsFunc.BackColor = System.Drawing.Color.Transparent;
            this.MultyParamsFunc.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.MultyParamsFunc.Location = new System.Drawing.Point(120, 60);
            this.MultyParamsFunc.Name = "MultyParamsFunc";
            this.MultyParamsFunc.Size = new System.Drawing.Size(290, 18);
            this.MultyParamsFunc.TabIndex = 10;
            this.MultyParamsFunc.Text = "Функции с несколькими параметрами";
            this.MultyParamsFunc.UseVisualStyleBackColor = false;
            // 
            // roundDigits
            // 
            this.roundDigits.FormattingEnabled = true;
            this.roundDigits.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.roundDigits.Location = new System.Drawing.Point(426, 63);
            this.roundDigits.Name = "roundDigits";
            this.roundDigits.Size = new System.Drawing.Size(121, 21);
            this.roundDigits.TabIndex = 11;
            // 
            // History
            // 
            this.History.FormattingEnabled = true;
            this.History.Location = new System.Drawing.Point(10, 4);
            this.History.Name = "History";
            this.History.Size = new System.Drawing.Size(537, 21);
            this.History.TabIndex = 12;
            this.History.SelectedIndexChanged += new System.EventHandler(this.History_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MyCalc.Properties.Resources._2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(601, 437);
            this.Controls.Add(this.History);
            this.Controls.Add(this.roundDigits);
            this.Controls.Add(this.MultyParamsFunc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.answerBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Калькулятор 2.1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox answerBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox MultyParamsFunc;
        private System.Windows.Forms.ComboBox roundDigits;
        private System.Windows.Forms.ComboBox History;
    }
}

