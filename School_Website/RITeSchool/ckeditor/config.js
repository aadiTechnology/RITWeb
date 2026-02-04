/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.removePlugins = 'resize';
    config.resize_enabled = false;
    config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_BR;
};


CKEDITOR.on('dialogDefinition', function (ev) {
    ev.data.definition.resizable = CKEDITOR.DIALOG_RESIZE_NONE;
});

CKEDITOR.on('instanceReady', function (ev) {
    ev.editor.dataProcessor.writer.setRules('br',
         {
             indent: false,
             breakBeforeOpen: false,
             breakAfterOpen: false,
             breakBeforeClose: false,
             breakAfterClose: false
         });
});

