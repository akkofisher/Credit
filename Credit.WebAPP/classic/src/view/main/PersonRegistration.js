Ext.define('CreditAPP.view.main.RegisterForm', {
    extend: 'Ext.form.Panel',
    xtype: 'PersonRegistration',

    title: 'Register New Person',
    bodyPadding: 10,
    width: 400,

    url: 'http://localhost:5000/Api/Person/AddPerson', 

    items: [
        {
            xtype: 'textfield',
            name: 'Email',
            fieldLabel: 'Email',
            allowBlank: false, 
            vtype: 'email',
            emptyText: 'Enter your email'
        },
        {
            xtype: 'textfield',
            name: 'FirstName',
            fieldLabel: 'First Name',
            allowBlank: false, 
            emptyText: 'Enter your first name'
        },
        {
            xtype: 'textfield',
            name: 'LastName',
            fieldLabel: 'Last Name',
            allowBlank: false, 
            emptyText: 'Enter your last name'
        },
        {
            xtype: 'textfield',
            name: 'PersonalNumber',
            fieldLabel: 'Personal Number',
            allowBlank: false, 
            emptyText: 'Enter your personal number'
        },
        {
            xtype: 'datefield',
            name: 'DateOfBirth',
            fieldLabel: 'Date of Birth',
            allowBlank: false, 
            format: 'Y-m-d',
            emptyText: 'Enter your date of birth'
        },
        {
            xtype: 'textfield',
            name: 'Password',
            fieldLabel: 'Password',
            inputType: 'password',
            allowBlank: false, 
            emptyText: 'Enter your password'
        },
        {
            xtype: 'textfield',
            name: 'ConfirmPassword',
            fieldLabel: 'Confirm Password',
            inputType: 'password',
            allowBlank: false, 
            emptyText: 'Confirm your password'
        }
    ],

    buttons: [
        {
            text: 'Register',
            formBind: true, 
            disabled: true,
            handler: function () {
                var form = this.up('form').getForm(); 

                var formData = form.getValues();

                  var dob = form.findField('DateOfBirth').getValue();
                  if (dob) {
                      formData.DateOfBirth = Ext.Date.format(dob, 'Y-m-d'); // Format date as yyyy-MM-dd
                  }

                if (form.isValid()) {
                    Ext.Ajax.request({
                        url: 'http://localhost:5000/api/Person/AddPerson',
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        jsonData: formData,
                        success: function (response) {
                            var responseData = Ext.decode(response.responseText);

                            Ext.Msg.alert('Success', 'Register Person successfully.', function() {
                                Ext.Viewport.add({
                                    xtype: 'mainlist'
                                });
                                Ext.Viewport.setActiveItem(1); 

                            });
                        },
                        failure: function (response) {
                            var responseData = Ext.decode(response.responseText);

                            //if responseData.errors is not null the write all errors array in a string
                            if (responseData && responseData.errors) {
                                var errors = '';
                                responseData.errors.forEach(function(error) {
                                    errors += error + '<br>';
                                });
                                Ext.Msg.alert('Failed', errors);
                            }
                            else if (responseData && responseData.error) {
                                Ext.Msg.alert('Failed', responseData.error);
                            } else {
                                Ext.Msg.alert('Failed', 'Login failed. Please try again.');
                            }
                        },
                        accept: 'application/json',
                    });
                }
            }
        },
        {
            text: 'Reset',
            handler: function () {
                this.up('form').getForm().reset(); 
            }
        }
    ],

});
