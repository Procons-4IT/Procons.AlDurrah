import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRoute } from '@angular/router';
import { CarService } from './CarService';

@Injectable()
export class CanActivateViaAuthGuard implements CanActivate {

    constructor(private authService: CarService, private route: ActivatedRoute, private router: Router) { }

    canActivate() {
         

        if (this.authService.isLogedIn()) {
            return true;
        }
        else {
            this.router.navigate(['/login'], { queryParams: { returnUrl: this.route.snapshot.url } });
            return false;
        }
    }
}