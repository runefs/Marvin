using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marvin.Examples
{

 
    public class MoneyTransfer<TSource,TDestination> where TSource : class
        where TDestination : class
    {

        
        public MoneyTransfer(TSource source,TDestination destination, decimal amount){
            Source = source;
            Destination = destination;
            Amount = amount;
        }

        role Source : TSource{
             contract {
               void DecreaseBalance(decimal amount);
               //decimal Balance();
            }
            void Withdraw(decimal amount){
                Console.WriteLine("MoneyTransfer Source Withdraw");
                Source.DecreaseBalance(amount);
            }
            entry void Transfer(decimal amount){
                Console.WriteLine("Source balance is: " );//+ Source.Balance());
                Console.WriteLine("Destination balance is: ");// + Destination.Balance());
                
                Console.WriteLine("Depositing");
                Destination.Deposit(amount);
                Console.WriteLine("Withdrawing");
                Source.Withdraw(amount);
                
                Console.WriteLine("Source balance is now: ");// + Source.Balance());
                Console.WriteLine("Destination balance is now: ");// + Destination.Balance());
            }
        }
       
       role Destination : TDestination
       {
            contract {
               void IncreaseBalance(decimal amount);
               //decimal Balance();
            }

            void Deposit(decimal amount){
                Console.WriteLine("MoneyTransfer Destination deposit");
                Destination.IncreaseBalance(amount);
            }
       }
      
       role Amount : decimal{}
       interaction void Transfer(){
           Source.Transfer(Amount);
       }
    }
}