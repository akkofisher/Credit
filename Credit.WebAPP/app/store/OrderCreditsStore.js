Ext.define('CreditAPP.store.OrderCreditsStore', {
    extend: 'Ext.data.Store',
    alias: 'store.ordercreditstore',

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

    pageSize: 100,

    proxy: {
        type: 'ajax',
        url: 'http://localhost:5000/Api/Credit/GetPersonOrderCredits', 
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