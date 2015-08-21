using System;

namespace ClawBinaryCompiler
{
	public class CodeLine
	{
		public string Content { get; private set; }

		public uint Number { get; private set; }

		public bool Processed { get; set; }

		public LineType Type { get; set; }

		public CodeLine(string Content, uint Number, string File)
		{
			this.Content = Content;
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

