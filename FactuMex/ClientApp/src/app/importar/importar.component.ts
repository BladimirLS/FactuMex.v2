import { Component, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Archivo, ParDB, CamposClientes, CamposAsignaciones, TrioDB, Par2Trio } from './importar';
import { Popup } from '../popup/popup';
import { Router, ActivatedRoute } from '@angular/router';
import { Subir } from '../subir/subir';
import { ResultadoBusqueda, ParResultado } from '../importar/busqueda';
import { Notificacion } from '../usuario/usuario';

@Component({
    selector: 'importar',
    templateUrl: './importar.component.html',
    styleUrls: ['./importar.component.css']
})
export class ImportarComponent {
    private cargando: boolean = false;
    private origen: string;
    private destino: string;
    private relActual: TrioDB;
    private plantillaSel: string;
    private plantillasSel: string[] = [];
    private PlantillasRel: TrioDB[] = [];
    private camposRel: ParDB[] = [];
    private camposActual: ParDB[] = [];
    private files: Archivo[] = [];
    private tipoActualizacion: string;
    private Idcampana: string;
    private ss: Sesion = new Sesion('', '', new Date(), '', '');
    private notif: Notificacion = new Notificacion('', '');
    private clrNotif() {
        setTimeout(x => { this.notif = new Notificacion('', ''); }, 5000);
    }
    private Relaciona(evento: any) {
        if (this.camposActual.length > 0 && this.ColumnsAsignacion.length > 0) {
            this.camposRel.push(new ParDB(this.destino, this.origen));
            this.camposActual = this.camposActual.filter((c: ParDB) => c.Destino != this.destino);
            this.ColumnsAsignacion = this.ColumnsAsignacion.filter((c: ParResultado) => c.Label != this.origen);
            this.selFirst();
        }
    }
    private selFirst() {
        if (this.camposActual.length > 0)
            this.destino = this.camposActual[0].Destino;
        if (this.ColumnsAsignacion.length > 0)
            this.origen = this.ColumnsAsignacion[0].Label;
    }
    private selActualizacion(sel: string) {
        this.camposRel = [];
        this.ColumnsAsignacion = this.Columns;
        this.tipoActualizacion = sel;
        switch (sel) {
            case 'Cliente':
                this.camposActual = new CamposClientes().Campos;
                break;
            case 'Asignacion':
                this.camposActual = new CamposAsignaciones().Campos;
                break;
        }
        this.selFirst();
        this.scrollToBottom();
    }
    constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string, public servicio: Servicio, private route: ActivatedRoute, private router: Router) {
        this.ss = servicio.getLogin();
        this.selActualizacion('Cliente');
    }
    ngOnInit() {
        this.Idcampana = this.route.snapshot.paramMap.get('idcampana') || '0';
        this.CargaArchivos();
    }
    @ViewChild('au')
    au: ElementRef;
    @ViewChild('wrap')
    wrap: ElementRef;
    @ViewChild('previa')
    previa: ElementRef;

    ngAfterViewChecked() {
        //this.scrollToBottom();
    }
    scrollToTop(): void {
        try {
            this.wrap.nativeElement.scrollTop = 0;
        } catch (err) { }
    }
    scrollToBottom(): void {
        try {
            this.wrap.nativeElement.scrollTop = this.wrap.nativeElement.scrollHeight;
        } catch (err) { }
    }
    scrollToPrevia(): void {
        try {
            this.wrap.nativeElement.scrollTop = this.previa.nativeElement.scrollHeight;
        } catch (err) { }
    }
    private filesIn: any;
    private totalKB: number = 0;
    private Archivos: Subir[] = [];
    private Descripciones: any[] = [];
    private lstDescripciones: number[] = [];
    private RemueveArchivo(Archivo: Subir) {
        this.Archivos.splice(this.Archivos.findIndex(a => a.Nombre == Archivo.Nombre), 1);
        return false;
    }
    private tResults: ResultadoBusqueda[] = [];
    private ColumnsAsignacion: ParResultado[] = [];
    private Columns: ParResultado[] = [];
    private plantillaActual: string;
    private currentArchivo: Archivo;
    private showGuardar: boolean = false;
    private showCargar: boolean = false;
    private cancelaRel(r: ParDB) {
        this.camposRel = this.camposRel.filter((c: ParDB) => c != r);
        this.camposActual.push(new ParDB(r.Destino, ''));
        this.ColumnsAsignacion.push(new ParResultado(r.Origen, this.Columns.filter((c: ParResultado) => c.Label == r.Origen)[0].Value));
    }
    private CargaTabla(Tabla: Archivo) {
        this.currentArchivo = Tabla;
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/tabla?t=' + Tabla.Tabla + '&usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.tResults = data;
            if (this.tResults.length > 0) {
                this.Columns = this.tResults[0].Result;
                this.ColumnsAsignacion = this.Columns;
            }
            this.scrollToPrevia();
            this.cargando = false;
        }, error => { this.cargando = true; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Masivo', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Masivo')); });
    }
    private CargaArchivos() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/importar' + '?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.files = data;
            this.cargando = false;
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Masivo', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Masivo')); });
    }
    public AgregaArchivo(Form: any) {
        this.scrollToTop();
        for (let file of Form.target.files) {
            if (this.Archivos.filter((Archivo: Subir) => { if (Archivo.Nombre == file.name) return true; }).length == 0) {
                if (file.size > 0) {
                    let reader = new FileReader();
                    reader.onloadend = (data: any) => {
                        this.notif = new Notificacion('Cargando archivo a la nube', 'Cargar');
                        var BinayString: any = data.currentTarget.result;
                        var Base64Url: any = BinayString;
                        var MimeType: any = Base64Url.substring(Base64Url.lastIndexOf("data") + 5, Base64Url.lastIndexOf(";"));
                        var Base64Url: any = Base64Url.substring(Base64Url.lastIndexOf("base64") + 7);
                        this.Archivos.push(new Subir(0, BinayString, Base64Url, MimeType, file.name, 0, Math.round(file.size / 1024), 0, 0, 'LaRuta'));
                        this.totalKB += Math.round(file.size / 1024);
                        this.SubirArchivos('');
                    };
                    reader.readAsDataURL(file);
                }
            }
        }


    }
    private SubirArchivos(event: any) {
        if (this.Archivos.length > 0) {
            console.log(this.Archivos.length);
            this.cargando = true;
            this.http.post(this.baseUrl + 'apiv1/importar', { Archivos: this.Archivos, usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION  }).subscribe((data: any) => {
                this.servicio.validaSesion(data);
                this.CargaArchivos();
                this.clrNotif();
                this.au.nativeElement.value = "";
                this.cargando = false;
            }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Masivo', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Masivo')); });
        }

    }
    private GuardaRel(event: any) {
        if (event.key == 'Enter') {
            this.cargando = true;
            this.http.post(this.baseUrl + 'apiv1/rel', { Relaciones: new Par2Trio(this.camposRel, this.plantillaActual).getTrio(), usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION  }).subscribe((data: any) => {
                this.servicio.validaSesion(data);
                this.noPopups();
                this.cargando = false;
            }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Masivo', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Masivo')); });
        }

        if (event.key == 'Escape') {
            this.noPopups();
        }
    }
    private Actualizar(event: any) {
        this.notif = new Notificacion('Actualizando datos', 'Actualizar');
        this.cargando = true;
        this.http.post(this.baseUrl + 'apiv1/masiva', { Campos: this.camposRel, TablaOrigen: this.currentArchivo.Tabla, TipoActualizacion: this.tipoActualizacion, usr: this.servicio.getUsuario().Email, token: this.servicio.getLogin().CODSESION  }).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.notif = new Notificacion('Datos actualizados', 'Actualizar');
            this.cargando = false;
        }, (error: any) => { this.cargando = false;; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Masivo', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Masivo'));});
    }
    private noPopups() {
        this.plantillaSel = '';
        this.showCargar = false;

        this.plantillaActual = '';
        this.showGuardar = false;
    }
    private cargaPlantilla() {
        this.camposRel = this.PlantillasRel.filter(p => p.Plantilla == this.plantillaSel).map(p => { return new ParDB(p.Destino, p.Origen) });
        this.noPopups();
    }
    private CargaRel(event: any) {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/rel' + '?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.PlantillasRel = data;
            var setPlantillas: any = new Set();
            this.PlantillasRel.map(p => p.Plantilla).forEach(p => setPlantillas.add(p));
            this.plantillasSel = Array.from(setPlantillas);
            this.showCargar = true;
            this.cargando = false;
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Masivo', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Masivo')); });
    }
    private BorraRel(event: any) {
        this.cargando = true;
        if (this.Archivos.length > 0) {
            this.http.delete(this.baseUrl + 'apiv1/rel?Plantilla=' + this.relActual).subscribe((data: any) => {
                this.servicio.validaSesion(data);
                this.cargando = false;
                this.CargaArchivos();
            }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Masivo', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Masivo')); });
        }
            
    }
}
