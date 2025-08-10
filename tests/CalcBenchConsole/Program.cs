// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using CalcBenchConsole;
using Calculation.Legacy;
using Calculation.Smart;
using NFun;

string[] values = [
    "2*-2+2+2+2+2+2+2+2-2-2-2-2-2*2*2*2*2*22-110012154+123456789",
        "1",
        "2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2+2",
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100+" +
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100+" +
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100+" +
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100+" +
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100+" +
        "abs(abs(-50*2)*sin(45)*100*cos(30)*100+tan(70))*100+sin(exp(7))*100+log(32)*100+sqrt(2*(log(100)))*100"
    ];

foreach ( var value in values )
{
    _ = new LegacyCalculator(6).Execute(value);
    _ = new SmartCalculator().Execute(value);
    if(value == "1")
    {
        _ = Funny.Calc<double>("abs(((((((((((((13-2))))))))))))*100*sin(45)**(1/2/2/2/2/2/2)+40)");
    }
    else
    {
        _ = Funny.Calc<double>(value);
    }
}

_ = BenchmarkRunner.Run<CalcBenchmarks>();
