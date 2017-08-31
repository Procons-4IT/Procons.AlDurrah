import {
    Component,
    OnInit,
    ViewChild,
    ElementRef,
    AfterViewInit,
    ComponentRef,
    ComponentFactory,
    ViewContainerRef,
    ComponentFactoryResolver,
    ChangeDetectorRef
} from '@angular/core';

import { DomSanitizer } from '@angular/platform-browser';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { MenuItem } from 'primeng/primeng';
import { DataGrid } from 'primeng/primeng';

import { searchFormComponent } from './SearchForm/SearchForm.component';
import { Worker } from '../Models/Worker'
import { SearchContent } from '../Models/SearchContent'
import { SearchCriteriaParams } from '../Models/ApiRequestType'

import { ApiService } from '../Services/ApiService';
import { ProconsModalSerivce } from '../Services/ProconsModalService';

@Component({
    selector: 'app-catalogue',
    templateUrl: './catalogue.component.html',
    styleUrls: ['./catalogue.component.css'],
    providers: [ProconsModalSerivce],
})
export class CatalogueComponent implements OnInit {
    public loading: boolean = false;
    public workers: Worker[];
    public selectedWorker: Worker;
    public searchCriteriaParams: SearchCriteriaParams;
    public showSearchSummary: boolean = true;
    public showSearchForm: boolean = false;
    public showSearchResultTable: boolean = false;
    public showProfile: boolean = false;

    constructor(private myApi: ApiService, private componentFactoryResolver: ComponentFactoryResolver,
        private sanitizer: DomSanitizer, private myModal: ProconsModalSerivce) {
    }


    public GoToSearch() {
        this.loading = true;
        this.myApi.getSearchCriteriaParameters().subscribe(searchCriteria => {
            this.loading = false;

            this.searchCriteriaParams = searchCriteria;
            this.showSearchSummary = false;
            this.showSearchForm = true;
        },
            onError => {
                this.loading = false;
                console.error('Error: ', onError);
                this.myModal.showErrorModal();
            }, () => {
                this.loading = false;
                console.log('subscription complete!');
            });
    }

    GoToResults(workerFilter: Worker) {
        console.log('Search-Filter ', workerFilter);
        this.loading = true;
        this.myApi.getAllWorkers(workerFilter).subscribe(workers => {
            this.loading = false
            this.workers = workers;
            this.showSearchForm = false;
            this.showSearchResultTable = true;
        }
            , onError => {
                console.log('error ', onError);
                this.loading = false;
                this.myModal.showErrorModal();
            });
    }



    GoToProfile(event: Worker) {
        console.log('selected Worker: ', event);
        this.selectedWorker = event;
        this.showSearchResultTable = false;
        this.showProfile = true;
    }

    ngOnInit() {
        // this.GetLookups();
    }

    GetLookups() {
        this.myApi.getSearchCriteriaParameters().subscribe(searchCriteriaParams => {
            console.log('look up values ', searchCriteriaParams);
        });
    }
    Book(onBook: Boolean) {
        var selectedWorker = this.selectedWorker;
        console.log('calling knetPayment! for worker ', selectedWorker);
        var paymentInformation = { SerialNumber: selectedWorker.serialNumber, CardCode: "C220Temp", Amount: "100", Code: selectedWorker.code }
        this.loading = true;
        this.myApi.knetPaymentRedirectUrl(paymentInformation)
        .map(url=> this.myApi.redirectToUrl(url))
        .subscribe(onSuccess=>{},onError=>{
            this.loading = false;
            this.myModal.showErrorModal();
        });
    }
}
