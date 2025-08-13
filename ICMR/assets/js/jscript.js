// JavaScript Document
function getNewHTTPObject()
	{
		try
		{
			xmlHttp=new XMLHttpRequest();
		}
		catch (e)
		{ 
			try
		   {    
				xmlHttp=new ActiveXObject("Msxml2.XMLHTTP");
			}
			catch (e)
			{    
				try
				{      
					xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");
				}
				catch (e)
				{      
					try
					{
						if(!xmlHttp && typeof XMLHttpRequest != "undefined")
						{
							xmlHttp = new XMLHttpRequest();
						}
					}
					catch(e)
					{
						alert("Your browser does not support AJAX!");
						return false;
					}
				}
			}
		}	
		return xmlHttp;
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
		
function validate()
{
	if((document.fleadcapture.txtName.value==""))
	{
		alert("Please enter your Name");
		document.fleadcapture.txtName.focus();
		return false;
	}
	else
	{
		var first = /^([a-zA-Z])+([a-zA-Z\s]+)$/
		if (!first.test(document.fleadcapture.txtName.value)) 
		{
			alert("! Please enter a valid Name !");
			document.fleadcapture.txtName.focus();
			document.fleadcapture.txtName.select();
			return false;
		}
	}
	
	if((document.fleadcapture.txtEmail.value==""))
	{
		alert("Please enter your Email Id");
		document.fleadcapture.txtEmail.focus();
		return false;
	}
	else
	{
		var mail = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
		if (!mail.test(document.fleadcapture.txtEmail.value)) 
		{
			alert("! Please enter a valid email address eg:support@gmail.com !");
			document.fleadcapture.txtEmail.focus();
			document.fleadcapture.txtEmail.select();
			return false;
		}		
	}
	
	if(document.fleadcapture.txtMobile.value=="")
	{
		alert("Please enter your Mobile Number");
		document.fleadcapture.txtMobile.focus();
		return false;
	}
	else
	{
		if(document.fleadcapture.txtMobile.value.indexOf("0")==0)
		{
			alert("Please enter 10 digit Mobile no., only without '0' in the beginning");
			document.fleadcapture.txtMobile.focus();
			return false;
		}
		
		if((document.fleadcapture.txtMobile.value=="1111111111") || (document.fleadcapture.txtMobile.value=="2222222222") || (document.fleadcapture.txtMobile.value=="3333333333") || (document.fleadcapture.txtMobile.value=="4444444444") || (document.fleadcapture.txtMobile.value=="5555555555") || (document.fleadcapture.txtMobile.value=="6666666666") || (document.fleadcapture.txtMobile.value=="7777777777") || (document.fleadcapture.txtMobile.value=="8888888888") || (document.fleadcapture.txtMobile.value=="9999999999"))
		{
			alert("Please enter 10 digit Mobile no., only");
			document.fleadcapture.txtMobile.focus();
			return false;
		}
	}

	if(isNaN(document.fleadcapture.txtMobile.value))
	{
		alert("Please enter only Numbers");
		document.fleadcapture.txtMobile.focus();
		return false;
	}
	
	if(document.fleadcapture.txtMobile.value.length<10)
	{
		alert("Please enter currect mobile number");
		document.fleadcapture.txtMobile.focus();
		return false;
	}
			
	if(document.fleadcapture.selCity.value=="")
	{
		alert("Please select the city from");
		document.fleadcapture.selCity.focus();
		return false;
	}
	
	if(document.fleadcapture.selPrg.value=="")
	{
		alert("Please select the program");
		document.fleadcapture.selPrg.focus();
		return false;
	}
	
	localStorage.setItem('email', document.fleadcapture.txtEmail.value);
	
	document.fleadcapture.submit();
}
	
	