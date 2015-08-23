using System;

namespace ClawAssembler
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

