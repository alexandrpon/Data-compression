using System;
using System.IO;

namespace HaffmanFileWork
{
	public class ArchWriter
	{
		const byte bufSize = 10;
		byte[] buf;
		byte oneByte;
		byte bitsCount;
		byte bytesCount;
		FileStream fs;
		/// <summary>
		/// Создает класс для побитовой записи архива. Для окончании записи используйте метод Finish
		/// </summary>
		/// <param name="fileName"> имя файла архива </param>
		public ArchWriter(string fileName)
		{
			buf = new byte[bufSize];
			oneByte = 0;
			bitsCount = bytesCount = 0;
			fs = new FileStream(fileName, FileMode.Create);
		}
		private void WriteByte()
		{
			buf[bytesCount++] = oneByte;
			oneByte = 0;
			bitsCount = 0;
			if (bytesCount == bufSize)
			{
				fs.Write(buf, 0, bufSize);
				bytesCount = 0;
			}
		}
		/// <summary>
		/// Запись в файл одного бита
		/// </summary>
		/// <param name="bit">Значение записываемого бита (0 или 1)</param>
		public void WriteBit(byte bit)
		{
			oneByte = (byte)((oneByte << 1) + bit);
			bitsCount++;
			if (bitsCount == 8)
				WriteByte();
		}
		/// <summary>
		/// Запись в файл битовой строки
		/// </summary>
		/// <param name="w">Битовая строка (должна содержать только символы 0 и 1)</param>
		public void WriteWord(string w)
		{
			foreach (var c in w)
				if (c == '0')
					WriteBit(0);
				else if (c == '1')
					WriteBit(1);
		}
		/// <summary>
		/// Завершает запись файла
		/// </summary>
		public void Finish()
		{
			while (bitsCount > 0) WriteBit(0);
			if (bytesCount > 0)
				fs.Write(buf, 0, bytesCount);
			fs.Close();
		}
	}
	public class ArchReader
	{
		const byte bufSize = 10;
		byte[] buf;
		byte[] oneByte; //actually bits[]
		byte bitsCount;
		byte bytesCount, byteIdx;
		FileStream fs;
		/// <summary>
		/// Создает класс для побитового чтения из архива. Для окончании чтения используйте метод Finish
		/// </summary>
		/// <param name="fileName"></param>
		public ArchReader(string fileName)
		{
			buf = new byte[bufSize];
			oneByte = new byte[8];
			bitsCount = bytesCount = byteIdx = 0;
			fs = new FileStream(fileName, FileMode.Open);
		}
		private bool ReadByte()
		{
			if (bytesCount == byteIdx)
			{
				bytesCount = (byte)fs.Read(buf, 0, bufSize);
				byteIdx = 0;
			}
			if (bytesCount == 0)
				return false;
			var onebyte = buf[byteIdx++];
			for (int i = 0; i < 8; i++)
			{
				oneByte[i] = (byte)(onebyte & 1);
				onebyte = (byte)(onebyte >> 1);
			}
			bitsCount = 8;

			return true;
		}
		/// <summary>
		/// Чтение из файла одного бита
		/// </summary>
		/// <param name="bit">Значение прочитанного бита</param>
		/// <returns>true, если чтение успешно; false если достигнут конец файла</returns>
		public bool ReadBit(out byte bit)
		{
			bool res = true;
			if (bitsCount == 0)
				res = ReadByte();
			if (res)
				bit = oneByte[--bitsCount];
			else
				bit = 0;
			return res;
		}
		/// <summary>
		/// Завершает чтение из файла (закрывает файл)
		/// </summary>
		public void Finish()
		{
			fs.Close();
		}
	}
}

