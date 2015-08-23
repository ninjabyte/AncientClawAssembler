using System;
using System.Collections.Generic;

namespace ClawAssembler
{
	public class ParserResult
	{
		public ClawToken[] Tokens { get; private set; }

		public CodeError[] Errors { get; private set; }

		public ParserResult(ClawToken[] Tokens, CodeLine[] Lines, CodeError[] CodeErrors)
		{
			var errors = new List<CodeError>();

			foreach (CodeLine line in Lines) {
				if (line.Type == CodeLine.LineType.Unknown)
					errors.Add(new CodeError(CodeError.ErrorType.SyntaxError, ErrorLevel.Warning, line));
			}
			errors.AddRange(CodeErrors);
			this.Errors = errors.ToArray();
			this.Tokens = Tokens;
		}
	}
}

