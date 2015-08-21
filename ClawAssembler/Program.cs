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
			if (args.Length < 1) {
				Console.WriteLine("No input file");
				return;
			}

			string inputFile = args[0];
			string outputFile = (args.Length > 1) ? args[1] : "out.cex";

			ClawToken[] tokens;

			try {
				tokens = Parser.Parse(Parser.PreProcess(inputFile));

				var binaryCode = new List<byte>();

				foreach (ClawToken token in tokens) {
					binaryCode.AddRange(token.Bytes);
				}

				File.WriteAllBytes(outputFile, binaryCode.ToArray());
			} catch (Exception ex) {
				Console.WriteLine("ERR: " + ex.Message);
				return;
			}
		}
	}
}
