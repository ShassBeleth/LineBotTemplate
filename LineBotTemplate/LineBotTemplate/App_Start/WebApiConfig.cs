using System.Web.Http;

namespace LineBotTemplate {

	public static class WebApiConfig {

		public static void Register( HttpConfiguration config ) {

			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name : "WebhookApi" ,
				routeTemplate : "api/{controller}"
			);

		}

	}

}