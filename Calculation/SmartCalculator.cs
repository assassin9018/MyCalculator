using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculation;

public class SmartCalculator
{
    private static readonly HashSet<string> _oneArgFunctions = new(Enum.GetValues<OneArgFunctionType>().Select(x => x.ToString().ToLower()));
    private static readonly HashSet<string> _twoArgFunctions = new(Enum.GetValues<TwoArgFunctionType>().Select(x => x.ToString().ToLower()));
    private static readonly HashSet<string> _emptyVariables = new();
    private readonly HashSet<string> _variables;

    public SmartCalculator()
    {
        _variables = _emptyVariables;
    }

    public SmartCalculator(IEnumerable<string> variables)
    {
        _variables = new();
        foreach(string variable in variables)
        {
            if(_oneArgFunctions.Contains(variable) || _twoArgFunctions.Contains(variable))
                throw new ArgumentException($"Could not declare variable {variable}, because there is fuction with the same name.");
            _variables.Add(variable);
        }
    }

    public IExpressionNode Parse(string expression, int accuracy)
    {
        string cleanedExpression = expression.Replace(" ", "").ToLower();
        ThrowIfNotValidExpression(cleanedExpression);

        IExpressionNode head = BuildTree(cleanedExpression.AsSpan());

        return new TwoArgFunctionNode(head, new ValueNode(accuracy), TwoArgFunctionType.Rnd2);
    }

    public double Execute(string expression, int round)
        => Parse(expression, round).Value;

    private IExpressionNode BuildTree(ReadOnlySpan<char> expressionStr)
    {
        List<(OperationType, IExpressionNode)> nodeWithOperations = new();
        int i = 0;
        OperationType curOp = OperationType.None;
        while(i < expressionStr.Length)
        {
            IExpressionNode node;
            node = GetNode(expressionStr[i..], out int handledCharsCount);
            nodeWithOperations.Add((curOp, node));
            i += handledCharsCount;
            if(i < expressionStr.Length)
                curOp = GetNextOperationType(expressionStr[i]);
            i++;
        }

        return CombineNodesWithPriority(nodeWithOperations);
    }

    private IExpressionNode GetNode(ReadOnlySpan<char> expressionStr, out int handledCharsCount)
    {
        char currentChar = expressionStr[0];
        IExpressionNode node;
        if(currentChar == '(')
            node = HandleBracketsPart(expressionStr, out handledCharsCount);
        else if(char.IsDigit(currentChar))
            node = GetValueNode(expressionStr, out handledCharsCount);
        else if(char.IsLetter(currentChar))
            node = HandleWord(expressionStr, out handledCharsCount);
        else if(currentChar == '-')
            node = HandleMinusSymbols(expressionStr, out handledCharsCount);
        else
            throw new ArgumentException("Invalid expression");

        return node;
    }

    private IExpressionNode HandleBracketsPart(ReadOnlySpan<char> expressionStr, out int handledCharsCount)
    {
        int i = 1, openBrCount = 1, lastBracket;
        while(openBrCount > 0)
        {
            if(expressionStr[i] == ')')
                openBrCount--;
            else if(expressionStr[i] == '(')
                openBrCount++;
            i++;
        }
        lastBracket = --i;

        var node = BuildTree(expressionStr[1..lastBracket]);
        handledCharsCount = lastBracket + 1;

        return node;
    }

    private static IExpressionNode GetValueNode(ReadOnlySpan<char> expressionStr, out int handledCharsCount)
    {
        int length = 0;
        while(length < expressionStr.Length && (char.IsDigit(expressionStr[length]) || expressionStr[length] == '.'))
            length++;

        handledCharsCount = length;
        return new ValueNode(double.Parse(expressionStr[..length]));
    }

    private IExpressionNode HandleWord(ReadOnlySpan<char> expressionStr, out int handledCharsCount)
    {
        int wordLength = 0;
        while(wordLength < expressionStr.Length && char.IsLetterOrDigit(expressionStr[wordLength]))
            wordLength++;

        string word = new(expressionStr[..wordLength]);
        //проверить является ли переменной или функцией(1 - 2 параметра), 
        if(IsVariable(word))
        {
            handledCharsCount = wordLength;
            return new VariableNode(word);
        }

        if(IsOneArgFunc(word))
        {
            IExpressionNode arg = HandleBracketsPart(expressionStr[wordLength..], out int innerCount);
            OneArgFunctionType funcType = ConvertToOneArgFuncType(word);
            handledCharsCount = wordLength+ innerCount;
            return new OneArgFunctionNode(arg, funcType);
        }

        if(IsTwoArgFunc(word))
        {
            (IExpressionNode firstArg, IExpressionNode secondArg) = GetTwoArgs(expressionStr[wordLength..], out int innerCount);
            TwoArgFunctionType funcType = ConvertToTwoArgFuncType(word);
            handledCharsCount = wordLength + innerCount;
            return new TwoArgFunctionNode(firstArg, secondArg, funcType);
        }

        throw new InvalidOperationException($"Not supported instruction({word}) detected!");
    }

