using System;
using CommandLine;
using CommandLine.Text;

namespace ClawBinaryCompiler
{
	public class CommandLineOptions
	{
		[Option('v', "verbose", DefaultValue = true,
			HelpText = "Prints all messages to standard output.")]
		public bool Verbose { get; set; }

		[ValueOption(0)]
		string FileName { get; set; }

		public CommandLineOptions()
		{
		}
	}
}

