using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Blockchain_MyEtherWallet.Models
{
    public class Block
    {
        private int Nonce;
        private readonly DateTime TimeStamp;

        public string PreviousHash { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string Hash { get; private set; }
        

        public Block(DateTime timeStamp, List<Transaction> transactions, string previousHash = "")
        {
            TimeStamp = timeStamp;
            Nonce = 0;
            PreviousHash = previousHash;
            Transactions = transactions;
            Hash = MakeHash();
        }

        public string MakeHash()
        {
            SHA256 sha256 = SHA256.Create();
            string data = PreviousHash + Transactions + TimeStamp + Nonce;
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Encoding.ASCII.GetString(bytes);
        }

        public void MiningBlock(int proofOfWorkDiff)
        {

        }
    }
}