import { Factura } from "../factura/factura";

export class Notificacion {
    constructor(
        public Msj: string,
        public Src: string
    ) {
    }
}

export class TasaUS {
    constructor(
        public Idtasa:number,
        public Tasa: number,
        public Fecha:Date
    ) {
    }
}
