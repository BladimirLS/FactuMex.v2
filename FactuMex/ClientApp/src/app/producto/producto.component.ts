import { Component, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Router, ActivatedRoute } from '@angular/router';
import { Popup } from '../popup/popup';
import { Notificacion, Usuario } from '../usuario/usuario';
import {  Item, slaItem, precioItem, Provincia, Municipio, Sector, Moneda } from './producto';

@Component({
    selector: 'producto',
    templateUrl: './producto.component.html',
    styleUrls: ['./producto.component.css']
})
export class ProductoComponent {
    private verSla: boolean = false;
    private verPr: boolean = false;
    private monto: number = 0;
    private serv: string = 'Entrega';
    private duracion: number = 0;
    private notif: Notificacion = new Notificacion('', '');
    private cargando: boolean = false;
    private item: Item = new Item('0', '', '');
    private sla: slaItem[] = [];
    private precio: precioItem[] = [];
    private provincia: Provincia[] = [];
    private moneda: Moneda[] = [];
    private cMoneda: string='1'
    private cProv: string = '2';
    private municipio: Municipio[] = [];
    private cMun: Municipio;
    private sector: Sector[] = [];
    private cSec: Sector;
    private agregaSLA() {
        this.sla.push(new slaItem('0', this.serv, this.item.Iditem, this.cProv, this.duracion.toString(), 'dias', '', ''));
        console.log(this.cProv);
        console.log(this.sla);
    }
    private agregaPrecio() {
        this.precio.push(new precioItem('0',this.cProv,this.item.Iditem,this.cMoneda,this.monto.toString()));
    }
    private borraPrecio(pr: precioItem) {
        this.precio = this.precio.filter(p => p != pr);
    }
    private borraSLA(s:slaItem) {
        this.sla = this.sla.filter(sl => sl != s);
    }
    private nuevoItem() {
        this.item = new Item('0', '', '');
        this.sla = [];
        this.precio = [];
        this.CargaLocalidad('p');
        this.CargaMoneda();
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
    private CargaLocalidad(Loc: string) {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/localidad?s=' + Loc + '&usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            switch (Loc) {
                case 'p':
                    this.provincia = data;
                    this.cProv = this.provincia[0].Idprovincia;
                    break;
                case 'm':
                    this.municipio = data;
                    this.CargaLocalidad('s');
                    break;
                case 's':
                    this.sector = data;
                    this.cSec = this.sector.filter(s => { s.Idmunicipio == this.cMun.Idmunicipio })[0];
                    break;
            }
            this.cargando = false;
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Configuración', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Configuración')); });
    }
    private retProvincia(idProv: string) {
        return this.provincia.filter(p => p.Idprovincia == idProv)[0].Provincia1;
    }
    private retMoneda(idMon: string) {
        console.log(idMon);
        return this.moneda.filter(p => p.Idmoneda == idMon)[0].Moneda1;
    }
    private guardaItem() {
        this.cargando = true;
        this.http.post(this.baseUrl + 'apiv1/item', { Item: this.item, SLAItem: this.sla, PrecioItem: this.precio, usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION }).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.notif = new Notificacion('El producto fue guardado exitosamente.', 'Guardar');
            this.clrNotif();
            this.item = data;
            this.cargando = false;
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Producto', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Producto')); });
    }
    constructor(
        public http: HttpClient,
        @Inject('BASE_URL') public baseUrl: string,
        public servicio: Servicio,
        private route: ActivatedRoute,
        private router: Router) {
        this.servicio.CallService(new TipoServicio('Menu', true, 'LstProductos', 'LstProductos'));
        this.nuevoItem();
    }
    ngOnInit() {
        if (this.route.snapshot.paramMap.get('id')) {
            this.cargaProducto(this.route.snapshot.paramMap.get('id'));
        }
    }
    private cargaProducto(id: string) {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/item/' + id + '?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.item = data.Item;
            this.sla = data.SLAItem;
            this.precio = data.PrecioItem;
        }, error => {
            this.cargando
                = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Cliente', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Cliente'));
        });
    }
    private clrNotif() {
        setTimeout(x => { this.notif = new Notificacion('', ''); }, 2000);
    }
}
