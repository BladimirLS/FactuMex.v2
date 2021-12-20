
export class Envios {
    constructor(
        public ENVACTUAL:number,
        public ENVANTERIOR:number,
        public COLACTUAL: number,
        public COLANTERIOR:number
    ) {}
}
export class Reno {
    constructor(
        public CLIDB: number,
        public CLIACT: number,
        public CLIANT: number,
        public EXCACT: number,
        public EXCANT: number,
        public SOLCACT: number,
        public PENDCUERPOACT: number,
        public PENDENVIOACT: number,
        public NOTREADY: number
    ) { }
}
export class Envio {
    constructor(
        ORIGEN: string, DESTINO: string, ASUNTO: string, FECHACOLOCADO: string, FECHAENVIADO: string
    ) { }
}
export class Cliente {
    constructor(
        public IDCliente: string
        , public Cliente: string
        , public Poliza: string
        , public PolizaExc: string
        , public Correo: string
        , public Vehiculos: string
        , public Asignacion: string
        , public Enviado: string
        , public FechaEnvio: string
        , public Envios: string
        , public SinAux: string
        , public Plantilla: string
        , public Fisica: string
    ) {

    }
}