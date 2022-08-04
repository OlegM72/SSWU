using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Task_14_2
{
    internal static class Serialization // different generic serialization methods
    {
        #region DATA CONTRACT serialization

        static public void DataContractSerializationTest(string dataContractFileName)
        {
            try
            {
                Storage storage;
                using (StreamReader reader = new StreamReader("../../../Database.txt"))
                    storage = new Storage(reader); // reading from the file
                if (storage.GetCount() == 0)
                    Console.WriteLine("The list of products in file 1 is empty.");
                else
                {
                    Console.WriteLine("The list of products from the file:");
                    Console.WriteLine(storage);
                }

                Console.WriteLine("\r\nDataContract serialization of the storage. See " + dataContractFileName);
                WriteDataContractObject<Storage>(storage, dataContractFileName);
                // works as well so: WriteDataContractObject(storage, dataContractFileName);

                Console.WriteLine("\r\nDataContract deserialization of the storage.");
                Storage deserializedStorage =
                    ReadDataContractObject<Storage>(dataContractFileName);
                Console.WriteLine("The list of products in the deserialized storage:");
                Console.WriteLine(deserializedStorage);
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

        public static void WriteDataContractObject<T>(T whatToWrite, string fileName) 
            // DataContract (XML) Serialization of an object with type T
        {
            using (FileStream stream = new(fileName, FileMode.Create))
            {
                DataContractSerializer serializer = new(typeof(T));
                serializer.WriteObject(stream, whatToWrite);
            }
        }

        public static T ReadDataContractObject<T>(string fileName)
            // DataContract (XML) Deserialization: returns an object of type T
        {
            using (FileStream fileReader = new FileStream(fileName, FileMode.Open))
            using (XmlDictionaryReader XMLreader = 
                XmlDictionaryReader.CreateTextReader(fileReader, new XmlDictionaryReaderQuotas()))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(XMLreader, true);
            }
        }

        #endregion 

        #region BINARY serialization
        // It is dangerous for use but we want just to try it

        static public void BinarySerializationTest(string binarySerializationFileName)
        {
            try
            {
                Storage storage;
                using (StreamReader reader = new StreamReader("../../../Database.txt"))
                    storage = new Storage(reader); // reading from the file again

                Console.WriteLine("\r\nBinary serialization of the storage. See " + binarySerializationFileName);
                IFormatter formatter = new BinaryFormatter();

                BinarySerialize<Storage>(storage, binarySerializationFileName, formatter);
                // works as well so: BinarySerialize(storage, binarySerializationFileName, formatter);

                Console.WriteLine("\r\nBinary deserialization of the storage.");
                Storage deserializedStorage =
                    BinaryDeserialize<Storage>(binarySerializationFileName, formatter);
                Console.WriteLine("The list of products in the deserialized storage:");
                Console.WriteLine(deserializedStorage);
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

        #region JSON serialization

        static public void JSONDataContractSerializationTest(string jsonDataContractFileName)
        {
            try
            {
                Storage storage;
                using (StreamReader reader = new StreamReader("../../../Database.txt"))
                    storage = new Storage(reader); // reading from the file
                if (storage.GetCount() == 0)
                    Console.WriteLine("The list of products in file 1 is empty.");
                else
                {
                    Console.WriteLine("The list of products from the file:");
                    Console.WriteLine(storage);
                }

                Console.WriteLine("\r\nJSON DataContract serialization of the storage. See " + jsonDataContractFileName);
                WriteJSONDataContractObject<Storage>(storage, jsonDataContractFileName);
                // works as well so: WriteDataContractObject(storage, dataContractFileName);

                Console.WriteLine("\r\nDataContract deserialization of the storage.");
                Storage deserializedStorage =
                    ReadJSONDataContractObject<Storage>(jsonDataContractFileName);
                Console.WriteLine("The list of products in the deserialized storage:");
                Console.WriteLine(deserializedStorage);
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

        public static void WriteJSONDataContractObject<T>(T whatToWrite, string fileName)
        // DataContract (JSON) Serialization of an object with type T
        {
            using (FileStream stream = new(fileName, FileMode.Create))
            {
                DataContractJsonSerializer serializer = new(typeof(T));
                serializer.WriteObject(stream, whatToWrite);
            }
        }

        public static T ReadJSONDataContractObject<T>(string fileName)
        // DataContract (JSON) Deserialization: returns an object of type T
        {
            using (FileStream fileReader = new FileStream(fileName, FileMode.Open))
            {
                DataContractJsonSerializer deserializer = new(typeof(T));
                return (T)deserializer.ReadObject(fileReader);
            }
        }
        #endregion
    }
}
