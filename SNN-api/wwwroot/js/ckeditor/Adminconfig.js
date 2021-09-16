/**
 * @license Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
    config.extraPlugins = 'youtube,tablemanager,cft,cleanupcontent,pastefromword,wordcount,notification,removeformat';
    config.height = 400;
    config.pasteFromWordRemoveStyles = false;
    config.allowedContent = true;
    config.toolbar = [
        { name: 'undo', items: ['Undo', 'Redo'] },
        { name: 'format', items: ['Format', '-', 'FontSize', '-', 'TextColor'] },
        { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord'] },
        { name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll'] },
        { name: 'insert1', items: ['SpecialChar', 'Maximize'] },
        '/',
        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Subscript', 'Superscript', '-', 'CopyFormatting', 'RemoveFormat'] },
        { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
        { name: 'links', items: ['Link', 'Unlink'] },
        { name: 'insert', items: ['Image', 'Youtube','Table', 'HorizontalRule'] },
        { name: 'tabletools', items: ['TableManager', 'cft', 'cleanupcontent'] },
        { name: 'document', items: ['Source'] }
    ];
    config.specialChars = [
      '&euro;', '&cent;', '&pound;', '&sect;', '&copy;', '&reg;', '&frac14;', '&frac12;', '&frac34;',
      '&Agrave;', '&Aacute;', '&Acirc;', '&Atilde;', '&Auml;', '&Aring;', '&Egrave;', '&Eacute;',
      '&Ecirc;', '&Euml;', '&Igrave;', '&Iacute;', '&Icirc;', '&Iuml;', '&Ograve;', '&Oacute;',
      '&Ocirc;', '&Otilde;', '&Ouml;', '&THORN;', '&szlig;', '&agrave;', '&aacute;', '&acirc;',
      '&atilde;', '&auml;', '&aring;', '&ccedil;', '&egrave;', '&eacute;', '&ecirc;', '&euml;', '&igrave;', '&iacute;',
      '&icirc;', '&iuml;', '&ograve;', '&oacute;', '&ocirc;', '&otilde;', '&ouml;', '&trade;'
    ];
    config.disableNativeSpellChecker = false;
    //config.removePlugins = 'wsc';
    //config.scayt_autoStartup = true;
    //config.scayt_maxSuggestions = 3;
};
