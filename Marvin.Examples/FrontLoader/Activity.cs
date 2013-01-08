using System;
using System.Collections.Generic;
using System.Linq;

namespace Marvin.Examples.FrontLoader
{
   internal class Activity{
       
       role duration{
            DateTime GetEndDate()
            {
               if(!Planned){
                    Plan();
                }
                return _earlyEndDate = EarlyStartDate + self;
            }
        }

        role predecessors{
            DateTime GetStartDate(){
                var list = ((IEnumerable<Activity>) self);
                if(list == null || !list.Any())
                    return DateTime.Now.Date;
                return list.Select(p => p.EarlyEndDate).Max();
            }
        }
     
       interaction void  Plan(){
            if(_planned) return;
            Console.WriteLine("Planning: " + Name);
            _earlyStartDate = predecessors.GetStartDate();
            _planned = true;
        }

       public Activity(string name, int duration, DateTime earlyStartDate, IEnumerable<Activity> predecessors){
           _earlyStartDate = earlyStartDate;
           _name = name;
           this.duration = duration;
           this.predecessors = predecessors ?? Enumerable.Empty<Activity>();
       }

       private bool _planned;
       private DateTime _earlyStartDate;
       private DateTime _earlyEndDate;
       private readonly string _name;
       public string Name{get { return _name; }}
        

        public DateTime EarlyEndDate{
            get{
                return _earlyEndDate;
            }
        }
        
        public DateTime EarlyStartDate{
            get{
                return _earlyStartDate;
            }
        }

        public bool Planned{
            get { return _planned; }
        }

       public int Duration{
            get { return duration; }
        }

       public IEnumerable<Activity> Predecessors{get { return predecessors; }}
        
    }
}
