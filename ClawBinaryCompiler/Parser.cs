using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ClawBinaryCompiler
{
	public static class Parser
	{
		private static Regex commentRegex;
		private static Regex dataRegex;
		private static Regex instructionRegex;
		private static Regex labelRegex;

		static Parser()
		{
			commentRegex = new Regex(";.*");
			dataRegex = new Regex("^.[Dd][Bb]([12368FfUuSs]+)[\\t ]* (?:\\(([\\t, '.-]*)\\)|\"([^\"]*)\")$");
			instructionRegex = new Regex("^([\\w]+)[\\t ,]*([AaBbCcDd])?[\\t ,]*([AaBbCcDd]*)?$");
			labelRegex = new Regex("^([\\w]):$");
		}

		public static ClawToken[] Parse(string Source)
		{
			var lines = new List<CodeLine>();

			uint lineCount = 1;

			// Remove all whitespace and all comments
			foreach (string line in Source.Split('\n')) {
				string trimmedLine = commentRegex.Replace(line, "").Trim();

				if (trimmedLine != "") {
					if (dataRegex.IsMatch(trimmedLine)) {
						lines.Add(new CodeLine(trimmedLine, lineCount){ Type = CodeLine.LineType.Data });
					} else if (instructionRegex.IsMatch(trimmedLine)) {
						lines.Add(new CodeLine(trimmedLine, lineCount){ Type = CodeLine.LineType.Instruction });
					} else if (labelRegex.IsMatch(trimmedLine)) {
						lines.Add(new CodeLine(trimmedLine, lineCount){ Type = CodeLine.LineType.Label });
					} else
						lines.Add(new CodeLine(trimmedLine, lineCount){ Type = CodeLine.LineType.Unknown });
				} else
					lines.Add(new CodeLine("", lineCount){ Type = CodeLine.LineType.Empty, Processed = true });

				lineCount++;
			}

			var tokens = new List<ClawToken>();

			// Process all elements
			foreach (CodeLine line in lines) {
				if (!line.Processed && line.Type != CodeLine.LineType.Unknown) {
					if (line.Type == CodeLine.LineType.Data) {
						Match match = dataRegex.Match(line.Line);
						string type = match.Groups[1].Value.ToUpper();
						string data = match.Groups[2].Value;
						string strval = match.Groups[3].Value;
					
						var values = new List<string>();

						// check if its not a string
						if (data != "") {
							// Trim all values and add all not empty ones to a list
							foreach (string value in data.Split(',')) {
								string trimmedValue = value.Trim();
								if (trimmedValue != "")
									values.Add(trimmedValue);
							}
						}

						if (type == "8") {
							if (values.Count > 1) {

							} else {
								tokens.Add(new DataToken(Convert.ToSByte(values[0])));
							}
						} else if (type == "8U") {
							if (values.Count > 1) {

							} else {
								tokens.Add(new DataToken(Convert.ToByte(values[0])));
							}
						} else if (type == "16") {
							if (values.Count > 1) {

							} else {
								tokens.Add(new DataToken(Convert.ToInt16(values[0])));
							}
						} else if (type == "16U") {
							if (values.Count > 1) {

							} else {
								tokens.Add(new DataToken(Convert.ToUInt16(values[0])));
							}
						} else if (type == "32") {
							if (values.Count > 1) {

							} else {
								tokens.Add(new DataToken(Convert.ToInt32(values[0])));
							}
						} else if (type == "32U") {
							if (values.Count > 1) {

							} else {
								tokens.Add(new DataToken(Convert.ToUInt32(values[0])));
							}
						} else if (type == "F") {
							if (values.Count > 1) {

							} else {
								tokens.Add(new DataToken(Convert.ToSingle(values[0])));
							}
						} else if (type == "S") {
							tokens.Add(new DataToken(strval));
						} else {
							// TODO: Some error handling
						}

						line.Processed = true;
					} else if (line.Type == CodeLine.LineType.Instruction) {
						Match match = instructionRegex.Match(line.Line);
						string mnemoric = match.Groups[1].Value.ToUpper();
						string instack = match.Groups[2].Value.ToUpper();
						string outstack = match.Groups[3].Value.ToUpper();

						ClawInstruction instruction = (ClawInstruction)Enum.Parse(typeof(ClawInstruction), mnemoric);
						ClawStack input_stack = (instack != "") ? (ClawStack)Enum.Parse(typeof(ClawStack), instack) : ClawStack.A;
						ClawStack output_stack = (outstack != "") ? (ClawStack)Enum.Parse(typeof(ClawStack), outstack) : input_stack;

						tokens.Add(new InstructionToken(instruction, input_stack, output_stack));

						line.Processed = true;
					} else if (line.Type == CodeLine.LineType.Label) {
						Match match = labelRegex.Match(line.Line);

						line.Processed = true;
					}
				}
			}

			return tokens.ToArray();
		}
	}
}

