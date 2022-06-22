using BlockchainEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainEngine.Miner
{
    public class TransactionPool
    {
        private List<Transaction> rawTransactionList;

        public TransactionPool()
        {
            rawTransactionList = new List<Transaction>();
        }
        public void AddRaw(string from, string to, string data)
        {
            rawTransactionList.Clear();
            var transaction = new Transaction(from, to, data);
            rawTransactionList.Add(transaction);
        }

        public List<Transaction> TakeAll()
        {
            var all = rawTransactionList.ToList();
            return all;
        }
    }
}
