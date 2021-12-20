import { Component, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Router, ActivatedRoute } from '@angular/router';
import { Popup } from '../popup/popup';
import { Notificacion, Usuario } from '../usuario/usuario';
import { DTOFac } from '../factura/factura';
import { TasaUS } from './tasa';

@Component({
    selector: 'tasa',
    templateUrl: './tasa.component.html',
    styleUrls: ['./tasa.component.css']
})
export class TasaComponent {
    private notif: Notificacion = new Notificacion('', '');
    private cargando: boolean = false;
    private tasa = new TasaUS(0, 0, new Date());
    private tasas: TasaUS[] = [];

    private nuevaTasa() {
        this.tasa = new TasaUS(0, 0, new Date());
    }
    private guardaTasa() {
        this.cargando = true;
        this.http.post(this.baseUrl + 'apiv1/tasaus' , { Tasa: this.tasa, actn: 'Guardar', usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION }).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.notif = new Notificacion('La tasa fue guardada exitosamente.', 'Guardar');
            this.clrNotif();
            this.cargando = false;
            this.cargaTasa();
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Tasa', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Tasa')); });
    }

    private cargaTasa() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/tasaus?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.tasas = data;
        }, error => {
            this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Tasa', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Tasa'));
        });
    }
    constructor(
        public http: HttpClient,
        @Inject('BASE_URL') public baseUrl: string,
        public servicio: Servicio,
        private route: ActivatedRoute,
        private router: Router) {
        this.servicio.CallService(new TipoServicio('Menu', true, 'Tasa', 'Tasa'));
        this.cargaTasa();
    }
    private clrNotif() {
        setTimeout(x => { this.notif = new Notificacion('', ''); }, 2000);
    }
}
