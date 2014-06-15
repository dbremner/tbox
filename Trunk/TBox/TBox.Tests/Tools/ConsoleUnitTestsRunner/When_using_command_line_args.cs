﻿using System;
using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts;
using NUnit.Framework;

namespace Mnk.TBox.Tests.Tools.ConsoleUnitTestsRunner
{
    [TestFixture]
    class When_using_command_line_args
    {
        [Test]
        public void Should_save_valid_defaults()
        {
            //Arrange
            var args = new CommandLineArgs();

            //Assert
            Assert.AreEqual(Environment.ProcessorCount, args.TestsInParallel);
            Assert.AreEqual(1, args.AssembliesInParallel);
            Assert.AreEqual(false, args.Prefetch);
            Assert.AreEqual(false, args.Clone);
            CollectionAssert.AreEqual(new[] { "*.dll", "*.config" }, args.CopyMasks);
            Assert.AreEqual(false, args.Sync);
            Assert.AreEqual(string.Empty, args.XmlReport);
            Assert.AreEqual(System.IO.Path.GetTempPath(), args.DirToCloneTests);
            Assert.AreEqual(string.Empty, args.CommandBeforeTestsRun);
            Assert.AreEqual(0, args.StartDelay);
            Assert.AreEqual(true, args.Logo);
            Assert.AreEqual(false, args.Labels);
            Assert.AreEqual(false, args.Wait);
            Assert.AreEqual(false, args.Teamcity);
            CollectionAssert.AreEqual(new List<string>(), args.Paths);
            Assert.AreEqual(TestsRunnerMode.Internal, args.Mode);
            Assert.AreEqual(false, args.ReturnSuccess);
        }

        [Test]
        public void Should_map_valid_defaults()
        {
            //Arrange
            var args = new CommandLineArgs();

            //Act
            var config = args.ToTestsConfig("test.dll");

            //Assert
            Assert.AreEqual("test.dll", config.TestDllPath);
            Assert.AreEqual(Environment.ProcessorCount, config.ProcessCount);
            Assert.AreEqual(false, config.OptimizeOrder);
            Assert.AreEqual(false, config.CopyToSeparateFolders);
            CollectionAssert.AreEqual(new[] { "*.dll", "*.config" }, config.CopyMasks);
            Assert.AreEqual(false, config.NeedSynchronizationForTests);
            Assert.AreEqual(System.IO.Path.GetTempPath(), config.DirToCloneTests);
            Assert.AreEqual(string.Empty, config.CommandBeforeTestsRun);
            Assert.AreEqual(0, config.StartDelay);
            Assert.AreEqual(TestsRunnerMode.Internal, config.Mode);
            Assert.AreEqual(null, config.IncludeCategories);
        }

        [Test]
        public void Should_be_able_to_save_exclude()
        {
            //Arrange
            var args = new CommandLineArgs("sample.dll","/exclude=abc;cde");

            //Assert
            CollectionAssert.AreEqual(new[] { "abc", "cde" }, args.Exclude);
            Assert.AreEqual(null, args.Include);
        }

        [Test]
        public void Should_be_able_to_save_include()
        {
            //Arrange
            var args = new CommandLineArgs("sample.dll", "/include=abc;cde");

            //Assert
            Assert.AreEqual(null, args.Exclude);
            CollectionAssert.AreEqual(new[] { "abc", "cde" }, args.Include);
        }

        [Test]
        public void Should_map_include_to_tests_config()
        {
            //Arrange
            var args = new CommandLineArgs("sample.dll", "/include=abc;cde");

            var config = args.ToTestsConfig("sample.dll");

            //Assert
            Assert.AreEqual(true, config.IncludeCategories);
            CollectionAssert.AreEqual(new[] { "abc", "cde" }, config.Categories);
        }

        [Test]
        public void Should_map_exclude_to_tests_config()
        {
            //Arrange
            var args = new CommandLineArgs("sample.dll", "/exclude=abc;cde");

            var config = args.ToTestsConfig("sample.dll");

            //Assert
            Assert.AreEqual(false, config.IncludeCategories);
            CollectionAssert.AreEqual(new[] { "abc", "cde" }, config.Categories);
        }
    }
}
