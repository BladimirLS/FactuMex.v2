export class Usuario {
    constructor(
        public IDUsuario: string,
        public Nombre: string,
        public Email: string,
        public Pwd: string,
        public Cedula: string,
        public Telefono: string,
        public Foto: string,
        public Borrado: string,
        public usr: string,
        public token: string
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
export class Direccion {
    constructor(public Iddireccion: string,
        public Idente: string,
        public TipoDir: string,
        public Correo: string,
        public Calle: string,
        public Nro: string,
        public Edificio: string,
        public Apto: string,
        public Residencial: string,
        public Idsector: string,
        public Sector: string,
        public Idmunicipio: string,
        public Municipio: string,
        public Idprovincia: string,
        public Provincia: string,
        public Idpais: string,
        public Pais: string){ }
}
export class Ente {
    constructor(
        public Idente: string,
        public TipoEnte: string,
        public Nombre: string,
        public Id: string,
        public TipoId: string,
        public Correo: string,
        public Tel1: string,
        public Tel2: string,
        public Tel3: string
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
export class itemPermiso {
    constructor(
        public IDPermiso: string,
        public Permiso: string,
        public Grupo: string
    ) {
    }
}
export class itemPlantilla {
    constructor(
        public IDPermisoxUsuario: string,
        public IDUsuario: string,
        public Permiso: string) { }
}

export class DTOPermisos {
    constructor(
        public Usuario: itemPlantilla[],
        public Plantilla: itemPermiso[]) { }
}