import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router'
import { Observable, ObservableInput } from 'rxjs/Observable';
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
  constructor(public myApi: ApiService, public utility: UtilityService, public activeRouter: ActivatedRoute) { }


  ngOnInit() {
    this.handlePaymentRoute();
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
