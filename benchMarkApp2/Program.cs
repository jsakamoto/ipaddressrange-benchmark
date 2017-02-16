using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using NetTools;

class Program
{
    private static volatile object[] stock = new object[20000];

    static void Main(string[] args)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ipv4ranges_dummy2.txt");
        var rangeTexts = File.ReadLines(path)
            .Select(text => text.Split('-'))
            .Select(parts => Tuple.Create(parts.First(), parts.Last()))
            .ToArray();

        Thread.Sleep(1000);

        var warmingUp = Enumerable.Range(0, 10)
            .Select(_ => new
            {
                Elased_mmsec_Type1 = Mesure(rangeTexts, ParseType1),
                Elased_mmsec_Type2 = Mesure(rangeTexts, ParseType2)
            })
            .ToArray();
        Debug.WriteLine(warmingUp.Count());

        Thread.Sleep(1000);

        var result = Enumerable.Range(0, 100)
            .Select(_ => new
            {
                Elased_mmsec_Type1 = Mesure(rangeTexts, ParseType1),
                Elased_mmsec_Type2 = Mesure(rangeTexts, ParseType2)
            })
            .ToArray();

        Console.WriteLine($"Condition: Parse {rangeTexts.Count():#,0} items x 100 times.");
        Console.WriteLine(
            $"Type 1: " +
            $"Avg.{result.Average(it => it.Elased_mmsec_Type1)} msec / " +
            $"Min.{result.Min(it => it.Elased_mmsec_Type1)} msec / " +
            $"Max.{result.Max(it => it.Elased_mmsec_Type1)} msec");
        Console.WriteLine(
            $"Type 2: " +
            $"Avg.{result.Average(it => it.Elased_mmsec_Type2)} msec / " +
            $"Min.{result.Min(it => it.Elased_mmsec_Type2)} msec / " +
            $"Max.{result.Max(it => it.Elased_mmsec_Type2)} msec");
    }

    public static IPAddressRange ParseType1(string ipFrom, string ipTo)
    {
        return new IPAddressRange(IPAddress.Parse(ipFrom), IPAddress.Parse(ipTo));
    }

    public static IPAddressRange ParseType2(string ipFrom, string ipTo)
    {
        return IPAddressRange.Parse($"{ipFrom} - {ipTo}");
    }

    public static long Mesure(Tuple<string, string>[] rangeTexts, Func<string, string, IPAddressRange> method)
    {
        var index = 0;
        var stopwatch = Stopwatch.StartNew();
        foreach (var rangeText in rangeTexts)
        {
            stock[index] = method(rangeText.Item1, rangeText.Item2);
            index++;
        }
        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }
}
