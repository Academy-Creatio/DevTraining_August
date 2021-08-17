using DevAugust.API;
using Terrasoft.Configuration;
using Terrasoft.Core;
using Terrasoft.Core.Factories;

namespace DevAugust
{
	[DefaultBinding(typeof(IConfToClio))]
	public class ConfToClio : IConfToClio
	{
		public void PostMessage(UserConnection userConnection, string senderName, string messageText)
		{
			MsgChannelUtilities.PostMessage(userConnection,
					senderName, messageText);
		}
		
		public void PostMessageToAll( string senderName, string messageText)
		{
			MsgChannelUtilities.PostMessageToAll(senderName, messageText);
		}

	}
} 