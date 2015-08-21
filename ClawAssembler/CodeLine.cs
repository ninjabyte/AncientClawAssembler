using System;

namespace ClawBinaryCompiler
{
	public class CodeLine
	{
		public string Line { get; private set; }

		public uint Number { get; private set; }

		public bool Processed { get; set; }

		public LineType Type { get; set; }

		public CodeLine(string Line, uint Number)
		{
			this.Line = Line;
			this.Number = Number;
		}

		public enum LineType : byte
		{
			Unknown,
			Empty,
			Preprocessor,
			Instruction,
			Data,
			Label
		}
	}
}

