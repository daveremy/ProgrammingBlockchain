using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using ProgrammingBlockchain.Chapters;
using NBitcoin;
using NBitcoin.Protocol;

namespace ProgrammingBlockchain
{
    class Program
    {
        static void Main(string[] args)
        {
            BitcoinSecret paymentSecret = new BitcoinSecret("L51eDMcsRUFSReQpz587UgcBSXGgskc6ssUGibXXHcYK6aMsAVzr");
            BitcoinAddress address = paymentSecret.GetAddress();
            Console.WriteLine("Address: {0}", address);
            var blockr = new BlockrTransactionRepository();
            Transaction fundingTransaction = blockr.Get("2ab7d787b02ebd40f5de87aa27569b118ff1cc801088fbf7bab3eec304011878");

            Pay(paymentSecret, new BitcoinAddress("1KF8kUVHK42XzgcmJF4Lxz4wcL5WDL97PB"), Money.Coins(0.004m), fundingTransaction);

            //Select the chapter here:
            //var chapter = new Chapter2();

            ////call the lesson here
            //chapter.Lesson1();

            //hold open the output window
            Console.WriteLine("\n\n\nPress enter to continue");
            Console.ReadLine();
        }

        static public void Pay(BitcoinSecret secret, BitcoinAddress toAddress, Money amount, Transaction fundingTransaction)
        {
            var fee = Money.Coins(0.0001m);

            Transaction payment = new Transaction();
            payment.Inputs.Add(new TxIn() 
                {
                    PrevOut = new OutPoint(fundingTransaction.GetHash(), 1)
                });

            payment.Outputs.Add(new TxOut()
                {
                    Value = amount,
                    ScriptPubKey = toAddress.ScriptPubKey
                });

            var output = fundingTransaction.Outputs[0];
            var change = output.Value - amount - fee;
            if (change < 0)
            {
                Console.WriteLine("There is not enough BTC in the funding transaction ({0}) to make this payment ({1})", output.Value, amount);
                Console.WriteLine("No payment being sent");
                return;
            }            

            payment.Outputs.Add(new TxOut()
                {
                    Value = output.Value - amount - fee,
                    ScriptPubKey = output.ScriptPubKey
                });

            //Feedback !
            var message = "Thanks ! :)";
            var bytes = Encoding.UTF8.GetBytes(message);
            payment.Outputs.Add(new TxOut()
                {
                    Value = Money.Zero,
                    ScriptPubKey = TxNullDataTemplate.Instance.GenerateScriptPubKey(bytes)
                });

            Console.WriteLine(payment);

            payment.Inputs[0].ScriptSig = fundingTransaction.Outputs[1].ScriptPubKey;
            payment.Sign(secret, false);

            using (var node = Node.Connect(Network.Main))
            {
                Console.WriteLine("Doing version handshake");
                node.VersionHandshake();
                Console.WriteLine("Sending message");
                node.SendMessage(new InvPayload(InventoryType.MSG_TX, payment.GetHash()));
                node.SendMessage(new TxPayload(payment));
                Thread.Sleep(500);
            }
        }
    }
}