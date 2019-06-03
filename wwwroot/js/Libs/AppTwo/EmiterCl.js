
class EmiterCl{
    RegisterEventModule(e){
        this.emitModule=e;
    }

    Emit(e){
        if( this.emitModule){
            this.emitModule.DispatchEvent(e);
        }
    }
}

export {EmiterCl}