using System;
using System.Collections.Generic;

namespace ClawAssembler
{
	public static class Compiler
	{
		static Compiler()
		{
		}

		public static byte[] Compile(ClawToken[] Tokens)
		{
			var binaryCode = new List<byte>();

			foreach (ClawToken token in Tokens)
				binaryCode.AddRange(token.Bytes);

			return binaryCode.ToArray();
		}
	}
}

