import React from "react";
import {ProblemDetails, RatesResponse} from "@kirillamurskiy/easyrates-reader-client/Client.v2.g";
import api from "../api";
import Input from "../components/input/input";
import RatesTable from "../module/ratesTable/ratesTable";
import {ApiException} from "@kirillamurskiy/easyrates-reader-client/Client.v2.g";


function Index(){
    const [currency,setCurrency] = React.useState<string>("");
    const [response,setResponse] = React.useState<RatesResponse>({currencyFrom:"", rates:[]});
    const [error,setError] = React.useState<string>("");
    
    const handleSubmit=React.useCallback((event: React.FormEvent) =>{
        event.preventDefault();
        
        api.getRates(currency)
            .then(ratesResponse=>{
                setResponse(ratesResponse);
                setError("");
            })
            .catch((error: ProblemDetails) => {
                console.info(error);
                setError(error.detail);
            });
    }, [currency]);
    
    return <div>
        <form onSubmit={handleSubmit}>
            <Input onChange={event => setCurrency(event.target.value)} value={currency} error={error}/>
        </form>
        <RatesTable currencyFrom={response.currencyFrom} rates={response.rates}/>
    </div>;
}
export default Index;