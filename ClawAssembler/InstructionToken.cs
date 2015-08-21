using System;

namespace ClawBinaryCompiler
{
	public class InstructionToken : ClawToken
	{
		public ClawInstruction Instruction { get; private set; }

		public ClawStack InputStack { get; private set; }

		public ClawStack OutputStack { get; private set; }

		public byte[] Bytes {
			get {
				unchecked {
					ushort value = (ushort)Instruction;
					value <<= 4;

					byte operand = (byte)InputStack;
					operand &= 0x3;
					operand <<= 2;

					value |= operand;

					operand = (byte)OutputStack;
					operand &= 0x3;

					value |= operand;

					return BitConverter.GetBytes(value);
				}
			}
		}

		public InstructionToken(ClawInstruction Instruction, ClawStack InputStack, ClawStack OutputStack)
		{
			this.Instruction = Instruction;
			this.InputStack = InputStack;
			this.OutputStack = OutputStack;
		}
	}
}

