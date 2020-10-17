$(function () {
    $('[data-toggle="tooltip"]').tooltip("show");

    //默认下载图片
    var img = new Image();
    img.onload = function () {
        var canvas = document.createElement('canvas')
        canvas.width = img.width
        canvas.height = img.height
        var ctx = canvas.getContext('2d')
        // 将img中的内容画到画布上
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height)
        // 将画布内容转换为base64
        var base64 = canvas.toDataURL();
        //设置背景图片
        $(".vague-background").css("background", "url(" + base64 + ")");


        $("#btnDownload").data("image", base64);
        //注册下载事件
        $("#btnDownload").click(function () {
            var base64 = $(this).data("image");
            // 创建a链接
            var a = document.createElement('a')
            a.href = base64
            a.download = getDateString();
            // 触发a链接点击事件，浏览器开始下载文件
            a.click()
        })
    }
    img.src = "https://uploadbeta.com/api/pictures/random/?key=BingEverydayWallpaperPicture";
    // 必须设置，否则canvas中的内容无法转换为base64
    img.setAttribute('crossOrigin', 'Anonymous');

    function getDateString() {
        var date = new Date();
        return date.getFullYear() + "" + (date.getMonth() + 1) + "" + date.getDate() + "" + date.getHours() + "" + date.getMinutes() + "" + date.getSeconds() + "" + date.getMilliseconds();
    }
})
