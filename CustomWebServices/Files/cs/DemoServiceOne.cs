using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Web.Common;

namespace CustomWebService
{
	/// <summary>
	/// Demo WebService
	/// </summary>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class DemoServiceOne : BaseService
	{
		#region Properties
		private SystemUserConnection _systemUserConnection;
		private SystemUserConnection SystemUserConnection
		{
			get
			{
				return _systemUserConnection ?? (_systemUserConnection = (SystemUserConnection)AppConnection.SystemUserConnection);
			}
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bankLineId"></param>
		/// <returns></returns>
		/// <remarks>
		/// To access this endpoint follow <see href="http://k_krylov:7020/0/rest/DemoServiceOne/PostMethodName"> this link</see>
		/// </remarks>
		#region Methods : REST
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		public string PostMethodName(ContactModel cm)
		{
			UserConnection userConnection = UserConnection ?? SystemUserConnection;
			const string tableName = "Contact";
			EntitySchema contactSchema = UserConnection.EntitySchemaManager.GetInstanceByName(tableName);
			Entity contact = contactSchema.CreateEntity(UserConnection);
			contact.SetDefColumnValues();

			Guid id = Guid.NewGuid();
			contact.SetColumnValue("Id", id);
			contact.SetColumnValue("Name", cm.Name);
			contact.SetColumnValue("Email",cm.Email);
			contact.SetColumnValue("BirthDate", cm.BirthDate);

			if (contact.Save())
			{
				return id.ToString(); ;
			}
			return "Error Occurred";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bankLineId"></param>
		/// <returns></returns>
		/// <remarks>
		/// To access this endpoint follow <see href="http://k_krylov:7020/0/rest/DemoServiceOne/GetMethodname"> this link</see>
		/// </remarks>
		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.WrappedRequest )]
		public IEnumerable<ContactModel> GetMethodname(string email)
		{
			UserConnection userConnection = UserConnection ?? SystemUserConnection;

			const string tableName = "Contact";
			EntitySchema contactSchema = UserConnection.EntitySchemaManager.GetInstanceByName(tableName);
			Entity contact = contactSchema.CreateEntity(UserConnection);
			contact.FetchFromDB("Email", email);

			List<ContactModel> listCm = new List<ContactModel>();

			ContactModel cm = new ContactModel
			{
				Name = contact.GetTypedColumnValue<string>("Name"),
				Email = contact.GetTypedColumnValue<string>("Email"),
				BirthDate = contact.GetTypedColumnValue<DateTime>("BirthDate")
			};
			listCm.Add(cm);
			return listCm;
		}

		#endregion

		#region Methods : Private

		#endregion
	}
}



