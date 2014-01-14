using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.IO;
using Mnk.Library.Common.Tools;

namespace Mnk.Library.Common.Network
{
	public class Request
	{
		private readonly Stopwatch stopwatch = new Stopwatch();
		public ResponseInfo GetResult(string url, Methods method = Methods.GET, string body = "", IEnumerable<IHeader> headers = null, int timeOut = 120000)
		{
			try
			{
				var request=(HttpWebRequest)WebRequest.Create(url);
				request.ReadWriteTimeout = request.Timeout = timeOut;
				request.Proxy = null;
				request.Credentials = CredentialCache.DefaultCredentials;
				if (headers != null)
				{
					foreach (var h in headers)
					{
						AddHeader(request, h);
					}
				}
				request.Method = method.ToString();
				if (!string.IsNullOrEmpty(body))
				{
				    var buffer = Encoding.UTF8.GetBytes(body);
					using (var s = request.GetRequestStream())
					{
						s.Write(buffer, 0, buffer.Length);
					}
				}
				return GetResponse(request);
			}
			catch (Exception ex)
			{
				return new ResponseInfo { Body = ex.Message };
			}
		}

		private static void AddHeader(HttpWebRequest request, IHeader h)
		{
			if (h.Key.EqualsIgnoreCase("Content-Type"))
			{
				request.ContentType = h.Value;
			}
			else if (h.Key.EqualsIgnoreCase("Host"))
			{
				request.Host = h.Value;
			}
			else if (h.Key.EqualsIgnoreCase("User-Agent"))
			{
				request.UserAgent = h.Value;
			}
			else if (h.Key.EqualsIgnoreCase("Referer"))
			{
				request.Referer = h.Value;
			}
			else if (h.Key.EqualsIgnoreCase("Accept"))
			{
				request.Accept = h.Value;
			}
			else
			{
				request.Headers.Add(h.Key, h.Value);
			}
		}

		private ResponseInfo GetResponse(HttpWebRequest request)
		{
			try
			{
				stopwatch.Restart();
				using(var response = (HttpWebResponse)request.GetResponse())
				{
					var result = ReadHeaders(string.Empty, response);
					var stream = response.GetResponseStream();
					if (stream == null)
					{
						result.Body += "Response is empty";
					}
					else
					{
						using (var s = new StreamReader(stream, Encoding.UTF8))
						{
							result.Body += s.ReadToEnd();
						}
					}
					result.Time = stopwatch.ElapsedMilliseconds;
					return result;
				}
			}
			catch (WebException wex)
			{
				var result = ReadHeaders(wex.Message, wex.Response);
				Stream stream;
				if (wex.Response != null && 
					(stream = wex.Response.GetResponseStream()) != null )
				{
					using (var s = new StreamReader(stream, Encoding.UTF8))
					{
						result.Body += s.ReadToEnd();
					}
				}
				result.Time = stopwatch.ElapsedMilliseconds;
				return result;
			}
			catch (Exception ex)
			{
				return new ResponseInfo { Body = ex.Message, Time = stopwatch.ElapsedMilliseconds };
			}
		}

		private static ResponseInfo ReadHeaders(string message, WebResponse response)
		{
			var result = new ResponseInfo { Body = message };
			if (!string.IsNullOrEmpty(message))
				result.Body += Environment.NewLine;
			try
			{
				var sb = new StringBuilder();
				var httpResponse = response as HttpWebResponse;
				if (httpResponse != null)
				{
					result.HttpStatusCode = httpResponse.StatusCode;
					sb.AppendFormat("HTTP/{0} {1} {2}",
						httpResponse.ProtocolVersion,
						(int)httpResponse.StatusCode,
						httpResponse.StatusDescription);
					sb.AppendLine();
				}
				for (var i = 0; i < response.Headers.Count; ++i)
				{
					sb.AppendLine(response.Headers.GetKey(i) + ": " + response.Headers.Get(i));
				}
				result.Headers = sb.ToString();
			}
			catch (Exception ex)
			{
				result.Headers = "Can't read headers info: " + ex.Message;
			}
			return result;
		}

	}

}
