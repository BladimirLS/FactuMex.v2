import { Ente, Direccion } from '../cliente/cliente';
export class RefxFacxTicket {
    public TipoRef: string;
    constructor(
        public Idref: string,
        public Idfac: string,
        public Ref: string
    ) {
        this.TipoRef = 'Ticket';
    }
}
export class RefxFacxRel {
    public TipoRef: string;
    constructor(
        public Idref: string,
        public Idfac: string,
        public Ref: string
    ) {
        this.TipoRef = 'Relacionado';
    }
}
export class RefxFacxRef {
    public TipoRef: string;
    constructor(
        public Idref: string,
        public Idfac: string,
        public Ref: string
    ) {
        this.TipoRef = 'Referencia';
    }
}
export class DTOFac {
    constructor(
        public Cliente: DTOEnte,
        public Fac: Factura,
        public Desglose: Desglose[],
        public Tickets: RefxFacxTicket[],
        public Rels: RefxFacxRel[],
        public Referencia: RefxFacxRef,
        public Stat:string
    ) {
    }
}
export class DTOEnte {
    constructor(
        public Ente: Ente,
        public DirEntrega: Direccion,
        public DirFacturacion: Direccion
    ) {
    }
}
export class Notificacion {
    constructor(
        public Msj: string,
        public Src: string
    ) {
    }
}
export class Factura {
    constructor(
        public Idfactura: string,
        public Ncf: string,
        public Idcliente: string,
        public Fecha: string,
        public Tipo: string,
        public Idempresa: string,
        public FormaPago: string,
        public MetodoPago: string
    ) {
    }
}
export class Desglose {
    constructor(
        public Iddesglose: string,
        public Concepto: string,
        public Producto: string,
        public Idprovincia: string,
        public Cantidad: string,
        public Moneda: string,
        public Tarifa: string,
        public Monto: string,
        public Idfactura: string
    ) {
    }
}