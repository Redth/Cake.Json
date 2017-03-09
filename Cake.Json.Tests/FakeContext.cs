﻿using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing;

namespace Cake.Json.Tests
{
    public class FakeCakeContext
    {
        ICakeContext context;
        FakeLog log;
        DirectoryPath testsDir;


        public FakeCakeContext ()
        {
            testsDir = new DirectoryPath (
                System.IO.Path.GetFullPath(
                    System.IO.Path.Combine (AppDomain.CurrentDomain.BaseDirectory, "../../")));

            var environment = Cake.Testing.FakeEnvironment.CreateUnixEnvironment (false);

            var fileSystem = new Cake.Testing.FakeFileSystem (environment);
            var globber = new Globber (fileSystem, environment);
            log = new Cake.Testing.FakeLog ();
            var args = new FakeCakeArguments ();
            var processRunner = new ProcessRunner (environment, log);
            var registry = new WindowsRegistry ();
            var configuration = new FakeConfiguration();
            var toolRepository = new ToolRepository (environment);
            var toolResolutionStrategy = new ToolResolutionStrategy (fileSystem, environment, globber, configuration);
            var tools = new ToolLocator(environment, toolRepository, toolResolutionStrategy);
            context = new CakeContext (fileSystem, environment, globber, log, args, processRunner, registry, tools);
            context.Environment.WorkingDirectory = testsDir;
        }

        public DirectoryPath WorkingDirectory {
            get { return testsDir; }
        }

        public ICakeContext CakeContext {
            get { return context; }
        }

        public string GetLogs ()
        {
            return string.Join(Environment.NewLine, log.Entries);
        }

        public void DumpLogs ()
        {
            foreach (var m in log.Entries)
                Console.WriteLine (m);
        }
    }
}
