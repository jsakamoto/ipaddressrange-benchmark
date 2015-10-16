using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetTools;

class Program
{
    static void Main(string[] args)
    {
        var stock = new object[20000];
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ipv4ranges_dummy.txt");
        var ipv4RangeTexts = File.ReadLines(path);
        var index = 0;

        Thread.Sleep(1000);
        var stopwatch = Stopwatch.StartNew();
        foreach (var ipv4RangeText in ipv4RangeTexts)
        {
            stock[index] = IPAddressRange.Parse(ipv4RangeText);
            index++;
        }
        stopwatch.Stop();

        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    }
}
