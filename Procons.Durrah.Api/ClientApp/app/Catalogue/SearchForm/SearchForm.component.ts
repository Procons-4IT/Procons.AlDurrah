import { Component, OnInit, ViewChild, ElementRef,
        ComponentRef, ComponentFactory,
        ViewContainerRef,
        ComponentFactoryResolver,
        ChangeDetectorRef, forwardRef  } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { catalogueComponent } from '../catalogue.component';
import { SearchResultsComponent } from '../SearchResults/SearchResults.component';
@Component({
    selector: '[app-searchForm]',
    templateUrl: './searchForm.component.html',
    styleUrls: ['./searchForm.component.css']
})
export class searchFormComponent implements OnInit {
    public items: MenuItem[];
    //@ViewChild(catalogueComponent) catalogue: catalogueComponent;
    @ViewChild(forwardRef(() => catalogueComponent)) catalogue: catalogueComponent;
    //public catalogue: catalogueComponent;

    constructor(private componentFactoryResolver: ComponentFactoryResolver) { }
    public Book() {
        console.log('searchFormComponent Book!');
        
        window.location.href = "http://knet.testyourprojects.co.in/";
    }

    ngOnInit() {
    }

    GotoResults() {

    }
}
