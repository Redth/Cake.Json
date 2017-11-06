﻿using System;
using System.IO;
using Cake.Core.IO;
using Xunit;

namespace Cake.Json.Tests
{
    public class JsonTests : IDisposable
    {
        FakeCakeContext context;

        const string SERIALIZED_JSON =  @"{""Name"":""Testing"",""Items"":[""One"",""Two"",""Three""],""KeysAndValues"":{""Key"":""Value"",""AnotherKey"":""AnotherValue"",""Such"":""Wow""},""Nested"":{""Id"":0,""Value"":7.3},""Multiples"":[{""Id"":1,""Value"":14.6},{""Id"":2,""Value"":29.2},{""Id"":3,""Value"":58.4}]}";

        private readonly string _serializedPrettyJson;

        public JsonTests ()
        {
            context = new FakeCakeContext ();   

            var file = new FilePath ("./serialized_pretty.json");
            _serializedPrettyJson = File.ReadAllText(file.MakeAbsolute(context.CakeContext.Environment).FullPath);
        }

        public void Dispose ()
        {
            context.DumpLogs ();
        }

        [Fact]
        public void SerializeToString ()
        {
            var obj = new TestObject ();

            var json = context.CakeContext.SerializeJson (obj);

            Assert.NotEmpty (json);
            Assert.Equal (SERIALIZED_JSON, json);
        }

        [Fact]
        public void SerializeToPrettyString ()
        {
            var obj = new TestObject ();

            var json = context.CakeContext.SerializeJsonPretty (obj);

            Assert.NotEmpty (json);
            Assert.Equal (_serializedPrettyJson, json);
        }

        [Fact]
        public void SerializeToFile ()
        {
            var obj = new TestObject ();

            var file = new FilePath ("./serialized.json");

            context.CakeContext.SerializeJsonToFile (file, obj);

            var json = File.ReadAllText (file.MakeAbsolute (context.CakeContext.Environment).FullPath);

            Assert.NotEmpty (json);
            Assert.Equal (SERIALIZED_JSON, json);
        }

        [Fact]
        public void SerializeToPrettyFile ()
        {
            var obj = new TestObject ();

            var file = new FilePath ("./serialized_pretty.json");

            context.CakeContext.SerializeJsonToPrettyFile (file, obj);

            var json = File.ReadAllText (file.MakeAbsolute (context.CakeContext.Environment).FullPath);

            Assert.NotEmpty (json);
            Assert.Equal (_serializedPrettyJson, json);
        }

        [Fact]
        public void DeserializeFromFile ()
        {
            var file = new FilePath ("test.json");

            var testObject = context.CakeContext.DeserializeJsonFromFile<TestObject> (file);

            Assert.NotNull (testObject);
            Assert.Equal ("Testing", testObject.Name);
        }

        [Fact]
        public void DeserializeFromString ()
        {
            var testObject = context.CakeContext.DeserializeJson<TestObject> (SERIALIZED_JSON);

            Assert.NotNull (testObject);
            Assert.Equal ("Testing", testObject.Name);
        }

        [Fact]
        public void ParseFromString ()
        {
            var testObject = context.CakeContext.ParseJson(SERIALIZED_JSON);

            Assert.NotNull (testObject);
            Assert.Equal ("Testing", testObject.Value<string> ("Name"));
        }

        [Fact]
        public void ParseFromFile()
        {
            var testObject = context.CakeContext.ParseJsonFromFile("test.json");

            Assert.NotNull(testObject);
            Assert.Equal ("Testing", testObject.Value<string> ("Name"));
        }
    }
}

