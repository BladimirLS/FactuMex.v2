import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Comprobante} from '../comprobante';

import {catchError, retry} from 'rxjs/operators';
import {Observable, throwError} from 'rxjs';
import {environment} from '../../../environments/environment';


@Injectable({
    providedIn: 'root'
})
export class ComprobanteService {

    myAppUrl: string;
    myApiUrl: string;
    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8'
        })
    };
    constructor(private http: HttpClient) {
        this.myAppUrl = environment.appUrl;
        this.myApiUrl = 'api/Clientes/';
    }

    getBlogPosts(): Observable<Comprobante[]> {
        return this.http.get<Comprobante[]>(this.myAppUrl + this.myApiUrl)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    getComprobante(Idcomp: string): Observable<Comprobante> {
        return this.http.get<Comprobante>(this.myAppUrl + this.myApiUrl + Idcomp)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    saveBlogPost(comprobante): Observable<Comprobante> {
        return this.http.post<Comprobante>(this.myAppUrl + this.myApiUrl, JSON.stringify(comprobante), this.httpOptions)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    updateBlogPost(Idcomp: string, comprobante): Observable<Comprobante> {
        return this.http.put<Comprobante>(this.myAppUrl + this.myApiUrl + Idcomp, JSON.stringify(comprobante), this.httpOptions)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    deleteComprobante(Idcomp: string): Observable<Comprobante> {
        return this.http.delete<Comprobante>(this.myAppUrl + this.myApiUrl + Idcomp)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    errorHandler(error) {
        let errorMessage = '';
        if (error.error instanceof ErrorEvent) {
            // Get client-side error
            errorMessage = error.error.message;
        } else {
            // Get server-side error
            errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }
        console.log(errorMessage);
        return throwError(errorMessage);
    }
}
