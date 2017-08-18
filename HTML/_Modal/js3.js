
document.addEventListener("DOMContentLoaded",function(){
	console.log("document addEventListener");
	testModule.addListeners();
	console.log("Document loaded");
});

//Module
var testModule = (
	function(){
		
		return {
			addListeners : function ()
			{

				sel_ = document.getElementById("modal");

				if(sel_){
					console.log("modals");
					sel_.onclick =
					function(event){
						console.log("Modal click ",event.target);
						testModule.modalClose(event);
					};
				}
				else{console.log("No modals");}

				sel_ = document.getElementById("optionSelect");
				
				if(sel_)
				{
					console.log("option select events");
					
					sel_.size=1;
					
					sel_.onmouseover = function(){
							testModule.dropDownEnlarge (this) ;
						};						
					sel_.onmouseout = function(){
						testModule.dropDownDecrease (this) ;
					};
				}
				
			}
			, dropDownEnlarge : function (element_)
			{
				console.log("dropDownEnlarge ", element_);
				element_.size = element_.length;
			}
			, dropDownDecrease : function (element_)
			{
				console.log("dropDownDecrease ", element_);
				element_.size = 1;
			}									
			, modalClose : function(event)
			{
				console.log("Modal close");
				var modal = document.getElementById("modal");
				var tag = document.getElementById("model-tag");
				
				if(event.target===modal || event.target === tag){
					modal.style.display="none";
				}
			}
			, modalShow : function()
			{
				console.log("Modal show");
				var modal = document.getElementById("modal");
				modal.style.display="block";
			}
			, modalListGen : function()
			{
				var elem = document.getElementById("modal-content");
				if(elem){
					//var child = document.createElement();
				}
			}
		}
		
	}()
);
