using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Calculation;

namespace CalcBenchConsole;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[RPlotExporter]
public class CalcBenchmarks
{
    [Params("2*-2+2--2", "int((((13-2)))*100*sin(45)^(1/2)+40)", "2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2")]
    public string N;

    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark(Baseline = true)]
    public void RunLegacy()
    {
        var calc = new LegacyCalculator(0);
        for(int i = 0; i < 1001; i++)
            calc.Handle(N);
    }

    [Benchmark]
    public void RunSmart()
    {
        var calc = new SmartCalculator(Array.Empty<string>());
        for(int i = 0; i < 1001; i++)
            calc.Execute(N, 0);
    }
}
