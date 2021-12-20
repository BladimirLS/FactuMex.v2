import { Component, ChangeDetectorRef } from '@angular/core';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Subscription } from 'rxjs';
import { Usuario } from '../usuario/usuario';
import { Router } from '@angular/router';
@Component({
    selector: 'app-nav-menu',
    templateUrl: './nav-menu.component.html',
    styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
    isExpanded = true;
    private Menu: any = { SinMenu: false, Dashboard: false, Comprobantes: false, Cliente: false, Productos: false, LstCliente: false, LstProductos: false, Conf: false, Usuario: false, Min: false, Salir: false,Valija:false }
    private subscriptionService: Subscription;
    private usuario: Usuario = new Usuario('0', '', '', '', '', '', '', '','','');
    private Permisos: any[] = [];
    private selm: string = 'Dashboard';
    private invisible: boolean = false;
    private sinMenu() {
        this.servicio.CallService(new TipoServicio('invisibleMenu', true, true, 'menu'));
        this.invisible = true;
    }
    toggle() {
        var expnd = !this.isExpanded;
        this.servicio.CallService(new TipoServicio('isExpanded', expnd, expnd, 'menu'));
    }
    constructor(private servicio: Servicio, private ref: ChangeDetectorRef, private router: Router) {
        this.subscriptionService = this.servicio.ServiceObservable.subscribe((TS: TipoServicio) => {
            switch (TS.NombreServicio) {
                case 'Usuario':
                    this.usuario = TS.Params;
                    break;

                case 'Permisos':
                    this.Permisos = TS.Params;
                    break;
                case 'isExpanded':
                    this.isExpanded = TS.Params;
                    break;
                case 'Menu':
                    this.selm = TS.Params;
                    this.select(TS.Params);
                    break;
            }
        });
    }
    private ngAfterContentInit() {
        if (window.innerWidth < 970) {
            this.isExpanded = false;
        }
        else {
            this.isExpanded = true;
        }
    }
    private select(sel: any) {
        this.deSel();
        this.Menu[sel] = true;
        //setTimeout(() => { this.deSel(); }, 5000);
    }
    private deSel() {
        this.Menu.Dashboard = false;
        this.Menu.Comprobantes = false;
        this.Menu.Cliente = false;
        this.Menu.Productos = false;
        this.Menu.LstCliente = false;
        this.Menu.LstProductos = false;
        this.Menu.Conf = false;
        this.Menu.Usuario = false;
        this.Menu.Min = false;
        this.Menu.Salir = false;
        this.Menu.SinMenu = false;
        this.Menu.Valija = false;
    }
    private callCancell() {
        return false;
    }
    private Salir() {
        this.servicio.destroyCookies();
        return false;
    }
}
