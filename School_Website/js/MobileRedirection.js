
$(document).ready(function () {
    var CookieName = 'ShowMainSiteFor' + " " + getMetaContents('SchoolName');
    var qsParm = GetQueryStringParameter(window.location.href);
    if (qsParm['mobile'] && qsParm['mobile'] == 1) {
        SetCookie(CookieName, 'Y');
    }

    var mobile = (/iphone|ipod|android|blackberry|phone|mini|windows\sce|palm/i.test(navigator.userAgent.toLowerCase()));
    if (!GetCookie(CookieName)) {
        if (mobile && getMetaContents('mobile') != "") {
            document.location = getMetaContents('mobile');
        }
    }
});

function GetCookie(d) {
    var c = document.cookie.indexOf(d + "=");
    var b = c + d.length + 1;
    if ((!c) && (d != document.cookie.substring(0, d.length))) {
        return null;
    }
    if (c == -1) {
        return null;
    }
    var a = document.cookie.indexOf(";", b);
    if (a == -1) {
        a = document.cookie.length;
    }
    return unescape(document.cookie.substring(b, a));
}

function SetCookie(c, e, a, g, d, f) {
    var b = new Date();
    if (a) {
        a = a * 1000 * 3600 * 24;
    }
    document.cookie = c + "=" + escape(e) + ((a) ? ";expires=" + new Date(b.getTime() + a).toGMTString() : "") + ((g) ? ";path=" + g : "") + ((d) ? ";domain=" + d : "") + ((f) ? ";secure" : "");
}

function getMetaContents(mn) {
    var m = document.getElementsByTagName('meta');
    for (var i in m) {
        if (m[i].name == mn) {
            return m[i].content;
        }
    }
}

function GetQueryStringParameter(href) {
    var vars = [], hash;
    var hashes = href.slice(href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}