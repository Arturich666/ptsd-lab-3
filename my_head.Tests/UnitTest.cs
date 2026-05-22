using NUnit.Framework;
using System;
using System.IO;

namespace my_head.Tests
{
    [TestFixture]
    public class AppTests
    {
        [Test]
        public void Run_StdinWithN_OutputsFirstNLines_Returns0()
        {
            var input = new StringReader("line1\nline2\nline3\nline4\n");
            var output = new StringWriter();
            var error = new StringWriter();

            int exitCode = App.Run(new[] { "-n", "2" }, input, output, error);

            Assert.That(exitCode, Is.EqualTo(0));
            Assert.That(output.ToString(), Is.EqualTo($"line1{Environment.NewLine}line2{Environment.NewLine}"));
            Assert.That(error.ToString(), Is.Empty);
        }

        [Test]
        public void Run_InvalidOption_ReturnsCode2()
        {
            var input = new StringReader("");
            var output = new StringWriter();
            var error = new StringWriter();

            int exitCode = App.Run(new[] { "--unknown-option" }, input, output, error);

            Assert.That(exitCode, Is.EqualTo(2));
            Assert.That(output.ToString(), Is.Empty);
            Assert.That(error.ToString(), Does.Contain("unknown").IgnoreCase);
        }

        [Test]
        public void Run_MissingNValue_ReturnsCode2()
        {
            var input = new StringReader("");
            var output = new StringWriter();
            var error = new StringWriter();

            int exitCode = App.Run(new[] { "-n" }, input, output, error);

            Assert.That(exitCode, Is.EqualTo(2));
            Assert.That(error.ToString(), Does.Contain("invalid number").IgnoreCase);
        }

        [Test]
        public void Run_ValidFile_OutputsLines_Returns0()
        {
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "a\nb\nc\n");

            var input = new StringReader("");
            var output = new StringWriter();
            var error = new StringWriter();

            int exitCode = App.Run(new[] { "-n", "1", tempFile }, input, output, error);

            Assert.That(exitCode, Is.EqualTo(0));
            Assert.That(output.ToString(), Is.EqualTo($"a{Environment.NewLine}"));
            Assert.That(error.ToString(), Is.Empty);

            File.Delete(tempFile);
        }

        [Test]
        public void Run_FileNotFound_ReturnsCode1()
        {
            var input = new StringReader("");
            var output = new StringWriter();
            var error = new StringWriter();

            int exitCode = App.Run(new[] { "nonexistent_file.txt" }, input, output, error);

            Assert.That(exitCode, Is.EqualTo(1));
            Assert.That(output.ToString(), Is.Empty);
            Assert.That(error.ToString(), Does.Contain("No such file").IgnoreCase);
        }
    }
}