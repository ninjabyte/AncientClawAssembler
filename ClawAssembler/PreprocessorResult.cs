using System;

namespace ClawAssembler
{
	public class PreprocessorResult
	{
		public CodeLine[] CodeLines { get; private set; }

		public CodeError[] Errors { get; private set; }

		public PreprocessorResult(CodeLine[] CodeLines, CodeError[] Errors)
		{
			this.CodeLines = CodeLines;
			this.Errors = Errors;
		}
	}
}

