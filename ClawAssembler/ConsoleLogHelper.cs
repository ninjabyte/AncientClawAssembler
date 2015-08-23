using System;
using System.Collections.Generic;
using System.IO;

namespace ClawAssembler
{
	public static class ConsoleLogHelper
	{
		private static TextWriter ErrorStream;

		static ConsoleLogHelper()
		{
			ErrorStream = new StreamWriter(Console.OpenStandardError());
		}

		public static void Output(string Message, ErrorLevel Level)
		{
			if (Level >= ErrorLevel.Error)
				ErrorStream.WriteLine("{0}: {1}", Level.ToString(), Message);
			else
				Console.WriteLine("{0}: {1}", Level.ToString(), Message);
		}

		public static void Output(CodeError Error)
		{
			if (Error.Level >= ErrorLevel.Error)
				ErrorStream.WriteLine("{0}: {1} ({2})  at {3} in file {4}", Error.Level.ToString(), Error.Type.ToString(), Error.Remarks, Error.Line.Number, Error.Line.File);
			else
				Console.WriteLine("{0}: {1} ({2})  at {3} in file {4}", Error.Level.ToString(), Error.Type.ToString(), Error.Remarks, Error.Line.Number, Error.Line.File);
		}
	}
}

