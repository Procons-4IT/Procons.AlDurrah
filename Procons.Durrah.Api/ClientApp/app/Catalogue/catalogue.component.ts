import {
    Component,
    OnInit,
    ViewChild,
    ElementRef,
    AfterViewInit,
    ComponentRef,
    ComponentFactory,
    ViewContainerRef,
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
import { UtilityService } from '../Services/UtilityService';

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

    constructor(private myApi: ApiService, private utility: UtilityService,
        private sanitizer: DomSanitizer, private myModal: ProconsModalSerivce) {
    }


    public GoToSearch() {
        var securityToken = this.myApi.GetSecurityToken();
        if (securityToken) {

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
                    
                });
        } else {
            this.myModal.showErrorModal('error.notLoggedIn');
        }
    }

    GoToResults(workerFilter: Worker) { 
        this.loading = true;
        this.myApi.getAllWorkers(workerFilter).subscribe(workers => {
             
            this.loading = false
            this.workers = workers;
            workers.forEach(element => {
                element.yearsOfExperience=0;
                element.experiences.forEach(e=>{
                    if(e.startDate!="")
                {  
                    let date1 = new Date(e.startDate).getTime();
                    let date2 = new Date(e.endDate).getTime();
                    let time = date2 - date1;  //msec
                    let hours = Math.abs((time) / 3600000);
                    let days= Math.abs((hours) / 24);
                    let numberofYears=Math.abs((days) / 365).toFixed(2);
                   // let hoursDiff = time / (3600 * 1000);
                    element.yearsOfExperience+= Number(numberofYears); //e.startDate - e.endDate;
                }
                //workers.push("yearsOfExperience",)
                });
                
                if (this.searchCriteriaParams.languages && Array.isArray(this.searchCriteriaParams.languages)) {
                   //filling the language name
                   // element.languages = 
                    element.languages.forEach( ele=> { 
                        this.searchCriteriaParams.languages.forEach(nameValuePair=>{
                            if(ele.value==nameValuePair.value)
                                ele.name=nameValuePair.name;
                                
                        });
                     }) as any;
                }
            });
            this.showSearchForm = false;
            this.showSearchResultTable = true;
        }
            , onError => {
                
                this.loading = false;
                this.myModal.showErrorModal();
            });
    }



    GoToProfile(event: Worker) {
  
        this.selectedWorker = event;
        this.showSearchResultTable = false;
        this.showProfile = true;
    }

    ngOnInit() {
        // this.GetLookups(); 
    }

    GetLookups() {
        this.myApi.getSearchCriteriaParameters().subscribe(searchCriteriaParams => {
            
        });
    }
    //TO-DO: REMOVE AMOUNT HERE!
    Book(onBook: Boolean) {
        var selectedWorker = this.selectedWorker;
        
        var paymentInformation = { SerialNumber: selectedWorker.serialNumber, CardCode: selectedWorker.agent, Amount: "100", Code: selectedWorker.code, WorkerCode: selectedWorker.workerCode }
    
        this.loading = true;
        this.myApi.knetPaymentRedirectUrl(paymentInformation)
            .map(url => this.utility.redirectToUrl(url))
            .subscribe(onSuccess => { }, onError => {
                this.loading = false;
                this.myModal.showErrorModal();
            });
    }
    ShowSearchResult() {
        this.showProfile = false;
        this.showSearchForm = false;
        this.showSearchSummary = false;
        this.showSearchResultTable = true;
    }
    ShowSearchFilter() {
        this.showProfile = false;
        this.showSearchForm = true;
        this.showSearchSummary = false;
        this.showSearchResultTable = false;

    }
}
