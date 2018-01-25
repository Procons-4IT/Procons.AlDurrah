import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { SearchResultsComponent } from '../SearchResults/SearchResults.component';
import { SearchCriteriaParams } from '../../Models/ApiRequestType';

import { Observable } from 'rxjs';


@Component({
    selector: 'search-form',
    templateUrl: './searchForm.component.html',
    styleUrls: ['./searchForm.component.css']
})
export class searchFormComponent implements OnInit {

    @Output() onSearchFilterCriteria = new EventEmitter<object>();
    @Input() searchCriteriaParams: SearchCriteriaParams;
    scp: SearchCriteriaParams;
    selectedLanguages = [];
    hobbies: string;
    location: string;
    isNew: boolean;
    period: number;
    yearsOfExperience: number;

    constructor() { }

    ngOnInit() {
        this.scp = Object.assign({}, this.searchCriteriaParams);
        this.scp.languages = this.searchCriteriaParams.languages.map(
            x => { return { label: x.name, value: x.value } }) as any;
    }

    GotoResults(workType, age, sex, nationality, maritalStatus, country) {
        let argumentKeys = ["WorkerType", "Age", "Gender", "Nationality", "MaritalStatus", "Country"];
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
        if (this.selectedLanguages.length)
            workerFilterParams["languages"] = this.selectedLanguages;

        if (this.hobbies != "")
            workerFilterParams["hobbies"] = this.hobbies;

        if (this.location != "")
            workerFilterParams["location"] = this.location;

        if (this.isNew) {
            workerFilterParams["isNew"] = 'Y';
        } else {
            workerFilterParams["isNew"] = 'N';
        }

        if (this.period != null || this.period > 0)
            workerFilterParams["period"] = this.period;

        if (this.yearsOfExperience != null || this.yearsOfExperience > 0)
            workerFilterParams["yearsOfExperience"] = this.yearsOfExperience;

        this.onSearchFilterCriteria.emit(workerFilterParams);
    }
}
// <!-- <div *ngFor "let nameValuePair of searchCriteriaParams.workerTypes;">
// {{nameValuePair.name }}
// </div> -->
