import { Component, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Router, ActivatedRoute } from '@angular/router';
import { Popup } from '../popup/popup';
import { Notificacion, Usuario } from '../usuario/usuario';
import { DTOFac, DTOEnte, Factura, Desglose, RefxFacxRel, RefxFacxTicket, RefxFacxRef } from './factura';
import { Ente } from '../lstcliente/lstcliente';
import { Direccion } from '../cliente/cliente';
import { Item, DTOItem, Moneda, precioItem } from '../producto/producto';
import { Comprobante } from '../comprobante/comprobante';
import { VALID } from '@angular/forms/src/model';
@Component({
    selector: 'factura',
    templateUrl: './factura.component.html',
    styleUrls: ['./factura.component.css']
})
export class FacturaComponent {
    private nuevoCliente: boolean = false;
    private notif: Notificacion = new Notificacion('', '');
    private usuarioActual: Usuario = new Usuario('0', '', '', '', '', '', '', '', '', '');
    private tipoEnte: string = 'org';
    private ttlNombre: string = 'Nombre de la Empresa';
    private ttlID: string = 'Número de RNC';
    private dirFac: boolean = false;
    private dirEnt: boolean = false;
    private cargando: boolean = false;
    private orgEstatus: string = '';
    private pasoSel: number = 1;
    private moneda: Moneda[] = [];
    private cMoneda: string = '1';
    private precio: number = 0;
    private monto: number = 0;
    private pagado: number = 0;
    private resta: number = 0;
    private sinPrecio: boolean = false;
    private comps: Comprobante[] = [];
    private compsRNC: Comprobante[] = [];
    private rnc: string;
    private idcomp: string;
    private cantidad: number = 1;
    private fp: string = 'Contado';
    private mp: string = 'Efectivo';
    private ticket: RefxFacxTicket = new RefxFacxTicket('0', '0', '');
    private rel: RefxFacxRel = new RefxFacxRel('0', '0', '');
    private showRels: boolean = false;
    private showTickets: boolean = false;
    private printFac() {
        window.open('http://localhost/Reports/report/Factura?IDFAC=' + this.f.Fac.Idfactura +'&rc:Toolbar=false', "_blank");
    }
    private nuevaFactura() {
        this.f = new DTOFac(
            new DTOEnte(
                new Ente('0', '', '', '', '', '', '', '', ''),
                new Direccion('0', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''),
                new Direccion('0', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '')),
            new Factura('0', '', '', '', '', '', '', ''), [], [], [], new RefxFacxRef('0', '0', 'ACS'),'');
        this.showRels = false;
        this.showTickets = false;
        this.ticket = new RefxFacxTicket('0', '0', '');
        this.rel = new RefxFacxRel('0', '0', '');
        this.mp = 'Efectivo';
        this.fp = 'Contado';
        this.cantidad = 1;
        this.nuevoCliente = false;
        this.tipoEnte = 'org';
        this.ttlNombre = 'Nombre de la Empresa';
        this.ttlID = 'Número de RNC';
        this.dirFac = false;
        this.dirEnt = false;
        this.cargando = false;
        this.orgEstatus = '';
        this.pasoSel = 1;
        this.moneda = [];
        this.cMoneda = '1';
        this.precio = 0;
        this.monto = 0;
        this.sinPrecio = false;
    }
    private remueveRel(r: string) {
        this.f.Rels = this.f.Rels.filter(ti => ti.Ref != r);
    }
    private agregaRel(e: any) {
        if (e.key == 'Enter') {
            if (this.rel.Ref.length > 0) {
                if (this.f.Rels.filter(re => re.Ref == this.rel.Ref).length == 0) {
                    this.f.Rels.push(this.rel);
                    this.rel = new RefxFacxRel('0', '', '');
                }
            }
        }
    }
    private remueveTicket(t: string) {
        this.f.Tickets = this.f.Tickets.filter(ti => ti.Ref != t);
    }
    private agregaTicket(e: any) {
        if (e.key == 'Enter') {
            if (this.ticket.Ref.length > 0)
                if (this.f.Tickets.filter(t => t.Ref == this.ticket.Ref).length == 0) {
                    this.f.Tickets.push(this.ticket);
                    this.ticket = new RefxFacxTicket('0', '0', '');
                }

        }
    }
    private selComps() {
        this.compsRNC = this.comps.filter(c => c.Rnc == this.rnc);
        if (this.compsRNC.length > 0) {
            this.idcomp = this.compsRNC[0].Idcomp;
        }
    }

