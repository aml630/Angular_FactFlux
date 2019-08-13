import { CanActivate, ActivatedRouteSnapshot, Router, RouterStateSnapshot } from "@angular/router";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class AuthGuard implements CanActivate {
    base: string = document.getElementsByTagName('base')[0].href;

    constructor(private http: HttpClient) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {

        return this.http.get<boolean>(this.base + `api/Identity/IsAdmin`);
    }
}

