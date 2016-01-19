﻿using NUnit.Framework;
using System;
using Cake.Core.IO;

namespace Cake.Json.Tests
{
    [TestFixture ()]
    public class JsonTests
    {
        FakeCakeContext context;

        const string SERIALIZED_JSON =  @"{""Name"":""Testing"",""Items"":[""One"",""Two"",""Three""],""KeysAndValues"":{""Key"":""Value"",""AnotherKey"":""AnotherValue"",""Such"":""Wow""},""Nested"":{""Id"":0,""Value"":7.3},""Multiples"":[{""Id"":1,""Value"":14.6},{""Id"":2,""Value"":29.2},{""Id"":3,""Value"":58.4}]}";
        const string SERIALIZED_JSON_INDENTED = @"{
  ""Name"": ""Testing"",
  ""Items"": [
    ""One"",
    ""Two"",
    ""Three""
  ],
  ""KeysAndValues"": {
    ""Key"": ""Value"",
    ""AnotherKey"": ""AnotherValue"",
    ""Such"": ""Wow""
  },
  ""Nested"": {
    ""Id"": 0,
    ""Value"": 7.3
  },
  ""Multiples"": [
    {
      ""Id"": 1,
      ""Value"": 14.6
    },
    {
      ""Id"": 2,
      ""Value"": 29.2
    },
    {
      ""Id"": 3,
      ""Value"": 58.4
    }
  ]
}";

        [SetUp]
        public void Setup ()
        {
            context = new FakeCakeContext ();           
        }

        [TearDown]
        public void Teardown ()
        {
            context.DumpLogs ();
        }

        [Test]
        public void SerializeToString ()
        {
            var obj = new TestObject ();

            var json = context.CakeContext.SerializeJson (obj);

            Assert.IsNotEmpty (json);
            Assert.AreEqual (SERIALIZED_JSON, json);
        }

        [Test]
        public void SerializeToSTringWithNoIndentation()
        {
            var obj = new TestObject();

            var json = context.CakeContext.SerializeJson (obj, Formatting.None);

            Assert.IsNotEmpty (json);
            Assert.AreEqual (SERIALIZED_JSON, json);
        }

        [Test]
        public void SerializeToStringWithIndentation()
        {
            var obj = new TestObject();

            var json = context.CakeContext.SerializeJson (obj, Formatting.Indented);

            Assert.IsNotEmpty (json);
            Assert.AreEqual (SERIALIZED_JSON_INDENTED, json);
        }

        [Test]
        public void SerializeToFile ()
        {
            var obj = new TestObject ();

            var file = new FilePath ("./serialized.json");

            context.CakeContext.SerializeJsonToFile (file, obj);

            var json = System.IO.File.ReadAllText (file.MakeAbsolute (context.CakeContext.Environment).FullPath);

            Assert.IsNotEmpty (json);
            Assert.AreEqual (SERIALIZED_JSON, json);
        }

        [Test]
        public void DeserializeFromFile ()
        {
            var file = new FilePath ("test.json");

            var testObject = context.CakeContext.DeserializeJsonFromFile<TestObject> (file);

            Assert.IsNotNull (testObject);
            Assert.AreEqual ("Testing", testObject.Name);
        }

        [Test]
        public void DeserializeFromString ()
        {
            var testObject = context.CakeContext.DeserializeJson<TestObject> (SERIALIZED_JSON);

            Assert.IsNotNull (testObject);
            Assert.AreEqual ("Testing", testObject.Name);
        }
    }
}

