using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marvin.Examples
{
    public class LedgerEntry
    {
        private readonly string _message;
        private readonly decimal _amount;

        public LedgerEntry(string message, decimal amount)
        {
            _message = message;
            _amount = amount;
        }

        public decimal Amount { get { return _amount; } }
        public string Message { get { return _message; } }
    }
}
