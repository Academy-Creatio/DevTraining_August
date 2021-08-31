define("ContactPageV2", ["ServiceHelper"], function(ServiceHelper) {
	return {
		entitySchemaName: "Contact",
		messages:{
			/**
			 * Published on: ContactSectionV2 iin FrontEnd package
			 * @tutorial https://academy.creatio.com/docs/developer/front-end_development/sandbox_component/module_message_exchange
			 */
			 "SectionActionClicked":{
				mode: this.Terrasoft.MessageMode.PTP,
				direction: this.Terrasoft.MessageDirectionType.SUBSCRIBE
			}
		},
		attributes: {
			"MyEvents": {
				dependencies: [
					{
						columns: ["Phone", "MobilePhone"],
						methodName: "onPhoneChanged"
					},
					{
						columns: ["Email"],
						methodName: "onEmailChanged"
					}
				]
			},
			"Account":{
				lookupListConfig: {
					columns: ["Web"]
				}
			}
		},
		modules: /**SCHEMA_MODULES*/{}/**SCHEMA_MODULES*/,
		details: /**SCHEMA_DETAILS*/{}/**SCHEMA_DETAILS*/,
		businessRules: /**SCHEMA_BUSINESS_RULES*/{}/**SCHEMA_BUSINESS_RULES*/,
		methods: {
				
			/**
			 * Initializes the initial values of the model.
			 * @inheritdoc Terrasoft.BasePageV2#init
			 * @overridden
			 */
			 init: function() {
				this.callParent(arguments);
				this.subscribeToMessages();
			},

			subscribeToMessages: function(){
				this.sandbox.subscribe(
					"SectionActionClicked",
					function(){this.onSectionMessageReceived();},
					this,
					[this.sandbox.id]
				)
			},
			onSectionMessageReceived: function(){
				this.showInformationDialog("Message received");
				
			},


			/**
			 * Handler of the entity initialized.
			 * @inheritdoc Terrasoft.BasePageV2#onEntityInitialized
			 * @overridden
			 * @protected
			 */
			onEntityInitialized: function() {
				this.callParent(arguments);
			},


			onPhoneChanged: function(a, columnChanged){
				var newValue = this.get(columnChanged);
				this.showInformationDialog(columnChanged+" has changed new value is: "+ newValue + " - handled with onPhoneChanged");
			},
			
			onEmailChanged: function(a, columnChanged){
				
				var accountDomain = "";
				if(this.$Account.Web){
					accountDomain = this.$Account.Web.split("://")[1];
				}

				var email = this.$Email;
				var emailDomain = "";
				if(email){
					emailDomain = this.$Email.split("@")[1];
				}

				if(emailDomain !== accountDomain){
					//this.showInformationDialog("Domains do not match, make sure you meant to do this");
				}
			},

			/**
			 * Returns collection actions card.
			 * @inheritdoc Terrasoft.BasePageV2#getActions
			 * @overridden
			 */
			 getActions: function() {
				var actionMenuItems = this.callParent(arguments);
				actionMenuItems.addItem(this.getButtonMenuSeparator());
				
				actionMenuItems.addItem(this.getButtonMenuItem({
					"Tag": "action1",
					"Caption": this.get("Resources.Strings.ActionOneCaption"),
					"Click": {"bindTo": "onActionClick"},
					ImageConfig: this.get("Resources.Images.CreatioSquare"),
				}));

				actionMenuItems.addItem(this.getButtonMenuItem({
					"Tag": "action2",
					"Caption": this.get("Resources.Strings.ActionTwoCaption"),
					"Click": {"bindTo": "onActionClick"},
					"Items": this.addSubItems()
				}));

				return actionMenuItems;
			},


			/**
			 * Handles action click
			 */
			 onActionClick: function(tag){
				this.showInformationDialog(tag+" pressed");
			 },

			 /**
			  * Add subitems to Actions Menu
			  * @returns collection of MenuItems
			  */
			 addSubItems: function(){
				var collection = this.Ext.create("Terrasoft.BaseViewModelCollection");
				collection.addItem(this.getButtonMenuItem({
					"Caption": this.get("Resources.Strings.SubActionOneCaption"),
					"Click": {"bindTo": "onActionClick"},
					"Tag": "sub1"
				}));
				collection.addItem(this.getButtonMenuItem({
					"Caption": this.get("Resources.Strings.SubActionTwoCaption"),
					"Click": {"bindTo": "onActionClick"},
					"Tag": "sub2"
				}));
				return collection;
			},

			/** Sets up synchronous validation, not suitable for async methods such as database requests or webservice calls
			 * @inheritdoc BaseSchemaViewModel#setValidationConfig
			 * @override
			 */
			 setValidationConfig: function() {
				this.callParent(arguments);
				//this.addColumnValidator("Email", this.emailValidator);
			},
			emailValidator: function() {
				var invalidMessage= ""; //good
				var newValue = this.$Email;
				var corpDomain = this.$Account.Web.split("://")[1];
				
				if (newValue.split("@")[1] !== corpDomain) {
					invalidMessage = "Primary email has to match to corporate domain."; //bad
				}
				else {
					invalidMessage = ""; //good
				}
				return {
					invalidMessage: invalidMessage
				};
			},

			/**
			 * If you only need to perform one async method, use this 
			 * Validates model values. Sends validate results to callback function.
			 * Using Terrasoft.chain allows us to execute multiple methods.
			 * For example first we validate checkEmailIsUnique and when the result comes back
			 * chain moves to checkEmailIsUnique.
			 * Terrasoft.chain will execute async methods synchronously
			 * @inheritdoc Terrasoft.BaseEntityV2#asyncValidate 
			 * @overridden
			 * @param {Function} callback Callback-function.
			 * @param {Object} scope Execution context.
			 */
			/*
			asyncValidate: function(callback, scope) 
			{
				this.callParent([
					function(response){
						if (response.success){
							this.checkEmailIsUnique(callback, scope || this);
						}
						else {
							callback.call(scope || this, response);
						}
					}, 
					this
				]);
			},
			*/
			asyncValidate: function(callback, scope) {
				this.callParent([function(response) {
					if (!this.validateResponse(response)) {
						return;
					}
					this.Terrasoft.chain(
						function(next) {

							this.checkEmailIsUniqueServer(function(response) {
								if (this.validateResponse(response)) {
									next();
								}
							}, this);
						},
						// function(next) {
						// 	this.checkEmailIsUnique(function(response) {
						// 		if (this.validateResponse(response)) {
						// 			next();
						// 		}
						// 	}, this);
						// },
						function(next) {
							callback.call(scope, response);
							next();	
						}, this);
				}, this]);
			},
			
			
			/** Validates that the email of the currently modified contact is unique */
			checkEmailIsUnique: function (callback, scope) 
			{
				var esq = Ext.create("Terrasoft.EntitySchemaQuery", { rootSchemaName: "Contact" });
				esq.addColumn("Email", "Email");
				
				var esqFirstFilter = esq.createColumnFilterWithParameter(
					Terrasoft.ComparisonType.EQUAL, "Email", this.$Email
				);
				
				var esqSecondFilter = esq.createColumnFilterWithParameter(
					Terrasoft.ComparisonType.NOT_EQUAL, "Id", this.$Id
				);

				esq.filters.logicalOperation = Terrasoft.LogicalOperatorType.AND;
				esq.filters.add("esqFirstFilter", esqFirstFilter);
				esq.filters.add("esqSecondFilter", esqSecondFilter);

				esq.getEntityCollection(function (result) {
					if (result.success && result.collection.getCount()>0) {
						if (callback) {
							callback.call(scope, {
									success: false,
									message: "Email has to be unique, checked with UI"
								}
							);
						}
					}
					else {
						if (callback) {
							callback.call(scope, 
								{success: true}
							);
						}
					}
				}, 
				this);
			},

			checkEmailIsUniqueServer: function (callback, scope) {
				
				//Payload
				var serviceData = {
					"person":{
						"email": this.$Email,
						"name": "",
						"age": 0
					}	
				};

				ServiceHelper.callService(
					"CustomExample",  //CS - ClassName
					"PostMethodName", //CS - Method
					function(response) 
					{
						var result = response.PostMethodNameResult;
						if(result.length >1){
							if (callback) {
								callback.call(scope, {
										success: false,
										message: "Email has to be unique, checked with WebService"
									}
								);
							}
						}
						else{
							if (callback) {
								callback.call(scope, 
									{success: true}
								);
							}
						}
					}, 
					serviceData, 
					this
				);
			}


		},
		dataModels: /**SCHEMA_DATA_MODELS*/{}/**SCHEMA_DATA_MODELS*/,
		diff: /**SCHEMA_DIFF*/[

			/** BUTTONS in left container */
			{
				"operation": "insert",
				"name": "PrimaryContactButtonRed",
				"parentName": "LeftContainer",
				"propertyName": "items",
				"values":{
					itemType: this.Terrasoft.ViewItemType.BUTTON,
					style: Terrasoft.controls.ButtonEnums.style.RED,
					classes: {
						"textClass": ["actions-button-margin-right"],
						"wrapperClass": ["actions-button-margin-right"]
					},
					caption: {bindTo: "Resources.Strings.MyRedBtnCaption"},
					hint: {bindTo:"Resources.Strings.MyRedBtnHint"},
					click: {"bindTo": "onMyMainButtonClick"},
					tag: "LeftContainer_Red"
				}
			},
			{
				"operation": "insert",
				"name": "MyGreenButton",
				"parentName": "LeftContainer",
				"propertyName": "items",
				"values":{
					"itemType": this.Terrasoft.ViewItemType.BUTTON,
					"style": Terrasoft.controls.ButtonEnums.style.GREEN,
					classes: {
						"textClass": ["actions-button-margin-right"],
						"wrapperClass": ["actions-button-margin-right"]
					},
					"caption": "Page Green button",
					"hint": "Page green button hint <a href=\"https://google.ca\"> Link to help",
					"click": {"bindTo": "onMyMainButtonClick"},
					tag: "LeftContainer_Green",
					"menu":{
						"items": [
							{
								caption: "Sub Item 1",
								click: {bindTo: "onMySubButtonClick"},
								visible: true,
								hint: "Sub item 1 hint",
								tag: "subItem1"
							},
							{
								caption: "Sub Item 2",
								click: {bindTo: "onMySubButtonClick"},
								visible: true,
								hint: "Sub item 2 hint",
								tag: "subItem2"
							}
						]
					}
				}
			}
		]/**SCHEMA_DIFF*/
	};
});