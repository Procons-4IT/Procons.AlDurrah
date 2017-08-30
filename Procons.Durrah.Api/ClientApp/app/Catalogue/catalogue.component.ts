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
import { Item } from '../Models/Item'
import { SearchContent } from '../Models/SearchContent'
import { WorkersService } from '../Services/WorkersService'
import { DataGrid } from 'primeng/primeng';
import { ApiService } from '../Services/ApiService';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
    selector: '[app-catalogue]',
    templateUrl: './catalogue.component.html',
    styleUrls: ['./catalogue.component.css'],
    providers: [WorkersService],
})
export class catalogueComponent implements OnInit {

    public workers: Worker[];
    public selectedWorker: Worker;

    public showSearchSummary: boolean = true;
    public showSearchForm: boolean = false;
    public showSearchResultTable: boolean = false;
    public showProfile: boolean = false;

    constructor(private myApi: ApiService, private componentFactoryResolver: ComponentFactoryResolver,
        private workersService: WorkersService, private sanitizer: DomSanitizer) { }


    public GoToSearch() {
        this.showSearchSummary = false;
        this.showSearchForm = true;
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
    GetAvailableCSS(worker: Worker) {
        var isAvaible = worker.status == "1" ? true : false;
        return {
            "glyphicon": true,
            "glyphicon-ok": isAvaible,
            "glyphicon-remove": !isAvaible
        };
    }
    Book(onBook: Boolean) {
        var selectedWorker = this.selectedWorker;
        console.log('calling knetPayment! for worker ', selectedWorker);
        var paymentInformation = { SerialNumber: selectedWorker.serialNumber, CardCode: "C220Temp", Amount: "100", Code: selectedWorker.code }
        this.myApi.knetPaymentRedirect(paymentInformation).subscribe();
    }
}
