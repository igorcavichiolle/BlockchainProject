using BlockchainEngine.Miner;
using BlockchainEngine.Model;

TransactionPool transactionPool = new TransactionPool();
BlockMiner blockMiner = new BlockMiner(transactionPool);
blockMiner.start();

//1
transactionPool.AddRaw("igor", "junior", "votou 1");
blockMiner.GenerateBlock();

//2
transactionPool.AddRaw("junior", "pedro", "votou 2");
blockMiner.GenerateBlock();

//3
transactionPool.AddRaw("pedro", "igor", "votou 3");
blockMiner.GenerateBlock();

Console.ReadKey();