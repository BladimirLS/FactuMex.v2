export class Item {
    constructor(
        public Iditem: string,
        public Nombre: string,
        public Descripcion: string
    ) {
    }
}

// tslint:disable-next-line:class-name
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

// tslint:disable-next-line:class-name
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
        public Idlocalidad: string,
        public Localidad: string,
        public Cp: string,
        public Zona: string,
        public Idmunicipio: string
    ) {
    }
}
export class Municipio {
    constructor(
        public Idmunicipio: string,
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
