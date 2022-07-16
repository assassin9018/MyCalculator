using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Calculation;
using System.Text;

namespace CalcBenchConsole;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[RPlotExporter]
public class CalcBenchmarks
{
    [Params("2*-2+2--2", 
        "intg((((13-2)))*100*sin(45)^(1/2)+40)", 
        "2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2",
        "intg(abs(-50*2)*sin(45)*cos(30)+tan(70))+rnd(exp(7))+ln(32)+sqrt(sqr(log(100)))")]
    public string N;
    private readonly ICalculator _legacy = new LegacyCalculator(0);
    private readonly ICalculator _smart = new SmartCalculator(Array.Empty<string>());

    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark(Baseline = true)]
    public double RunLegacy() 
        => _legacy.Execute(N);

    [Benchmark]
    public double RunSmart() 
        => _smart.Execute(N);
}
