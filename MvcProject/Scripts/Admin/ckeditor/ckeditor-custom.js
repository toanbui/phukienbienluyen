"use strict";
CKEDITOR.replace('editor1', {
    toolbar: [{
        name: 'clipboard',
        items: ['Undo', 'Redo']
    }, {
        name: 'styles',
        items: ['Styles', 'Format']
    }, {
        name: 'basicstyles',
        items: ['Bold', 'Italic', 'Strike', '-', 'RemoveFormat']
    }, {
        name: 'paragraph',
        items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote']
    }, {
        name: 'links',
        items: ['Link', 'Unlink']
    }, {
        name: 'insert',
        items: ['Image', 'EmbedSemantic', 'Table']
    }, {
        name: 'tools',
        items: ['Maximize']
    }, {
        name: 'editing',
        items: ['Scayt']
    }
    ],
    customConfig: '',
    extraPlugins: 'autoembed,embedsemantic,image2,uploadimage,uploadfile',
    imageUploadUrl: '/uploader/upload.php?type=Images',
    uploadUrl: '/uploader/upload.php',
    removePlugins: 'image',
    height: 461,
    contentsCss: ['/Content/gradient/pages/ckeditor/contents.css', '/Content/gradient/pages/ckeditor/artical.css'],
    bodyClass: 'article-editor',
    format_tags: 'p;h1;h2;h3;pre',
    removeDialogTabs: 'image:advanced;link:advanced',
    stylesSet: [{
        name: 'Marker',
        element: 'span',
        attributes: {
            'class': 'marker'
        }
    }, {
        name: 'Cited Work',
        element: 'cite'
    }, {
        name: 'Inline Quotation',
        element: 'q'
    }, {
        name: 'Special Container',
        element: 'div',
        styles: {
            padding: '5px 10px',
            background: '#eee',
            border: '1px solid #ccc'
        }
    }, {
        name: 'Compact table',
        element: 'table',
        attributes: {
            cellpadding: '5',
            cellspacing: '0',
            border: '1',
            bordercolor: '#ccc'
        },
        styles: {
            'border-collapse': 'collapse'
        }
    }, {
        name: 'Borderless Table',
        element: 'table',
        styles: {
            'border-style': 'hidden',
            'background-color': '#E6E6FA'
        }
    }, {
        name: 'Square Bulleted List',
        element: 'ul',
        styles: {
            'list-style-type': 'square'
        }
    }, {
        name: 'Illustration',
        type: 'widget',
        widget: 'image',
        attributes: {
            'class': 'image-illustration'
        }
    }, {
        name: '240p',
        type: 'widget',
        widget: 'embedSemantic',
        attributes: {
            'class': 'embed-240p'
        }
    }, {
        name: '360p',
        type: 'widget',
        widget: 'embedSemantic',
        attributes: {
            'class': 'embed-360p'
        }
    }, {
        name: '480p',
        type: 'widget',
        widget: 'embedSemantic',
        attributes: {
            'class': 'embed-480p'
        }
    }, {
        name: '720p',
        type: 'widget',
        widget: 'embedSemantic',
        attributes: {
            'class': 'embed-720p'
        }
    }, {
        name: '1080p',
        type: 'widget',
        widget: 'embedSemantic',
        attributes: {
            'class': 'embed-1080p'
        }
    }
    ]
});
CKEDITOR.replace('editor2', {
    toolbar: [{
        name: 'document',
        items: ['Print']
    }, {
        name: 'clipboard',
        items: ['Undo', 'Redo']
    }, {
        name: 'styles',
        items: ['Format', 'Font', 'FontSize']
    }, {
        name: 'basicstyles',
        items: ['Bold', 'Italic', 'Underline', 'Strike', 'RemoveFormat', 'CopyFormatting']
    }, {
        name: 'colors',
        items: ['TextColor', 'BGColor']
    }, {
        name: 'align',
        items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock']
    }, {
        name: 'links',
        items: ['Link', 'Unlink']
    }, {
        name: 'paragraph',
        items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote']
    }, {
        name: 'insert',
        items: ['Image', 'Table']
    }, {
        name: 'tools',
        items: ['Maximize']
    }, {
        name: 'editing',
        items: ['Scayt']
    }
    ],
    customConfig: '',
    disallowedContent: 'img{width,height,float}',
    extraAllowedContent: 'img[width,height,align]',
    extraPlugins: 'tableresize,uploadimage,uploadfile',
    imageUploadUrl: '/uploader/upload.php?type=Images',
    uploadUrl: '/uploader/upload.php',
    height: 800,
    contentsCss: ['/Content/gradient/pages/ckeditor/contents.css', '/Content/gradient/pages/ckeditor/document.css'],
    bodyClass: 'document-editor',
    format_tags: 'p;h1;h2;h3;pre',
    removeDialogTabs: 'image:advanced;link:advanced',
    stylesSet: [{
        name: 'Marker',
        element: 'span',
        attributes: {
            'class': 'marker'
        }
    }, {
        name: 'Cited Work',
        element: 'cite'
    }, {
        name: 'Inline Quotation',
        element: 'q'
    }, {
        name: 'Special Container',
        element: 'div',
        styles: {
            padding: '5px 10px',
            background: '#eee',
            border: '1px solid #ccc'
        }
    }, {
        name: 'Compact table',
        element: 'table',
        attributes: {
            cellpadding: '5',
            cellspacing: '0',
            border: '1',
            bordercolor: '#ccc'
        },
        styles: {
            'border-collapse': 'collapse'
        }
    }, {
        name: 'Borderless Table',
        element: 'table',
        styles: {
            'border-style': 'hidden',
            'background-color': '#E6E6FA'
        }
    }, {
        name: 'Square Bulleted List',
        element: 'ul',
        styles: {
            'list-style-type': 'square'
        }
    }
    ]
});
CKEDITOR.on('instanceCreated', function (event) {
    var editor = event.editor,
        element = editor.element;
    if (element.is('h1', 'h2', 'h3') || element.getAttribute('id') == 'taglist') {
        editor.on('configLoaded', function () {
            editor.config.removePlugins = 'colorbutton,find,flash,font,' +
                'forms,iframe,image,newpage,removeformat,' +
                'smiley,specialchar,stylescombo,templates';
            editor.config.toolbarGroups = [{
                name: 'editing',
                groups: ['basicstyles', 'links']
            }, {
                name: 'undo'
            }, {
                name: 'clipboard',
                groups: ['selection', 'clipboard']
            }, {
                name: 'about'
            }
            ];
        });
    }
});
