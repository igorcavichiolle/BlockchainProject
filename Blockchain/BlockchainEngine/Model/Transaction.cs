using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainEngine.Model
{
    public class Transaction
    {
        public Transaction() { }
        public Transaction(string from, string to, string data)
        {
            From = from;
            To = to;
            Data = data;
        }
        public string From { get; set; }
        public string To { get; set; }
        public string Data { get; set; }
    }
}
