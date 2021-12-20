import { Component, Inject, ViewChild, ElementRef, Input, EventEmitter, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Router, ActivatedRoute } from '@angular/router';
import { Popup } from '../popup/popup';
import { Notificacion, Usuario } from '../usuario/usuario';
import { RNCEmpresa, Ente, Direccion } from './cliente';
import { Municipio, Sector, Provincia } from '../producto/producto';

@Component({
    selector: 'cliente',
    templateUrl: './cliente.component.html',
    styleUrls: ['./cliente.component.css']
})
export class ClienteComponent {
    @Input() public wizardFac: boolean = false;
    @Output() clientSaved = new EventEmitter();
    private cProv: string = '2';
    private provincia: Provincia[] = [];
    private municipio: Municipio[] = [];
    private muns: Municipio[] = [];
    private cMun: string;
    private sector: Sector[] = [];
    private secs: Sector[] = [];
    private cSec: string;
    private notif: Notificacion = new Notificacion('', '');
    private usuarioActual: Usuario = new Usuario('0', '', '', '', '', '', '', '', '', '');
    private tipoEnte: string = 'org';
    private ttlNombre: string = 'Nombre de la Empresa';
    private ttlID: string = 'Número de RNC';
    private dirFac: boolean = false;
    private dirEnt: boolean = false;
    private cargando: boolean = false;
    private rnc: string = '';
    private RNCEmp: RNCEmpresa = new RNCEmpresa('', '', '', '', '', '', '', '', '', '', '');
    private cliente: Ente = new Ente('0', 'org', '', '', '', '', '', '', '');
    private dirFact: Direccion = new Direccion('0', '', 'fac', '', '', '', '', '', '', '', '', '', '', '', '', '', '');
    private dirEntr: Direccion = new Direccion('0', '', 'ent', '', '', '', '', '', '', '', '', '', '', '', '', '', '');
    private orgEstatus: string = '';
    private to: any = null;
    eclientSaved() {
        this.clientSaved.emit(this.cliente);
    }
    private nuevoCliente() {
        this.cliente = new Ente('0', 'org', '', '', 'RNC', '', '', '', '');
        this.dirFact = new Direccion('0', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '');
        this.dirEntr = new Direccion('0', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '');
        this.setOrg();
        this.dirFac = false;
        this.dirEnt = false;
    }
    private validaCliente(): boolean {
        this.to = null;
        if (this.cliente.Nombre.length < 3) {
            this.notif = new Notificacion('El nombre del cliente es un campo requerido.', 'cliente.nombre')
            return false;
        }
        if (this.rnc.length < 5) {
            this.notif = new Notificacion('La identificación del cliente es un campo requerido.', 'cliente.id')
            return false;
        }
        if (this.cliente.Tel1.length < 10) {
            this.notif = new Notificacion('El Teléfono 1 es un campo requerido.', 'cliente.tel1')
            return false;
        }
        
        if (this.dirEntr.Calle.length < 1) {
            this.dirEnt = true;
            this.notif = new Notificacion('La calle es un campo requerido.', 'dirent.calle')
            return false;
        }
        if (this.dirEntr.Nro.length < 1) {
            this.dirEnt = true;
            this.notif = new Notificacion('El número es un campo requerido.', 'dirent.nro')
            return false;
        }
        if (!(parseInt(this.cSec) > 0)) {
            console.log(this.cSec);
            this.dirEnt = true;
            this.notif = new Notificacion('El sector es un campo requerido.', 'dirent.sector')
            return false;
        }
        return true;
    }
    private setDirEnt() {
        this.dirEntr.Idsector = this.cSec;
        this.dirEntr.Sector = this.sector.filter(s => s.Idlocalidad == this.cSec)[0].Localidad;
        this.dirEntr.Idmunicipio = this.cMun;
        this.dirEntr.Municipio = this.municipio.filter(mu => mu.Idmunicipio == this.cMun)[0].Municipio1;
        this.dirEntr.Idprovincia = this.cProv;
        this.dirEntr.Provincia = this.provincia.filter(pr => pr.Idprovincia == this.cProv)[0].Provincia1;
        this.cliente.Id = this.rnc;
    }
    private guardaCliente() {
        if (this.validaCliente()) {
            this.cargando = true;
            this.setDirEnt();
            this.http.post(this.baseUrl + 'apiv1/ente', { Ente: this.cliente, DirEntrega: this.dirEntr, DirFacturacion: this.dirFact, usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION }).subscribe((data: any) => {
                this.servicio.validaSesion(data);
                this.notif = new Notificacion('El cliente fue guardado exitosamente.', 'Guardar');
                this.clrNotif();
                this.cliente = data;
                this.cargando = false;
                if (this.wizardFac)
                    this.eclientSaved();
            }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Cliente', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Cliente')); });
        }
    }
    private guardaCliFactura() {
        this.guardaCliente();
    }
    private setOrg() {
        this.ttlNombre = 'Nombre de la Empresa';
        this.ttlID = 'Número de RNC';
        this.tipoEnte = 'org';
        this.cliente.TipoEnte = 'org';
        this.cliente.TipoId = 'RNC';
    }
    private setPer() {
        this.ttlNombre = 'Nombre de la Persona';
        this.ttlID = 'Cédula o Pasaporte';
        this.tipoEnte = 'per';
        this.cliente.TipoEnte = 'per';
        this.cliente.TipoId = 'ID';
    }
    constructor(
        public http: HttpClient,
        @Inject('BASE_URL') public baseUrl: string,
        public servicio: Servicio,
        private route: ActivatedRoute,
        private router: Router) {
        this.CargaLocalidad('p');
        this.CargaLocalidad('m');
        this.CargaLocalidad('s');
        this.servicio.CallService(new TipoServicio('Menu', true, 'LstCliente', 'cliente'));
    }
    ngOnInit() {
        if (this.route.snapshot.paramMap.get('id')) {
            this.cargaCliente(this.route.snapshot.paramMap.get('id'));
        }
    }
    private cargaEmpresaxRNC(e: any) {
        if (e.key == 'Enter' && this.cliente.Idente == '0') {
            this.cargando = true;
            this.http.get(this.baseUrl + 'apiv1/rnc/' + this.rnc + '?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
                this.servicio.validaSesion(data);
                this.cargando = false;
                this.RNCEmp = <RNCEmpresa>data;
                this.cliente.Nombre = this.RNCEmp.Nombre1;
                this.cliente.Tel1 = this.RNCEmp.Tel;
                this.dirFact.Calle = this.RNCEmp.Dir1;
                this.dirFact.Nro = this.RNCEmp.Dir2;
                this.dirFact.Sector = this.RNCEmp.Dir3;
                this.orgEstatus = this.RNCEmp.Estatus;
            }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Cliente', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Cliente')); });
        }
    }
    private cargaCliente(id: string) {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/ente/' + id + '?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.cliente = data.Ente;
            this.dirEntr = data.DirEntrega;
            this.dirFact = data.DirFacturacion;
            this.rnc = this.cliente.Id;
            this.tipoEnte = this.cliente.TipoEnte;
        }, error => {
            this.cargando
                = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Cliente', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Cliente'));
        });
    }
    private clrNotif() {
        this.to = setTimeout(x => { this.notif = new Notificacion('', ''); }, 5000);
    }
    private cargaSec() {
        this.secs = this.sector.filter(s => s.Idmunicipio == this.cMun);
        if (this.secs.length > 0)
            this.cSec = this.secs[0].Idlocalidad;
    }
    private cargaMun() {
        this.muns = this.municipio.filter(m => m.Idprovincia == this.cProv);
        if (this.muns.length > 0) {
            this.cMun = this.muns[0].Idmunicipio;
            this.cargaSec();
        }

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
                    this.cargaMun();
                    break;
                case 's':
                    this.sector = data;
                    break;
            }
            this.cargando = false;
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Configuración', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Configuración')); });
    }
}
