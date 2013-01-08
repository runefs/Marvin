using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marvin.Examples
{
    
   public class Account<T> where T : ICollection<LedgerEntry>{
        public Account(T ledgers){
            Ledgers = ledgers;
        }

        role Ledgers : T {
           contract {
              void Add(LedgerEntry item);
          }

            entry void AddEntry(string message,decimal amount){
                Console.WriteLine("Account ledgers AddEntry");
                Ledgers.Add(new LedgerEntry(message, amount));
            }
            entry decimal GetBalance(){
                Console.WriteLine("Account ledgers GetBalance");
                return ((ICollection<LedgerEntry>)Ledgers).Sum(e => e.Amount);
            }
           
        }

      public decimal Balance(){
           Console.WriteLine("Account Balance");
              return Ledgers.GetBalance();
      }

      interaction void IncreaseBalance(decimal amount)
      {
          Console.WriteLine("Account IncreaseBalance");
          Ledgers.AddEntry("depositing",amount);
      }

      interaction void DecreaseBalance(decimal amount)
      {
          Console.WriteLine("Account DecreaseBalance");
          Ledgers.AddEntry("withdrawing",0-amount);
      }
    }
}
