import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { PaymentConfirmationComponent } from './paymentConfirmation/paymentConfirmation.component';
import { CanActivateViaAuthGuard } from "./Services/ActivationGuard"
import { Routes, RouterModule } from '@angular/router'


const routes: Routes = [
  { path: 'Home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'PaymentConfirmation', component: PaymentConfirmationComponent },
  { path: 'ResetPassword', component: LoginComponent },
  { path: '', pathMatch: 'full', redirectTo: '/Home' },
  { path: '**', pathMatch: 'full', redirectTo: '/Home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class RoutingModule { }

export const routingComponents = [HomeComponent];
