using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Task_14_2
{
    internal static class OwnSerialization // different generic serialization methods
    {
        #region Own BINARY serialization (ISerializable interface)
        static public void OwnBinarySerializationTest(string binarySerializationFileName)
        {
            try
            {
                SerializableStorage serialStorage;
                using (StreamReader reader = new StreamReader("../../../Database.txt"))
                    serialStorage = new SerializableStorage(reader); // reading from the file again

                Console.WriteLine("\r\nOwn binary serialization of the storage. See " + binarySerializationFileName);
                IFormatter formatter = new BinaryFormatter(); // It is dangerous for use but we want just to try it

                BinarySerialize<SerializableStorage>(serialStorage, binarySerializationFileName, formatter);
                // works as well so: BinarySerialize(serialStorage, binarySerializationFileName, formatter);

                Console.WriteLine("\r\nOwn binary deserialization of the storage.");
                SerializableStorage deserializedSerialStorage =
                    BinaryDeserialize<SerializableStorage>(binarySerializationFileName, formatter);
                Console.WriteLine("The list of products in the deserialized storage:");
                Console.WriteLine(deserializedSerialStorage);
            }
            catch (SerializationException serExc)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Serialization failed: " + serExc.Message);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        public static void BinarySerialize<T>(T whatToWrite, string fileName, IFormatter formatter)
        // Serialization of Storage using Binary or Soap formatter
        // The compiler warns: "IFormatter.Serialize(Stream, object)" is obsolete: 'BinaryFormatter serialization
        // is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.'
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
                formatter.Serialize(stream, whatToWrite);
        }

        public static T BinaryDeserialize<T>(string fileName, IFormatter formatter)
        // Deserialization of Storage using Binary or Soap formatter
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
                return (T)formatter.Deserialize(stream);
        }

        #endregion

        #region Own XML serialization (IXMLSerializable interface)
        static public void OwnXMLSerializationTest(string xmlSerializationFileName)
        {
            try
            {
                SerializableStorage serialStorage;
                using (StreamReader reader = new StreamReader("../../../Database.txt"))
                    serialStorage = new SerializableStorage(reader); // reading from the file again

                Console.WriteLine("\r\nOwn XML serialization of the storage. See " + xmlSerializationFileName);
                XMLSerialize<SerializableStorage>(serialStorage, xmlSerializationFileName);
                // works as well so: BinarySerialize(serialStorage, binarySerializationFileName, formatter);

                Console.WriteLine("\r\nOwn XML deserialization of the storage.");
                SerializableStorage deserializedSerialStorage =
                    XMLDeserialize<SerializableStorage>(xmlSerializationFileName);
                Console.WriteLine("The list of products in the deserialized storage:");
                Console.WriteLine(deserializedSerialStorage);
            }
            catch (SerializationException serExc)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Serialization failed: " + serExc.Message);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        public static void XMLSerialize<T>(T whatToWrite, string fileName)
        // Serialization of Storage using XMLSerializer interface
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                XmlSerializer writer = new(typeof(T));
                writer.Serialize(stream, whatToWrite);
            }
        }

        public static T XMLDeserialize<T>(string fileName)
        // Deserialization of Storage using XMLSerializer interface
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                XmlSerializer reader = new(typeof(T));
                return (T)reader.Deserialize(stream);
            }
        }
        #endregion
    }
}
