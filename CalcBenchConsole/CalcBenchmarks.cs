using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NFun;
using Calculation.Legacy;
using Calculation.Smart;

namespace CalcBenchConsole;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
[RPlotExporter]
public class CalcBenchmarks
{
    private const string PriorityCase = PriorityCaseExp;
    private const string PriorityCaseExp = "2+2*2^3*2+2^1";
    private const string PriorityCaseExpNfan = "2+2*2**3*2+2**1";

    [Params(
        "10",
        "1+1+1",
        PriorityCase,
        "2*-2+2+2+2+2+2+2+2-2-2-2-2-2*2*2*2*2*22-110012154+123456789",
        "2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2",
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100+" +
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100+" +
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100+" +
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100+" +
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100+" +
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100")]
    public string N;

    private static readonly LegacyCalculator _legacy = new LegacyCalculator(5);
    private static readonly SmartCalculator _smart = new SmartCalculator(Array.Empty<string>());

    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark(Baseline = true)]
    public double Legacy()
    {
        var exp = N == PriorityCase ? PriorityCaseExp : N;
        return _legacy.Execute(exp);
    }

    [Benchmark]
    public double Smart()
    {
        var exp = N == PriorityCase ? PriorityCaseExp : N;
        return _smart.Execute(exp);
    }

    [Benchmark]
    public double Nfan()
    {
        var exp = N == PriorityCase ? PriorityCaseExpNfan : N;
        return Funny.Calc<double>(exp);
    }
}
