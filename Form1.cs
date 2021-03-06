﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCalc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            roundDigits.SelectedIndex = 2;
        }

        int RoundDigit;

        private string compute(string calcStr, int opNumber)
        {
            char operation = calcStr[opNumber];
            double a = Convert.ToDouble(calcStr.Substring(0, opNumber));
            double b = Convert.ToDouble(calcStr.Substring(opNumber + 1, calcStr.Length - opNumber - 1));
            switch (operation)
            {
                case '^': a = Math.Round(Math.Pow(a, b), RoundDigit); break;
                case '*': a = Math.Round(a * b, RoundDigit); break;
                case '/': a = Math.Round(a / b, RoundDigit); break;
                case '+': a = Math.Round(a + b, RoundDigit); break;
                case '-': a = Math.Round(a - b, RoundDigit); break;
            }
            return Convert.ToString(a);
        }

        void mathFunctions(ref string str)//str-строка с мат. операцией
        {
            bool haveOperations = true;
            while (haveOperations)
            {
                int startWord = 0;
                while ((startWord < str.Length) && ((Convert.ToByte(str[startWord]) < Convert.ToByte('a')) || (Convert.ToByte(str[startWord]) > Convert.ToByte('z'))))
                    startWord++;//ищём первую букву в строке(a-z)
                if (startWord < str.Length)
                {
                    int firstFuncBracket = startWord + 1;
                    while ((firstFuncBracket < str.Length) && (str[firstFuncBracket] != '('))
                        firstFuncBracket++;
                    int brackets = 1;
                    int lastFuncBracket = firstFuncBracket + 1;
                    while (brackets != 0)
                    {
                        if (str[lastFuncBracket] == '(') brackets++;
                        else if (str[lastFuncBracket] == ')') brackets--;
                        if (brackets != 0) lastFuncBracket++;
                    }
                    string mathOperation = str.Substring(startWord, firstFuncBracket - startWord);
                    string strBetweenBrackets = str.Substring(firstFuncBracket + 1, lastFuncBracket - firstFuncBracket - 1);
                    str = str.Remove(startWord, lastFuncBracket - startWord + 1);
                    if (mathOperation != "pow")// заменить на (если > 1 аргумента, то вычислить n параметров функции, и затем вызвать нужную)
                        strBetweenBrackets = calculate(strBetweenBrackets);
                    double temp = 0;
                    switch (mathOperation)
                    {
                        case "abs": temp = Math.Abs(Convert.ToDouble(strBetweenBrackets)); break;
                        case "sin": temp = Math.Sin(Convert.ToDouble(strBetweenBrackets) / 57.2958); break;
                        case "cos": temp = Math.Cos(Convert.ToDouble(strBetweenBrackets) / 57.2958); break;
                        case "tan": temp = Math.Tan(Convert.ToDouble(strBetweenBrackets) / 57.2958); break;
                        case "int": temp = Math.Truncate(Convert.ToDouble(strBetweenBrackets)); break;
                        case "rnd": temp = Math.Round(Convert.ToDouble(strBetweenBrackets)); break;
                        case "exp": temp = Math.Exp(Convert.ToDouble(strBetweenBrackets)); break;
                        case "ln": temp = Math.Log10(Convert.ToDouble(strBetweenBrackets)); break;
                        case "sqr": temp = Math.Pow(Convert.ToDouble(strBetweenBrackets), 2); break;
                        case "sqrt": temp = Math.Sqrt(Convert.ToDouble(strBetweenBrackets)); break;
                        /*кейсы с функциями
                         * frac?
                         * case "max":
                         * case "min":
                         * case "pow":
                         * case "pow":
                         * case "log":
                          */
                    }
                    string result = Convert.ToString(Math.Round(temp, RoundDigit));
                    str = str.Insert(startWord, result);
                }
                else haveOperations = false;
            }
        }

        private string priority(string str)
        {
            int i;
            for (int currentPriority = 0; currentPriority <= 2; currentPriority++)
            {
                i = 0;
                while (i < str.Length - 1)
                {
                    switch (currentPriority)
                    {
                        case 0:
                            while ((str[i] != '^') && (i < str.Length - 1))
                                i++;
                            break;
                        case 1: while ((str[i] != '*') && (str[i] != '/') && (i < str.Length - 1))
                                i++;
                            break;
                        case 2: while ((str[i] != '+') && (str[i] != '-') && (i < str.Length - 1) || ((i == 0)))
                                i++;
                            break;
                        default: i = str.Length; break;
                    }
                    if (i < str.Length - 1)
                    {
                        int operationAdrres = i--;
                        while (((Convert.ToByte(str[i]) > 47) || (str[i] == ',')) && (i != 0))
                            i--;
                        int startFirstNumber= i==0 ? i : i + 1;
                        int endSecondNumber = str[operationAdrres + 1] == '-' ? operationAdrres + 2 : operationAdrres + 1;
                        while (((Convert.ToByte(str[endSecondNumber]) > 47) || (str[endSecondNumber] == ',')) && (endSecondNumber != str.Length - 1))
                            endSecondNumber++;
                        if (endSecondNumber != str.Length - 1)
                            endSecondNumber--;
                        string computeStr = str.Substring(startFirstNumber, endSecondNumber - startFirstNumber + 1);
                        str = str.Remove(startFirstNumber, endSecondNumber - startFirstNumber + 1);
                        str = str.Insert(startFirstNumber, compute(computeStr, operationAdrres - startFirstNumber));
                    }
                }
            }
            return str;
        }

        private string calculate(string calcStr)
        {
            mathFunctions(ref calcStr);
            //начнём со скобок, а потом передадим строку дальше для обработки простых мат. операций
            int firstBracket = 0;
            while (firstBracket < calcStr.Length)
            {
                if (calcStr[firstBracket] == '(')
                {
                    int brackets = 1, lastBracket = firstBracket + 1;
                    while (brackets > 0)
                    {
                        if (calcStr[lastBracket] == ')')
                            brackets--;
                        else if (calcStr[lastBracket] == '(')
                            brackets++;
                        lastBracket++;
                    }
                    lastBracket--;
                    string newCalcStr = calcStr.Substring(firstBracket + 1, lastBracket - firstBracket - 1);
                    calcStr = calcStr.Remove(firstBracket, lastBracket - firstBracket + 1);
                    newCalcStr = calculate(newCalcStr);
                    calcStr = calcStr.Insert(firstBracket, newCalcStr);
                    firstBracket = newCalcStr.Length;
                }
                firstBracket++;
            }
            return priority(calcStr);
        }

        string getCalcStr()
        {
            string calcStr = textBox1.Text.ToLower();
            if (MultyParamsFunc.Checked)
            {
                calcStr = calcStr.Replace('.', ' ');
                calcStr = calcStr.Replace(',', '.');
                calcStr = calcStr.Replace(' ', ',');
            }
            else
                calcStr = calcStr.Replace('.', ',');
            int i = 0;
            int brackets = 0;
            while (i < calcStr.Length)
            {
                if (calcStr[i] == ' ')
                    calcStr = calcStr.Remove(i, 1);
                else
                {
                    if (calcStr[i] == '(') brackets++;
                    else if (calcStr[i] == ')') brackets--;
                    i++;
                }
            }
            if (brackets != 0)
                throw new Exception("Количество скобок \"()\" не совпадает!");
            return calcStr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                RoundDigit = int.Parse(roundDigits.SelectedItem.ToString());
                answerBox.Text = calculate(getCalcStr());
                if (History.Items.Count == 0 || textBox1.Text != History.Items[History.Items.Count - 1].ToString())
                    History.Items.Add(textBox1.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==(char)Keys.Enter)
                button1_Click(sender, new EventArgs());
        }

        private void History_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = History.SelectedItem.ToString();
        }
    }
}
