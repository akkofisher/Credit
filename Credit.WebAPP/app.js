/*
 * This file launches the application by asking Ext JS to create
 * and launch() the Application class.
 */
Ext.application({
    extend: 'CreditAPP.Application',

    name: 'CreditAPP',

    requires: [
        // This will automatically load all classes in the CreditAPP namespace
        // so that application classes do not need to require each other.
        'CreditAPP.*'
    ],

    // The name of the initial view to create.
    mainView: 'CreditAPP.view.main.Main'
});