    private bool IsVariable(string word) 
        => _variables.Contains(word);

    private static bool IsOneArgFunc(string word)
        => _oneArgFunctions.Contains(word);

    private static bool IsTwoArgFunc(string word)
        => _twoArgFunctions.Contains(word);

    private static OneArgFunctionType ConvertToOneArgFuncType(string word)
    {
        return word switch
        {
            "abs" => OneArgFunctionType.Abs,
            "sin" => OneArgFunctionType.Sin,
            "cos" => OneArgFunctionType.Cos,
            "tan" => OneArgFunctionType.Tan,
            "int" => OneArgFunctionType.Int,
            "rnd" => OneArgFunctionType.Rnd,
            "exp" => OneArgFunctionType.Exp,
            "ln" => OneArgFunctionType.Ln,
            "sqr" => OneArgFunctionType.Sqr,
            "sqrt" => OneArgFunctionType.Sqrt,
            _ => throw new InvalidOperationException($"Not supported instruction({word}) detected!"),
        };
    }

    private static TwoArgFunctionType ConvertToTwoArgFuncType(string word)
    {
        return word switch
        {
            "min" => TwoArgFunctionType.Min,
            "max" => TwoArgFunctionType.Max,
            "pow" => TwoArgFunctionType.Pow,
            "log" => TwoArgFunctionType.Log,
            "rnd2" => TwoArgFunctionType.Rnd2,
            _ => throw new InvalidOperationException($"Not supported instruction({word}) detected!"),
        };
    }

    private static (IExpressionNode firstArg, IExpressionNode secondArg) GetTwoArgs(ReadOnlySpan<char> readOnlySpan, out int innerCount)
    {
        throw new NotImplementedException();
    }

    private IExpressionNode HandleMinusSymbols(ReadOnlySpan<char> expressionStr, out int handledCharsCount)
    {
        int localCount = 0;
        int modifier = 1;

        while(expressionStr[localCount++] == '-')
            modifier *= -1;
        --localCount;
        IExpressionNode expressionNode = GetNode(expressionStr[localCount..], out int innerCount);
        handledCharsCount = localCount + innerCount;

        return new ArithmeticOperationNode(expressionNode, new ValueNode(modifier), OperationType.Mul);
    }

    private static OperationType GetNextOperationType(char operation)
    {
        return operation switch
        {
            '^' => OperationType.Exp,
            '*' => OperationType.Mul,
            '/' => OperationType.Div,
            '+' => OperationType.Add,
            '-' => OperationType.Sub,
            _ => throw new ArgumentException($"Not valid operation type - '{operation}'"),
        };
    }

    private static IExpressionNode CombineNodesWithPriority(List<(OperationType operation, IExpressionNode value)> nodeWithOperations)
    {
        CombineSelectedOperations(nodeWithOperations, ExpSelector);//возводим в степень
        CombineSelectedOperations(nodeWithOperations, MulDivSelector);//умножаем и делим
        CombineSelectedOperations(nodeWithOperations, AddSubSelector);//вычитаем и прибавляем

        return nodeWithOperations[0].value;

        static bool ExpSelector(OperationType operation) => operation == OperationType.Exp;
        static bool MulDivSelector(OperationType operation) => operation == OperationType.Mul || operation == OperationType.Div;
        static bool AddSubSelector(OperationType operation) => operation == OperationType.Add || operation == OperationType.Sub;

        static void CombineSelectedOperations(List<(OperationType operation, IExpressionNode value)> nodeWithOperations, Func<OperationType, bool> selector)
        {
            for(int i = 0; i < nodeWithOperations.Count; i++)//делим и умножаем
            {
                var (operation, value) = nodeWithOperations[i];
                if(selector(operation))
                {
                    var prevNode = nodeWithOperations[i - 1];
                    nodeWithOperations[i - 1] = (prevNode.operation, new ArithmeticOperationNode(prevNode.value, value, operation));
                    nodeWithOperations.RemoveAt(i--);
                }
            }
        }
    }

    private static void ThrowIfNotValidExpression(string calcStr)
    {
        int brackets = 0;
        foreach(char simbol in calcStr)
            if(simbol == '(')
                brackets++;
            else if(simbol == ')')
                brackets--;

        if(brackets != 0)
            throw new Exception("Невалидное выражение!");
    }
}