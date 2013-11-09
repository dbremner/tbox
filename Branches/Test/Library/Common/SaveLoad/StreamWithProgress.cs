using System;
using System.IO;

namespace Common.SaveLoad
{
	public class StreamWithProgress : Stream
	{
		private readonly Stream file;
		private readonly long length;
		private long bytesRead;
		public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

		public StreamWithProgress(Stream file)
		{
			this.file = file;
			length = file.Length;
			bytesRead = 0;
			if (ProgressChanged != null) ProgressChanged(this, new ProgressChangedEventArgs(bytesRead, length));
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override void Flush() { }

		public override long Length
		{
			get { throw new NotSupportedException(); }
		}

		public override long Position
		{
			get { return bytesRead; }
			set { throw new NotImplementedException(); }
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			var result = file.Read(buffer, offset, count);
			bytesRead += result;
			if (ProgressChanged != null) ProgressChanged(this, new ProgressChangedEventArgs(bytesRead, length));
			return result;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}
	}
}
