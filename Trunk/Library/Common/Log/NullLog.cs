namespace Mnk.Library.Common.Log
{
	public sealed class NullLog : AbstractLog
	{
		public override void Write(string value){}
	}
}
