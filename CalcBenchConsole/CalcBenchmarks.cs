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
    public double RunLegacy()
    {
        double sum = 0;
        var calc = new LegacyCalculator(0);
        for(int i = 0; i < 1001; i++)
            sum += calc.Handle(N);

        return sum;
    }

    [Benchmark]
    public double RunSmart()
    {
        double sum = 0;
        var calc = new SmartCalculator(Array.Empty<string>());
        for(int i = 0; i < 1001; i++)
            sum += calc.Execute(N, 0, new());

        return sum;
    }
}
