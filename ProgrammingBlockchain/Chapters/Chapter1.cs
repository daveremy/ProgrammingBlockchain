using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;

namespace ProgrammingBlockchain.Chapters
{
    class Chapter1
    {
        public void Lesson1()
        {
            Key key = new Key(); //new private key
            PubKey pubKey = key.PubKey;
            Console.WriteLine("Public Key: {0}", pubKey);
            KeyId hash = pubKey.Hash;
            Console.WriteLine("Hashed Public Key: {0}", hash);
            BitcoinAddress address = pubKey.GetAddress(Network.Main);
            Console.WriteLine("Address: {0}", address);
            Script scriptPubKeyFromAddress = address.ScriptPubKey;
            Console.WriteLine("ScriptPubKey from address: {0}", scriptPubKeyFromAddress);
            Script scriptPubKeyFromHash = hash.ScriptPubKey;
            Console.WriteLine("ScriptPubKey from hash: {0}", scriptPubKeyFromHash);
        }

        public void Lesson2()
        {
            Script scriptPubKey = new Script("OP_DUP OP_HASH160 46859ffeab7acb63417674ab3a8f4a187f386cae OP_EQUALVERIFY OP_CHECKSIG");
            BitcoinAddress address = scriptPubKey.GetDestinationAddress(Network.Main);
            Console.WriteLine("Bitcoin Address: {0}", address);
        }

        public void Lesson3()
        {
            Script scriptPubKey = new Script("OP_DUP OP_HASH160 46859ffeab7acb63417674ab3a8f4a187f386cae OP_EQUALVERIFY OP_CHECKSIG");
            KeyId hash = (KeyId)scriptPubKey.GetDestination();
            Console.WriteLine("Public Key Hash: {0}", hash);
            BitcoinAddress address = new BitcoinAddress(hash, Network.Main);
            Console.WriteLine("Bitcoin Address: {0}", address);
        }
        
        public void Lesson4()
        {
            Key key = new Key();
            BitcoinSecret secret = key.GetBitcoinSecret(Network.Main);
            Console.WriteLine("Bitcoin Secret: {0}", secret);
        }

    }
}
