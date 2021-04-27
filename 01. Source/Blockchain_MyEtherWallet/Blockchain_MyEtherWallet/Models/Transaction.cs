using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blockchain_MyEtherWallet.Models
{
    public class Transaction
    {
        public string From { get; }
        public string To { get; }
        public int Amount { get; }

        public Transaction(string from, string to, int amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }
    }
}