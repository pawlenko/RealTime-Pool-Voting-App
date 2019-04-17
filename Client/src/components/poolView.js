import React from 'react'
import {connect} from 'react-redux';
import {getPool,addAnswer} from '../actions/poolActions';
import CanvasJSReact  from '../canvasjs.react';
import { HubConnectionBuilder, LogLevel} from '@aspnet/signalr';



class PoolView extends React.Component{

    constructor(){
        super();
        this.state = {
            pool :{
                options:[]
            },
            hubConnection : null
        }
    }


    addSymbols(e) {
        console.log(e.value);
		return CanvasJSReact.CanvasJS .formatNumber(e.value);	
	}


    componentWillUnmount (){
        this.state.hubConnection.invoke('LeaveRoom', this.props.match.params.id);
        this.state.hubConnection.stop()
    }

    componentDidMount(){

        const connection = new HubConnectionBuilder()
        .withUrl("/hub/vote")
        .configureLogging(LogLevel.Information)
        .build();

   

        connection.start()
        .then(() =>  connection.invoke('JoinRoom', this.props.match.params.id) )
        .catch(err => console.log('Error while establishing connection :('));


        connection.on('sendToGroup', (receivedMessage) => {
            this.setState({
                pool:receivedMessage
            })
          });


          this.setState({
              hubConnection : connection
          })
      


        this.props.getPool(this.props.match.params.id)
        .then((res)=>{
                this.setState({
                    pool :  res.data.result
                })
        },(err)=>{
            this.props.history.push("/");
        })
    }

    hadleChange =  (event,id) =>{
     
        if(event.target.value){
           
            this.props.addAnswer(this.props.match.params.id,id)
            .then((res)=>{

            },(err)=>{

            })
        }

    }

    render(){

        const options1 = {
			animationEnabled: true,
			theme: "light2", 
			axisX: {
				labelAngle: 0
            },
			axisY: {
                valueFormatString:""
			},
			data: [{
				type: "column",
                dataPoints: this.state.pool.options.map((item,i)=>{
                    return {label:item.name,y:item.answers}
                }
                )
			}]
		}

        const options  = this.state.pool.options.map((item,i)=>
                <div className="radio" key={item.id} >
                <label><input type="radio" name="optradio"  onChange={(e)=>this.hadleChange(e,item.id)} checked={item.answered}/> {item.name}</label>
                </div>
        );

        return(
        <div className="container">
            <div className="row">
              <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                <div className="card card-signin my-5">
                  <div className="card-body">
                    <h5 className="card-title text-center">{this.state.pool.name }</h5>

                    <div className="form-group">
                    <CanvasJSReact.CanvasJSChart options = {options1} onRef={ref => this.chart = ref} />
                    </div>

                        <div className="form-group">
                        {options}
                        </div>
                  </div>
                </div>
              </div>
            </div>
        </div>
        )
    }


}


export default connect((state)=>{return state},{getPool,addAnswer})(PoolView);
