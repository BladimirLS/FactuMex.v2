import { Archivo } from "../importar/importar";
import { Subir } from "../subir/subir";

export class Anexo {
    constructor(
        public Arc: Subir,
        public Index:number
    ) {
    }
}

export class AnConf {
    constructor(
        public INDICE: number,
        public NOMBRE: string
    ) {
    }
}