using System;
using System.Collections.Generic;

namespace ClawAssembler
{
	public static class ConsoleLogHelper
	{
		static ConsoleLogHelper()
		{
		}

		public static void Print(IEnumerable<string> Strings, ErrorLevel Level)
		{

		}

		public static void Print(string Message, ErrorLevel Level)
		{
			Console.WriteLine("{0}: {1}", Level.ToString(), Message);
		}

		public static void Print(CodeError Error)
		{
			Console.WriteLine("{0}: {1} ({2})  at {3} in file {4}", Error.Level.ToString(), Error.Type.ToString(), Error.Remarks, Error.Line.Number, Error.Line.File);
		}
	}
}

