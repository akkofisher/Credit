Ext.define('CreditAPP.view.main.Login', {
    extend: 'Ext.form.Panel', 
    xtype: 'Login',

    title: 'Login',
    width: 300,
    bodyPadding: 10,

    items: [
        {
            xtype: 'textfield',
            name: 'email',
            fieldLabel: 'Email',
            allowBlank: false,
            vtype: 'email', 
            emptyText: 'Enter your email'
        },
        {
            xtype: 'textfield',
            name: 'password',
            fieldLabel: 'Password',
            allowBlank: false,
            inputType: 'password', 
            emptyText: 'Enter your password'
        }
    ],

    buttons: [
        {
            text: 'Login',
            formBind: true, 
            disabled: true,
            handler: function () {
                var form = this.up('form').getForm();
                if (form.isValid()) {
                    var formData = form.getValues();

                    Ext.Ajax.request({
                        url: 'http://localhost:5000/api/Person/LoginPerson',
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        jsonData: formData,
                        success: function (response) {
                            var responseData = Ext.decode(response.responseText);

                            sessionStorage.setItem('authToken', responseData.data);

                            Ext.Msg.alert('Success', 'Logged in successfully.', function() {
                                Ext.Viewport.add({
                                    xtype: 'mainlist'
                                });
                                Ext.Viewport.setActiveItem(1); 
                            });
                        },
                        failure: function (response) {
                            var responseData = Ext.decode(response.responseText);
                            
                            if (responseData && responseData.error) {
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
    ]
});
