using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ClawAssembler
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
		private static Regex includeRegex;

		static Parser()
		{
			// init regexes
			commentRegex = new Regex(@";.*");
			definitionMatchRegex = @"(?:^{SEARCH}$)|(?:(?<PRE0>[\t ,:\(\)""\]){SEARCH}$)|(?:{SEARCH}(?<SUF0>[\t ,:\(\)""\])$)|(?:(?<PRE1>[\t ,:\(\)""\]){SEARCH}(?<SUF1>[\t ,:\(\)""\]))";
			definitionReplaceRegex = @"${PRE0}${PRE1}{REPLACE}${SUF0}${SUF1}";

			dataRegex = new Regex(@"\.[Dd][Bb](8[Uu]?|16[Uu]?|32[Uu]?|[Ss])[\t ]*(?:\(([\d\t ,\'\.\-XxBb]*)\)|\""([^\""]*)\"")$");
			instructionRegex = new Regex(@"^([\w]+)[\t ,]*([AaBbCcDd])?[\t ,]*([AaBbCcDd]*)?$");
			labelRegex = new Regex(@"^([\w]):$");
			defineRegex = new Regex(@"#[Dd][Ee][Ff][Ii][Nn][Ee][\t ]+([^=]+)(?:[\t ]*=[\t ]*(.+))?$");
			undefineRegex = new Regex(@"^#[Uu][Nn][Dd][Ee][Ff](?:[Ii][Nn][Ee])?[\t ]+([\w]+)$");
			includeRegex = new Regex(@"^#[Ii][Nn][Cc][Ll][Uu][Dd][Ee][\t ]+""([^""]+)""$");
		}

		public static PreprocessorResult Preprocess(string Filename)
		{
			var codeLines = new List<CodeLine>();
			var defines = new Dictionary<string, string>();
			var errors = new List<CodeError>();

			string mainContents = File.ReadAllText(Filename);
			mainContents = mainContents.Replace("\r\n", "\n");
			mainContents = mainContents.Replace("\\\n", "");

			string[] lines = mainContents.Split('\n');

			//var replacementRegexPairs = new Dictionary<Regex, string>();
			//foreach (KeyValuePair<string, string> kv in defines)
			//	

			uint lineCount = 1;

			// Remove all whitespace and all comments
			foreach (string line in lines) {
				string processedLine = commentRegex.Replace(line, "").Trim();

				//	foreach (KeyValuePair<Regex, string> replacementPair in replacementRegexPairs)
				//		processedLine = replacementPair.Key.Replace(line, replacementPair.Value).Trim();

				if (processedLine != "") {
					if (dataRegex.IsMatch(processedLine)) {
						codeLines.Add(new CodeLine(processedLine, lineCount, Filename){ Type = CodeLine.LineType.Data });
					} else if (instructionRegex.IsMatch(processedLine)) {
						codeLines.Add(new CodeLine(processedLine, lineCount, Filename){ Type = CodeLine.LineType.Instruction });
					} else if (labelRegex.IsMatch(processedLine)) {
						codeLines.Add(new CodeLine(processedLine, lineCount, Filename){ Type = CodeLine.LineType.Label });
					} else if (defineRegex.IsMatch(line)) {
						CodeLine thisLine = new CodeLine(processedLine, lineCount, Filename) {
							Type = CodeLine.LineType.Preprocessor,
							Processed = true
						};

						Match match = defineRegex.Match(line);
						string search = match.Groups[1].Value;
						string replace = match.Groups[2].Value;
						replace = (replace != "") ? replace : "1";

						if (defines.ContainsKey(search))
							defines[search] = replace;
						else
							defines.Add(search, replace);

						codeLines.Add(thisLine);
					} else if (undefineRegex.IsMatch(line)) {
						CodeLine thisLine = new CodeLine(processedLine, lineCount, Filename) {
							Type = CodeLine.LineType.Preprocessor,
							Processed = true
						};

						Match match = defineRegex.Match(line);
						string search = match.Groups[1].Value.Trim();

						if (defines.ContainsKey(search))
							defines.Remove(search);
						else
							errors.Add(new CodeError(CodeError.ErrorType.DefineNotExistant, ErrorLevel.Warning, thisLine));

						codeLines.Add(thisLine);
					} else
						codeLines.Add(new CodeLine(processedLine, lineCount, Filename){ Type = CodeLine.LineType.Unknown });
				} else if (includeRegex.IsMatch(line)) {
					CodeLine thisLine = new CodeLine(processedLine, lineCount, Filename) {
						Type = CodeLine.LineType.Preprocessor,
						Processed = true
					};
					codeLines.Add(thisLine);

					Match match = includeRegex.Match(line);
					string filename = match.Groups[1].Value;

					if (!File.Exists(filename))
						errors.Add(new CodeError(CodeError.ErrorType.IncludeNotFound, ErrorLevel.Error, thisLine));
					else {
						PreprocessorResult includeResult = Preprocess(filename);
						codeLines.AddRange(includeResult.CodeLines);
						errors.AddRange(includeResult.Errors);
					}
				} else {
					codeLines.Add(new CodeLine(line, lineCount, Filename) {
						Type = CodeLine.LineType.Empty,
						Processed = true
					});
				}

				lineCount++;
			}

			foreach (CodeLine line in codeLines) {
				if (line.Type == CodeLine.LineType.Unknown) {
					errors.Add(new CodeError(CodeError.ErrorType.SyntaxError, ErrorLevel.Error, line));
					line.Processed = true;
				}
			}

			return new PreprocessorResult(codeLines.ToArray(), errors.ToArray());
		}

		/// <summary>
		/// Parse the preprocessed source code lines.
		/// </summary>
		/// <param name="CodeLines">Source code lines</param>
		public static ParserResult Parse(CodeLine[] CodeLines)
		{
			var tokens = new List<ClawToken>();
			var errors = new List<CodeError>();

			// Process all elements
			foreach (CodeLine line in CodeLines) {

				if (!line.Processed && line.Type != CodeLine.LineType.Unknown) {
					if (line.Type == CodeLine.LineType.Data) {
						Match match = dataRegex.Match(line.Content);
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
							foreach (string value in values)
								tokens.Add(new DataToken(Convert.ToSByte(value)));
						} else if (type == "8U") {
							foreach (string value in values)
								tokens.Add(new DataToken(Convert.ToByte(value)));
						} else if (type == "16") {
							foreach (string value in values)
								tokens.Add(new DataToken(Convert.ToInt16(value)));
						} else if (type == "16U") {
							foreach (string value in values)
								tokens.Add(new DataToken(Convert.ToUInt16(value)));
						} else if (type == "32") {
							foreach (string value in values)
								tokens.Add(new DataToken(Convert.ToInt32(value)));
						} else if (type == "32U") {
							foreach (string value in values)
								tokens.Add(new DataToken(Convert.ToUInt32(value)));
						} else if (type == "F") {
							foreach (string value in values)
								tokens.Add(new DataToken(Convert.ToSingle(value)));
						} else if (type == "S") {
							strval = strval.Replace("\\\\", "\\").Replace("\\n", "\n");
							tokens.Add(new DataToken(strval));
						} else {
							errors.Add(new CodeError(CodeError.ErrorType.UnknownDatatype, ErrorLevel.Error, line));
						}

						line.Processed = true;
					} else if (line.Type == CodeLine.LineType.Instruction) {
						Match match = instructionRegex.Match(line.Content);
						string mnemoric = match.Groups[1].Value.ToUpper();
						string instack = match.Groups[2].Value.ToUpper();
						string outstack = match.Groups[3].Value.ToUpper();

						ClawInstruction instruction;
						ClawStack input_stack;
						ClawStack output_stack;

						if (Enum.TryParse<ClawInstruction>(mnemoric, true, out instruction)) {
							input_stack = Enum.TryParse<ClawStack>(instack, true, out input_stack) ? input_stack : ClawStack.A;
							output_stack = Enum.TryParse<ClawStack>(outstack, true, out output_stack) ? output_stack : input_stack;

							tokens.Add(new InstructionToken(instruction, input_stack, output_stack));
						} else
							errors.Add(new CodeError(CodeError.ErrorType.UnknownInstruction, ErrorLevel.Error, line));

						line.Processed = true;
					} else if (line.Type == CodeLine.LineType.Label) {
						Match match = labelRegex.Match(line.Content);

						line.Processed = true;
					}
				}
			}

			foreach (CodeLine line in CodeLines)
				if (line.Processed == false)
					errors.Add(new CodeError(CodeError.ErrorType.SyntaxError, ErrorLevel.Warning, line));

			return new ParserResult(tokens.ToArray(), CodeLines, errors.ToArray());
		}
	}
}

