﻿using System.ServiceModel;
using System.ServiceModel.Web;

namespace FeedbackService
{
	[ServiceContract]
	interface IFeedbackServer
	{
		[OperationContract]
		[WebInvoke(Method = "POST", UriTemplate = "send/{subject}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		void Send(string subject, string body);
	}
}
