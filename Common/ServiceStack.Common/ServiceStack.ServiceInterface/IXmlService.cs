using ServiceStack.LogicFacade;

namespace ServiceStack.ServiceInterface
{
	public interface IXmlService
	{
		string Execute(IOperationContext context);
	}
}