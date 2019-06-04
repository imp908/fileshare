import React from 'react';

class Square extends React.Component{

    constructor(props){
        super(props);
        this.state = { value: props.value, player: props.player};
        console.log('Square')
    }

    render(){
        if (!this.props.value) {
            return (
                <button className="square"
                    onClick={(e) => { this.clickHandler(e)}}>                    
                </button>
            );
        }

        return (
            <button className="square"
                onClick={(e) => { this.clickHandler(e) }}>
                {this.props.value}
            </button>
        );       
       
        return null;
    }

   clickHandler(e){
       this.props.onClick(e,this)
   }
}

class Board extends React.Component
{
    constructor(props){
        super(props);
        this.state = {
            //squares: Array(9).fill(null),
            //squares:['x','o',null,'o','x',null,'o','x','o'], 
            //squares: ['x', 'o'],
            squares: props.history[0],
            player:'x',
        };  

        this.history = props.history;
    }

    renderSquare(i){
        if (typeof this.state.squares[i] !== 'undefined'){           
            return <Square value={this.state.squares[i]} 
                player = {this.state.player} 
                clicked = {this.state.clicked} 
                onClick = {(event, item) => this.handleClick(event, item, i)}
            />; 
        }
        return null;
    }

   render(){
       const status = `Player: ${this.state.player}`
       return(
           <div>
               <div>{status}</div>
               <div className="board-row">                   
                    {this.renderSquare(0)}
                    {this.renderSquare(1)}
                    {this.renderSquare(2)}
               </div>
               <div className="board-row">
                   {this.renderSquare(3)}
                   {this.renderSquare(4)}
                   {this.renderSquare(5)}
               </div>
           </div>
       );
   }

    handleClick(event, item, num){
        if (item && item.props) 
        {
            this.swichClicker(num);
        }
    }

    swichClicker(num){
        let newSquares = { ...this.state.squares };
        newSquares[num] = this.state.player;
        this.setState({
            clicked: true,
            player: (this.state.player && this.state.player === 'x') ? 'o' : 'x',
            squares: newSquares,
        });        
    }
    
}

export class Game extends React.Component
{

    constructor(props){
        super(props);
        this.state = {
            history: [
                { squares: ['x', 'o', null, 'o', 'x', null, 'o', 'x', 'o'] }
            ]};
    }

    render(){
        return(
            <div className="game">
                <div className="board">
                    <Board history={this.props.history}/>
                </div>
                <div className="game-info">
                    <div>{/**status */}</div>
                    <div>{/**TODO */}</div>
                </div>
            </div>
        );
    }
}

export default Game;