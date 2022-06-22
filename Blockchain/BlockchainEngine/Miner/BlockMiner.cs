using BlockchainEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainEngine.Miner
{
    public class BlockMiner
    {
        private readonly TransactionPool transactionPool;

        public List<Block> Blockchain { get; private set; }

        public BlockMiner(TransactionPool transactionPool)
        {
            this.transactionPool = transactionPool;
            Blockchain = new List<Block>();
        }

        public void GenerateBlock(bool isGenesisBlock = false)
        {
            var lastBlock = Blockchain.LastOrDefault();

            var transactionList = transactionPool.TakeAll();

            if (isGenesisBlock)
            {
                transactionList.Add(new Transaction()
                {
                    Data = "",
                    From = "-",
                    To = "Genessis Block"
                });
            }

            var block = new Block()
            {
                TimeStamp = DateTime.Now,
                Nounce = 0,
                TransactionList = transactionList,
                Index = (lastBlock?.Index + 1 ?? 0),
                PrevHash = lastBlock?.Hash ?? string.Empty
            };
            MineBlock(block);
            Blockchain.Add(block);
        }

        public void start()
        {
            GenerateBlock(true);
        }

        private void MineBlock(Block block)
        {
            var merkleRootHash = FindMerkleRootHash(block.TransactionList);
            long nounce = -1;
            var hash = string.Empty;
            do
            {
                nounce++;
                var rowData = block.Index + block.PrevHash + block.TimeStamp.ToString() + nounce + merkleRootHash;
                hash = CalculateHash(CalculateHash(rowData));
            }
            while (!hash.StartsWith("0000"));

            Console.WriteLine("HashMinerado! : " + hash);
            block.Hash = hash;
            block.Nounce = nounce;
        }

        private string FindMerkleRootHash(IList<Transaction> transactionList)
        {
            var transactionStrList = transactionList.Select(tran => CalculateHash(CalculateHash(tran.From + tran.To + tran.Data))).ToList();
            return BuildMerkleRootHash(transactionStrList);
        }

        private string BuildMerkleRootHash(IList<string> merkelLeaves)
        {
            if (merkelLeaves == null || !merkelLeaves.Any())
                return string.Empty;

            if (merkelLeaves.Count() == 1)
                return merkelLeaves.First();

            if (merkelLeaves.Count() % 2 > 0)
                merkelLeaves.Add(merkelLeaves.Last());

            var merkleBranches = new List<string>();

            for (int i = 0; i < merkelLeaves.Count(); i += 2)
            {
                var leafPair = string.Concat(merkelLeaves[i], merkelLeaves[i + 1]);
                merkleBranches.Add(CalculateHash(CalculateHash(leafPair)));
            }
            return BuildMerkleRootHash(merkleBranches);
        }

        private static string CalculateHash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
