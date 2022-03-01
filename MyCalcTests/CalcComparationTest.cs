using Calculation;
using NUnit.Framework;
using System;

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
        int round = 0;
        string exprassion = "2*2";
        CompareCalcs(round, exprassion);
    }

    [Test]
    public void BracketsTest()
    { 
        int round = 0;
        string exprassion = "(2+2)*2";
        CompareCalcs(round, exprassion);
    }

    [Test]
    public void PriorityTest()
    {
    }

    [Test]
    public void OneArgFuncTest()
    { 
        int round = 0;
        string exprassion = "int(16.0) + abs(-4)";
        CompareCalcs(round, exprassion);
    }

    private static void CompareCalcs(int round, string exprassion)
    {
        SmartCalculator smartCalc = new(Array.Empty<string>());
        LegacyCalculator legacyCalc = new(round);
        double legacyResult = legacyCalc.Handle(exprassion);
        double smartResult = smartCalc.Execute(exprassion, round);
        Assert.AreEqual(legacyResult, smartResult);
        Assert.Pass();
    }
}