Ext.define('CreditAPP.store.OrdersCreditsFromQueueStore', {
    extend: 'Ext.data.Store',
    alias: 'store.orderscreditsfromqueuestore',

    fields: [
        'id', 
        'requestAmount', 
        'currency', 
        'status',
        { name: 'periodStart', type: 'date', dateFormat: 'Y-m-d' }, 
        { name: 'periodEnd', type: 'date', dateFormat: 'Y-m-d' }, 
        'personId', 
        { name: 'person', type: 'auto' }
    ],

    proxy: {
        type: 'ajax',
        url: 'http://localhost:5000/Api/AdminCredit/GetOrdersCreditsFromQueue', 
        reader: {
            type: 'json',
            rootProperty: 'data' 
        },
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('authToken') 
        }
    },

    autoLoad: false
});