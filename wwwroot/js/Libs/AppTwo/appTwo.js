//importing service sample bus 
import {MediatorCl} from './MediatorCl.js'

class AppTwo
{
    constructor()
    {
        this.mtr = new MediatorCl();      
    }

    Init(){
        console.log('AppTwo Inited');
        this.mtr.Register();
    }

    Emit(){
        this.mtr.Emit();
    }
}

export {AppTwo};