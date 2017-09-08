import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router'
import { Observable, ObservableInput } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/do';


import { ApiService } from '../Services/ApiService';
import { UtilityService } from '../Services/UtilityService';

declare var $;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  public paymentModalText: string = "";
  public loadingPayment: boolean = false;
  constructor(public myApi: ApiService
    , public utility: UtilityService,
    public activeRouter: ActivatedRoute) { }


  ngOnInit() {
    this.handlePaymentRoute();
    this.handleConfirmEmailRoute();
  }


  handleConfirmEmailRoute() {
    console.log('## Checking if ConfirmEmailRoute');

    this.activeRouter.data
      .filter((data, idx) => { return data.isConfirmEmail; })
      .do(x => {
        $('#modalIncomingPayment').modal('toggle');
        this.loadingPayment = true;
        this.paymentModalText = "Confirming Email Please Wait!";

      })
      .mergeMap(x => { return this.utility.getResetPasswordUrlProperties(this.activeRouter) })
      .mergeMap(params => { return this.myApi.confirmEmail(params) })
      .subscribe(x => {
        console.log('Recieved X', x);
        this.loadingPayment = false;
        if (x) {
          this.paymentModalText = 'Email Confirmed!';
        } else {
          this.paymentModalText = 'Invalid Email!';
        }
      }, onError => {
        this.loadingPayment = false;
        this.paymentModalText = 'Something Went Wrong!'
      });

  }
  handlePaymentRoute() {
    console.log('## Checking if PaymentRoute');

    this.activeRouter.data
      .filter((data, idx) => { return data.isPayment; })
      .do(x => {
        $('#modalIncomingPayment').modal('toggle');
        this.loadingPayment = true;
        this.paymentModalText = "Proccesing Payment Please Wait!";

      })
      .mergeMap(x => { return this.utility.getKnetUrlProperties(this.activeRouter) })
      .mergeMap(params => { return this.myApi.createIncomingPayment(params) })
      .subscribe(x => {
        console.log('Recieved X', x);
        this.loadingPayment = false;
        this.paymentModalText = x;
      }, onError => {
        this.loadingPayment = false;
        this.paymentModalText = 'Something Went Wrong!'
      });

  }

}
