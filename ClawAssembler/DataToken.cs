using System;
using System.Text;

namespace ClawAssembler
{
	public class DataToken : ClawToken
	{
		public byte[] Bytes { get; private set; }

		public DataType DataType { get; private set; }

		public DataToken(sbyte Value)
		{
			Bytes = new byte[] { unchecked ((byte)Value) };
			DataType = DataType.Int8;
		}

		public DataToken(byte Value)
		{
			Bytes = new byte[] { Value };
			DataType = DataType.UInt8;
		}

		public DataToken(short Value)
		{
			Bytes = BitConverter.GetBytes(Value);
			DataType = DataType.Int16;
		}

		public DataToken(ushort Value)
		{
			Bytes = BitConverter.GetBytes(Value);
			DataType = DataType.UInt16;
		}

		public DataToken(int Value)
		{
			Bytes = BitConverter.GetBytes(Value);
			DataType = DataType.Int32;
		}

		public DataToken(uint Value)
		{
			Bytes = BitConverter.GetBytes(Value);
			DataType = DataType.UInt32;
		}

		public DataToken(float Value)
		{
			Bytes = BitConverter.GetBytes(Value);
			DataType = DataType.Float;
		}

		public DataToken(string String)
		{
			Bytes = Encoding.ASCII.GetBytes(String + '\0');
			DataType = DataType.UInt8;
		}

		public DataToken(byte[] Values)
		{
			Bytes = Values;
			DataType = DataType.UInt8;
		}
	}

	public enum DataType : byte
	{
		Int8,
		UInt8,
		Int16,
		UInt16,
		Int32,
		UInt32,
		Float
	}
}
