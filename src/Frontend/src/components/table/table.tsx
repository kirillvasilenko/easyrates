import React from "react";
import Styles from "./table.module.css"


export function Table(props: React.PropsWithChildren<{}>) {
    return <table className={Styles.table}>
        <tbody>
        {
            props.children
        }
        </tbody>
    </table>
}

export function Tr(props: React.PropsWithChildren<{}>){
    return <tr className={Styles.tr}>{
        props.children
    }</tr>
}

export function Td(props: React.PropsWithChildren<{}>){
    return <td className={Styles.td}>{props.children}</td>
}