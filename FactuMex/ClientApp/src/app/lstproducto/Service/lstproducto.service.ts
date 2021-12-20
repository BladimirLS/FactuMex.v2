import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Observable, throwError} from 'rxjs';
import {Item} from '../lstproducto';
import {catchError, retry} from 'rxjs/operators';


@Injectable({
    providedIn: 'root'
})
// tslint:disable-next-line:class-name
export class productoService {

    myAppUrl: string;
    myApiUrl: string;
    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8'
        })
    };
    constructor(private http: HttpClient) {
        this.myAppUrl = environment.appUrl;
        this.myApiUrl = 'api/Items/';
    }

    getProductos(): Observable<[]> {
        // @ts-ignore
        return this.http.get<Item[]>(this.myAppUrl + this.myApiUrl)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    getProducto(Iditem: string): Observable<Item> {
        return this.http.get<Item>(this.myAppUrl + this.myApiUrl + Iditem)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    saveBlogPost(item): Observable<Item> {
        return this.http.post<Item>(this.myAppUrl + this.myApiUrl, JSON.stringify(item), this.httpOptions)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    updateBlogPost(Iditem: string, item): Observable<Item> {
        return this.http.put<Item>(this.myAppUrl + this.myApiUrl + Iditem, JSON.stringify(item), this.httpOptions)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    deleteProducto(Iditem: string): Observable<Item> {
        return this.http.delete<Item>(this.myAppUrl + this.myApiUrl + Iditem)
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
