using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;

namespace DevAugust.API
{
	/// <summary>
	/// Features of Terrasoft.Confiiguration exposed to clio package
	/// </summary>
	public interface IConfToClio
	{
		/// <summary>
		/// Sends web socket message to a user identified by the <paramref name="userConnection"/>
		/// </summary>
		/// <param name="userConnection"></param>
		/// <param name="senderName"></param>
		/// <param name="messageText"></param>
		/// <remarks>
		/// See <seealso href="https://academy.creatio.com/docs/developer/front-end_development/websocket_message_sending_mechanism/the_client-side_websocket_message_handler"> academy documentation </seealso> for details
		/// </remarks>
		void PostMessage(UserConnection userConnection, string senderName, string messageText);

		/// <summary>
		/// Broadcasts web socket message to all users
		/// </summary>
		/// <param name="senderName"></param>
		/// <param name="messageText"></param>
		/// <remarks>
		/// See <seealso href="https://academy.creatio.com/docs/developer/front-end_development/websocket_message_sending_mechanism/the_client-side_websocket_message_handler"> academy documentation </seealso> for details
		/// </remarks>
		void PostMessageToAll(string senderName, string messageText);
	}
}
