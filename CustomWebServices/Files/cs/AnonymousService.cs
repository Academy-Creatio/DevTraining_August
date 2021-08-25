using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Web.Common;

namespace CustomWebService
{

	/// <summary>
	/// 
	/// 
	/// </summary>
	/// <remarks>
	/// See <see href="https://academy.creatio.com/docs/developer/back-end_development/configuration_web_service/configuration_web_service#case-1240">official</see> documentation for details
	/// </remarks>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class AnonymousService
	{
		#region Properties: Protected

		private HttpContextBase _httpContext;
		protected virtual HttpContextBase CurrentHttpContext
		{
			get { return _httpContext ?? (_httpContext = new HttpContextWrapper(HttpContext.Current)); }
		}

		private UserConnection _userConnection;
		protected UserConnection UserConnection
		{
			get
			{
				if (_userConnection != null)
				{
					return _userConnection;
				}
				_userConnection = CurrentHttpContext.Session["UserConnection"] as UserConnection;
				if (_userConnection != null)
				{
					return _userConnection;
				}
				var appConnection = (AppConnection)CurrentHttpContext.Application["AppConnection"];
				_userConnection = appConnection.SystemUserConnection;
				return _userConnection;
			}
		}
		#endregion

		#region Methods: Private

		private void SetOptionsCORS()
		{
			CurrentHttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
			CurrentHttpContext.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
			CurrentHttpContext.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
		}

		private void SetHeaderCORS()
		{
			CurrentHttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
		}

		#endregion

		[OperationContract]
		[WebInvoke(Method = "OPTIONS", UriTemplate = "*")]
		public void GetWebRequestOptions()
		{
			SetOptionsCORS();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		/// <remarks>
		/// call method by following <see href="http://k_krylov:7020/0/ServiceModel/AnonymousService.svc/GetContactByEmail?email=andrew@mail.io">this link</see>
		/// </remarks>
		[OperationContract]
		[WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest,
			ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
		public ContactModel GetContactByEmail(string email)
		{
			const string tableName = "Contact";
			EntitySchema contactSchema = UserConnection.EntitySchemaManager.GetInstanceByName(tableName);
			Entity contact = contactSchema.CreateEntity(UserConnection);
			contact.FetchFromDB("Email", email);

			ContactModel cm = new ContactModel
			{
				Name = contact.GetTypedColumnValue<string>("Name"),
				Email = contact.GetTypedColumnValue<string>("Email"),
				BirthDate = contact.GetTypedColumnValue<DateTime>("BirthDate")
			};
			SetOptionsCORS();
			return cm;
		}
	}
}



