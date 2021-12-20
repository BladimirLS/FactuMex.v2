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
export class Notificacion {
    constructor(
        public Msj: string,
        public Src: string
    )
    {
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