using System;

namespace Calculation;

public class LegacyCalculator : ICalculator
{
    private readonly int _roundDigit;

    public LegacyCalculator(int roundDigit)
    {
        _roundDigit = roundDigit;
    }

    private string Compute(string calcStr, int opNumber)
    {
        char operation = calcStr[opNumber];
        double a = Convert.ToDouble(calcStr.Substring(0, opNumber));
        double b = Convert.ToDouble(calcStr.Substring(opNumber + 1, calcStr.Length - opNumber - 1));
        switch(operation)
        {
            case '^': a = Math.Round(Math.Pow(a, b), _roundDigit); break;
            case '*': a = Math.Round(a * b, _roundDigit); break;
            case '/': a = Math.Round(a / b, _roundDigit); break;
            case '+': a = Math.Round(a + b, _roundDigit); break;
            case '-': a = Math.Round(a - b, _roundDigit); break;
        }
        return Convert.ToString(a);
    }

    private void MathFunctions(ref string str)//str-строка с мат. операцией
    {
        bool haveOperations = true;
        while(haveOperations)
        {
            int startWord = 0;
            while((startWord < str.Length) && ((Convert.ToByte(str[startWord]) < Convert.ToByte('a')) || (Convert.ToByte(str[startWord]) > Convert.ToByte('z'))))
                startWord++;//ищём первую букву в строке(a-z)
            if(startWord < str.Length)
            {
                int firstFuncBracket = startWord + 1;
                while((firstFuncBracket < str.Length) && (str[firstFuncBracket] != '('))
                    firstFuncBracket++;
                int brackets = 1;
                int lastFuncBracket = firstFuncBracket + 1;
                while(brackets != 0)
                {
                    if(str[lastFuncBracket] == '(') brackets++;
                    else if(str[lastFuncBracket] == ')') brackets--;
                    if(brackets != 0) lastFuncBracket++;
                }
                string mathOperation = str.Substring(startWord, firstFuncBracket - startWord);
                string strBetweenBrackets = str.Substring(firstFuncBracket + 1, lastFuncBracket - firstFuncBracket - 1);
                str = str.Remove(startWord, lastFuncBracket - startWord + 1);
                if(mathOperation != "pow")// заменить на (если > 1 аргумента, то вычислить n параметров функции, и затем вызвать нужную)
                    strBetweenBrackets = Calculate(strBetweenBrackets);
                double temp = mathOperation switch
                {
                    "abs" => Math.Abs(Convert.ToDouble(strBetweenBrackets)),
                    "sin" => Math.Sin(Convert.ToDouble(strBetweenBrackets) / 57.2958),
                    "cos" => Math.Cos(Convert.ToDouble(strBetweenBrackets) / 57.2958),
                    "tan" => Math.Tan(Convert.ToDouble(strBetweenBrackets) / 57.2958),
                    "intg" => Math.Truncate(Convert.ToDouble(strBetweenBrackets)),
                    "rnd" => Math.Round(Convert.ToDouble(strBetweenBrackets)),
                    "exp" => Math.Exp(Convert.ToDouble(strBetweenBrackets)),
                    "ln" => Math.Log(Convert.ToDouble(strBetweenBrackets)),
                    "log" => Math.Log10(Convert.ToDouble(strBetweenBrackets)),
                    "sqr" => Math.Pow(Convert.ToDouble(strBetweenBrackets), 2),
                    "sqrt" => Math.Sqrt(Convert.ToDouble(strBetweenBrackets)),
                    _ => throw new InvalidOperationException($"Not supported instruction ({mathOperation}) detected."),
                };
                string result = Convert.ToString(Math.Round(temp, _roundDigit));
                str = str.Insert(startWord, result);
            }
            else haveOperations = false;
        }
    }

    private string Priority(string str)
    {
        int i;
        for(int currentPriority = 0; currentPriority <= 2; currentPriority++)
        {
            i = 0;
            while(i < str.Length - 1)
            {
                switch(currentPriority)
                {
                    case 0:
                        while((str[i] != '^') && (i < str.Length - 1))
                            i++;
                        break;
                    case 1:
                        while((str[i] != '*') && (str[i] != '/') && (i < str.Length - 1))
                            i++;
                        break;
                    case 2:
                        while((str[i] != '+') && (str[i] != '-') && (i < str.Length - 1) || ((i == 0)))
                            i++;
                        break;
                    default: i = str.Length; break;
                }
                if(i < str.Length - 1)
                {
                    int operationAdrres = i--;
                    while(((Convert.ToByte(str[i]) > 47) || (str[i] == ',')) && (i != 0))
                        i--;
                    int startFirstNumber = i == 0 ? i : i + 1;
                    int endSecondNumber = str[operationAdrres + 1] == '-' ? operationAdrres + 2 : operationAdrres + 1;
                    while(((Convert.ToByte(str[endSecondNumber]) > 47) || (str[endSecondNumber] == ',')) && (endSecondNumber != str.Length - 1))
                        endSecondNumber++;
                    if(endSecondNumber != str.Length - 1)
                        endSecondNumber--;
                    string computeStr = str.Substring(startFirstNumber, endSecondNumber - startFirstNumber + 1);
                    str = str.Remove(startFirstNumber, endSecondNumber - startFirstNumber + 1);
                    str = str.Insert(startFirstNumber, Compute(computeStr, operationAdrres - startFirstNumber));
                }
            }
        }
        return str;
    }

    private string Calculate(string calcStr)
    {
        MathFunctions(ref calcStr);
        //начнём со скобок, а потом передадим строку дальше для обработки простых мат. операций
        int firstBracket = 0;
        while(firstBracket < calcStr.Length)
        {
            if(calcStr[firstBracket] == '(')
            {
                int brackets = 1, lastBracket = firstBracket + 1;
                while(brackets > 0)
                {
                    if(calcStr[lastBracket] == ')')
                        brackets--;
                    else if(calcStr[lastBracket] == '(')
                        brackets++;
                    lastBracket++;
                }
                lastBracket--;
                string newCalcStr = calcStr.Substring(firstBracket + 1, lastBracket - firstBracket - 1);
                calcStr = calcStr.Remove(firstBracket, lastBracket - firstBracket + 1);
                newCalcStr = Calculate(newCalcStr);
                calcStr = calcStr.Insert(firstBracket, newCalcStr);
                firstBracket = newCalcStr.Length;
            }
            firstBracket++;
        }
        return Priority(calcStr);
    }

    private static string GetCalcStr(string expression)
    {
        string calcStr = expression.ToLower();
        int i = 0;
        int brackets = 0;
        while(i < calcStr.Length)
        {
            if(calcStr[i] == ' ')
                calcStr = calcStr.Remove(i, 1);
            else
            {
                if(calcStr[i] == '(') brackets++;
                else if(calcStr[i] == ')') brackets--;
                i++;
            }
        }
        if(brackets != 0)
            throw new Exception("Количество скобок \"()\" не совпадает!");
        return calcStr;
    }

    public double Execute(string expression)
        => double.Parse(Calculate(GetCalcStr(expression)));
}

/*
    private static int FindArithmeticSymbolIndex(string str, int i, int currentPriority)
    {
        switch(currentPriority)
        {
            case 0:
                while((str[i] != '^') && (i < str.Length - 1))
                    i++;
                break;
            case 1:
                while((str[i] != '*') && (str[i] != '/') && (i < str.Length - 1))
                    i++;
                break;
            case 2:
                while((str[i] != '+') && (str[i] != '-') && (i < str.Length - 1) || ((i == 0)))
                    i++;
                break;
            default: 
                return -1;
        }

        return --i;
    }*/