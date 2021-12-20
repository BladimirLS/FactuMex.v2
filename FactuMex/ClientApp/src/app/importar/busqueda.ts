export class Busqueda {
    constructor(
        public Idcampana: number,
        public Text: string,
        public Idusuario: number
    ) {
    }
}
export class ParResultado {
    constructor(
        public Label: string,
        public Value: string
    ) {
    }
}
export class ResultadoBusqueda {
    constructor(
        public Result: ParResultado[],
        public Tracking: string
    ) {
    }
}