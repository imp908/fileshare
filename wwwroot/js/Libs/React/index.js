import React from 'react';
import ReactDOM from 'react-dom';
import ShoppingList from './tutorials/ShoppingList';
import { BoardGameContainer } from './tutorials/components/boardGame/app.js';

class ReactContainer{
    constructor(){

    }
    
    ShoppingListInit()
    {
        ReactDOM.render(<ShoppingList />, document.getElementById('root'));
    }

    BoardGameInit(){
        var a = new BoardGameContainer();
        a.Init('root');
    }    
}
export { ReactContainer}
