export class Comprobante {
    constructor(
        public Idcomp: string,
        public EstatDesde: string,
        public NumDesde: string,
        public EstatHasta: string,
        public NumHasta: string,
        public Venc: string,
        public TipoComp: string,
        public Rnc: string,
        public Empresa:string
    ) {
    }
}
export class RNCEmpresa {
    constructor(
        public ActividadEconomica: string,
        public Constitucion: string,
        public Dir1: string,
        public Dir2: string,
        public Dir3: string,
        public Estatus: string,
        public Nombre1: string,
        public Nombre2: string,
        public Regimen: string,
        public Rnc: string,
        public Tel: string
    ) {
    }
}
export class Moneda {
    constructor(public Idmoneda: string,
        public Moneda1: string,
        ) { }
}
export class Item {
    constructor(
        public Iditem: string,
        public Nombre: string,
        public Descripcion: string
    ) {
    }
}

export class slaItem {
    constructor(
        public Idsla: string,
        public TipoSla: string,
        public Iditem: string,
        public Idprovincia: string,
        public Duracion: string,
        public Und: string,
        public NotificarExc: string,
        public Correo: string
    ) {
    }
}

export class precioItem {
    constructor(
        public Idprecio: string,
        public Idprovincia: string,
        public Iditem: string,
        public Moneda: string,
        public Precio: string
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
export class Sector {
    constructor(
        public Idlocalidad:string,
        public Localidad: string,
        public Cp: string,
        public Zona: string,
        public Idmunicipio: string
    ) {
    }
}
export class Municipio {
    constructor(
        public Idmunicipio:string,
        public Municipio1: string,
        public Cp: string,
        public Zona: string,
        public Idprovincia: string) { }
}

export class Provincia {
    constructor(
        public Idprovincia: string,
        public Provincia1: string,
        public Cp: string,
        public Zona: string) { }
}