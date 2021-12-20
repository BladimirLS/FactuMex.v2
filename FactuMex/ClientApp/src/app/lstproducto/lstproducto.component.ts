import {Component, Inject, ViewChild, ElementRef, Input, Output, EventEmitter, OnInit} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Router, ActivatedRoute } from '@angular/router';
import {productoService} from './Service/lstproducto.service';
import { Popup } from '../popup/popup';
import { Notificacion, Usuario } from '../usuario/usuario';
import { Item } from '../producto/producto';
import {Observable} from 'rxjs';



@Component({
    // tslint:disable-next-line:component-selector
    selector: 'lstproducto',
    templateUrl: './lstproducto.component.html',
    styleUrls: ['./lstproducto.component.css']
})
export class LstproductoComponent implements OnInit {
    @Input() public wizardFac = false;
    items$: Observable<Item[]>;
    private verSla = false;
    private verPr = false;
    private monto = 0;
    private serv = 'Entrega';
    private duracion = 0;
    private notif: Notificacion = new Notificacion('', '');
    private cargando = false;
    private producto: Item[] = [];
    private cProducto: Item = new Item('0', '', '');
    private inp = '';
    @Output() valueChange = new EventEmitter();
    valueChanged() {
        this.valueChange.emit(this.cProducto);
    }
    private cargaProducto() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/item?s=' + this.inp + '&usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.producto = data;
        }, error => {
            this.cargando
                = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Comprobante', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Comprobante'));
        });
    }
    private cargaProductos(e: any) {
        if (e.key == 'Enter') {
            this.cargaProducto();
        }
    }
    // @ts-ignore
    constructor(
        public http: HttpClient,
        private ProductoService : productoService,
        @Inject('BASE_URL') public baseUrl: string,
        public servicio: Servicio,
        private route: ActivatedRoute,
        private router: Router) {
        this.servicio.CallService(new TipoServicio('Menu', true, 'LstProductos', 'LstProductos'));
    }
    ngOnInit() {
        // if (this.wizardFac) {
        //    this.inp = '(TODOS)';
        //    //this.cargaProducto();
        // }

        this.cargaProducto();
        this.productoss();
    }
    productoss() {
        // @ts-ignore
        this.items$ = this.ProductoService.getProductos();
    }
    delete(Iditem) {
        const ans = confirm('Seguro que quieres eliminar este producto?: ' + Iditem);
        if (ans) {
            // @ts-ignore
            this.ProductoService.deleteProducto(Iditem).subscribe((data) => {
                this.productoss();
            });
        }
    }
    private clrNotif() {
        setTimeout(x => { this.notif = new Notificacion('', ''); }, 2000);
    }
}
