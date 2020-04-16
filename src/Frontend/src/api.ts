import {RatesClientV1} from "@kirillamurskiy/easyrates-reader-client/Client.v1.g";
import fetch from "isomorphic-unfetch"

const api = new RatesClientV1("http://localhost:5011", {fetch});

export default api;