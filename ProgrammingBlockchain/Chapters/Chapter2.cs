using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;

namespace ProgrammingBlockchain.Chapters
{
    class Chapter2
    {
        public void Lesson1()
        {
            var blockr = new BlockrTransactionRepository();
            Transaction transaction = blockr.Get("d6a57b2ca44327acae03eb408eedab50ea6d4fb304f03aeb5d5251ec37b4baf5");
            // Console.WriteLine(transaction.ToString());
            Console.WriteLine(transaction.Outputs[0].ScriptPubKey.GetDestinationAddress(Network.Main));
        }

    }
}
