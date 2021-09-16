/**
 * @license Copyright (c) 2003-2020, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    config.pasteFromWordRemoveStyles = false;
    config.contentsCss = ["https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css","https://use.fontawesome.com/06433525d0.css"];
    config.allowedContent = true;
};

//config.toolbar = [
//    { name: 'undo', items: ['Undo', 'Redo'] },
//    { name: 'format', items: ['Format', '-', 'FontSize', '-', 'TextColor'] },
//    { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord'] },
//    { name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll'] },
//    { name: 'insert1', items: ['SpecialChar', 'Maximize'] },
//    '/',
//    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Subscript', 'Superscript', '-', 'CopyFormatting', 'RemoveFormat'] },
//    { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
//    { name: 'links', items: ['Link', 'Unlink'] },
//    { name: 'insert', items: ['Image', 'Youtube', 'Table', 'HorizontalRule'] },
//    { name: 'tabletools', items: ['TableManager', 'cft', 'cleanupcontent'] },
//    { name: 'document', items: ['Source'] }
//];