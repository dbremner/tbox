using System;
using System.Collections.ObjectModel;
using Common.Network;

namespace Requestor.Code.Settings
{
	[Serializable]
	public sealed class RequestConfig : ICloneable
	{
		public string Url { get; set; }
		public Methods Method { get; set; }

		public string Body { get; set; }
		public ObservableCollection<Header> Headers { get; set; }

		public RequestConfig()
		{
			Url = @"http://google.com";
			Method = Methods.GET;
			Headers = new ObservableCollection<Header>();
		}

		public object Clone()
		{
			var c = new RequestConfig
			{
				Url = Url,
				Body = Body,
				Headers = new ObservableCollection<Header>(Headers),
				Method = Method,
			};
			return c;
		}
	}
}
