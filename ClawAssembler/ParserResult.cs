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
			this.Errors = CodeErrors;
			this.Tokens = Tokens;
		}
	}
}

