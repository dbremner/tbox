namespace Common.Base.Log
{
	public sealed class NullLog : AbstractLog
	{
		public override void Write(string value){}
	}
}
