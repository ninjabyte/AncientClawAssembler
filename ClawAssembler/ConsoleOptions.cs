using System;
using CommandLine;
using CommandLine.Text;

namespace ClawAssembler
{
	public class ConsoleOptions
	{
		[Value(0, MetaName = "Input Filename", MetaValue = "STRING", HelpText = "Input file path", Required = true)]
		public string InputFilename{ get; set; }

		[Option('o', "output", Default = "out.cex",
			HelpText = "Path to where to output the executable.")]
		public string OutputFilename { get; set; }

		[Option('v', "verbose", Default = false,
			HelpText = "Prints all messages to standard output.")]
		public bool Verbose { get; set; }

		[Option('l', "errorlevel", Default = ErrorLevel.Error)]
		public ErrorLevel ErrorLevel { get; set; }
	}
}

