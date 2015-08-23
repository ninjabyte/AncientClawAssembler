using System;
using CommandLine;
using CommandLine.Text;

namespace ClawAssembler
{
	public class ConsoleOptions
	{
		[Value(0, MetaName = "Input Filename", Required = true,
			HelpText = "Path to the file to process.")]
		public string InputFilename{ get; set; }

		[Option('o', "output", Default = "out.cex",
			HelpText = "Path to where to output the executable.")]
		public string OutputFilename { get; set; }

		[Option('v', "verbose", Default = false,
			HelpText = "Prints info messages to standard output.")]
		public bool Verbose { get; set; }

		[Option('l', "errorlevel", Default = ErrorLevel.Error,
			HelpText = "Minimum error level to stop compiling.")]
		public ErrorLevel ErrorLevel { get; set; }
	}
}

