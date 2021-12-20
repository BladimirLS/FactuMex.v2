import { Component, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Login, Sesion } from './login';
import { Servicio, TipoServicio } from '../Service';
import { Usuario, Notificacion, itemPlantilla } from '../usuario/usuario';
import { ActivatedRoute, Router } from '@angular/router';
import { BFP } from '../BFP';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent {
    @ViewChild('pwd')
    pwd: ElementRef;
    private foco: boolean = false;
    private Notif: Notificacion = new Notificacion('', '');
    private Email: string = '';
    private Pwd: string = '';
    private usuario: Usuario = new Usuario('0', '', '', '', '', '', '', '', '', '');
    private permisos: string[] = [];
    private Sesion: Sesion = new Sesion('', '', new Date(), '', '');
    private login: boolean = false;
    constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string, public servicio: Servicio, private route: ActivatedRoute,
        private router: Router) {
        this.Notif = new Notificacion('Bienvenido al Sistema de Facturación de Efectos! Por favor identifícate con tu correo electrónico corporativo.', 'Login');
        
    }
    private enfocado() {
        this.foco = true;
    }
    //Carga los usuarios y revisa si el email pertenece al dominio
    private cargaUsuario(e: any) {

        if (e.key == 'Enter' || this.Email.includes('@domex.com.do')) {
            this.http.get(this.baseUrl + 'apiv1/sesion?email=' + this.Email).subscribe((result: any) => {
                if (result.Usuario.length > 0) {
                    this.usuario = result.Usuario[0];
                    this.permisos = result.Permisos;
                    this.Notif = new Notificacion('Hola ' + this.usuario.Nombre.split(' ')[0] + '! Ahora coloca tu contraseña para iniciar sesión.', 'Login');
                    this.pwd.nativeElement.focus();
                }
                else
                    this.Notif = new Notificacion('No se reconoce el correo electrónico, asegúrate de que lo estás digitando correctamente.', 'Login');
            }, error => console.error(error));
        }
        else {
            this.usuario = new Usuario('0', '', '', '', '', '', '', '', '', '');
            this.Notif = new Notificacion('Bienvenido al Sistema de Renovaciones! Por favor identifícate con tu correo electrónico corporativo.', 'Login');
        }
    }
    private iniciaSesion(e: any) {
        if (e.key == 'Enter')
            this.empezar();
    }
    // funcion que se utiliza para traer el login
    private empezar() {
        if (this.usuario.IDUsuario != '0') {
            var FP: BFP = new BFP();
            this.http.post(this.baseUrl + 'apiv1/sesion', { Email: this.Email, Pwd: this.Pwd, Meta:FP.FPBrowser() }).subscribe((result: any[]) => {
                if (result.length > 0) {
                    this.Sesion = result[0];
                    this.usuario.usr = this.Email;
                    this.usuario.token = this.Sesion.CODSESION;
                    this.servicio.CallService(new TipoServicio('Usuario', true, this.usuario, 'Login'));
                    this.servicio.CallService(new TipoServicio('Login', true, this.Sesion, 'Login'));
                    this.servicio.CallService(new TipoServicio('Permisos', true, this.permisos, 'Login'));
                }
                else {
                    this.Notif = new Notificacion('No se reconoce la contraseña, asegúrate de que la estas digitando correctamente.', 'Login');
                }
            }, error => console.error(error));
        }
        else {
            this.Notif = new Notificacion('No se reconoce el correo electrónico, asegúrate de que lo estás digitando correctamente.', 'Login');
        }
    }

}
