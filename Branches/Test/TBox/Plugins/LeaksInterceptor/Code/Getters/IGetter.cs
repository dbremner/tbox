using System.Diagnostics;

namespace LeaksInterceptor.Code.Getters
{
	interface IGetter
	{
		float Get(Process p);
	}
}
