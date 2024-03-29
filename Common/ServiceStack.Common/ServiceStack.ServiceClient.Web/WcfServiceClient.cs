using System;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using ServiceStack.ServiceClient.Web;

namespace ServiceStack.ServiceClient.Web
{
	public abstract class WcfServiceClient : IWcfServiceClient
	{
		const string XPATH_SOAP_FAULT = "/s:Fault";
		const string XPATH_SOAP_FAULT_REASON = "/s:Fault/s:Reason";
		const string NAMESPACE_SOAP = "http://www.w3.org/2003/05/soap-envelope";
		const string NAMESPACE_SOAP_ALIAS = "s";

		public string Uri { get; set; }

		public abstract void SetProxy(Uri proxyAddress);
		protected abstract MessageVersion MessageVersion { get; }
		protected abstract Binding Binding { get; }

		private static XmlNamespaceManager GetNamespaceManager(XmlDocument doc)
		{
			var nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr.AddNamespace(NAMESPACE_SOAP_ALIAS, NAMESPACE_SOAP);
			return nsmgr;
		}

		private static Exception CreateException(Exception e, XmlReader reader)
		{
			var doc = new XmlDocument();
			doc.Load(reader);
			var node = doc.SelectSingleNode(XPATH_SOAP_FAULT, GetNamespaceManager(doc));
			if (node != null)
			{
				string errMsg = null;
				var nodeReason = doc.SelectSingleNode(XPATH_SOAP_FAULT_REASON, GetNamespaceManager(doc));
				if (nodeReason != null)
				{
					errMsg = nodeReason.FirstChild.InnerXml;
				}
				return new Exception(string.Format("SOAP FAULT '{0}': {1}", errMsg, node.InnerXml), e);
			}
			return e;
		}

		private ServiceEndpoint SyncReply
		{
			get
			{
				var contract = new ContractDescription("ServiceStack.ServiceClient.Web.ISyncReply", "http://services.servicestack.net/");
				var addr = new EndpointAddress(Uri);
				var endpoint = new ServiceEndpoint(contract, Binding, addr);
				return endpoint;
			}
		}

		public Message Send(object request)
		{
			return Send(request, request.GetType().Name);
		}

		public Message Send(object request, string action)
		{
			return Send(Message.CreateMessage(MessageVersion, action, request));
		}

		public Message Send(XmlReader reader, string action)
		{
			return Send(Message.CreateMessage(MessageVersion, action, reader));
		}

		public Message Send(Message message)
		{
			using (var client = new GenericProxy<ISyncReply>(SyncReply))
			{
				var response = client.Proxy.Send(message);
				return response;
			}
		}

		public static T GetBody<T>(Message message)
		{
			var buffer = message.CreateBufferedCopy(int.MaxValue);
			try
			{
				return buffer.CreateMessage().GetBody<T>();
			}
			catch (Exception ex)
			{
				throw CreateException(ex, buffer.CreateMessage().GetReaderAtBodyContents());
			}
		}

		public T Send<T>(object request)
		{
			var response = Send(request);
			return response.GetBody<T>();
		}

		public void SendOneWay(object request)
		{
			SendOneWay(request, request.GetType().Name);
		}

		public void SendOneWay(object request, string action)
		{
			SendOneWay(Message.CreateMessage(MessageVersion, action, request));
		}

		public void SendOneWay(XmlReader reader, string action)
		{
			SendOneWay(Message.CreateMessage(MessageVersion, action, reader));
		}

		public void SendOneWay(Message message)
		{
			using (var client = new GenericProxy<IOneWay>(SyncReply))
			{
				client.Proxy.SendOneWay(message);
			}
		}

		public void Dispose()
		{
		}
	}
}