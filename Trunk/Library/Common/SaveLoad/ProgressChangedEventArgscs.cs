using System;

namespace Mnk.Library.Common.SaveLoad
{
	public class ProgressChangedEventArgs : EventArgs
	{
		public long BytesRead { get; set; }
		public long Length { get; set; }

		public ProgressChangedEventArgs(long bytesRead, long length)
		{
			BytesRead = bytesRead;
			Length = length;
		}
	}
}
