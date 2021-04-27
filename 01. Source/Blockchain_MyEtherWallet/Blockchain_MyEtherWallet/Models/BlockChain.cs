﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blockchain_MyEtherWallet.Models
{
    public class BlockChain
    {
        private readonly int Reward;
        private readonly int ProofOfWorkDiff;
        private List<Transaction> PendingTransactions;

        public List<Block> BChain { get; set; }

        public BlockChain(int reward, int proofOfWorkDiff)
        { 
            Reward = reward;
            ProofOfWorkDiff = proofOfWorkDiff;
            PendingTransactions = new List<Transaction>();
            BChain = new List<Block> { GenerateGenesisBlock() };
        }

        private Block GenerateGenesisBlock()
        {
            List<Transaction> transactions = new List<Transaction> { new Transaction("", "", 0) };
            return new Block(DateTime.Now, transactions, "0");
        }

        public bool IsValidChain()
        {
            for (int i = 1; i < BChain.Count; i++)
            {
                Block prevBlock = BChain[i - 1];
                Block currBlock = BChain[i];
                if (currBlock.Hash != currBlock.MakeHash())
                    return false;
                if (currBlock.PreviousHash != prevBlock.Hash)
                    return false;
            }
            return true;
        }

        public void NewTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        public double GetBalance(string minerAddress)
        {
            double balance = 0;
            foreach (Block block in BChain)
            {
                foreach (Transaction transaction in block.Transactions)
                {
                    if (transaction.From == minerAddress)
                    {
                        balance -= transaction.Amount;
                    }
                    if (transaction.To == minerAddress)
                    {
                        balance += transaction.Amount;
                    }
                }
            }
            return balance;
        }

        public void MineBlock(string minerAddress)
        {
            Transaction minerRewardTransaction = new Transaction(null, minerAddress, Reward);
            PendingTransactions.Add(minerRewardTransaction);
            Block block = new Block(DateTime.Now, PendingTransactions);
            block.MiningBlock(ProofOfWorkDiff);
            block.PreviousHash = BChain.Last().Hash;
            BChain.Add(block);
            PendingTransactions = new List<Transaction>();
        }
    }
}