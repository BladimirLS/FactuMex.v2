import { Component, Inject, ElementRef, ViewChild, ChangeDetectorRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Console } from '@angular/core/src/console';
import { Subscription } from 'rxjs';
import { Popup } from '../popup/popup';


@Component({
    selector: 'vprevia',
    templateUrl: './vprevia.component.html',
    styleUrls: ['./vprevia.component.css']
})
export class vPreviaComponent {
    private cargando: boolean = false;
    private subscriptionService: Subscription = new Subscription();
    private ss: Sesion = new Sesion('', '', new Date(), '', '');
    private Frm: SafeHtml;
    private Poliza: string;
    private data: any = [];
    private fscreen: boolean = false;
    private isVisible: boolean = false;
    constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string, public servicio: Servicio, private sanitizer: DomSanitizer) {
        this.subscriptionService = this.servicio.ServiceObservable.subscribe((TS: TipoServicio) => {
            switch (TS.NombreServicio) {
                case 'Vista Previa':
                    if (TS.Status == true) {
                        this.Poliza = TS.Params;
                        this.cargaVistaPrevia();
                    }
                    else {
                        this.isVisible = false;
                    }
                    break;
            }
        });
    }
    private fullscreen() {
        this.fscreen = !this.fscreen;
    }
    private cerrar() {
        this.servicio.CallService(new TipoServicio('Vista Previa', false, null, 'Vista Previa'));
    }
    private initializeCapture() {
        this.Frm = this.sanitizer.bypassSecurityTrustHtml('');
    }
    private cargaVistaPrevia() {
        this.initializeCapture();
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/mail?Poliza=' + this.Poliza + '&usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe(result => {
            this.servicio.validaSesion(result);
            this.cargando = false;
            if (result[0] != undefined && result[0] != null) {
                this.data = result[0].Cuerpo;
                this.DibujaFrm();
                this.isVisible = true;
            }
            else {
                this.data = '<br/><h1>No existe información asociada a tu petición</h1>';
                this.DibujaFrm();
                this.isVisible = true;
            }
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Vista Previa', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Vista Previa')); });
    }
    private DibujaFrm() {
        this.Frm = this.sanitizer.bypassSecurityTrustHtml(this.data);
    }
}
