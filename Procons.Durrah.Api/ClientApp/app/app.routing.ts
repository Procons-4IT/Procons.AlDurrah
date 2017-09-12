import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { CanActivateViaAuthGuard } from "./Services/ActivationGuard"
import { Routes, RouterModule, DefaultUrlSerializer, UrlTree, UrlSerializer } from '@angular/router'

export class LowerCaseUrlSerializer extends DefaultUrlSerializer {
  parse(url: string): UrlTree {
    return super.parse(url.toLocaleLowerCase());
  }
}

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'paymentconfirmation', data: { isPayment: true }, component: HomeComponent },
  { path: 'confirmemail', data: { isConfirmEmail: true }, component: HomeComponent },
  { path: 'resetpassword', data: { isPasswordReset: true }, component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: '', pathMatch: 'full', redirectTo: '/home' },
  { path: '**', pathMatch: 'full', redirectTo: '/home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [{ provide: UrlSerializer, useClass: LowerCaseUrlSerializer }]
})
export class RoutingModule { }

export const routingComponents = [HomeComponent];

