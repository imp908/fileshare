import React from 'react';
import ReactDOM from 'react-dom';
import Game from './BoardGameClasses';

class BoardGameContainer{
    constructor(){
        
    }
    
    Init(containerDomID){
        ReactDOM.render(<Game />, document.getElementById(containerDomID));
        this.registerWindowEvents();
    }

    registerWindowEvents = () =>
    {
        window.addEventListener('mousedown',function(e){
            document.body.classList.add('mouse-navigation');
            document.body.classList.remove('kbd-navigation');
        });

        window.addEventListener('keydown',function(e){
            if(e.keyCode === 9){
                document.body.classList.add('kbd-navigation');
                document.body.classList.remove('mouse-navigation');
            }
        });

        window.addEventListener('click',function(e){
            if(e.target.tagname==='A' && e.target.getAttribute('href') === "#"){
                e.preventDefault();
            }
        });

        window.onerror = function(message,source,line,col,error)
        {
            var text = error 
            ? error.stack || error 
            : `${message}(at ${source} : ${line} : ${col} )`;
        };
    }
}
export { BoardGameContainer}

