// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using CalcBenchConsole;

var summary = BenchmarkRunner.Run<CalcBenchmarks>();
