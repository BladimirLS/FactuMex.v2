export class Archivo {
    constructor(
        public IDMigracion:number,
        public Tabla: string,
        public Ruta:string,
        public Nombre: string,
        public Fecha: Date,
        public Borrado:boolean
    ) {
    }
}
export class TrioDB {
    constructor(
        public Destino: string,
        public Origen: string,
        public Plantilla: string
    ) {
    }
}
export class ParDB {
    constructor(
        public Destino: string,
        public Origen: string
    ) {
    }
}
export class CamposClientes{
    public Campos: ParDB[] = [];
    constructor(
    ) {
        this.Campos.push(new ParDB('NroFiscal', ''));
        this.Campos.push(new ParDB('ApellidoRazonSocial', ''));
        this.Campos.push(new ParDB('NomEnte', ''));
        this.Campos.push(new ParDB('BuscarComo', ''));
        this.Campos.push(new ParDB('Telefono1', ''));
        this.Campos.push(new ParDB('Telefono2', ''));
        this.Campos.push(new ParDB('Telefono3', ''));
        this.Campos.push(new ParDB('EMail', ''));
        this.Campos.push(new ParDB('ObsEnte', ''));
        this.Campos.push(new ParDB('Abreviatura', ''));
        this.Campos.push(new ParDB('SegundoApellido', ''));
        this.Campos.push(new ParDB('WebSite', ''));
        this.Campos.push(new ParDB('Cortesia', ''));
    }
}
export class CamposAsignaciones{
    public Campos: ParDB[] = [];
    constructor(
    ) {
        this.Campos.push(new ParDB('NroFiscal', ''));
        this.Campos.push(new ParDB('ValorDetalleItemCob', ''));
    }
}
export class Par2Trio{
    constructor(
        public Pares: ParDB[],
        public Plantilla: string
    ) {
    }
    public getTrio() {
        var Trios: TrioDB[]=[];
        this.Pares.forEach((Par:ParDB) => Trios.push(new TrioDB(Par.Destino, Par.Origen, this.Plantilla)));
        return Trios;
    }
}