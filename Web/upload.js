/**
 * Created by Ep on 2018/3/22.
 */

var uploadindex = 0;
function UploadKPJD() {
    uploadindex = 1;
    var GUID  = WebUploader.Base.guid(); //当前页面是生成的GUID作为标示
    var $list = $("#thelistupload");
    var uploader = WebUploader.create({

        // swf文件路径
        swf: 'common/webuploader/0.1.5/Uploader.swf',

        // 文件接收服务端。
        server: WebServiceAddress_upload,

        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#pickerfile',
        formData: { guid: GUID },
        // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
        resize: false,
        //切片
        chunked: true,
        //每片的大小，C#接收文件也有默认大小，也可以自己在C#中修改
        chunkSize: 2 * 1024 * 1024,
        threads: 1,
        accept: {
            title: 'File',
            extensions: 'zip',
            mimeTypes: 'application/zip'
        }
    });
    // 当有文件被添加进队列的时候
    uploader.on( 'fileQueued', function( file ) {
        $list.append('<div id="' + file.id + '" class="item" style="padding-left: 38%;">' +
            '<h4 class="info">' + file.name + '</h4>' +
            '<p class="state">等待上传...</p>' +
            '</div>');

    });
    uploader.on( 'beforeFileQueued', function( file ) {
        $list.empty();
        uploader.reset();
    });
    // 文件上传过程中创建进度条实时显示。
    uploader.on( 'uploadProgress', function( file, percentage ) {
        var $li = $( '#'+file.id ),
            $percent = $li.find('.progress .progress-bar');

        // 避免重复创建
        if ( !$percent.length ) {
            $percent = $('<div class="progress progress-striped active">' +
                '<div class="progress-bar" role="progressbar" style="width: 0%">' +
                '</div>' +
                '</div>').appendTo( $li ).find('.progress-bar');
        }

        $li.find('p.state').text('上传中');

        $percent.css( 'width', percentage * 100 + '%' );
    });
    uploader.on( 'uploadSuccess', function( file ) {
        $( '#'+file.id ).find('p.state').text('已上传');
    });

    uploader.on( 'uploadError', function( file ,code) {
        $( '#'+file.id ).find('p.state').text('上传出错');
    });

    uploader.on( 'uploadComplete', function( file ) {
        $( '#'+file.id ).find('.progress').fadeOut();
    });

    uploader.on( 'uploadAccept', function( file, response ) {
        if ( response['hasError']!=false ) {
            // 通过return false来告诉组件，此文件上传有错。

            return false;
        }
        else
        {
            return true;
        }
    });
    $("#ctlBtnUp").on('click', function () {
        uploader.upload();
    });
}

if (uploadindex === 0) {
    UploadKPJD();
}
