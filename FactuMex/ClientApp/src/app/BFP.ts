import { Injectable, Inject } from '@angular/core';
import { Subject, Subscriber } from 'rxjs';
import { Login, Sesion } from './login/login';
import { Popup } from './popup/popup';
import { HttpClient } from '@angular/common/http';
import { Usuario } from './usuario/usuario';
import { Router } from '@angular/router';

export class BFP {
    private appVersion: string = '';
    private product: string = '';
    private productSub: string = '';
    private cookieEnabled: string = '';
    private hardwareConcurrency: string = '';
    private languages: string[] = [];
    private languageLongString: string = '';
    private MimeTypeArray: string[] = [];
    private mimeLongString: string = '';
    private plugins: string[] = [];
    private pluginLongString: string = '';
    private metaLongString: string = '';
    private height: string = window.outerHeight.toString();
    private width: string = window.outerWidth.toString();
    constructor() {

    }
    public FPBrowser() {
        try {
            if (window.navigator) {
                if (window.navigator.appVersion)
                    this.appVersion = window.navigator.appVersion;
                if (window.navigator.product)
                    this.product = window.navigator.product;
                if (window.navigator.productSub)
                    this.productSub = window.navigator.productSub;
                if (window.navigator.cookieEnabled != null && window.navigator.cookieEnabled != undefined)
                    this.cookieEnabled = window.navigator.cookieEnabled.toString();
                if (window.navigator.hardwareConcurrency)
                    this.hardwareConcurrency = window.navigator.hardwareConcurrency.toString();
                window.navigator.languages.forEach((l: string) => { this.languages.push(l); this.languageLongString += l; });
                //if (window.navigator.mimeTypes) {
                //    var lmt: number = window.navigator.mimeTypes.length;
                //    if (lmt > 0) {
                //        for (var i = 0; i < lmt; i++) {
                            //this.MimeTypeArray.push(window.navigator.mimeTypes.item(i).type)
                            //this.mimeLongString += window.navigator.mimeTypes.item(i).type + '|';
                //        }
                //    }
                //}
                //if (window.navigator.plugins) {
                //    var pl: number = window.navigator.plugins.length;
                //    if (pl > 0) {
                //        for (var i = 0; i < pl; i++) {
                //            this.plugins.push(window.navigator.plugins.item(i).name)
                //            this.pluginLongString += window.navigator.plugins.item(i).name + '|';
                //        }
                //    }
                //}
                this.metaLongString = this.appVersion + '|' + this.product + '|' + this.productSub + '|' + this.cookieEnabled + '|' + this.hardwareConcurrency + '|' + this.languageLongString + '|' + this.mimeLongString + '|' + this.pluginLongString + '|' + this.height + '|' + this.width;
            }
        } catch (e) {
            console.log(e);
        }
        return this.metaLongString;
    }
}
