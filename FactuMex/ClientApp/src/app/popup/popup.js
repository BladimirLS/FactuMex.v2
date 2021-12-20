export class Login {
    constructor(Idente, IdentePadre, Ente1, Id, IdtipoEnte, Email, Pwd) {
        this.Idente = Idente;
        this.IdentePadre = IdentePadre;
        this.Ente1 = Ente1;
        this.Id = Id;
        this.IdtipoEnte = IdtipoEnte;
        this.Email = Email;
        this.Pwd = Pwd;
    }
}
export class Sesion {
    constructor(Idsesion, Idusuario, Token, Desde, Hasta) {
        this.Idsesion = Idsesion;
        this.Idusuario = Idusuario;
        this.Token = Token;
        this.Desde = Desde;
        this.Hasta = Hasta;
    }
}
//# sourceMappingURL=login.js.map