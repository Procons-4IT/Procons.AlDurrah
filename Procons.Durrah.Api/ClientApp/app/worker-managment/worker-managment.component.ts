//Consider Changing the Navigation Logic to a router-outlet with Child Components and a Feature Module (issue: Fix the NavBar Navigations)
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/mergeMap';

import { Worker, WorkerManagementData } from "../Models/Worker";
import { Item } from "../Models/Item";
import { WorkerTypeParam, SearchCriteriaParams } from '../Models/ApiRequestType';
import { ApiService } from "../Services/ApiService";
import { ProconsModalSerivce } from "../Services/ProconsModalService";


@Component({
    selector: "worker-managment",
    templateUrl: "./worker-managment.component.html",
    styles: ["./worker-managment.component.css"]
})
export class WorkerMangmentComponent implements OnInit {
    public filteredItems: Item[];
    ngOnInit(): void {

        let $workerDisplayData = Observable.of('').do(x => { this.loading = true })
            .mergeMap(x => this.myApi.getWorkerManagmentData())
            .do(workerMangmentServerData => { this.bindServerState(workerMangmentServerData) })
            .map(workerMangmentServerData => { return this.state.workers }) //use the same object as parent component
            .do(data => { this.loading = false });

        this.state.$workers = $workerDisplayData.map(workerDisplayData => { return [workerDisplayData, this.state.workersServerData] })

        this.ShowWorkerAgents();
    }

    bindServerState(allTheData) {
        console.log('i got a bunch of workers ', JSON.stringify(allTheData));
        this.state.workers = allTheData.workerDisplayData;
        this.state.workersServerData = allTheData.workerServerData;
        this.state.searchCriteriaParams = allTheData.searchCriteria;
        this.state.selectedWorker = this.state.workersServerData[0];
    }

    loading = true;
    state = {
        showWorkerAgents: false,
        showProfiles: false,
        showAddProfile: false,
        searchCriteriaParams: null,
        workers: null,
        $workers: null,
        workersServerData: null,
        selectedWorker: null
    }

    constructor(public myApi: ApiService, public myModal: ProconsModalSerivce) {
    }

    ShowProfiles() {
        this.state.showWorkerAgents = false;
        this.state.showAddProfile = false;
        this.state.showProfiles = true;
    }
    ShowWorkerAgents() {
        this.state.showAddProfile = false;
        this.state.showProfiles = false;
        this.state.showWorkerAgents = true;
    }


    ShowAddWorker() {
        this.state.showWorkerAgents = false;
        this.state.showProfiles = false;
        this.state.selectedWorker = null;
        this.state.showAddProfile = true;
    }
    ShowEditWorker(worker, i) {
        //Temp Fix
        this.ShowAddWorker();
        this.state.selectedWorker = this.state.workersServerData[i];
    }
    performAction(navigateAction: NavigateAction) {
        console.log(navigateAction);
        switch (navigateAction.action) {
            case WorkerManagmentTransition.AddProfile:
                this.ShowAddWorker();
                break;
            case WorkerManagmentTransition.EditProfile:
                this.ShowEditWorker(navigateAction.value[0], navigateAction.value[1]);
                break;
            case WorkerManagmentTransition.DeleteWorker:
                this.Delete(navigateAction.value as Worker);
                break;
            case WorkerManagmentTransition.ViewProfiles:
                this.ShowProfiles();
                break;

            default:
                break;
        }

    }

    Delete(worker: Worker, index: number = 0) {
        //fix delete!
        this.state.workers.splice(index, 1);
        this.loading = true;
        this.myApi.deleteWorker(worker.code).subscribe(
            onSucces => {
                this.loading = false;
                this.myModal.showSuccessModal("Worker Deleted!");
            }, onError => {
                this.loading = false;
                this.myModal.showErrorModal("Failed to Delete Worker");
            });
    }

