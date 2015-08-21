using System;
using System.IO;
using System.Collections.Generic;
using CommandLine;

namespace ClawBinaryCompiler
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			ClawToken[] tokens;

			try {
				tokens = Parser.Parse(File.ReadAllText("claw_sample.asm"));
			} catch (Exception ex) {
				Console.WriteLine("ERR: " + ex.Message);
				return;
			}

			var binaryCode = new List<byte>();

			foreach (ClawToken token in tokens) {
				binaryCode.AddRange(token.Bytes);
			}

			File.WriteAllBytes("claw_sample.cex", binaryCode.ToArray());
		}
	}
}
