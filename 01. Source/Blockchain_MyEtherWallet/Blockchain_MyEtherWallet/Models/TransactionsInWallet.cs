using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blockchain_MyEtherWallet.Models
{
    public class TransactionsInWallet
    {
        public string WalletName { get; set; }
        public string WalletBalance { get; set; }

        public TransactionsInWallet(string walletName, string walletBalance)
        {
            WalletName = walletName;
            WalletBalance = walletBalance;
        }
    }
}