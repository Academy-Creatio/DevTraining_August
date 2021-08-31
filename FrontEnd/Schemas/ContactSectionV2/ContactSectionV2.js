define("ContactSectionV2", ["ProcessModuleUtilities"], function(ProcessModuleUtilities) {
	return {
		entitySchemaName: "Contact",
		messages:{
			
			/**
			 * Subscribed on: ContactPageV2 in FrontEnd package
			 * @tutorial https://academy.creatio.com/docs/developer/front-end_development/sandbox_component/module_message_exchange
			 */
			"SectionActionClicked":{
				mode: this.Terrasoft.MessageMode.PTP,
				direction: this.Terrasoft.MessageDirectionType.PUBLISH
			}
		},
		details: /**SCHEMA_DETAILS*/{}/**SCHEMA_DETAILS*/,
		methods: {

			onMyMainButtonClick: function(a,b,c,tag){
				if(tag === "CombinedModeActionButtonsCardLeftContainer_Red"){
					this.sandbox.publish("SectionActionClicked", null, [this.sandbox.id+"_CardModuleV2"]);
				}
				if(tag === "ActionButtonsContainer_Red" && this.$ActiveRow){
					
					var primaryColumnValue = this.$ActiveRow
					this.runProcess(primaryColumnValue);
				}
			},
			getSectionActions: function() {
				var actionMenuItems = this.callParent(arguments);
				actionMenuItems.addItem(this.getButtonMenuSeparator());
				actionMenuItems.addItem(this.getButtonMenuItem({
					"Tag": "action1",
					"Caption": "Section Action One",
					"Click": {"bindTo": "onActionClick"},
				}));
				actionMenuItems.addItem(this.getButtonMenuItem({
					"Tag": "action2",
					"Caption": "Section Action Two",
					"Click": {"bindTo": "onActionClick"}
				}));
				return actionMenuItems;
			},
			onActionClick: function(tag){
				this.showInformationDialog("Section Action Clicked with tag: "+ tag);
			},


			runProcess: function(primaryColumnValue){
				var scope = this;
				var args = {
					sysProcessName: "Process_c36e957",
					parameters:{
						contactId: primaryColumnValue
					},
					callback: this.onProcessCompleted(),
					scope: scope
				}
				ProcessModuleUtilities.executeProcess(args);
			},

			onProcessCompleted: function(){

				this.showInformationDialog("Process Completed");
			}
		},
		diff: /**SCHEMA_DIFF*/[
			{
				"operation": "insert",
				"name": "PrimaryContactButtonRed",
				"parentName": "CombinedModeActionButtonsCardLeftContainer", //INVISIBLE in section, visible on the page
				"propertyName": "items",
				"values":{
					itemType: this.Terrasoft.ViewItemType.BUTTON,
					style: Terrasoft.controls.ButtonEnums.style.RED,
					classes: {
						"textClass": ["actions-button-margin-right"],
						"wrapperClass": ["actions-button-margin-right"]
					},
					caption: "Section Red Button",
					hint: "Section red button hint",
					click: {"bindTo": "onMyMainButtonClick"},
					tag: "CombinedModeActionButtonsCardLeftContainer_Red"
				}
			},
			{
				"operation": "insert",
				"name": "PrimaryContactButtonGreen",
				//"parentName": "ActionButtonsContainer", //visible in section and on a page
				//"parentName": "SeparateModeActionButtonsRightContainer", //container on the right where Print button goes
				"parentName": "SeparateModeActionButtonsLeftContainer", //container on the right where Print button goes
				"propertyName": "items",
				"values":{
					itemType: this.Terrasoft.ViewItemType.BUTTON,
					style: Terrasoft.controls.ButtonEnums.style.GREEN,
					classes: {
						"textClass": ["actions-button-margin-right"],
						"wrapperClass": ["actions-button-margin-right"]
					},
					caption: "Section Green Button",
					hint: "Section red button hint",
					click: {"bindTo": "onMyMainButtonClick"},
					tag: "ActionButtonsContainer_Red"
				}
			},
		]/**SCHEMA_DIFF*/
	};
});
