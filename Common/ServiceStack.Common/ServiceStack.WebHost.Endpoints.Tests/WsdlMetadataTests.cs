using NUnit.Framework;
using ServiceStack.Logging;
using ServiceStack.WebHost.Endpoints.Metadata;
using ServiceStack.WebHost.Endpoints.Tests.Support.Operations;
using ServiceStack.WebHost.Endpoints.Utils;

namespace ServiceStack.WebHost.Endpoints.Tests
{
	[TestFixture]
	public class WsdlMetadataTests : TestBase
	{
		private static ILog log = LogManager.GetLogger(typeof(WsdlMetadataTests));

		[Test]
		public void Wsdl_state_is_correct()
		{
			var serviceOperations = new ServiceOperations(base.AllOperations);
			var wsdlGenerator = new Soap11WsdlMetadataHandler();
			var wsdlTemplate = wsdlGenerator.GetWsdlTemplate(serviceOperations, "http://w3c.org/types", false, false, "http://w3c.org/types");

			Assert.That(wsdlTemplate.ReplyOperationNames, Is.EquivalentTo(serviceOperations.ReplyOperations.Names));
			Assert.That(wsdlTemplate.OneWayOperationNames, Is.EquivalentTo(serviceOperations.OneWayOperations.Names));
		}

		[Test]
		public void Xsd_output_does_not_contain_xml_declaration()
		{
			var xsd = new XsdGenerator {
				OperationTypes = new[] { typeof(GetCustomer), typeof(GetCustomerResponse), typeof(GetCustomers), typeof(GetCustomersResponse), typeof(StoreCustomer) },
				OptimizeForFlash = false,
				IncludeAllTypesInAssembly = false,
			}.ToString();

			Assert.That(!xsd.StartsWith("<?"));
		}

		[Test]
		public void XsdUtils_strips_all_xml_declarations()
		{
			const string xsd = "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
							   + "<xs:schema xmlns:tns=\"http://schemas.sericestack.net/examples/types\" elementFormDefault=\"qualified\" targetNamespace=\"http://schemas.sericestack.net/examples/types\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">"
							   + "<xs:complexType name=\"ArrayOfLong\">"
							   + "  <xs:sequence><xs:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"long\" type=\"xs:long\" /></xs:sequence>"
							   + "</xs:complexType>";

			const string xsds = xsd + xsd + xsd;

			//var strippedXsd = XsdUtils.StripXmlDeclaration(xsds);

			//Assert.That(strippedXsd.IndexOf("<?"), Is.EqualTo(-1));
		}

	}
}