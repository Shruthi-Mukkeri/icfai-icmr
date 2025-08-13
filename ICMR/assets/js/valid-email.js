function getNewHTTPObject(){try{xmlHttp=new XMLHttpRequest();}catch (e){try{xmlHttp=new ActiveXObject("Msxml2.XMLHTTP");}catch (e){try{xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");}catch (e){try{if(!xmlHttp && typeof XMLHttpRequest != "undefined"){xmlHttp = new XMLHttpRequest();}}catch(e){alert("Your browser does not support AJAX!");return false;}}}}return xmlHttp;}

function isNumber(evt) 
{
	evt = (evt) ? evt : window.event;
	var charCode = (evt.which) ? evt.which : evt.keyCode;
	if (charCode > 31 && (charCode < 48 || charCode > 57))
	{
		return false;
	}
	return true;
}

function openpdf()
{			
	
	document.getElementById("fade").style.display="block";
	document.getElementById("light").style.display="block";	
	var windowheight =document.body.parentNode.scrollHeight;
	document.getElementById("fade").style.height=windowheight+"px";
	scroll(0,0);
	return false;
}


function valid_email()
{
	if((document.f1.txtName.value=="") || (document.f1.txtName.value=="Name"))
	{
		alert("Please enter your Name")
		document.f1.txtName.focus();
		return false;
	}
	else
	{
		var first = /^([a-zA-Z])+([a-zA-Z\s]+)$/
		if (!first.test(document.f1.txtName.value)) 
		{
			alert("! Please enter a valid Name !");
			document.f1.txtName.focus();
			document.f1.txtName.select();
			return false;
		}
	}
	

	if((document.f1.txtEmail.value=="") || (document.f1.txtEmail.value=="Email Id"))
	{
		alert("Please enter your Email Id")
		document.f1.txtEmail.focus();
		return false;
	}
	else
	{
		var mail = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
		if (!mail.test(document.f1.txtEmail.value)) 
		{
			alert("! Please enter a valid email address eg:support@gmail.com !");
			document.f1.txtEmail.focus();
			document.f1.txtEmail.select();
			return false;
		}			
	}
	
	if((document.f1.txtMobile.value=="") || (document.f1.txtMobile.value=="Mobile No."))
	{
		alert("Please enter your Mobile Number");
		document.f1.txtMobile.focus();
		return false;
	}
	else
	{
		if((document.f1.txtMobile.value.charAt("0")==0) || (document.f1.txtMobile.value.charAt("0")==1) || (document.f1.txtMobile.value.charAt("0")==2) || (document.f1.txtMobile.value.charAt("0")==3) || (document.f1.txtMobile.value.charAt("0")==4))
			{
				alert("Please enter correct 10 digit Mobile number");
				document.f1.txtMobile.focus();
				return false;
			}
			
			if((document.f1.txtMobile.value=="1111111111") || (document.f1.txtMobile.value=="2222222222") || (document.f1.txtMobile.value=="3333333333") || (document.f1.txtMobile.value=="4444444444") || (document.f1.txtMobile.value=="5555555555") || (document.f1.txtMobile.value=="6666666666") || (document.f1.txtMobile.value=="7777777777") || (document.f1.txtMobile.value=="8888888888") || (document.f1.txtMobile.value=="9999999999"))
			{
			    alert("Please enter 10 digit Mobile no., only");
				document.f1.txtMobile.focus();
				return false;
			}
	}
		
	if(isNaN(document.f1.txtMobile.value))
	{
		alert("Please enter only Numbers");
		document.f1.txtMobile.focus();
		return false;
	}
	if(document.f1.txtMobile.value.length<10)
	{
		alert("Please enter correct mobile number");
		document.f1.txtMobile.focus();
		return false;
	}
		
	
	if((document.f1.llocation.value=="") || (document.f1.llocation.value=="Location"))
	{
		alert("Please enter your location")
		document.f1.llocation.focus();
		return false;
	}
	else
	{
		var first = /^([a-zA-Z0-9-.,&])+([a-zA-Z0-9-.,&\s]+)$/
		if (!first.test(document.f1.llocation.value)) 
		{
			alert("! Please enter a valid location name with out any special characters !");
			document.f1.llocation.focus();
			document.f1.llocation.select();
			return false;
		}
	}
	
	if(document.f1.lBestDescribes.value=="0")
	{
		alert("Please select any option")
		document.f1.lBestDescribes.focus();
		return false;
	}

	if(document.f1.lAreaofInterest.value=="0")
	{
		alert("Please select any option")
		document.f1.lAreaofInterest.focus();
		return false;
	}

	if(document.f1.lauthorisecheck.checked==false)
	{
		alert("Please accept terms and conditions ")
		document.f1.lauthorisecheck.focus();
		return false;
	}
	
		
	
	// if(document.f1.txtCaptcha.value=="")
	// {
	// 	alert("Please enter the Image Code");
	// 	document.f1.txtCaptcha.focus();
	// 	document.f1.txtCaptcha.select();
	// 	return false;
	// }
	
var xmlHttp = getNewHTTPObject();	
	//alert(xmlHttp);
	xmlHttp.onreadystatechange=function()
	{
		if(xmlHttp.readyState==4)
		{	
			
			 //salert(xmlHttp.responseText)
			 if(xmlHttp.responseText=="yes")
			 {
			
			
				window.location="Thankyou.html";
					
			 }
			
		}
	}
	
	
	
	
	xmlHttp.open("GET","insertemail_request.asp?txtName="+document.f1.txtName.value+"&txtMobile="+document.f1.txtMobile.value+"&txtEmail="+document.f1.txtEmail.value+"&source=IFHE-ODL&location="+document.f1.llocation.value+"&BestDescribes="+document.f1.lBestDescribes.value+"&AreaofInterest="+document.f1.lAreaofInterest.value,true);
	xmlHttp.send(null);	
}


function valid_email2()
{
	if((document.f2.txtName.value=="") || (document.f2.txtName.value=="Name"))
	{
		alert("Please enter your Name")
		document.f2.txtName.focus();
		return false;
	}
	else
	{
		var first = /^([a-zA-Z])+([a-zA-Z\s]+)$/
		if (!first.test(document.f2.txtName.value)) 
		{
			alert("! Please enter a valid Name !");
			document.f2.txtName.focus();
			document.f2.txtName.select();
			return false;
		}
	}
	

	if((document.f2.txtEmail.value=="") || (document.f2.txtEmail.value=="Email Id"))
	{
		alert("Please enter your Email Id")
		document.f2.txtEmail.focus();
		return false;
	}
	else
	{
		var mail = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
		if (!mail.test(document.f2.txtEmail.value)) 
		{
			alert("! Please enter a valid email address eg:support@gmail.com !");
			document.f2.txtEmail.focus();
			document.f2.txtEmail.select();
			return false;
		}			
	}
	
	if((document.f2.txtMobile.value=="") || (document.f2.txtMobile.value=="Mobile No."))
	{
		alert("Please enter your Mobile Number");
		document.f2.txtMobile.focus();
		return false;
	}
	else
	{
		if((document.f2.txtMobile.value.charAt("0")==0) || (document.f2.txtMobile.value.charAt("0")==1) || (document.f2.txtMobile.value.charAt("0")==2) || (document.f2.txtMobile.value.charAt("0")==3) || (document.f2.txtMobile.value.charAt("0")==4))
			{
				alert("Please enter correct 10 digit Mobile number");
				document.f2.txtMobile.focus();
				return false;
			}
			
			if((document.f2.txtMobile.value=="1111111111") || (document.f2.txtMobile.value=="2222222222") || (document.f2.txtMobile.value=="3333333333") || (document.f2.txtMobile.value=="4444444444") || (document.f2.txtMobile.value=="5555555555") || (document.f2.txtMobile.value=="6666666666") || (document.f2.txtMobile.value=="7777777777") || (document.f2.txtMobile.value=="8888888888") || (document.f2.txtMobile.value=="9999999999"))
			{
			    alert("Please enter 10 digit Mobile no., only");
				document.f2.txtMobile.focus();
				return false;
			}
	}
		
	if(isNaN(document.f2.txtMobile.value))
	{
		alert("Please enter only Numbers");
		document.f2.txtMobile.focus();
		return false;
	}
	if(document.f2.txtMobile.value.length<10)
	{
		alert("Please enter correct mobile number");
		document.f2.txtMobile.focus();
		return false;
	}
		
	
	
	if((document.f2.slocation.value=="") || (document.f2.slocation.value=="Location"))
	{
		alert("Please enter your location")
		document.f2.slocation.focus();
		return false;
	}
	else
	{
		var first = /^([a-zA-Z0-9-.,&])+([a-zA-Z0-9-.,&\s]+)$/
		if (!first.test(document.f2.slocation.value)) 
		{
			alert("! Please enter a valid location name with out any special characters !");
			document.f2.slocation.focus();
			document.f2.slocation.select();
			return false;
		}
	}
	
	if(document.f2.BestDescribes.value=="0")
	{
		alert("Please select any option")
		document.f2.BestDescribes.focus();
		return false;
	}

	if(document.f2.AreaofInterest.value=="0")
	{
		alert("Please select any option")
		document.f2.AreaofInterest.focus();
		return false;
	}

	if(document.f2.authorise.checked==false)
	{
		alert("Please accept terms and conditions ")
		document.f2.authorise.focus();
		return false;
	}
	
	
	
	// if(document.f2.txtprg.value=="0")
	// {
	// 	alert("Please select the program")
	// 	document.f2.txtprg.focus();
	// 	return false;
	// }
	
		
	
	// if(document.f2.txtCaptcha.value=="")
	// {
	// 	alert("Please enter the Image Code");
	// 	document.f2.txtCaptcha.focus();
	// 	document.f2.txtCaptcha.select();
	// 	return false;
	// }
	
	var xmlHttp = getNewHTTPObject();	
	//alert(xmlHttp);
	xmlHttp.onreadystatechange=function()
	{
		if(xmlHttp.readyState==4)
		{	
			
			 //alert(xmlHttp.responseText)
			 if(xmlHttp.responseText=="yes")
			 {
			
			
				window.location="Thankyou.html";
					
			 }
			
		}
	}
	
	localStorage.setItem('email', document.f2.txtEmail.value);
	
	xmlHttp.open("GET","insertemail_request.asp?txtName="+document.f2.txtName.value+"&txtMobile="+document.f2.txtMobile.value+"&txtEmail="+document.f2.txtEmail.value+"&location="+document.f2.slocation.value+"&BestDescribes="+document.f2.BestDescribes.value+"&AreaofInterest="+document.f2.AreaofInterest.value+"&source=IFHE-ODL",true);
	xmlHttp.send(null);	
}


function RefreshImage(valImageId) 
{
	var objImage = document.getElementById(valImageId)
    if (objImage == undefined) 
	{
    	return;
    }
    var now = new Date();
    objImage.src = objImage.src.split('?')[0] + '?x=' + now.toUTCString();
}

function valid_callrequest()
{
	if((document.fCallRequest.txtCallRequestName.value=="") || (document.fCallRequest.txtCallRequestName.value=="Name"))
	{
		alert("Please enter your Name")
		document.fCallRequest.txtCallRequestName.focus();
		return false;
	}
	else
	{
		var first = /^([a-zA-Z])+([a-zA-Z\s]+)$/
		if (!first.test(document.fCallRequest.txtCallRequestName.value)) 
		{
			alert("! Please enter a valid Name !");
			document.fCallRequest.txtCallRequestName.focus();
			document.fCallRequest.txtCallRequestName.select();
			return false;
		}
	}
	
	if((document.fCallRequest.txtCallRequestMobile.value=="") || (document.fCallRequest.txtCallRequestMobile.value=="Mobile No."))
	{
		alert("Please enter your Mobile Number");
		document.fCallRequest.txtCallRequestMobile.focus();
		return false;
	}
	else
	{
		if(document.fCallRequest.txtCallRequestMobile.value.indexOf("0")==0)
			{
				alert("Please enter 10 digit Mobile no., only without '0' in the beginning");
				document.fCallRequest.txtCallRequestMobile.focus();
				return false;
			}
			
			if((document.fCallRequest.txtCallRequestMobile.value=="1111111111") || (document.fCallRequest.txtCallRequestMobile.value=="2222222222") || (document.fCallRequest.txtCallRequestMobile.value=="3333333333") || (document.fCallRequest.txtCallRequestMobile.value=="4444444444") || (document.fCallRequest.txtCallRequestMobile.value=="5555555555") || (document.fCallRequest.txtCallRequestMobile.value=="6666666666") || (document.fCallRequest.txtCallRequestMobile.value=="7777777777") || (document.fCallRequest.txtCallRequestMobile.value=="8888888888") || (document.fCallRequest.txtCallRequestMobile.value=="9999999999"))
			{
			    alert("Please enter 10 digit Mobile no., only");
				document.fCallRequest.txtCallRequestMobile.focus();
				return false;
			}
	}
		
	if(isNaN(document.fCallRequest.txtCallRequestMobile.value))
	{
		alert("Please enter only Numbers");
		document.fCallRequest.txtCallRequestMobile.focus();
		return false;
	}
	if(document.fCallRequest.txtCallRequestMobile.value.length<10)
	{
		alert("Please enter currect mobile number");
		document.fCallRequest.txtCallRequestMobile.focus();
		return false;
	}
	if(document.fCallRequest.authorisecheck.checked==false)
	{
		alert("Please accept terms and conditions");
		document.fCallRequest.authorisecheck.select();
		return false;
	}
	
	var xmlHttp = getNewHTTPObject();	
	xmlHttp.onreadystatechange=function()
	{
		if(xmlHttp.readyState==4)
		{	
			 //alert(xmlHttp.responseText)
			if(xmlHttp.responseText=="yes")
			{
				//alert("Our Team will contact you on the Mobile Number : "+document.fCallRequest.txtCallRequestMobile.value);
				//document.fCallRequest.txtCallRequestName.value="";
				//document.fCallRequest.txtCallRequestMobile.value="";
				window.location="thankyou.html";
			}
			else
			{
				alert("Sorry. We could not process your request. Please try after sometime.");
			}			
		}
	}
	
	xmlHttp.open("GET","insertRequestaCall.asp?txtName="+document.fCallRequest.txtCallRequestName.value+"&txtMobile="+document.fCallRequest.txtCallRequestMobile.value,true);
	xmlHttp.send(null);	
}


function jsvisitor()
{
	//alert("in");
	var xmlHttp = getNewHTTPObject();
	var requrl= window.location.href;
	
	var queryString = window.location.search;				
			
	var resArray = queryString.substring(1).split("&");
	var utm_source1,utm_medium1, utm_campaign1, utm_id2, utm_term1,utm_content1;
	var params = {};
	for (var i = 0; i < resArray.length; i++) 
	{
		var pair = resArray[i].split('=');
		switch(pair[0])
		{
			case "utm_source":
			utm_source1=pair[1];
			break;
			
			case "utm_medium":
			utm_medium1=pair[1];
			break;
			
			case "utm_campaign":
			utm_campaign1=pair[1];
			break;
			
			case "utm_id":
			utm_id2=pair[1];
			break;
			
			case "utm_term":
			utm_term1=pair[1];
			break;	
			
			case "utm_content":
			utm_content1=pair[1];
			break;	
		}				
	}
	
	
	
	
	xmlHttp.open("GET","insertVisitorLog.asp?requrl="+requrl+"&refURL="+document.referrer+"&utm_source1="+utm_source1+"&utm_medium1="+utm_medium1+"&utm_campaign1="+utm_campaign1+"&utm_id1="+utm_id2+"&utm_term1="+utm_term1+"&utm_content1="+utm_content1,true);
	xmlHttp.send(null);
}


function registeremail()
{
	if((document.form_RegEmail.txtRegEmail.value=="") || (document.form_RegEmail.txtRegEmail.value=="Email Id"))
	{
		alert("Please enter your Email Id")
		document.form_RegEmail.txtRegEmail.focus();
		return false;
	}
	else
	{
		var mail = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
		if (!mail.test(document.form_RegEmail.txtRegEmail.value)) 
		{
			alert("! Please enter a valid email address eg:support@gmail.com !");
			document.form_RegEmail.txtRegEmail.focus();
			document.form_RegEmail.txtRegEmail.select();
			return false;
		}			
	}
	var xmlHttp = getNewHTTPObject();	
	//alert(xmlHttp);
	xmlHttp.onreadystatechange=function()
	{
		if(xmlHttp.readyState==4)
		{	
			//alert(xmlHttp.responseText);
			//return false;
			if(xmlHttp.responseText=="Inserted")
			{
				alert("Thank you for signing up your Email ID");				
			}
			else
			{
				if(xmlHttp.responseText=="Duplicate")
				{
					alert("You have already signed up your Email ID");				
					
				}
				else
				{
					alert("Something went wrong. Please try after sometime.");				
				}
			}			
		}
	}
	xmlHttp.open("GET","insertemailpopup.asp?txtRegEmail="+document.form_RegEmail.txtRegEmail.value,true);
	xmlHttp.send(null);
	closePopUp();
	return false;
}


