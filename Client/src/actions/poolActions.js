import Axios from 'axios';


export function addPool(data){
    return dispaptch =>{
        return Axios.post('/api/Vote',data);
    }
}

export function getPool(id){
    return dispatch=>{
        return Axios.get('/api/Vote/'+id);
    }
}

export function addAnswer(poolID,optionID){
    return dispatch=>{
        return Axios.post('/api/Vote/'+poolID +'/Option/'+optionID+'/Answer');
    }
}