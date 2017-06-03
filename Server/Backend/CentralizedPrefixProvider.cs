using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Backend
{
	public class CentralizedPrefixProvider : DefaultDirectRouteProvider
	{
		private readonly string _centralizedPrefix;

		public CentralizedPrefixProvider(string centralizedPrefix)
		{
			_centralizedPrefix = centralizedPrefix;
		}

		protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
		{
			var existingPrefix = base.GetRoutePrefix(controllerDescriptor);
			if (existingPrefix == null) return _centralizedPrefix;

			return $"{_centralizedPrefix}/{existingPrefix}";
		}
	}
}