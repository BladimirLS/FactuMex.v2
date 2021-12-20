import { Component, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Router, ActivatedRoute } from '@angular/router';
import { Popup } from '../popup/popup';
import { Notificacion, Usuario } from '../usuario/usuario';
import { Valija, ValxFac } from './valija';
import { DTOFac, RefxFacxRel } from '../factura/factura';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
    selector: 'valija',
    templateUrl: './valija.component.html',
    styleUrls: ['./valija.component.css']
})
export class ValijaComponent {
    private showTickets: number = 0;
    private showValija: boolean = false;
    private notif: Notificacion = new Notificacion('', '');
    private cargando: boolean = false;
    private cValija: Valija = new Valija('0', '', '', '', '', '');
    private valijas: Valija[] = [];
    private envalijar: boolean = false;
    private desenvalijar: boolean = false;
    private facturas: DTOFac[] = [];
    private facturasF: DTOFac[] = [];
    private incomp: RefxFacxRel[] = [];
    private buscar: string;
    private cambiaRel(ref: RefxFacxRel) {
        if (this.incomp.includes(ref))
            this.incomp = this.incomp.filter(i => i != ref);
        else
            this.incomp.push(ref);
    }
    private printFac(idfac: string) {
        window.open('http://localhost/Reports/report/Factura?IDFAC=' + idfac + '&rc:Toolbar=false', "_blank");
    }
    private nuevaValija() {
        this.showValija = true;
        this.cValija = new Valija('0', '', '', '', '', '');
    }
    private buscaFacs() {
        if (this.buscar.length > 0)
            this.facturasF = this.facturas.filter(f => f.Cliente.Ente.Nombre.includes(this.buscar) || f.Tickets.filter(t => t.Ref == (this.buscar)).length > 0 || f.Rels.filter(r => r.Ref == (this.buscar)).length > 0 || f.Cliente.Ente.Tel1 == this.buscar || f.Cliente.Ente.Tel2 == this.buscar || f.Cliente.Ente.Tel3 == this.buscar || f.Cliente.Ente.Correo == this.buscar || f.Fac.Idfactura == this.buscar)
        else
            this.facturasF = this.facturas;
    }
    private colocaEnValijaInc(f: DTOFac) {
        this.cargando = true;
        var act: string = this.desenvalijar ? 'desenvalija2' : 'envalija2';
        this.http.post(this.baseUrl + 'apiv1/' + act, { val: this.cValija.Idvalija, fac: f.Fac.Idfactura, rel:this.incomp,actn: 'Guardar', usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION }).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.notif = new Notificacion('La factura incompleta fue anexada exitosamente a la valija.', 'Guardar');
            this.cValija = data;
            this.clrNotif();
            this.cargando = false;
            this.CargaValija();
            this.facturas = this.facturas.filter(r => r != f);
            this.facturasF = this.facturas;
            this.incomp = [];
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Comprobante', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Comprobante')); });
    }
    private colocaEnValija(f: DTOFac) {
        this.cargando = true;
        var act: string = this.desenvalijar ? 'desenvalija' : 'envalija';
        this.http.post(this.baseUrl + 'apiv1/' + act, { val: this.cValija.Idvalija, fac: f.Fac.Idfactura, actn: 'Guardar', usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION }).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.notif = new Notificacion('La factura fue anexada exitosamente a la valija.', 'Guardar');
            this.cValija = data;
            this.clrNotif();
            this.cargando = false;
            this.CargaValija();
            this.facturas = this.facturas.filter(r => r != f);
            this.facturasF = this.facturas;
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Comprobante', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Comprobante')); });
    }
    private imprimeFac(f: DTOFac) {
        this.cargando = true;
        this.http.post(this.baseUrl + 'apiv1/imprimefac', { val: this.cValija.Idvalija, fac: f.Fac.Idfactura, actn: 'Guardar', usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION }).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.notif = new Notificacion('La factura fue colocada en la cola de impresión.', 'Guardar');
            this.cValija = data;
            this.clrNotif();
            this.cargando = false;
            this.CargaValija();
            this.facturas = this.facturas.filter(r => r != f);
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Comprobante', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Comprobante')); });
    }
    private terminarEnvalijado() {
        this.envalijar = false;
        this.showValija = false;
    }
    private cargaFacturas() {
        this.cargando = true;
        var idval: string = this.desenvalijar ? '1' : '0';
        this.http.get(this.baseUrl + 'apiv1/factura?val=' + this.cValija.Idvalija + '&idval=' + idval + '&usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.facturas = data;
            this.facturasF = this.facturas;
        }, error => {
            this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Factura', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Factura'));
        });
    }
    private CargaValija() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/valija?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.valijas = data;
        }, error => {
            this.cargando
                = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Comprobante', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Comprobante'));
        });
    }
    private guardaValija() {
        this.cargando = true;
        this.http.post(this.baseUrl + 'apiv1/valija', { val: this.cValija, usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION }).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.notif = new Notificacion('La valija fue creada exitosamente.', 'Guardar');
            this.cValija = data;
            this.clrNotif();
            this.cargando = false;
            this.CargaValija();
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Comprobante', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Comprobante')); });
    }
    constructor(
        public http: HttpClient,
        @Inject('BASE_URL') public baseUrl: string,
        public servicio: Servicio,
        private route: ActivatedRoute,
        private router: Router) {
        this.servicio.CallService(new TipoServicio('Menu', true, 'Valija', 'Valija'));
        this.CargaValija();
    }
    private clrNotif() {
        setTimeout(x => { this.notif = new Notificacion('', ''); }, 2000);
    }
}
