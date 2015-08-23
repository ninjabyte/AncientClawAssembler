using System;
using System.IO;
using CommandLine;

namespace ClawAssembler
{
	public static class ConsoleInterface
	{
		public static int Run(string[] args)
		{
			ParserResult<ConsoleOptions> result = CommandLine.Parser.Default.ParseArguments<ConsoleOptions>(args);

			int exitCode = result
				.MapResult(
				               options => {
					return Execute(options);
				},
				               errors => {
					//ConsoleLogHelper.Print(errors.ToString, ErrorLevel.Critical);
					return 1;
				});

			return exitCode;
		}

		static int Execute(ConsoleOptions Options)
		{
			try {
				CodeLine[] lines = Parser.PreProcess(Options.InputFilename);
				ParserResult result = Parser.Parse(lines);
				foreach (CodeError error in result.Errors) {
					ConsoleLogHelper.Output(error);
					
					if (error.Level >= Options.ErrorLevel) {
						ConsoleLogHelper.Output("Aborting assemblying!", ErrorLevel.Critical);
						return 1;
					}
				}

				byte[] bytes = Compiler.Compile(result.Tokens);
				File.WriteAllBytes(Options.OutputFilename, bytes);

				return 0;
			} catch (Exception ex) {
				ConsoleLogHelper.Output(ex.Message, ErrorLevel.Critical);
				return 1;
			}
		}
	}
}