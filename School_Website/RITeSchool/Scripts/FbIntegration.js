/* FbIntegration.js - This file contains all the functions & events of facebook for the client side.*/

window.fbAsyncInit = function () {
    FB.init({
        status: true, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        xfbml: true  // parse XFBML
    });

    // Load the SDK Asynchronously
    (function (d) {
        var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement('script'); js.id = id; js.async = true;
        js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
        ref.parentNode.insertBefore(js, ref);
    } (document));

    (function () {
        var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
        po.src = 'https://apis.google.com/js/plusone.js';
        var s = document.getElementsByTagName('script')[1]; s.parentNode.insertBefore(po, s);
    })();
}
window.twttr = (function (d, s, id) {
    var t, js, fjs = d.getElementsByTagName(s)[2];
    if (d.getElementById(id)) return; js = d.createElement(s); js.id = id;
    js.src = "//platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs);
    return window.twttr || (t = { _e: [], ready: function (f) { t._e.push(f) } });
} (document, "script", "twitter-wjs"));
