import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private router: Router) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // Récupérer le jeton JWT depuis le local storage ou tout autre emplacement où vous l'avez stocké
        const token = localStorage.getItem('authToken');

        // Ajouter l'en-tête d'autorisation Bearer à la requête si le jeton existe
        if (token) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
            });
        }

        // Passer la requête modifiée à la suite de l'intercepteur
        return next.handle(request).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status === 401) {
                    // Supprimer le token invalide/expiré
                    localStorage.removeItem('authToken');
                    this.router.navigate(['/login']);
                }

                // Renvoyer l'erreur pour que d'autres intercepteurs puissent également la gérer
                return throwError(() => error);
            })
        );
    }
}