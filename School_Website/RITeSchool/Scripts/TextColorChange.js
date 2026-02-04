// JScript File

//Highlight form element- © Dynamic Drive (www.dynamicdrive.com)
//For full source code, 100's more DHTML scripts, and TOS,
//visit http://www.dynamicdrive.com

var highlightcolor="PapayaWhip"
var ns6=document.getElementById&&!document.all
var previous=''
var eventobj

//Regular expression to highlight only form elements
var intended=/INPUT|TEXTAREA|OPTION/

//Function to check whether element clicked is form element
function checkel(which){
if (which.style&&intended.test(which.tagName)){
if (ns6&&eventobj.nodeType==3)
eventobj=eventobj.parentNode.parentNode
return true
}
else
return false
}

//Function to highlight form element
function highlight(e)
{
    /*if (e.target !=null)
    {*/
        eventobj=ns6? e.target : event.srcElement
        if (previous!='')
        {
            if (checkel(previous))
                previous.style.backgroundColor=''
            previous=eventobj
            if (checkel(eventobj))
                eventobj.style.backgroundColor=highlightcolor
        }
        else
        {
            if (checkel(eventobj))
                eventobj.style.backgroundColor=highlightcolor
            previous=eventobj
        }
    /*}*/
}
/*function fnover(varname,doc)
        {
            var objTXT = doc.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "maroon";
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)";
            //objTXT.style.color = "maroon";
        }

        function fnout(varname,doc)
        {
            var objTXT = doc.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "#a3c07b";
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)";
            //objTXT.style.color = "Black";
        }*/