import React from "react";
import {Table, Td, Tr} from "../../components/table/table";
import {RateInfo} from "@kirillamurskiy/easyrates-reader-client/Client.v1.g";

type Props = {
    currencyFrom: string,
    rates: RateInfo[]
}

function RatesTable({ currencyFrom, rates=[]}: Props){

    return <Table>
            {
                rates.map(rate => {
                    return <Tr>
                        <Td>{
                            currencyFrom
                        }</Td>
                        <Td>{
                            rate.currencyTo
                        }</Td>
                        <Td>{
                            rate.rate
                        }</Td>
                    </Tr>
                })
            }
        </Table>
}

export default RatesTable;