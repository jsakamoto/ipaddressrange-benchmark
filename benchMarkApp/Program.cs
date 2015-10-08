using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        foreach (var ipv4RangeText in ipv4RangeTexts)
        {
            stock[index] = IPAddressRange.Parse(ipv4RangeText);
            index++;
        }
    }
}
