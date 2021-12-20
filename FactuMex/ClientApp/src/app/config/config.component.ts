import { Component, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Sesion } from '../login/login';
import { Router, ActivatedRoute } from '@angular/router';
import { Subir } from '../subir/subir';
import { Archivo } from '../importar/importar';
import { Notificacion } from '../usuario/usuario';
import { Anexo, AnConf } from './config';
import { Popup } from '../popup/popup';


@Component({
    selector: 'config',
    templateUrl: './config.component.html',
    styleUrls: ['./config.component.css']
})
export class ConfigComponent {
    @ViewChild('au')
    au: ElementRef;
    private filesIn: any;
    private totalKB: number = 0;
    private Archivos: Anexo[] = [];
    private currentArchivo: Archivo;
    private buscar: string = '';
    private cargando: boolean = false;
    private repPwd: string = '';
    private foc: number = 0;
    private notif: Notificacion = new Notificacion('', '');
    private showPermisos: boolean;
    private lstGrupo: string[] = [];
    private numbers: number[] = [1, 2, 3, 4, 5, 6];
    private ano: number;
    constructor(
        public http: HttpClient,
        @Inject('BASE_URL') public baseUrl: string,
        public servicio: Servicio,
        private route: ActivatedRoute,
        private router: Router) {
        this.CargaConf();
        this.CargaAno();
        this.servicio.CallService(new TipoServicio('Menu', true, 'Conf', 'Configuracion'));
    }
    private CargaAno() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/config?ano=si' + '&usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.ano = data[0].VALOR;
            this.cargando = false;
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Configuración', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Configuración')); });
    }
    private CargaConf() {
        this.cargando = true;
        this.http.get(this.baseUrl + 'apiv1/config' + '?usr=' + this.servicio.getUsuario().Email + '&token=' + this.servicio.getLogin().CODSESION).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            var Anexos: AnConf[] = <AnConf[]>data;
            Anexos.forEach(a => this.Archivos.push(new Anexo(new Subir(0, 'empty', 'empty', 'pdf', a.NOMBRE, 0, 0, 0, 0, 'LaRuta'), a.INDICE)));
        }, error => { console.log(error); this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Configuración', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Configuración'));});
    }
    private existsIndex(i: number) {
        return (this.Archivos.filter((a: Anexo) => a.Index == i).length > 0)
    }
    private getName(i: number) {
        var a: Anexo[] = this.Archivos.filter((a: Anexo) => a.Index == i);
        if (a.length == 1) {
            return a[0].Arc.Nombre;
        }
    }
    private getLabel(i: number) {
        switch (i) {
            case 1:
                return 'ACTO DE DESCARGO';
                break;
            case 2:
                return 'CONOZCA SU CLIENTE';
                break;
            case 3:
                return 'RECOMENDACIONES LEY';
                break;
            case 4:
                return 'RECOMENDACIONES DAÑOS PROPIOS';
                break;
            case 5:
                return 'TALLERES';
                break;
            case 6:
                return 'DISPONIBLE';
                break;
            case 7:
                return 'DISPONIBLE';
                break;
            case 8:
                return 'DISPONIBLE';
                break;
            case 9:
                return 'DISPONIBLE';
                break;
        }
    }
    private AgregaArchivo(Form: any, arcIndex: number) {
        for (let file of Form.target.files) {
            if (this.Archivos.filter((Archivo: Anexo) => Archivo.Arc.Nombre == file.name).length == 0) {
                if (file.size > 0) {
                    let reader = new FileReader();
                    reader.onloadend = (data: any) => {
                        var BinayString: any = data.currentTarget.result;
                        var Base64Url: any = BinayString;
                        var MimeType: any = Base64Url.substring(Base64Url.lastIndexOf("data") + 5, Base64Url.lastIndexOf(";"));
                        var Base64Url: any = Base64Url.substring(Base64Url.lastIndexOf("base64") + 7);
                        this.Archivos.filter((a: Anexo) => a.Index == arcIndex).forEach((a: Anexo) => this.Archivos.splice(this.Archivos.indexOf(a), 1));
                        this.Archivos.push(new Anexo(new Subir(0, BinayString, Base64Url, MimeType, file.name, 0, Math.round(file.size / 1024), 0, 0, 'LaRuta'), arcIndex));
                        this.totalKB += Math.round(file.size / 1024);
                        console.log(this.Archivos);
                    };
                    reader.readAsDataURL(file);
                }
            }
        }
    }

    private clrNotif() {
        setTimeout(x => { this.notif = new Notificacion('', ''); }, 2000);
    }

    private GuardaConfig() {
        this.cargando = true;
        this.http.post(this.baseUrl + 'apiv1/config', { ano: this.ano, anexos: this.Archivos,usr:this.servicio.getUsuario().Email,token:this.servicio.getLogin().CODSESION }).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.notif = new Notificacion('Los anexos fueron guardados exitosamente.', 'Guardar');
            this.clrNotif();
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Configuración', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Configuración'));});

    }
    private DeleteConfig(i: number) {
        this.cargando = true;
        this.http.delete(this.baseUrl + 'apiv1/config?i=' + i).subscribe((data: any) => {
            this.servicio.validaSesion(data);
            this.cargando = false;
            this.notif = new Notificacion('El anexo fue eliminado exitosamente.', 'Eliminar');
            this.clrNotif();
            this.Archivos = [];
            this.CargaConf();
        }, error => { this.cargando = false; this.servicio.CallService(new TipoServicio('Popup', true, new Popup('', 'Configuración', 'Se produjo el siguiente error ' + error.message + '.', 'ok|timer[10000]'), 'Configuración'));});

    }
    private GetFileConfig(i: any) {
        return false;
    }
}
