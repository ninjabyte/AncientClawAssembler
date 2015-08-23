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
					return 1;
				});
			
			return exitCode;
		}

		static int Execute(ConsoleOptions Options)
		{
			try {
				bool abort = false;
				PreprocessorResult preprocessorResult = Parser.Preprocess(Options.InputFilename);

				foreach (CodeError error in preprocessorResult.Errors) {
					ConsoleLogHelper.Output(error);
					abort = (abort || error.Level >= Options.ErrorLevel);
				}

				if (abort) {
					ConsoleLogHelper.Output("Preprocessing failed! Aborting!", ErrorLevel.Critical);
					return 1;
				}

				ParserResult parserResult = Parser.Parse(preprocessorResult.CodeLines);

				foreach (CodeError error in parserResult.Errors) {
					ConsoleLogHelper.Output(error);
					abort = (abort || error.Level >= Options.ErrorLevel);
				}

				if (abort) {
					ConsoleLogHelper.Output("Parsing failed! Aborting!", ErrorLevel.Critical);
					return 1;
				}

				byte[] bytes = Compiler.Compile(parserResult.Tokens);
				File.WriteAllBytes(Options.OutputFilename, bytes);

				return 0;
			} catch (Exception ex) {
				ConsoleLogHelper.Output("Unhandeled error (please report at https://github.com/microcat-dev/ClawAssembler/issues)!" + ex.Message, ErrorLevel.Critical);
				return 1;
			}
		}
	}
}