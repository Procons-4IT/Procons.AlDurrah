import { Component, OnInit, ViewChild } from '@angular/core';
import { DataTable } from 'primeng/primeng';
import { ComponentBase } from '../../app.ComponentBase';
import { Http, Response, Headers, RequestOptions } from '@angular/http';


@Component({
  selector: 'app-gv-control',
  templateUrl: './gv-control.component.html',
  styleUrls: ['./gv-control.component.css']
})
export class GvControlComponent implements OnInit {

    displayDialog: boolean;
    account: any = null;
    //selectedAccount: Account;
    newAccount: boolean;
    accounts: any[] = [];

    constructor() {
    }

    ngOnInit() {
    }
}




