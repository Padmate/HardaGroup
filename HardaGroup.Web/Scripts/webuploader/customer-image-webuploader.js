
/**************************************
初始化图片上传插件
引用之前需要引用webuploader.min.js插件
***************************************/
jQuery.fn.InitImageWebUploader = function(args, UploadFinishCallBack)
{

    if (!WebUploader.Uploader.support()) {
        alert('当前页面一些功能不支持您的浏览器！如果你使用的是IE浏览器，请尝试升级 flash 播放器');
        throw new Error('function does not support the browser you are using.');
    }

    var pickId = this[0].id;   //Id
    var url = args.url;         //url
    var imageSize = args.size * 1024 * 1024;  //图片大小限制
    var multiple = args.multiple == null ? true : args.multiple; //默认多选
    var autoUpload = args.autoupload == null ? false : args.autoupload; //默认不自动上传
    var thumbId = args.thumbid; //缩略图Id
    var singleUpload = args.singleupload == null ? false : args.singleupload; //默认一次可上传多个文件


    // 初始化Web Uploader
    var uploader = WebUploader.create({
        auto: false,   // 选完文件后，是否自动上传。
        swf: '../Scripts/webuploader/Uploader.swf',
        server: url,
        duplicate: true, //可重复上传同一文件
        pick: {
            id: "#"+pickId,
            //只能选择一个文件上传
            multiple: multiple
        },
        compress: false, //图片不压缩
        accept: {           // 只允许选择图片文件。
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/*'
        },
        fileSingleSizeLimit: imageSize, // 单个文件最大不能超过imageSize M
        thumb: {        //缩略图配置
            // 为空的话则保留原有图片格式。
            // 否则强制转换成指定的类型。
            type: ''

        }
    });
    uploader.on('beforeFileQueued', function (file) {

        //如果每次只能上传一个文件，则在选择文件前清空文件队列
        if (singleUpload) {
            var files = uploader.getFiles();
            for (var i = 0; i < files.length; i++) {
                //删除队列中的文件
                uploader.removeFile(files[i], true);
            }
        }
    });
    uploader.on('fileQueued', function (file) {
        //生成预览图片
        uploader.makeThumb(file, function (error, src) {
            if (error) {
                $(thumbId).attr('alt', "预览错误");
            } else {
                $(thumbId).attr('src', src);
            }
        }, 1, 1);
    });

    uploader.on('startUpload', function () {

    });
    uploader.on('uploadFinished', function () {

        //删除完成回调
        UploadFinishCallBack();
    });

    uploader.on('error', function (handler) {

        //隐藏遮罩
        $("body").hideLoading();

        if (handler == "Q_EXCEED_NUM_LIMIT") {
            alert("超出最大张数");
        }
        if (handler == "F_DUPLICATE") {
            alert("文件重复");
        }
        if (handler == "F_EXCEED_SIZE") {
            $("#status").html("<lable style='color:red;'>图片大小不能超过1M</lable>");
        }
    });


    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    uploader.on('uploadSuccess', function (file, response) {

        if (response.Success == false || response.Success == "false") {

        } else {

        }

    });

    // 完成上传完了，成功或者失败，先删除进度条。
    uploader.on('uploadComplete', function (file) {

    });

    return uploader;
};
 
