using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Events;
using global::Common.Logging;
using Terrasoft.Common;

namespace CreatioEvents.Files.cs
{
	/// <summary>
	/// Listener for 'EntityName' entity events.
	/// </summary>
	/// <seealso cref="Terrasoft.Core.Entities.Events.BaseEntityEventListener" />
	[EntityEventListener(SchemaName = "Contact")]
	class ContactEventListener : BaseEntityEventListener
	{
		#region Enum
		#endregion

		#region Delegates
		#endregion

		#region Constants
		#endregion

		#region Fields

		#region Fileds : Private
		ILog _logger = LogManager.GetLogger("GuidedLearningLogger");
		#endregion

		#region Fileds : Protected
		#endregion

		#region Fileds : Internal
		#endregion

		#region Fileds : Protected Internal
		#endregion

		#region Fileds : Public
		#endregion

		#endregion

		#region Properties

		#region Properties : Private
		#endregion

		#region Properties : Protected
		#endregion

		#region Properties : Internal
		#endregion

		#region Properties : Protected Internal
		#endregion

		#region Properties : Public
		#endregion

		#endregion

		#region Events
		#endregion

		#region Methods

		#region Methods : Private

		#endregion

		#region Methods : Public

		#region Methods : Public : OnSave
		public override void OnSaving(object sender, EntityBeforeEventArgs e)
		{
			base.OnSaving(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
		}
		public override void OnSaved(object sender, EntityAfterEventArgs e)
		{
			base.OnSaved(sender, e);
			//Entity entity = (Entity)sender;
			//UserConnection userConnection = entity.UserConnection;
		}
		#endregion

		#region Methods : Public : OnInsert
		public override void OnInserting(object sender, EntityBeforeEventArgs e)
		{
			base.OnInserting(sender, e);
			_logger.Info("BEFORE Record Created, handled with ContactEventListener");

			Entity entity = (Entity)sender;
			entity.Validating += Entity_Validating;
			

			string name = entity.GetTypedColumnValue<string>("Name");
			string notes = entity.GetTypedColumnValue<string>("Notes");
			entity.SetColumnValue("Notes", $"{notes} - {name}");
		}
		public override void OnInserted(object sender, EntityAfterEventArgs e)
		{
			base.OnInserted(sender, e);
			_logger.Info("Record Created, handled with ContactEventListener");
		}

		private void Entity_Validating(object sender, EntityValidationEventArgs e)
		{
			Entity entity = (Entity)sender;
			string email = entity.GetTypedColumnValue<string>("Email");

			if (email.Contains("ac.com"))
			{
				EntityValidationMessage evm = new EntityValidationMessage
				{
					MassageType = MessageType.Error,
					Text = "ac.com domain is not allowed for any Contact",
					Column = entity.Schema.Columns.FindByName("Email")
				};
				entity.ValidationMessages.Add(evm);
				_logger.Info("Contact Validating, handled with ContactEventListener");
			}
		}
		#endregion

		#region Methods : Public : OnUpdate
		public override void OnUpdating(object sender, EntityBeforeEventArgs e)
		{
			base.OnUpdating(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
		}
		public override void OnUpdated(object sender, EntityAfterEventArgs e)
		{
			base.OnUpdated(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
		}
		#endregion

		#region Methods : Public : OnDelete
		public override void OnDeleting(object sender, EntityBeforeEventArgs e)
		{
			base.OnDeleting(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
		}
		public override void OnDeleted(object sender, EntityAfterEventArgs e)
		{
			base.OnDeleted(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
		}
		#endregion

		#endregion

		#endregion
	}
}
