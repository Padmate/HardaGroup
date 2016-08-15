//kindeditor全局配置
//Create by Allen
var kindeditorConfig = {

    cssPath: '/kindeditor/plugins/code/prettify.css',
    //上传管理
    uploadJson: '/kindeditor/asp.net/upload_json.ashx',
    //文件管理
    fileManagerJson: '/kindeditor/asp.net/file_manager_json.ashx',
    allowFileManager: true,
    //编辑器高度
    width: '100%',
    //编辑器宽度
    height: '500px;',
    //配置编辑器的工具栏
    items: [
    'source', '|', 'undo', 'redo', '|', 'preview', 'print', 'template', 'code', 'cut', 'copy', 'paste',
    'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
    'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
    'superscript', 'clearhtml', 'quickformat', 'selectall', '|', 'fullscreen', '/',
    'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
    'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|', 'image', 'multiimage',
    'flash', 'media', 'insertfile', 'table', 'hr', 'emoticons', 'baidumap', 'pagebreak',
    'anchor', 'link', 'unlink', '|', 'about'
    ]
}