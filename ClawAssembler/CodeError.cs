using System;

namespace ClawAssembler
{
	public class CodeError
	{
		public ErrorType Type { get; private set; }

		public ErrorLevel Level { get; private set; }

		public CodeLine Line { get; private set; }

		public string Remarks { get; private set; }

		public CodeError(ErrorType Type, ErrorLevel Level, CodeLine Line, string Remarks = "")
		{
			this.Type = Type;
			this.Level = Level;
			this.Line = Line;
			this.Remarks = Remarks;
		}

		public enum ErrorType : byte
		{
			SyntaxError,
			UnknownDatatype,
			DatatypeMissmatch,
			UnknownInstruction
		}
	}
}

