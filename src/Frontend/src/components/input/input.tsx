import React from "react";
import Styles from "./input.module.css"

type Props = {
    onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    value: string,
    error: string
}

function Input({onChange, value, error=""}: Props) {
    const className = error.length > 0
        ? Styles.input + " " + Styles.hasError
        : Styles.input;
    
    return <div className={Styles.inputWrapper}>
        <input onChange={onChange} value={value} className={className}/>
        {
            error.length > 0 && <span className={Styles.error}>{error}</span>
        }
    </div>
}

export default Input;