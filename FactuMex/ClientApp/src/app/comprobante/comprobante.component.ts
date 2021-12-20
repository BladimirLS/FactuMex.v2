import {Component, Inject, ViewChild, ElementRef, OnInit} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Router, ActivatedRoute } from '@angular/router';
import { Popup } from '../popup/popup';
import { Notificacion, Usuario } from '../usuario/usuario';
import { Comprobante } from './comprobante';
import {Observable} from 'rxjs';
import {ComprobanteService} from './Service/comprobante.service';

@Component({
    selector: 'comprobante',
    templateUrl: './comprobante.component.html',
    styleUrls: ['./comprobante.component.css']
})
export class ComprobanteComponent implements OnInit {
    comprobante$: Observable<Comprobante[]>;
    private verSla = false;
    private verPr = false;
    private monto = 0;
    private serv = 'Entrega';
    private duracion = 0;
    private notif: Notificacion = new Notificacion('', '');
    private cargando = false;
    private comp: Comprobante = new Comprobante('0', '', '', '', '', '', '', '', '');
    private comps: Comprobante[] = [];
    private nuevoComprobante() {
        this.comp = new Comprobante('0', '', '', '', '', '', '', '', '');
    }
    private CargaComprobante() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/comprobante?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.comps = data;
            this.cargando = false;
        }, error => { this.cargando
                = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Comprobante', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Comprobante')); });
    }
    private guardaComprobante() {
        this.cargando = true;
        this.http.post(this.baseUrl + 'apiv1/comprobante', {comp: this.comp, usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION }).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.notif = new Notificacion('Los comprobantes fueron guardado exitosamente.', 'Guardar');
            this.clrNotif();
            this.comp = data;
            this.cargando = false;
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Comprobante', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Comprobante')); });
    }
    constructor(
        public http: HttpClient,
        private comprobanteService: ComprobanteService,
        @Inject('BASE_URL') public baseUrl: string,
        public servicio: Servicio,
        private route: ActivatedRoute,
        private router: Router) {
        this.nuevoComprobante();
        this.CargaComprobante();
        this.servicio.CallService(new TipoServicio('Menu', true, 'Comprobantes', 'Comprobantes'));
    }
    ngOnInit() {
    }

    private clrNotif() {
        setTimeout(x => { this.notif = new Notificacion('', ''); }, 2000);
    }
    comprobantes() {
        // @ts-ignore
        this.comprobante$ = this.comprobanteService.getComprobante();
    }
    delete(Idcomp) {
        const ans = confirm('Seguro que quieres eliminar este comprobante?: ' + Idcomp);
        if (ans) {
            // @ts-ignore
            this.comprobanteService.deleteComprobante(Idcomp).subscribe((data) => {
                this.comprobantes();
            });
        }
    }
}
