import React from 'react';
import {BrowserRouter,Route,Switch} from 'react-router-dom'
import AddVote from './components/addVote';
import PoolView from './components/poolView';

class Router extends React.Component{

    render(){
        return( 
             <BrowserRouter>
                 <Switch>
                         <Route  path="/pool/:id" component={PoolView}/>  
                         <Route  path="/" component={AddVote}/>  
                 </Switch>
             </BrowserRouter>
        )
     }
}


export default Router;