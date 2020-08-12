using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzBus;

namespace DiceRoller.Statistics
{
    public class DiceRolledHandler : IHandle<DiceRolled>
    {
        private static readonly DiceStatistics statistics = new DiceStatistics();

        public Task Handle(DiceRolled message)
        {
            statistics.AddRoll(message.Result);
            ClearLastLine();
            Console.WriteLine(statistics);
            return Task.CompletedTask;
        }

        public static void ClearLastLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }

    public class DiceStatistics
    {
        private readonly Dictionary<int, int> results = new Dictionary<int, int>();

        public void AddRoll(int result)
        {
            if (!results.ContainsKey(result))
            {
                results.Add(result, 0);
            }

            results[result]++;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var item in results.OrderBy(x => x.Key))
            {
                sb.Append($"[{item.Key}:{item.Value}] ");
            }

            sb.Append($"Rolls: {results.Sum(x => x.Value)}");

            return sb.ToString();
        }
    }
}