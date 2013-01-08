using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marvin.Examples
{
    public class Program
    {

        public static void Main()
        {
            try
            {
                System.Diagnostics.Debugger.Launch();
                var list = new List<LedgerEntry>();
                list.Add(new LedgerEntry("start", 0m));
                list.Add(new LedgerEntry("first deposit", 1000m));
                var source = new Account<List<LedgerEntry>>(list);
                var destination = new Account<List<LedgerEntry>>(new List<LedgerEntry>());
                var context = new MoneyTransfer<Account<List<LedgerEntry>>, Account<List<LedgerEntry>>>(source, destination, 245m);
                context.Transfer();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }
        }
    }
}
