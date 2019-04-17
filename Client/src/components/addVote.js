import React from 'react';
import {connect} from 'react-redux';
import {addPool} from '../actions/poolActions';

class AddVote extends React.Component{

    constructor(){
        super();
        this.state = {
            paramName:"",
            name:"",
            options:[],
            error:""
        }
    }


    onSubmit = (event) =>{
        event.preventDefault();

            this.setState({
                error:""
            })
         
        if(this.state.name.length ===0){
            this.setState({
                error:"Please type pool name"
            })
            return;
        }

        if(this.state.options.length===0){
            this.setState({
                error:"Please type minimum one option to your pool"
            })
            return;
        }

        this.props.addPool({name : this.state.name,voteOptions : this.state.options})
        .then((res)=>{
            this.props.history.push("/pool/"+ res.data.id);
        },(err)=>{

        })

    }

    handleChange = (event) => {
        this.setState({
            [event.target.name] : event.target.value
        });
    }


    removeOption = i =>{
        let temp = this.state.options;
        temp.splice(i,1);

            this.setState({
                options: temp
            })
    }

    addOption = () =>{
       
        this.setState({
            error:""
        })

        if(!this.state.paramName || this.state.paramName.length ===0){
            this.setState({
                error:"Please type parameter name"
            })
            return;
        }
     
       if(this.state.options.findIndex(x=>x.name.toUpperCase() === this.state.paramName.toUpperCase() ) >-1){
        this.setState({
            error:"This parameter already exist on your list"
        })
           return;
       }

       var temp = {name : this.state.paramName}

        this.setState({
            options:[
                ...this.state.options,
                temp
            ],
            paramName : ""
        }
        )
    }


    render(){

       
        const options = this.state.options.map((item,i)=>
                <li className="list-group-item d-flex justify-content-between align-items-center" key ={i} >{item.name}
                 <span class="badge badge-danger badge-pill removeOption" onClick={() =>this.removeOption(i)}>Remove</span>
                </li>
        );


        return(
            <React.Fragment>
          <div className="container">
            {
                this.state.error.length > 0 && 
                            <div class="alert alert-danger" role="alert">  {this.state.error} </div>
            }



            <div className="row">
              <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                <div className="card card-signin my-5">
                  <div className="card-body">
                    <h5 className="card-title text-center">Add new pool</h5>
                    <form  onSubmit={this.onSubmit}>
                      <div className="form-group">
                        <input type="text" id="name" name="name" className="form-control" placeholder="Name" required autoFocus
                        onChange={this.handleChange}  />
                      </div>
                        <hr className="my-4" />
                         <div className="form-group">
                            <input type="text" id="paramName" name="paramName" className="form-control" placeholder="Option name" 
                            onChange={this.handleChange}   value={this.state.paramName} />
                            </div>
                        <div className="form-group">
                            <div    className="btn btn-lg btn-success btn-block " onClick={this.addOption}>Add </div>
                        </div>
                            <ul class="list-group">
                                {options}
                            </ul>
                        <hr className="my-4" />
                      <button  disabled={this.state.isLoading}  className="btn btn-lg btn-success btn-block " type="submit">Create</button>
                    </form>
                  </div>
                </div>
              </div>
            </div>
          </div>
            </React.Fragment>
        )
    }
}

export default connect((state)=>{return state},{addPool})(AddVote);
