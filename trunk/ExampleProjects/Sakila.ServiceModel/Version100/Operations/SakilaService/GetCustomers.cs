using System.Runtime.Serialization;
using Sakila.ServiceModel.Version100.Types;

namespace Sakila.ServiceModel.Version100.Operations.SakilaDb4oService
{
	[DataContract(Namespace = "http://schemas.servicestack.net/types/")]
	public class GetCustomers : SakilaService.GetCustomers
	{
	}
}