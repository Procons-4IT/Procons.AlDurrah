import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { SearchResultsComponent } from '../SearchResults/SearchResults.component';
import { SearchCriteriaParams} from '../../Models/ApiRequestType';

import { Observable } from 'rxjs';


@Component({
    selector: 'search-form',
    templateUrl: './searchForm.component.html',
    styleUrls: ['./searchForm.component.css']
})
export class searchFormComponent implements OnInit {

    @Output() onSearchFilterCriteria = new EventEmitter<object>();
    @Input() searchCriteriaParams: SearchCriteriaParams;

    constructor() { }

    ngOnInit() {
        
    }

    GotoResults(workType, age, sex, nationality, maritalStatus, language) {
        let argumentKeys = ["workType", "age", "gender", "nationality", "maritalStatus", "language"];
        let workerFilterParams = {};
        for (var i = 0; i < arguments.length; i++) {
            let argument = arguments[i];
            if (argument.type == 'select-one') {
                let isFirstElement = argument.selectedIndex === 0;
                if (!isFirstElement) {
                    let keyName: string = argumentKeys[i];
                    workerFilterParams[keyName] = argument.value;
                }
            }
        }
        
        this.onSearchFilterCriteria.emit(workerFilterParams);
    }
}
// <!-- <div *ngFor "let nameValuePair of searchCriteriaParams.workerTypes;">
// {{nameValuePair.name }}
// </div> -->
