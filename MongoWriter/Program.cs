using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoWriter
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            string directory = args[0]; //"/Users/emirozturk/Desktop/WikiMWCA";
            string collectionName = args[1]; //"ZlibMWCA";

            var client = new MongoClient("mongodb://127.0.0.1:27017");
            var list = client.ListDatabaseNames();
            var database = client.GetDatabase("bigData");
            var collection = database.GetCollection<BsonDocument>(collectionName);
            string[] files = Directory.GetFiles(directory);
            sw.Start();
            foreach(string file in files)
            {
                string extension = Path.GetExtension(file);
                if (extension == "") extension = "raw";
                var document = new BsonDocument
                {
                    { "name", Path.GetFileName(file)},
                    { "extension", extension },
                    { "content", File.ReadAllText(file) }
                };
                collection.InsertOne(document);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
