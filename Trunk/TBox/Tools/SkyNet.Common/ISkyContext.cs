namespace SkyNet.Common
{
    public interface ISkyContext
    {
        void Update(float porgress, string data);
        bool ShouldBeTerminated { get; }
    }
}
