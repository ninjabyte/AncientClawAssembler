using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ClawBinaryCompiler
{
	public static class Parser
	{
		private static Regex commentRegex;
		private static string definitionMatchRegex;
		private static string definitionReplaceRegex;

		private static Regex dataRegex;
		private static Regex instructionRegex;
		private static Regex labelRegex;
		private static Regex defineRegex;
		private static Regex undefineRegex;

		static Parser()
		{
			// init regexes
			commentRegex = new Regex(";.*");
			definitionMatchRegex = "(?:^{SEARCH})|(?:([\\t ,:]){SEARCH}$)|(?:([\\t ,:]){REPLACE}([\\t ,:]))";
			definitionReplaceRegex = "$1$2{REPLACE}$3";

			dataRegex = new Regex("\\.[Dd][Bb]([12368FfUuSs]+)[\\t ]*(?:\\(([\\d\\t ,'.-XxBb]*)\\)|\\\"([^\\\"]*)\\\")");
			instructionRegex = new Regex("^([\\w]+)[\\t ,]*([AaBbCcDd])?[\\t ,]*([AaBbCcDd]*)?$");
			labelRegex = new Regex("^([\\w]):$");
			defineRegex = new Regex("^#define[\\t ]+(\\w)+(?:[\t ]+.+)?");
			undefineRegex = new Regex("^#undefine[\\t ]+(\\w)+");
		}

		public static CodeLine[] PreProcess(string Filename)
		{
			var codeLines = new List<CodeLine>();
			var defines = new Dictionary<string, string>();

			string mainContents = File.ReadAllText(Filename);
			mainContents = mainContents.Replace("\r\n", "\n");
			mainContents = mainContents.Replace("\\\n", "");
			mainContents = mainContents.Replace("\\n", "\n");

			string[] lines = mainContents.Split('\n');



			uint lineCount = 1;

			// Remove all whitespace and all comments
			foreach (string line in lines) {
				string trimmedLine = commentRegex.Replace(line, "").Trim();

				if (trimmedLine != "") {
					if (dataRegex.IsMatch(trimmedLine)) {
						codeLines.Add(new CodeLine(trimmedLine, lineCount, Filename){ Type = CodeLine.LineType.Data });
					} else if (instructionRegex.IsMatch(trimmedLine)) {
						codeLines.Add(new CodeLine(trimmedLine, lineCount, Filename){ Type = CodeLine.LineType.Instruction });
					} else if (labelRegex.IsMatch(trimmedLine)) {
						codeLines.Add(new CodeLine(trimmedLine, lineCount, Filename){ Type = CodeLine.LineType.Label });
					} else
						codeLines.Add(new CodeLine(trimmedLine, lineCount, Filename){ Type = CodeLine.LineType.Unknown });
				} else
					codeLines.Add(new CodeLine("", lineCount, Filename){ Type = CodeLine.LineType.Empty, Processed = true });

				lineCount++;
			}

			return codeLines.ToArray();

			//foreach (KeyValuePair<string, string> kv in Defines)
			//	line = Regex.Replace(line, definitionMatchRegex.Replace("{SEARCH}", kv.Key.Trim()), definitionReplaceRegex.Replace("{REPLACE}", kv.Value.Trim())).Trim();
		}

		/// <summary>
		/// Parse the preprocessed source code lines.
		/// </summary>
		/// <param name="CodeLines">Source code lines</param>
		public static ClawToken[] Parse(CodeLine[] CodeLines)
		{
			var tokens = new List<ClawToken>();

			// Process all elements
			foreach (CodeLine sourceLine in CodeLines) {
				string line = sourceLine.Content;

				if (!sourceLine.Processed && sourceLine.Type != CodeLine.LineType.Unknown) {
					if (sourceLine.Type == CodeLine.LineType.Data) {
						Match match = dataRegex.Match(line);
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

						sourceLine.Processed = true;
					} else if (sourceLine.Type == CodeLine.LineType.Instruction) {
						Match match = instructionRegex.Match(sourceLine.Content);
						string mnemoric = match.Groups[1].Value.ToUpper();
						string instack = match.Groups[2].Value.ToUpper();
						string outstack = match.Groups[3].Value.ToUpper();

						ClawInstruction instruction = (ClawInstruction)Enum.Parse(typeof(ClawInstruction), mnemoric);
						ClawStack input_stack = (instack != "") ? (ClawStack)Enum.Parse(typeof(ClawStack), instack) : ClawStack.A;
						ClawStack output_stack = (outstack != "") ? (ClawStack)Enum.Parse(typeof(ClawStack), outstack) : input_stack;

						tokens.Add(new InstructionToken(instruction, input_stack, output_stack));

						sourceLine.Processed = true;
					} else if (sourceLine.Type == CodeLine.LineType.Label) {
						Match match = labelRegex.Match(sourceLine.Content);

						sourceLine.Processed = true;
					}
				}
			}

			return tokens.ToArray();
		}
	}
}

