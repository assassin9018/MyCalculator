using Calculation;
using Calculation.Legacy;
using Calculation.Smart;
using NUnit.Framework;
using System;
using System.Linq;

namespace MyCalcTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void MulTest()
    {
        string exprassion = "2*2";
        CompareCalcs(exprassion);
        Assert.Pass();
    }

    [Test]
    public void BracketsTest()
    {
        string exprassion = "(2+2)*2";
        CompareCalcs(exprassion);
        Assert.Pass();
    }

    [Test]
    public void ArithmeticPriorityTest()
    {
        string exprassion = "2+2*2";
        CompareCalcs(exprassion);
        Assert.Pass();
    }

    [Test]
    public void FunctionPriorityTest()
    {
        string exprassion = "2+2*log(100)^2";
        CompareCalcs(exprassion);
        Assert.Pass();
    }

    [Test]
    public void OneArgFuncTest()
    {
        int round = 5;
        var names = OneArgFunctionTypeExtensions.GetNames();
        foreach(string func in names)
            CompareCalcs(func + "(1)", round);
        Assert.Pass();
    }

    [Test]
    public void IgnoreCaseTest()
    {
        int round = 5;
        var names = OneArgFunctionTypeExtensions.GetNames();

        foreach(string func in names.Select(x=>x.ToUpper()))
            CompareCalcs(func + "(1)", round);

        foreach(string func in names.Select(x => x.ToLower()))
            CompareCalcs(func + "(1)", round);

        Assert.Pass();
    }

    private static void CompareCalcs(string exprassion, int round = 0)
    {
        SmartCalculator smartCalc = new(Array.Empty<string>());
        LegacyCalculator legacyCalc = new(round);
        double legacyResult = legacyCalc.Execute(exprassion);
        double smartResult = smartCalc.Execute(exprassion, round, new());
        Assert.AreEqual(legacyResult, smartResult);
    }
}