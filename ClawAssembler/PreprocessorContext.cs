using System;
using System.Collections.Generic;

namespace ClawAssembler
{
	public class PreprocessorContext
	{
		public Dictionary<string, string> Defines { get; private set; }

		public string Filename { get; private set; }

		public CodeLine[] CodeLines { get; private set; }

		public PreprocessorContext(string Filename, Dictionary<string, string> Defines, CodeLine[] CodeLines)
		{
			this.Defines = Defines;
			this.Filename = Filename;
			this.CodeLines = CodeLines;
		}
	}
}

