import {Component, Inject, ViewChild, ElementRef, Input, Output, EventEmitter, OnInit} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Router, ActivatedRoute } from '@angular/router';
import { Popup } from '../popup/popup';
import { Notificacion, Usuario } from '../usuario/usuario';
import { Ente } from '../cliente/cliente';
import {FactmexService} from '../Service/factmex.service';
import {Observable} from 'rxjs';

@Component({
    selector: 'lstcliente',
    templateUrl: './lstcliente.component.html',
    styleUrls: ['./lstcliente.component.css']
})
export class LstclienteComponent implements OnInit{
    @Input() public wizardFac: boolean = false;
    ente$: Observable<Ente[]>;
    private verSla: boolean = false;
    private verPr: boolean = false;
    private monto: number = 0;
    private serv: string = 'Entrega';
    private duracion: number = 0;
    private notif: Notificacion = new Notificacion('', '');
    private cargando: boolean = false;
    private cliente: Ente[] = [];
    private cCliente: Ente = new Ente('0', '', '', '', '', '', '', '', '');
    private inp: string;
    @Output() valueChange = new EventEmitter();
    valueChanged() { 
        this.valueChange.emit(this.cCliente);
    }
    private cargaCliente(e: any) {
        if (e.key == 'Enter') {
            this.cargando = true;
            this.http.get(this.baseUrl + 'apiv1/ente?s='+this.inp+'&usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
                this.servicio.validaSesion(data);
                this.cargando = false;
                this.cliente = data;
            }, error => {
                this.cargando
                    = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Comprobante', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Comprobante'));
            });
        }
    }

    private cargaTodosClientes() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/ente?s=&usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.cliente = data;
        }, error => {
            this.cargando = false;
            this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Comprobante', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Comprobante'));
        });
    }

    constructor(
        public http: HttpClient,
        private factmexService: FactmexService,
        @Inject('BASE_URL') public baseUrl: string,
        public servicio: Servicio,
        private route: ActivatedRoute,
        private router: Router) {
        this.servicio.CallService(new TipoServicio('Menu', true, 'LstCliente', 'cliente'));
    }

    ngOnInit() {
        this.cargaTodosClientes();
        this.loadBlogPosts();
    }
    loadBlogPosts() {
        // @ts-ignore
        this.ente$ = this.factmexService.getBlogPosts();
    }
    delete(Idente) {
        const ans = confirm('Seguro que quieres eliminar este cliente?: ' + Idente);
        if (ans) {
            this.factmexService.deleteBlogPost(Idente).subscribe((data) => {
                this.loadBlogPosts();
            });
        }
    }

    private clrNotif() {
        setTimeout(x => { this.notif = new Notificacion('', ''); }, 2000);
    }
}
