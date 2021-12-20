
export class Login {
    constructor(
        public Idente:number,
        public IdentePadre:number,
        public Ente1:string,
        public Id:number,
        public IdtipoEnte:number,
        public Email:string,
        public Pwd: string,
        public Ext: string
    ) {}
}
export class Sesion {
    constructor(
        public BFP: string,
        public CODSESION: string,
        public EXPIRA: Date,
        public IDSESION: string,
        public IDUSUARIO: string
    ) { }
}