//Authorization popup window code
$.oauthpopup = function (options) {
    options.windowName = options.windowName || 'ConnectVsoWithOAuth'; // should not include space for IE
    options.windowOptions = options.windowOptions || 'location=0,status=0,width=1024,height=836';
    options.callback = options.callback || function () { window.location.reload(); };
    var that = this;
    that._oauthWindow = window.open(options.path, options.windowName, options.windowOptions);
    that._oauthInterval = window.setInterval(function () {
        if (that._oauthWindow.closed) {
            window.clearInterval(that._oauthInterval);
            options.callback();
        }
    }, 1000);
};

function oAuthRequest(url, form) {
    $.oauthpopup({
        path: url,
        callback: function () {
            form.action = '/refresh';
            form.submit();
        }
    });
}