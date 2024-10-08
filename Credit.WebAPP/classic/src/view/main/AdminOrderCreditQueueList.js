Ext.define('CreditAPP.view.main.AdminOrderCreditQueueListGrid', {
    extend: 'Ext.grid.Panel',
    xtype: 'Adminordercreditqueue',

    title: 'Person Order Credits',
    store: {
        type: 'orderscreditsfromqueuestore'
    },

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
        },
        {
            text: 'Period Start',
            dataIndex: 'periodStart',
            flex: 1,
            xtype: 'datecolumn',
            format: 'Y-m-d',
        },
        {
            text: 'Period End',
            dataIndex: 'periodEnd',
            flex: 1,
            xtype: 'datecolumn',
            format: 'Y-m-d',
        },
        {
            text: 'First Name',
            dataIndex: 'person',
            flex: 1,
            renderer: function (value) {
                return value.firstName;
            }
        },
        {
            text: 'Last Name',
            dataIndex: 'person',
            flex: 1,
            renderer: function (value) {
                return value.lastName;
            }
        },
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
                    iconCls: 'x-fa fa-cog',
                    tooltip: 'Change Status',
                    handler: function (grid, rowIndex) {
                        var record = grid.getStore().getAt(rowIndex);
                        showStatusChangeWindowQueue(record, grid);
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
        }
    },

    height: 800,
    width: 1024,

});

function showStatusChangeWindowQueue(record, grid) {
    Ext.create('Ext.window.Window', {
        title: 'Change Order Status',
        modal: true,
        width: 300,
        items: [
            {
                xtype: 'button',
                text: 'Approve',
                margin: '10',
                cls: 'approve-btn',
                handler: function () {
                    changeOrderCreditStatusQueue(record.get('id'), 2, grid);
                    this.up('window').close();
                }
            },
            {
                xtype: 'button',
                text: 'Decline',
                cls: 'decline-btn',
                margin: '10',
                handler: function () {
                    changeOrderCreditStatusQueue(record.get('id'), 3, grid);
                    this.up('window').close();
                }
            }
        ],
        buttons: [
            {
                text: 'Close',
                handler: function () {
                    this.up('window').close();
                }
            }
        ]
    }).show();
}


function changeOrderCreditStatusQueue(id, status, grid) {
    Ext.Ajax.request({
        url: 'http://localhost:5000/Api/AdminCredit/ChangeOrderCreditStatus',
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + sessionStorage.getItem('authToken')
        },
        jsonData: {
            Id: id,
            Status: status
        },
        success: function (response) {
            Ext.Msg.alert('Success', 'Order status updated successfully.');

            //remove item from grid
            var store = grid.getStore();
            var record = store.findRecord('id', id);
            store.remove(record);
        },
        failure: function (response) {
            Ext.Msg.alert('Error', 'Failed to update order status.');
        }
    });
}