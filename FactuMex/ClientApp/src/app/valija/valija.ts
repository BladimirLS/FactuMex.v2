import { Factura } from "../factura/factura";

export class Notificacion {
    constructor(
        public Msj: string,
        public Src: string
    ) {
    }
}

export class Valija {
    constructor(
        public Idvalija: string,
        public Etiqueta: string,
        public Fecha: string,
        public Cb: string,
        public Cs: string,
        public Usuario: string
    ) {
    }
}
export class ValxFac {
    constructor(
        public val:Valija,
        public fac:Factura,
        public actn:string
    ) {
    }
}