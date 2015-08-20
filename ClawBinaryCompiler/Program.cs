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
			ClawToken[] tokens = Parser.Parse(File.ReadAllText("claw_sample.asm"));

			var binaryCode = new List<byte>();

			foreach (ClawToken token in tokens) {
				binaryCode.AddRange(token.Bytes);
			}

			File.WriteAllBytes("claw_sample.cex", binaryCode.ToArray());

			string hex = ByteArrayToString(binaryCode.ToArray());
			File.WriteAllText("claw_sample.cex.txt", hex);
		}

		public static string ByteArrayToString(byte[] ba)
		{
			string hex = BitConverter.ToString(ba);
			return hex.Replace("-", " ");
		}
	}
}
