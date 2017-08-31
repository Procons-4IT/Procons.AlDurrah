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
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { MenuItem } from 'primeng/primeng';
import { searchFormComponent } from './SearchForm/SearchForm.component';
import { Worker } from '../Models/Worker'
import { SearchContent } from '../Models/SearchContent'
import {SearchCriteriaParams} from '../Models/ApiRequestType'
import { WorkersService } from '../Services/WorkersService'
import { DataGrid } from 'primeng/primeng';
import { ApiService } from '../Services/ApiService';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
    selector: 'app-catalogue',
    templateUrl: './catalogue.component.html',
    styleUrls: ['./catalogue.component.css'],
    providers: [WorkersService],
})
export class CatalogueComponent implements OnInit {

    public workers: Worker[];
    public selectedWorker: Worker;
    public searchCriteriaParams: SearchCriteriaParams;
    public showSearchSummary: boolean = true;
    public showSearchForm: boolean = false;
    public showSearchResultTable: boolean = false;
    public showProfile: boolean = false;

    constructor(private myApi: ApiService, private componentFactoryResolver: ComponentFactoryResolver,
        private workersService: WorkersService, private sanitizer: DomSanitizer) {
            console.log('hello other world!')
         }


    public GoToSearch() {
        this.myApi.getSearchCriteriaParameters().subscribe(searchCriteria=>{

            this.searchCriteriaParams = searchCriteria;
            this.showSearchSummary = false;
            this.showSearchForm = true;
        })
    }

    GoToResults(workerFilter: Worker) {
        console.log('Search-Filter ', workerFilter);
        this.myApi.getAllWorkers(workerFilter).subscribe(workers => {
            this.workers = workers;
            this.showSearchForm = false;
            this.showSearchResultTable = true;
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
        this.myApi.knetPaymentRedirect(paymentInformation).subscribe();
    }
}
