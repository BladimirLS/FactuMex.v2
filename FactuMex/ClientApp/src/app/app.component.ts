import { Component } from '@angular/core';
import { Servicio, TipoServicio } from './Service';
import { Subscription } from 'rxjs';
import { trigger, transition, animate, style } from '@angular/animations'
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    private subscriptionService: Subscription = new Subscription();
    private isLogged: boolean = false;
    private ShowPopup: boolean = false;
    private isExpanded: boolean = true;
    private imInvisible: boolean = false;
    private vistaPrevia: boolean = false;
    title = 'RenoMax';
    constructor(private Servicio: Servicio) {
        
        this.subscriptionService = this.Servicio.ServiceObservable.subscribe((TS: TipoServicio) => {
            switch (TS.NombreServicio) {
                case 'Login':
                    this.isLogged = TS.Status;
                    break;
                case 'Popup':
                    this.ShowPopup = TS.Status;
                    break;
                case 'isExpanded':
                    this.isExpanded = TS.Status;
                    break;
                case 'invisibleMenu':
                    this.imInvisible = TS.Status;
                    break;
                case 'Vista Previa':
                    this.vistaPrevia = TS.Status;
                    break;
            }
        });
    }
    private ngAfterContentInit() {
        if (window.innerWidth < 970) {
            this.minimiza();
        }
        else {
            this.maximiza();
        }
        this.Servicio.validaLogin();
    }
    private minimiza() {
        this.Servicio.CallService(new TipoServicio('isExpanded', false, false, 'menu'))
    }
    private maximiza() {
        this.Servicio.CallService(new TipoServicio('isExpanded', true, true, 'menu'))
    }
    private onResize(event) {
        if (event.target.innerWidth < 970) {
            this.minimiza();
        }
        else {
            this.maximiza();
        }
            
    }
}
