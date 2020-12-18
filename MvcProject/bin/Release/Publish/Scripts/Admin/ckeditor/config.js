/**
 * @license Copyright (c) 2003-2018, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
    config.extraPlugins = 'lineheight,image2,indent,indentlist,indentblock';
    config.htmlEncodeOutput = false;
    config.entities = false;
    config.font_names = "Open Sans";
    config.filebrowserBrowseUrl = '/Content/gradient/pages/CKFinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/Content/gradient/pages/CKFinder/ckfinder.html?type=Images';
    config.filebrowserImageUploadUrl = '/Content/gradient/pages/CKFinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
};
