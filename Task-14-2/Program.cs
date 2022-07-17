using System;

namespace Task_14_2
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("This is the example of DataContract serialization.");
            Serialization.DataContractSerializationTest("../../../Storage.DataContractSerialized.xml");
            
            Console.WriteLine("\r\nThis is the example of Binary serialization (not recommended to use).");
            Serialization.BinarySerializationTest("../../../Storage.BinarySerialized.bin");

            Console.WriteLine("\r\nThis is the example of Own binary serialization (ISerialized).");
            OwnSerialization.OwnBinarySerializationTest("../../../Storage.OwnBinarySerialized.bin");

            Console.WriteLine("\r\nThis is the example of Own XML serialization (IXMLSerialized).");
            OwnSerialization.OwnXMLSerializationTest("../../../Storage.OwnXMLSerialized.xml");

            Console.WriteLine("\r\nDone.");
        }
    }
}