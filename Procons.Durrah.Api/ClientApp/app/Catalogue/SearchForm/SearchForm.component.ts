import { Component, OnInit, Output,EventEmitter} from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { catalogueComponent } from '../catalogue.component';
import { SearchResultsComponent } from '../SearchResults/SearchResults.component';

@Component({
    selector: 'search-form',
    templateUrl: './searchForm.component.html',
    styleUrls: ['./searchForm.component.css']
})
export class searchFormComponent implements OnInit {

    @Output() onSearchFilterCriteria = new EventEmitter<object>();

    constructor() { }

    ngOnInit() { }

    GotoResults(workType, age, sex, nationality, maritalStatus, language) {
        let argumentKeys = ["workType", "age", "sex", "nationality", "maritalStatus", "language"];
        let workerFilterParams = {};
        for (var i = 0; i < arguments.length; i++) {
            let argument = arguments[i];
            if (argument.type == 'select-one') {

                let isFirstElement = argument.value === argument.options[0].value;
                if (!isFirstElement) {
                    let keyName: string = argumentKeys[i];
                    workerFilterParams[keyName] = argument.value;
                }
            }
        }
        console.log('captured searchFilter ',workerFilterParams);
        this.onSearchFilterCriteria.emit(workerFilterParams);
    }
}
