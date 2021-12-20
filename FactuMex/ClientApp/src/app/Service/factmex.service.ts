import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Ente} from '../lstcliente/lstcliente';

import {catchError, retry} from 'rxjs/operators';
import {Observable, throwError} from 'rxjs';
import {environment} from '../../environments/environment';



@Injectable({
    providedIn: 'root'
})
export class FactmexService {

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

    getBlogPosts(): Observable<Ente[]> {
        return this.http.get<Ente[]>(this.myAppUrl + this.myApiUrl)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    getBlogPost(Idente: string): Observable<Ente> {
        return this.http.get<Ente>(this.myAppUrl + this.myApiUrl + Idente)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    saveBlogPost(blogPost): Observable<Ente> {
        return this.http.post<Ente>(this.myAppUrl + this.myApiUrl, JSON.stringify(blogPost), this.httpOptions)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    updateBlogPost(postId: number, blogPost): Observable<Ente> {
        return this.http.put<Ente>(this.myAppUrl + this.myApiUrl + postId, JSON.stringify(blogPost), this.httpOptions)
            .pipe(
                retry(1),
                catchError(this.errorHandler)
            );
    }

    deleteBlogPost(Idente: string): Observable<Ente> {
        return this.http.delete<Ente>(this.myAppUrl + this.myApiUrl + Idente)
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
