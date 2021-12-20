import { Injectable, Inject } from '@angular/core';
import { Subject, Subscriber } from 'rxjs';
import { Login, Sesion } from './login/login';
import { Popup } from './popup/popup';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Usuario } from './usuario/usuario';
import { Router } from '@angular/router';
import { BFP } from './BFP';

@Injectable({providedIn: 'root'
    })

export class Servicio {

    validaSesion(data: any) {
        try {
            if (data.denied) {
                this.destroyCookies();
                this.router.navigateByUrl('');
            }
            else {
                var FP: BFP = new BFP();
                this.http.post(this.baseUrl + 'apiv1/validasesion', { Token: this.ssInfo.CODSESION, Meta: FP.FPBrowser() }).subscribe((dataVal: any) => {
                    if (dataVal.denied == 1) {
                        this.destroyCookies();
                        this.router.navigateByUrl('');
                    }
                }, error => console.error(error));
            }
        } catch (e) {
            var FP: BFP = new BFP();
            this.http.post(this.baseUrl + 'apiv1/validasesion', { Token: this.ssInfo.CODSESION, Meta: FP.FPBrowser() }).subscribe((dataVal: any) => {
                if (dataVal.denied == 1) {
                    this.destroyCookies();
                    this.router.navigateByUrl('');
                }
            }, error => console.error(error));
        }
        
    }
    
    public ServiceObservable: Subject<TipoServicio> = new Subject<TipoServicio>();
    private ssInfo: Sesion = new Sesion('', '', new Date(), '', '');
    private loadingInfo: string = '';
    private Permisos: string[] = [];
    private popupInfo: Popup = new Popup('', '', '', '');
    private vistaPreviaPoliza: string = '';
    private usuario: Usuario = new Usuario('0', '', '', '', '', '', '', '','','');
    private permisos: any[] = [];
    private xpanded: boolean = true;
    private menu: string='Dashboard';
    constructor(public http: HttpClient,@Inject('BASE_URL') public baseUrl: string,private router: Router) {
        try {
            this.validaLogin();
        } catch (e) {

        }
    }
    public destroyCookies() {
        sessionStorage.removeItem('us');
        sessionStorage.removeItem('pr');
        sessionStorage.removeItem('ss');
        this.usuario = new Usuario('0', '', '', '', '', '', '', '','','');
        this.permisos = [];
        this.ssInfo = new Sesion('', '', new Date(), '', '');
        this.ServiceObservable.next(new TipoServicio('Login', false, this.ssInfo, 'Login'));
    }
    public VerificaPermiso(Permiso: string) {
        return this.permisos.filter((permiso: any) => permiso.PERMISO == Permiso).length > 0;
    }
    public getPermisos() {
        return this.permisos;
    }
    public getUsuario() {
        return this.usuario;
    }
    public validaLogin() {
        this.ssInfo = <Sesion>JSON.parse(sessionStorage.getItem('ss'));

        if (this.ssInfo == null)
            this.ServiceObservable.next(new TipoServicio('Login', false, this.ssInfo, 'Login'));
        else {
            //var exp: Date = (new Date(this.ssInfo.EXPIRA.toString().replace('T', ' ')));
            //if (exp > new Date()) {
            this.permisos = <string[]>JSON.parse(sessionStorage.getItem('pr'));
            this.usuario = <Usuario>JSON.parse(sessionStorage.getItem('us'));
            this.ServiceObservable.next(new TipoServicio('Login', true, this.ssInfo, 'Login'));
            this.ServiceObservable.next(new TipoServicio('Permisos', true, this.permisos, 'Login'));
            this.ServiceObservable.next(new TipoServicio('Usuario', true, this.usuario, 'Login'));
            //}
            //else {
            //    this.ServiceObservable.next(new TipoServicio('Login', false, this.ssInfo, 'Login'));
            //}
        }
    }
    public CallService(TS: TipoServicio) {
        if (TS.NombreServicio != 'Login')
            this.validaLogin();
        switch (TS.NombreServicio) {
            case 'Menu':
                this.menu = TS.Params;
                break;
            case 'Usuario':
                this.usuario = TS.Params;
                sessionStorage.setItem('us', JSON.stringify(this.usuario));
                break;
            case 'Permisos':
                this.permisos = TS.Params;
                sessionStorage.setItem('pr', JSON.stringify(this.permisos));
                break;
            case 'Login':
                this.ssInfo = TS.Params;
                sessionStorage.setItem('ss', JSON.stringify(this.ssInfo));
                break;
            case 'Vista Previa':
                this.vistaPreviaPoliza = TS.Params;
                break;
            case 'isExpanded':
                this.xpanded = TS.Params;
                break;
            case 'Popup':
                this.popupInfo = TS.Params;
                break;
            //default:
            //    break;
        }
        this.ServiceObservable.next(TS);
    }
    public getMenu() {
        return this.menu;
    }
    public getLogin() {
        return this.ssInfo;
    }
    public getPopupResult() {
        return this.popupInfo;
    }
    public getObservable() {
        return this.ServiceObservable.asObservable();
    }
}
export class TipoServicio {
    constructor(public NombreServicio: string, public Status: boolean, public Params: any, public Caller: string) {

    }
}
