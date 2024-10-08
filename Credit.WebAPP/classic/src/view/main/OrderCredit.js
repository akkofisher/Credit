Ext.define('CreditAPP.view.main.OrderCreditForm', {
    extend: 'Ext.form.Panel',
    xtype: 'OrderCredit',

    title: 'Order Credit',
    width: 400,
    bodyPadding: 10,
    url: 'http://localhost:5000/api/Credit/OrderCredit',

 
    items: [
        {
            xtype: 'numberfield',
            name: 'RequestAmount',
            fieldLabel: 'Request Amount',
            allowBlank: false,
            minValue: 0, 
            emptyText: 'Enter the requested amount'
        },
        {
            xtype: 'combobox',
            name: 'Currency',
            fieldLabel: 'Currency',
            store: {
                fields: ['id', 'name'],
                data: [
                    { id: 1, name: 'GEL' },
                    { id: 2, name: 'USD' },
                    { id: 3, name: 'Euro' },
                    { id: 4, name: 'GBP' }
                ]
            },
            queryMode: 'local',
            displayField: 'name',
            valueField: 'id',
            editable: false, 
            allowBlank: false, 
            emptyText: 'Select a currency'
        },
        {
            xtype: 'datefield',
            name: 'PeriodStart',
            fieldLabel: 'Period Start',
            format: 'Y-m-d',
            allowBlank: false,
            emptyText: 'Select start period'
        },
        {
            xtype: 'datefield',
            name: 'PeriodEnd',
            fieldLabel: 'Period End',
            format: 'Y-m-d', 
            allowBlank: false,
            emptyText: 'Select end period'
        }
    ],

    buttons: [
        {
            text: 'Submit',
            formBind: true, 
            disabled: true,
            handler: function () {
                var form = this.up('form').getForm();

                if (form.isValid()) {
                    var formData = form.getValues();
                    var token = sessionStorage.getItem('authToken');

                    if (!token) {
                        Ext.Msg.alert('Error', 'Authentication token not found. Please log in.');
                        return;
                    }

                    Ext.Ajax.request({
                        url: 'http://localhost:5000/api/Credit/OrderCredit',
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + token 
                        },
                        jsonData: formData,
                        success: function (response) {
                            Ext.Msg.alert('Success', 'Order credit submitted successfully.');
                        },
                        failure: function (response) {
                            Ext.Msg.alert('Failed', 'Failed to submit the order credit. Please try again.');
                        }
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
