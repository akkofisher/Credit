Ext.define('CreditAPP.view.main.OrderCreditsGrid', {
    extend: 'Ext.grid.Panel',
    xtype: 'Ordercreditsgrid',

    title: 'Person Order Credits',
    store: {
        type: 'ordercreditstore'
    },

    //editing plugin
    plugins: [{
        ptype: 'rowediting',
        clicksToEdit: 1
    }],

    columns: [
        {
            text: 'Id',
            dataIndex: 'id',
            flex: 1
        },
        {
            text: 'Request Amount',
            dataIndex: 'requestAmount',
            flex: 1,
            renderer: function (value, metaData, record) {
                var currency = record.get('currency');
                switch (currency) {
                    case 1:
                        return '₾' + value;
                    case 2:
                        return '$' + value;
                    case 3:
                        return '€' + value;
                    case 4:
                        return '£' + value;
                    default:
                        return value;
                }
            },
            editor: {
                xtype: 'numberfield',
                allowBlank: false
            },
        },
        {
            text: 'Currency',
            dataIndex: 'currency',
            flex: 1,
            renderer: function (value) {

                switch (value) {
                    case 1: return 'GEL';
                    case 2: return 'USD';
                    case 3: return 'Euro';
                    case 4: return 'GBP';
                    default: return 'Unknown';
                }
            },
            editor: {
                xtype: 'combobox',
                store: [
                    [1, 'GEL'],
                    [2, 'USD'],
                    [3, 'Euro'],
                    [4, 'GBP']
                ],
                editable: false,
                allowBlank: false
            }
        },
        {
            text: 'Period Start',
            dataIndex: 'periodStart',
            flex: 1,
            xtype: 'datecolumn',
            format: 'Y-m-d',
            editor: {
                xtype: 'datefield',
                format: 'Y-m-d',
                allowBlank: false
            }
        },
        {
            text: 'Period End',
            dataIndex: 'periodEnd',
            flex: 1,
            xtype: 'datecolumn',
            format: 'Y-m-d',
            editor: {
                xtype: 'datefield',
                format: 'Y-m-d',
                allowBlank: false
            }
        },
        // {
        //     text: 'First Name',
        //     dataIndex: 'person',
        //     flex: 1,
        //     renderer: function (value) {
        //         return value.firstName;
        //     }
        // },
        // {
        //     text: 'Last Name',
        //     dataIndex: 'person',
        //     flex: 1,
        //     renderer: function (value) {
        //         return value.lastName;
        //     }
        // },
        {
            text: 'Status',
            dataIndex: 'status',
            flex: 1,
            renderer: function (value) {
                switch (value) {
                    case 0: return 'On Edit';
                    case 1: return 'Pending';
                    case 2: return 'Approved';
                    case 3: return 'Rejected';
                    default: return 'Unknown';
                }
            }
        },
        {
            xtype: 'actioncolumn',
            width: 100,
            items: [
                {
                    iconCls: 'x-fa fa-trash',
                    tooltip: 'Delete',
                    isDisabled: function (view, rowIndex, colIndex, item, record) {
                        return record.get('status') !== 0;
                    },
                    handler: function (grid, rowIndex, colIndex) {

                        var record = grid.getStore().getAt(rowIndex);
                        Ext.Msg.confirm('Delete', 'Are you sure you want to delete this record?', function (choice) {
                            if (choice === 'yes') {
                                Ext.Ajax.request({
                                    url: 'http://localhost:5000/api/credit/DeleteCredit?id=' + record.get('id'),
                                    method: 'DELETE',
                                    headers: {
                                        'Authorization': 'Bearer ' + sessionStorage.getItem('authToken')
                                    },
                                    success: function (response) {
                                        Ext.Msg.alert('Success', 'Record deleted successfully');
                                        grid.getStore().reload();
                                    },
                                    failure: function (response) {
                                        Ext.Msg.alert('Error', 'Failed to delete record');
                                    }
                                });
                            }
                        });
                    }
                },
                {
                    iconCls: 'x-fa fa-paper-plane',
                    tooltip: 'Send',
                    isDisabled: function (view, rowIndex, colIndex, item, record) {
                        return record.get('status') !== 0;
                    },
                    handler: function (grid, rowIndex, colIndex) {
                        var record = grid.getStore().getAt(rowIndex);
                        Ext.Msg.confirm('Send', 'Are you sure you want to send this record?', function (choice) {
                            if (choice === 'yes') {
                                Ext.Ajax.request({
                                    url: 'http://localhost:5000/api/credit/SendCredit?id=' + record.get('id'),
                                    method: 'POST',
                                    headers: {
                                        'Authorization': 'Bearer ' + sessionStorage.getItem('authToken')
                                    },
                                    success: function (response) {
                                        Ext.Msg.alert('Success', 'Credit sent successfully');
                                        grid.getStore().reload();
                                    },
                                    failure: function (response) {
                                        Ext.Msg.alert('Error', 'Failed to send credit');
                                    }
                                });
                            }
                        });
                    }
                }
            ]
        },
    ],

    listeners: {
        afterrender: function () {
            var store = this.getStore();
            if (!store.isLoaded()) {
                store.load();
            }
        },
        edit: function (editor, context) {
            var updatedData = context.record.getData();

            updatedData.periodStart = Ext.Date.format(context.record.get('periodStart'), 'Y-m-d');
            updatedData.periodEnd = Ext.Date.format(context.record.get('periodEnd'), 'Y-m-d');

            Ext.Ajax.request({
                url: 'http://localhost:5000/API/Credit/EditCredit',
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + sessionStorage.getItem('authToken')
                },
                jsonData: updatedData,
                success: function (response) {
                    Ext.Msg.alert('Success', 'Record updated successfully');
                    // context.record.commit(); 
                    context.grid.getStore().reload();
                },
                failure: function (response) {
                    Ext.Msg.alert('Error', 'Failed to update record');
                }
            });
        },
    },

    height: 800,
    width: 1024,
});
