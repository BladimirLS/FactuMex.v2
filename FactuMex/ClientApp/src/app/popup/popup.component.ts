import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Servicio, TipoServicio } from '../Service';
import { Popup } from './popup';
import { Subscription, timer } from 'rxjs';
@Component({
    selector: 'popup',
    templateUrl: './popup.component.html',
    styleUrls: ['./popup.component.css']
})
export class PopupComponent {
    initializePopup(): any {
        this.showNo = false;
        this.showSi = false;
        this.showOk = false;
        this.popupInfo = new Popup('', '', '', '');
        this.timerCntr = 30;
        this.timer = null;
    }
    private showSi: boolean = false;
    private showNo: boolean = false;
    private showOk: boolean = false;
    private popupInfo: Popup = new Popup('', '', '', '');
    private subscriptionService: Subscription = new Subscription();
    private to: any;
    private timerCntr: number = 30;
    private timer: any = null;
    private hdr: string = '';
    constructor(@Inject('BASE_URL') public baseUrl: string, public servicio: Servicio) {
        this.subscriptionService = this.servicio.ServiceObservable.subscribe((TS: TipoServicio) => {
            switch (TS.NombreServicio) {
                case 'Popup':
                    this.hdr = TS.Caller;
                    if (TS.Status == true) {
                        this.initializePopup();
                        this.popupInfo = TS.Params;
                        var btns: string = this.popupInfo.Btns;
                        if (btns.includes('si'))
                            this.showSi = true;
                        if (btns.includes('no'))
                            this.showNo = true;
                        if (btns.includes('ok'))
                            this.showOk = true;
                        if (btns.includes('timer')) {
                            var timeOut: number = parseInt(this.popupInfo.Btns.substring(btns.indexOf('timer[') + 6, btns.indexOf(']')).trim());
                            this.timerCntr = timeOut / 1000;
                            if (!this.timer) {
                                this.timer = setInterval(x => {
                                    if (this.timerCntr > 0) {
                                        this.timerCntr -= 1;
                                    }
                                }, 1000);
                            }
                            this.to = setTimeout(x => {
                                clearInterval(this.timer);
                                this.timer = null;
                                this.Accion('No');
                            }, timeOut);
                        }
                    }
                    break;
            }
        });
    }
    Accion(Arg: string) {
        clearInterval(this.timer);
        this.timer = null;
        this.popupInfo.Action = Arg;
        this.servicio.CallService(new TipoServicio('Popup', false, this.popupInfo,this.popupInfo.SummonedBy));
    }
}
