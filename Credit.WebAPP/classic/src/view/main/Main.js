/**
 * This class is the main view for the application. It is specified in app.js as the
 * "mainView" property. That setting automatically applies the "viewport"
 * plugin causing this view to become the body element (i.e., the viewport).
 *
 * TODO - Replace this content of this view to suite the needs of your application.
 */
Ext.define('CreditAPP.view.main.Main', {
    extend: 'Ext.tab.Panel',
    xtype: 'app-main',

    requires: [
        'Ext.plugin.Viewport',
        'Ext.window.MessageBox',

        'CreditAPP.view.main.MainController',
        'CreditAPP.view.main.MainModel',
    ],

    controller: 'main',
    viewModel: 'main',

    ui: 'navigation',

    tabBarHeaderPosition: 1,
    titleRotation: 0,
    tabRotation: 0,

    header: {
        layout: {
            align: 'stretchmax'
        },
        title: {
            bind: {
                text: '{name}'
            },
            flex: 0
        },
        iconCls: 'fa-th-list'
    },

    tabBar: {
        flex: 1,
        layout: {
            align: 'stretch',
            overflowHandler: 'none'
        }
    },

    responsiveConfig: {
        tall: {
            headerPosition: 'top'
        },
        wide: {
            headerPosition: 'left'
        }
    },

    defaults: {
        bodyPadding: 20,
        tabConfig: {
            responsiveConfig: {
                wide: {
                    iconAlign: 'left',
                    textAlign: 'left'
                },
                tall: {
                    iconAlign: 'top',
                    textAlign: 'center',
                    width: 120
                }
            }
        }
    },

    items: [{
        title: 'Login',
        iconCls: 'fa-door-open',
        items: [{
            xtype: 'Login'
        }]
    }, {
        title: 'Register Person',
        iconCls: 'fa-user-plus',
        items: [{
            xtype: 'PersonRegistration'
        }]
    }, {
        title: 'Order Credit',
        iconCls: 'fa-cart-plus',
        items: [{
            xtype: 'OrderCredit'
        }]
    }, {
        title: 'Person Credit List',
        iconCls: 'fa-file-alt',
        items: [{
            xtype: 'Ordercreditsgrid'
        }]
    }, {
        title: 'Admin Credits',
        iconCls: 'fa-users',
        items: [{
            xtype: 'Admincreditsgrid'
        }]
    },  {
        title: 'Admin Credits Queue',
        iconCls: 'fa-users',
        items: [{
            xtype: 'Adminordercreditqueue'
        }]
    },]
});
