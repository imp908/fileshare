import React from 'react';
import ReactDOM from 'react-dom';
import ShoppingList from './tutorials/ShoppingList';

class ReactContainer{
    constructor(){

    }
    
    Init()
    {
        ReactDOM.render(<ShoppingList />, document.getElementById('root'));
    }

}
export { ReactContainer}