    //Testing Functions for quicker developemnt
    GetFakeServerData() {
        let FakeServerData = JSON.parse(`{"searchCriteria":{"languages":[{"name":"English","value":"1"},{"name":"Arabic","value":"2"},{"name":"Hindi","value":"3"}],"age":[{"name":"21-25","value":"21-25"},{"name":"25-35","value":"25-35"},{"name":"35-45","value":"35-45"},{"name":"45-55","value":"45-55"}],"education":[{"name":"University","value":"1"},{"name":"High School","value":"2"},{"name":"None","value":"3"},{"name":"Technical","value":"4"}],"religion":[{"name":"Christian","value":"1"},{"name":"Muslim","value":"2"},{"name":"Hindu","value":"3"}],"nationality":[{"name":"India","value":"1"},{"name":"Bengladish","value":"2"},{"name":"Philipin","value":"3"},{"name":"Ethioipia","value":"4"},{"name":"Srilanka","value":"5"},{"name":"Egypt","value":"6"}],"gender":[{"name":"Male","value":"M"},{"name":"Female","value":"F"}],"maritalStatus":[{"name":"Single","value":"1"},{"name":"Married","value":"2"},{"name":"Divorced","value":"3"}],"workerTypes":[{"name":"Driver","value":"Driver"},{"name":"Cook","value":"Cook"},{"name":"Maid","value":"Maid"},{"name":"Worker","value":"Worker"}]},"workerServerData":[{"workerCode":"50090112","workerName":"Test123","serialNumber":"123456789106","agent":"SA004","mobile":"215554685","age":27,"name":"Maid - SriLanka","code":"50090112","birthDate":"5/6/1991","gender":"M","nationality":"3","religion":"2","maritalStatus":"1","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=test.b484a8ad-4017-44e0-8025-fe27fe12a30f.txt","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=test.a20ad60f-1f1f-4d7b-a1dd-8449a6a3ffb8.txt","price":550,"salary":100,"weight":"60","height":"190","education":"3","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=test.a3be57ef-0c33-44a4-8f37-98ec8283a257.txt","video":"https://www.youtube.com/watch?v=H7DGigU3V2I","passportNumber":"50090112","passportIssDate":"5/6/1991 12:00:00 AM","passportExpDate":"5/6/1991","passportPoIssue":"Germany","civilId":"123456789106","status":"1","workerType":"Maid","languages":[{"name":"الانكليزية","value":"1"}],"hobbies":"","location":"","isNew":"N","period":0,"experiences":{"workerID":"50090112","startDate":"5/6/2001 12:00:00 AM","endDate":"5/6/2002 12:00:00 AM","title":"Business Development","description":"Job Description","companyName":"Procons"}},{"workerCode":"1231231","workerName":"RoRO","serialNumber":"12312","agent":"SA004","mobile":"18","age":118,"name":"Domestic worker","code":"DW00001","birthDate":"8/1/1900","gender":"M","nationality":"1","religion":"1","maritalStatus":"1","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=ben.fe6ba62a-1b97-4b83-8507-f1f86723f0b4.jpg","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=ben.6cab0923-0a97-48a6-98c3-75b0f05e0254.jpg","price":1212,"salary":21212,"weight":"12313","height":"123123","education":"1","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=ben.929bf777-9990-459c-b885-e6ad304b1be0.jpg","video":"google.com","passportNumber":"1231231","passportIssDate":"10/1/2018 12:00:00 AM","passportExpDate":"3/1/2018","passportPoIssue":"gogo","civilId":"12312","status":"1","workerType":"Worker","languages":[{"name":"الانكليزية","value":"1"},{"name":"العربية","value":"2"}],"hobbies":"213123","location":"12313","isNew":"Y","period":0,"experiences":{"workerID":"1231231","startDate":"","endDate":"","title":"Good","description":"Better","companyName":"Best"}},{"workerCode":"1238879","workerName":"Ben","serialNumber":"1238879","agent":"SA004","mobile":"200","age":38,"name":"Domestic worker","code":"DW00001","birthDate":"11/1/1980","gender":"M","nationality":"2","religion":"2","maritalStatus":"1","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=","price":500,"salary":10,"weight":"3000","height":"0.5","education":"2","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=","video":"google.com","passportNumber":"1238879","passportIssDate":"11/1/2018 12:00:00 AM","passportExpDate":"11/1/2018","passportPoIssue":"GG","civilId":"1238879","status":"1","workerType":"Worker","languages":[{"name":"الانكليزية","value":"1"},{"name":"هندي","value":"3"}],"hobbies":"","location":"","isNew":"Y","period":0,"experiences":{"workerID":"222444555","startDate":"5/6/2001 12:00:00 AM","endDate":"5/6/2002 12:00:00 AM","title":"Accountant","description":"Job Description","companyName":"Procons"}},{"workerCode":"123123123123","workerName":"Houssam The Great","serialNumber":"555333444","agent":"SA004","mobile":"200","age":118,"name":"Domestic worker","code":"DW00001","birthDate":"2/1/1900","gender":"M","nationality":"2","religion":"2","maritalStatus":"1","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion_guitar.c6dea341-8748-45e8-bedf-0a3b826fbbe9.png","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minons.ffebf301-c5c5-4378-94e4-20e8f50c2da3.jpeg","price":500,"salary":10,"weight":"3000","height":"0.5","education":"2","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion2.8cff8618-f495-42b6-bfc6-a362496b5d56.jpeg","video":"google.com","passportNumber":"123123123123","passportIssDate":"2/1/2018 12:00:00 AM","passportExpDate":"2/1/2018","passportPoIssue":"GG","civilId":"555333444","status":"1","workerType":"Worker","languages":[{"name":"الانكليزية","value":"1"},{"name":"هندي","value":"3"}],"hobbies":"","location":"","isNew":"Y","period":0,"experiences":{"workerID":"123123123123","startDate":"5/6/2001 12:00:00 AM","endDate":"5/6/2002 12:00:00 AM","title":"Accountant","description":"Job Description","companyName":"Procons"}},{"workerCode":"222444555","workerName":"Houssam The Great updated","serialNumber":"555333444","agent":"SA004","mobile":"20000000","age":118,"name":"Domestic worker","code":"DW00001","birthDate":"12/1/1900","gender":"M","nationality":"2","religion":"2","maritalStatus":"1","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion_guitar.ac90ab4e-d6c3-485b-b3bd-c7ee7e00a8f7.png","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minons.90b4fbfe-a0e8-4651-a243-4c290cf36ad0.jpeg","price":500,"salary":10,"weight":"3000","height":"0.5","education":"2","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion2.57d19f3a-1fcb-4f8d-b508-50156b04a9d5.jpeg","video":"google.com","passportNumber":"222444555","passportIssDate":"12/1/1900 12:00:00 AM","passportExpDate":"12/1/1900","passportPoIssue":"GG","civilId":"555333444","status":"1","workerType":"Worker","languages":[{"name":"الانكليزية","value":"1"},{"name":"هندي","value":"3"}],"hobbies":"Dancing","location":"LEBANON","isNew":"Y","period":0,"experiences":[{"workerID":"222444555","startDate":"","endDate":"","title":"Accountant","description":"Job Description","companyName":"Procons"}]},{"workerCode":"500901123","workerName":"Test123","serialNumber":"123456789106","agent":"SA004","mobile":"12345555","age":27,"name":"Maid - Ethiopia","code":"DW00002","birthDate":"5/6/1991","gender":"M","nationality":"3","religion":"2","maritalStatus":"1","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=lenovo-yoga-book-feature-notetaking-windows-full-width.fd7e6317-adeb-4746-98d6-28927bd5b478.jpg","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion_guitar.1622c28e-6a76-4726-9a7a-789c948b0aef.png","price":550,"salary":100,"weight":"60","height":"190","education":"3","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion2.8a3f5737-47dc-4277-bde8-69d98df33a11.jpeg","video":"https://www.youtube.com/watch?v=H7DGigU3V2I","passportNumber":"500901123","passportIssDate":"5/6/1991 12:00:00 AM","passportExpDate":"5/6/1991","passportPoIssue":"Germany","civilId":"123456789106","status":"1","workerType":"Maid","languages":[{"name":"الانكليزية","value":"1"}],"hobbies":"dd","location":"asdad","isNew":"Y","period":0,"experiences":[{"workerID":"500901123","startDate":"","endDate":"","title":"Maid","description":"Job Description","companyName":"Procons"}]}],"workerDisplayData":[{"workerCode":"50090112","workerName":"Test123","serialNumber":"123456789106","agent":"SA004","mobile":"215554685","age":27,"name":"Maid - SriLanka","code":"50090112","birthDate":"5/6/1991","gender":"Male","nationality":"Philipin","religion":"Muslim","maritalStatus":"Single","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=test.b484a8ad-4017-44e0-8025-fe27fe12a30f.txt","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=test.a20ad60f-1f1f-4d7b-a1dd-8449a6a3ffb8.txt","price":550,"salary":100,"weight":"60","height":"190","education":"None","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=test.a3be57ef-0c33-44a4-8f37-98ec8283a257.txt","video":"https://www.youtube.com/watch?v=H7DGigU3V2I","passportNumber":"50090112","passportIssDate":"5/6/1991 12:00:00 AM","passportExpDate":"5/6/1991","passportPoIssue":"Germany","civilId":"123456789106","status":"متوفر","workerType":"Maid","languages":[{"name":"الانكليزية","value":"1"}],"hobbies":"","location":"","isNew":"N","period":0,"experiences":{"workerID":"50090112","startDate":"5/6/2001 12:00:00 AM","endDate":"5/6/2002 12:00:00 AM","title":"Developer","description":"Job Description","companyName":"Procons 4 IT"}},{"workerCode":"1231231","workerName":"RoRO","serialNumber":"12312","agent":"SA004","mobile":"18","age":118,"name":"Domestic worker","code":"DW00001","birthDate":"8/1/1900","gender":"Male","nationality":"India","religion":"Christian","maritalStatus":"Single","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=ben.fe6ba62a-1b97-4b83-8507-f1f86723f0b4.jpg","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=ben.6cab0923-0a97-48a6-98c3-75b0f05e0254.jpg","price":1212,"salary":21212,"weight":"12313","height":"123123","education":"University","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=ben.929bf777-9990-459c-b885-e6ad304b1be0.jpg","video":"google.com","passportNumber":"1231231","passportIssDate":"10/1/2018 12:00:00 AM","passportExpDate":"3/1/2018","passportPoIssue":"gogo","civilId":"12312","status":"متوفر","workerType":"Worker","languages":[{"name":"الانكليزية","value":"1"},{"name":"العربية","value":"2"}],"hobbies":"213123","location":"12313","isNew":"Y","period":0,"experiences":{"workerID":"1231231","startDate":"","endDate":"","title":"Bad","description":"Worse","companyName":"Worst"}},{"workerCode":"1238879","workerName":"Ben","serialNumber":"1238879","agent":"SA004","mobile":"200","age":38,"name":"Domestic worker","code":"DW00001","birthDate":"11/1/1980","gender":"Male","nationality":"Bengladish","religion":"Muslim","maritalStatus":"Single","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=","price":500,"salary":10,"weight":"3000","height":"0.5","education":"None","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=","video":"google.com","passportNumber":"1238879","passportIssDate":"11/1/2018 12:00:00 AM","passportExpDate":"11/1/2018","passportPoIssue":"GG","civilId":"1238879","status":"متوفر","workerType":"Worker","languages":[{"name":"الانكليزية","value":"1"},{"name":"هندي","value":"3"}],"hobbies":"","location":"","isNew":"Y","period":0,"experiences":{"workerID":"222444555","startDate":"5/6/2001 12:00:00 AM","endDate":"5/6/2002 12:00:00 AM","title":"Developer","description":"Job Description","companyName":"Procons 4 IT"}},{"workerCode":"123123123123","workerName":"Houssam The Great","serialNumber":"555333444","agent":"SA004","mobile":"200","age":118,"name":"Domestic worker","code":"DW00001","birthDate":"2/1/1900","gender":"Male","nationality":"Bengladish","religion":"Muslim","maritalStatus":"Single","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion_guitar.c6dea341-8748-45e8-bedf-0a3b826fbbe9.png","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minons.ffebf301-c5c5-4378-94e4-20e8f50c2da3.jpeg","price":500,"salary":10,"weight":"3000","height":"0.5","education":"None","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion2.8cff8618-f495-42b6-bfc6-a362496b5d56.jpeg","video":"google.com","passportNumber":"123123123123","passportIssDate":"2/1/2018 12:00:00 AM","passportExpDate":"2/1/2018","passportPoIssue":"GG","civilId":"555333444","status":"متوفر","workerType":"Worker","languages":[{"name":"الانكليزية","value":"1"},{"name":"هندي","value":"3"}],"hobbies":"","location":"","isNew":"Y","period":0,"experiences":{"workerID":"123123123123","startDate":"5/6/2001 12:00:00 AM","endDate":"5/6/2002 12:00:00 AM","title":"Developer","description":"Job Description","companyName":"Procons 4 IT"}},{"workerCode":"222444555","workerName":"Houssam The Great updated","serialNumber":"555333444","agent":"SA004","mobile":"20000000","age":118,"name":"Domestic worker","code":"DW00001","birthDate":"12/1/1900","gender":"Male","nationality":"Bengladish","religion":"Muslim","maritalStatus":"Single","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion_guitar.ac90ab4e-d6c3-485b-b3bd-c7ee7e00a8f7.png","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minons.90b4fbfe-a0e8-4651-a243-4c290cf36ad0.jpeg","price":500,"salary":10,"weight":"3000","height":"0.5","education":"High School","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion2.57d19f3a-1fcb-4f8d-b508-50156b04a9d5.jpeg","video":"google.com","passportNumber":"222444555","passportIssDate":"12/1/1900 12:00:00 AM","passportExpDate":"12/1/1900","passportPoIssue":"GG","civilId":"555333444","status":"متوفر","workerType":"Worker","languages":[{"name":"الانكليزية","value":"1"},{"name":"هندي","value":"3"}],"hobbies":"Dancing","location":"LEBANON","isNew":"Y","period":0,"experiences":[{"workerID":"222444555","startDate":"","endDate":"","title":"Accountant","description":"Job Description","companyName":"Procons"}]},{"workerCode":"500901123","workerName":"Test123","serialNumber":"123456789106","agent":"SA004","mobile":"12345555","age":27,"name":"Maid - Ethiopia","code":"DW00002","birthDate":"5/6/1991","gender":"Male","nationality":"Philipin","religion":"Muslim","maritalStatus":"Single","language":null,"photo":"https://dev.procons-scloud.com:443/api/Workers/Image?path=lenovo-yoga-book-feature-notetaking-windows-full-width.fd7e6317-adeb-4746-98d6-28927bd5b478.jpg","license":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion_guitar.1622c28e-6a76-4726-9a7a-789c948b0aef.png","price":550,"salary":100,"weight":"60","height":"190","education":"None","passport":"https://dev.procons-scloud.com:443/api/Workers/Image?path=minion2.8a3f5737-47dc-4277-bde8-69d98df33a11.jpeg","video":"https://www.youtube.com/watch?v=H7DGigU3V2I","passportNumber":"500901123","passportIssDate":"5/6/1991 12:00:00 AM","passportExpDate":"5/6/1991","passportPoIssue":"Germany","civilId":"123456789106","status":"متوفر","workerType":"Maid","languages":[{"name":"الانكليزية","value":"1"}],"hobbies":"dd","location":"asdad","isNew":"Y","period":0,"experiences":[{"workerID":"500901123","startDate":"","endDate":"","title":"Maid","description":"Job Description","companyName":"Procons"}]}]}`);

        return Observable.of(FakeServerData);
    }
    InitializeAddWorkerForTesting() {
        let mockData: SearchCriteriaParams = { "age": [{ "name": "30", "value": "3" }], "languages": [{ "name": "English", "value": "1" }, { "name": "Arabic", "value": "2" }, { "name": "french", "value": "3" }], "education": [{ "name": "University", "value": "1" }, { "name": "High School", "value": "2" }, { "name": "None", "value": "3" }, { "name": "middle school", "value": "4" }], "religion": [{ "name": "Christian", "value": "1" }, { "name": "Muslim", "value": "2" }, { "name": "Hindu", "value": "3" }, { "name": "atheist", "value": "4" }], "nationality": [{ "name": "India", "value": "1" }, { "name": "Bengladish", "value": "2" }, { "name": "Philipin", "value": "3" }, { "name": "Ethioipia", "value": "4" }], "gender": [{ "name": "Male", "value": "M" }, { "name": "Female", "value": "F" }], "maritalStatus": [{ "name": "Single", "value": "1" }, { "name": "Married", "value": "2" }, { "name": "Divorced", "value": "3" }, { "name": "Single Mom", "value": "4" }], "workerTypes": [{ "name": "Driver", "value": "Driver" }, { "name": "Worker", "value": "Worker" }, { "name": "Maid", "value": "Maid" }, { "name": "House boy", "value": "Houseboy" }] };
        this.state.searchCriteriaParams = mockData;
        this.loading = false;
        this.ShowAddWorker();
    }
}


export enum WorkerManagmentTransition {
    AddProfile,
    DeleteWorker,
    EditProfile,
    ViewProfiles
}
export interface NavigateAction {
    action: WorkerManagmentTransition,
    value: any
}


