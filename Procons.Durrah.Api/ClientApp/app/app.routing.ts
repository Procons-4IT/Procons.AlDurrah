import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { CanActivateViaAuthGuard } from "./Services/ActivationGuard"
import { Routes, RouterModule } from '@angular/router'


const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/Home' },
  { path: 'Home', component: HomeComponent},
  { path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class RoutingModule { }

export const routingComponents = [HomeComponent];