    private i: DTOItem = new DTOItem(new
        Item('0', '', ''),
        [],
        []);
    private f: DTOFac = new DTOFac(
        new DTOEnte(
            new Ente('0', '', '', '', '', '', '', '', ''),
            new Direccion('0', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''),
            new Direccion('0', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '')),
        new Factura('0', '', '', '', '', '', '', ''), [], [], [], new RefxFacxRef('0', '0', 'ACS'),'');
    constructor(
        public http: HttpClient,
        @Inject('BASE_URL') public baseUrl: string,
        public servicio: Servicio,
        private route: ActivatedRoute,
        private router: Router) {
        this.CargaMoneda();
        this.CargaComprobante();
        this.servicio.CallService(new TipoServicio('Menu', true, 'Facturacion', 'Facturacion'));
    }
    ngOnInit() {
        if (this.route.snapshot.paramMap.get('id')) {
            this.cargaFactura(this.route.snapshot.paramMap.get('id'));
        }
    }
    private validaFactura(): boolean {
        if (this.cMoneda == '1')
            if (this.pagado < 53) {
                this.notif = new Notificacion('Debe abonar al menos un dólar o su equivalente en pesos', 'pagado');
                return false;
            }
        if (this.cMoneda == '2')
            if (this.pagado < 1) {
                this.notif = new Notificacion('Debe abonar al menos un dólar o su equivalente en pesos', 'pagado');
                return false;
            }
        return true;
    }
    private emiteFactura() {
        if (this.validaFactura()) {
    this.f.Fac.Idcliente = this.f.Cliente.Ente.Idente;
            this.f.Fac.Idempresa = this.idcomp;
            this.f.Fac.FormaPago = this.fp;
            this.f.Fac.MetodoPago = this.mp;
            this.cargando = true;
            this.http.post(this.baseUrl + 'apiv1/factura', { Cliente: this.f.Cliente, Fac: this.f.Fac, Desglose: this.f.Desglose, Tickets: this.f.Tickets, Rels: this.f.Rels, Referencia: this.f.Referencia, Pagado:this.pagado,usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION }).subscribe((data: any) => {
                this.servicio.validaSesion(data);
                this.notif = new Notificacion('La factura fue guardada exitosamente.', 'Guardar');
                this.clrNotif();
                this.f = data;
                this.cargando = false;
                this.pasoSel = 4;
            }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Factura', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Factura')); });
        }
    }
    private cargaFactura(id: string) {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/factura/' + id + '?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
        }, error => {
            this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Factura', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Factura'));
        });
    }
    private calc() {
        this.resta = this.monto-this.pagado;
        if (this.resta <= 0)
            this.fp = 'Contado';
        else
            this.fp = 'Crédito';
    }
    private clrNotif() {
        setTimeout(x => { this.notif = new Notificacion('', ''); }, 2000);
    }
    private selCliente(cl: Ente) {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/ente/' + cl.Idente + '?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.f.Cliente = data;
            this.nuevoCliente = false
            this.pasoSel = 2;
        }, error => {
            this.cargando
                = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Factura', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Factura'));
        });
    }
    private cargaPrecio() {
        var pr: precioItem[] = this.i.PrecioItem.filter(p => p.Idprovincia == this.f.Cliente.DirEntrega.Idprovincia && p.Moneda == this.cMoneda);
        if (pr.length > 0) {
            this.precio = parseFloat(pr[0].Precio);
            this.monto = this.precio*1.18;
            this.f.Desglose = [];
            this.f.Desglose.push(new Desglose('0', this.i.Item.Nombre, this.i.Item.Iditem, this.f.Cliente.DirEntrega.Idprovincia, this.cantidad.toString(), this.cMoneda, this.precio.toString(), this.monto.toString(), '0'));
        }
        else {
            this.precio = 0;
            this.sinPrecio = true;
        }
    }
    private selItem(it: Item) {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/item/' + it.Iditem + '?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.i = data;
            this.moneda = this.moneda.filter(m => this.i.PrecioItem.filter(p => p.Moneda == m.Idmoneda).length > 0);
            if (this.moneda.length > 0) {
                this.cMoneda = this.moneda[0].Idmoneda;
                this.cargaPrecio();
            }
            this.pasoSel = 3;
        }, error => {
            this.cargando
                = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Factura', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Factura'));
        });
    }
    private CargaMoneda() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/moneda?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.moneda = data;
            this.cMoneda = this.moneda[0].Idmoneda;
            this.cargando = false;
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Configuración', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Configuración')); });
    }
    private CargaComprobante() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/comprobante?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.comps = data;
            //var compsb: Comprobante[] = [];
            //this.comps = this.comps.filter(c => compsb.includes(c));
            if (this.comps.length > 0) {
                this.rnc = this.comps[0].Rnc;
                this.selComps();
            }

            this.cargando = false;
        }, error => {
            this.cargando
                = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Comprobante', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Comprobante'));
        });
    }
    private regresar() {
        this.pasoSel -= 1;
    }
}
