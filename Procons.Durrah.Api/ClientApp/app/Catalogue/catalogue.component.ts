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
    providers: [WorkersService]
})
export class catalogueComponent implements OnInit {
    public items: MenuItem[];
    public isVisible = true;
    public visiblePart = 0;
    public selectedWorker: Worker;
    public workers: Worker[];
    public countries: any[];
    public languages: any[];
    public maritalStatus: any[];
    public gender: any[];
    public workerTypes: any[];
    public searchContent = new SearchContent();
    public selectedLanguage: any;
    public selectedAge: any;
    public selectedGender: any;
    public selectedType: any;
    public selectedStatus: any;
    public selectedCountry: any;


    //@ViewChild('catalogue', { read: ViewContainerRef }) catalogue: ViewContainerRef;
    @ViewChild('catalogue') catalogue: ElementRef;
    @ViewChild('tabcatalogue') tabcatalogue: ElementRef;
    @ViewChild('tabSearchForm') tabSearchForm: ElementRef;
    @ViewChild('tabSearchResults') tabSearchResults: ElementRef;
    @ViewChild('tabprofile') tabprofile: ElementRef;



    constructor(private myApi: ApiService, private componentFactoryResolver: ComponentFactoryResolver,
        private workersService: WorkersService, private sanitizer: DomSanitizer) { }


    public GotoSearch() {

        this.selectedStatus = "";
        this.selectedLanguage = "";
        this.selectedAge = "";
        this.selectedGender = "";
        this.selectedType = "";
        this.selectedStatus = "";
        this.selectedCountry = "";
        //let compFactory: ComponentFactory<any>;
        //compFactory = this.componentFactoryResolver.resolveComponentFactory(searchFormComponent);
        //this.catalogue.createComponent(compFactory);
        this.tabcatalogue.nativeElement.classList.remove('active', 'in');
        //this.catalogue.nativeElement.querySelector('.tab-pane .active').classList.remove('active', 'in');
        this.tabSearchForm.nativeElement.classList.add('active', 'in');
    }

    GotoResults() {
        this.myApi.getAllWorkers().subscribe(workers => {
            console.log('workers Format! ', workers);
            this.workers = workers;
            this.tabSearchForm.nativeElement.classList.remove('active', 'in');
            this.tabSearchResults.nativeElement.classList.add('active', 'in');

        });
    }

    GotoProfile(event: Worker) {

        this.selectedWorker = event;
        this.tabSearchResults.nativeElement.classList.remove('active', 'in');
        this.tabprofile.nativeElement.classList.add('active', 'in');
    }

    ngOnInit() {
        // this.GetLookups();
    }

    onRowSelect(event) {
    }

    GetLookups() {
        this.workersService.GetLookups("/api/Workers/GetSearchLookups").subscribe(response => {
            this.languages = response.json().languages;
            this.countries = response.json().nationality;
            this.maritalStatus = response.json().maritalStatus;
            this.gender = response.json().gender;
            this.workerTypes = response.json().workerTypes;
        },
            (error) => {

            });
    }
    GetAvailableCSS(worker: Worker) {
        //remove to status instead of getting the whole woker
        var isAvaible = worker.status == true;
        return {
            "glyphicon": true,
            "glyphicon-ok": isAvaible
            "glyphicon-remove": !isAvaible
        }
    }
    Book(selectedWorker: Worker) {
        console.log('calling knetPayment! for worker ', selectedWorker);
        var paymentInformation = { SerialNumber: selectedWorker.serialNumber, CardCode: "C220Temp", Amount: "100", Code: selectedWorker.code }

        this.myApi.knetPaymentRedirect(paymentInformation).subscribe();
    }
}
