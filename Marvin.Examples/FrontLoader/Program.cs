using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marvin.Examples.FrontLoader
{
    public class Program
    {

        public static void Main()
        {
            var activities = GetActivities(()=>GetActivities(null));
            var loader = new FrontLoader(activities);
            var planned = loader.Plan();
            Print(planned,0);
        }

        private static void Print(IEnumerable<Activity> activities, int indent)
        {
            if (activities == null)
                return;
            foreach (var activity in activities)
            {
                Print(activity.Predecessors, indent + 1);
                var ind = indent == 0 ? string.Empty : "|" + new string('-', indent * 2 - 1);
                Console.WriteLine(ind + activity.Name + " " + activity.Duration + " " + activity.EarlyStartDate);

            }
        }

        private static IEnumerable<Activity> GetActivities(Func<IEnumerable<Activity>> getPredecessor)
        {
            var rnd = new Random();
            return (from ac in System.Linq.Enumerable.Range(0, 10)
                   let duration = rnd.Next(100)
                   let n = rnd.Next(500)
                   let start = DateTime.Now.Date.AddDays(n - 250)
                   select new Activity("Activity" + n.ToString() + duration, duration, start, getPredecessor == null ? null : getPredecessor())).ToList();
        }
    }
}
