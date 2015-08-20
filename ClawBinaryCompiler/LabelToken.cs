using System;

namespace ClawBinaryCompiler
{
	public class LabelToken : ClawToken
	{
		public byte[] Bytes {
			get { return new byte[0]; }
		}

		public LabelToken()
		{
		}
	}
}

