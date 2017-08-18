
window.onclick = function(event){
	console.log("Wndow clicked :",event.target);
	module.elemHideShowbyID("mask");
}; 

var module = (function(){
	return{
		elemHideShowbyID: function (ID_)
		{			
			var elem = document.getElementById(ID_);
			console.log("Elment hide show by ID",elem);
			if(elem)
			{
				if(elem.style.display==="none")
				{
					elem.style.display="block";
				}else
				{
					elem.style.display="none";
				}
			}
		}
	}
	
}());