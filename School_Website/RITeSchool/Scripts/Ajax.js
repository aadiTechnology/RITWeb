// JScript File

function CreateHTTPReqObj()
{
    var xmlHttpObj;
    
    if (window.ActiveXObject)
    {
        try
        {
        xmlHttpObj = new ActiveXObject("Microsoft.XMLHTTP");
        }
        catch (e)
        {
        xmlHttpObj = new ActiveXObject("Msxml2.XMLHTTP");
        }
    }
    else
        xmlHttpObj = new XMLHttpRequest();
        
        return xmlHttpObj;
}
 function noCache(uri)
 {
	return uri.concat(/\?/.test(uri)?"&":"?","noCache=",(new Date).getTime(),".",Math.random()*1234567);
 }