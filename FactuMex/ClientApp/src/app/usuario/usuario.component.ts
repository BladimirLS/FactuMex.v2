import { Component, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Router, ActivatedRoute } from '@angular/router';
import { Subir } from '../subir/subir';
import { Archivo } from '../importar/importar';
import { Usuario, Notificacion, itemPermiso, DTOPermisos, itemPlantilla } from './usuario';
import { Popup } from '../popup/popup';

@Component({
    selector: 'usuario',
    templateUrl: './usuario.component.html',
    styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent {
    @ViewChild('au')
    au: ElementRef;
    private filesIn: any;
    private totalKB: number = 0;
    private Archivos: Subir[] = [];
    private currentArchivo: Archivo;
    private usuarios: Usuario[] = [];
    private buscar: string = '';
    private cargando: boolean = false;
    private usuarioActual: Usuario = new Usuario('0', '', '', '', '', '', '', '', '', '');
    private repPwd: string = '';
    private foc: number = 0;
    private notif: Notificacion = new Notificacion('', '');
    private showPermisos: boolean;
    private lstPermiso: itemPermiso[] = [];
    private lstGrupo: string[] = [];
    private dtPermisos: DTOPermisos = new DTOPermisos([], []);
    constructor(
        public http: HttpClient,
        @Inject('BASE_URL') public baseUrl: string,
        public servicio: Servicio,
        private route: ActivatedRoute,
        private router: Router) {
        this.servicio.CallService(new TipoServicio('Menu', true, 'Usuario', 'Usuario'));
        this.CargaUsuarios();
    }
    private verificaPermiso(item: itemPermiso) {
        if (this.dtPermisos.Usuario.filter((up: itemPlantilla) => up.Permiso == (item.Grupo + '.' + item.Permiso)).length > 0)
            return true;
        else
            return false;
    }
    private togglePermisos(state: string) {
        if (state == 'show') {
            this.showPermisos = true;
            this.cargaPermisos();
        }
        else
            this.showPermisos = false;
    }
    private retornaPermisos(gr: string) {
        return this.lstPermiso.filter((ip => ip.Grupo == gr));
    }
    private togglePermiso(ip: itemPermiso, evento: any) {
        this.cargando = true;
        this.http.post(this.baseUrl + 'apiv1/permiso', { IDUsuario: this.usuarioActual.IDUsuario, Permiso: ip.Grupo + '.' + ip.Permiso, Estatus: evento.target.checked, usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION }).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Usuario', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Usuario')); });
    }
    private cargaPermisos() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/permiso?idusuario=' + this.usuarioActual.IDUsuario + '&usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.dtPermisos = data;
            this.lstPermiso = this.dtPermisos.Plantilla;
            var Grupo: string[] = this.lstPermiso.map((ip: itemPermiso) => ip.Grupo);
            var Grupos: Set<string> = new Set<string>(Grupo);
            this.lstGrupo = Array.from(Grupos);
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Usuario', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Usuario')); });
    }

    private busca(event: any) {
        if (event.code == 'Enter') {
            this.CargaUsuarios();
        }
    }
    private AgregaArchivo(Form: any) {
        this.Archivos = [];
        for (let file of Form.target.files) {
            if (this.Archivos.filter((Archivo: Subir) => { if (Archivo.Nombre == file.name) return true; }).length == 0) {
                if (file.size > 0) {
                    let reader = new FileReader();
                    reader.onloadend = (data: any) => {
                        var BinayString: any = data.currentTarget.result;
                        var Base64Url: any = BinayString;
                        var MimeType: any = Base64Url.substring(Base64Url.lastIndexOf("data") + 5, Base64Url.lastIndexOf(";"));
                        var Base64Url: any = Base64Url.substring(Base64Url.lastIndexOf("base64") + 7);
                        this.Archivos.push(new Subir(0, BinayString, Base64Url, MimeType, file.name, 0, Math.round(file.size / 1024), 0, 0, 'LaRuta'));
                        this.totalKB += Math.round(file.size / 1024);
                    };
                    reader.readAsDataURL(file);
                }
            }
        }
        this.au.nativeElement.value = "";
    }

    private getImage(imageUrl: string): any {
        return this.http.get(imageUrl, { responseType: 'blob' }).subscribe((data: any) => {
            console.log(imageUrl+':'+data);
        });
    }

    private CargaUsuarios() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/usuario?u=' + this.buscar + '&usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.usuarios = data;
        });
    }
    private validaCreacion() {
        if (this.usuarioActual.IDUsuario == '0')
            if (this.Archivos.length == 0) {
                this.notif.Msj = 'Debes cargar una foto para identificar el usuario.';
                this.notif.Src = 'Foto';
                return false;
            }
        if (this.usuarioActual.Nombre.length == 0) {
            this.notif.Msj = 'Debes colocar el nombre para identificar el usuario.';
            this.notif.Src = 'Nombre';
            return false;
        }
        if (this.usuarioActual.Cedula.length == 0) {
            this.notif.Msj = 'Debes colocar la cedula para identificar el usuario.';
            this.notif.Src = 'Cedula';
            return false;
        }
        if (this.usuarioActual.Email.length == 0) {
            this.notif.Msj = 'Debes colocar el Correo Electrónico para que el usuario pueda recibir notificaciones.';
            this.notif.Src = 'Email';
            return false;
        }
        if (this.usuarioActual.Telefono.length == 0) {
            this.notif.Msj = 'Debes colocar el Telefono.';
            this.notif.Src = 'Telefono';
            return false;
        }
        if (this.usuarioActual.Pwd.length < 6) {
            this.notif.Msj = 'La contrasena debe poseer al menos 6 caracteres.';
            this.notif.Src = 'Pwd';
            return false;
        }
        if (this.usuarioActual.Pwd.includes(' ')) {
            this.notif.Msj = 'La contrasena no debe incluir espacios.';
            this.notif.Src = 'Pwd';
            return false;
        }
        if (this.usuarioActual.Pwd != this.repPwd) {
            this.notif.Msj = 'Las contrasenas no coinciden.';
            this.notif.Src = 'RepPwd';
            return false;
        }
        return true;
    }
    private clrNotif() {
        setTimeout(x => { this.notif = new Notificacion('', ''); }, 2000);
    }
    private cargaUsuario(u: Usuario) {
        this.usuarioActual = u;
        this.repPwd = u.Pwd;
        this.Archivos = [];
    }
    private NuevoUsuario() {
        this.usuarioActual = new Usuario('0', '', '', '', '', '', '', '', '', '');
        this.Archivos = [];
        this.repPwd = '';
    }
    private GuardaUsuario() {
        if (this.validaCreacion()) {
            if (this.Archivos.length > 0)
                this.usuarioActual.Foto = this.Archivos[0].Base64Url;
            this.cargando = true;
            this.usuarioActual.usr = this.servicio.getUsuario().Email;
            this.usuarioActual.token = this.servicio.getLogin().CODSESION;
            this.http.post(this.baseUrl + 'apiv1/usuario', this.usuarioActual).subscribe((data: any) => {
                this.servicio.validaSesion(data);
                this.notif = new Notificacion('El usuario fue guardado exitosamente.', 'Guardar');
                this.clrNotif();
                this.CargaUsuarios();
                this.usuarioActual.IDUsuario = data;
                this.cargando = false;
            }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Usuario', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Usuario')); });
        }
    }
}
