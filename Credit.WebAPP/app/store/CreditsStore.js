Ext.define('CreditAPP.store.CreditsStore', {
    extend: 'Ext.data.Store',
    alias: 'store.creditstore',

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
        url: 'http://localhost:5000/Api/AdminCredit/GetCredits', 
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