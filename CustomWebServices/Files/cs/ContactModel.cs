using System;
using System.Runtime.Serialization;

namespace CustomWebService
{
	[DataContract]
	public class ContactModel
	{
		[DataMember(Name="name")]
		public string Name { get; set; }

		[DataMember(Name = "email")]
		public string Email { get; set; }

		[DataMember(Name = "birthDate")]
		public DateTime BirthDate { get; set; }
	}
}



