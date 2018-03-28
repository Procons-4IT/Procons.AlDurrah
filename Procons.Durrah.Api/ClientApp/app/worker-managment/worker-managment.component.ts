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
              //  this.Delete(navigateAction.value as Worker);
              this.Delete(navigateAction.value[0], navigateAction.value[1])
                break;
            case WorkerManagmentTransition.ViewProfiles:
                this.ShowProfiles();
                break;

            default:
                break;
        }

    }

    //Delete(worker: Worker, index: number = 0) 
    Delete(worker, index){
        //fix delete!
        this.state.workers.splice(index, 1);
        this.loading = true;
        this.myApi.deleteWorker(worker.workerCode).subscribe(
            onSucces => {
                this.loading = false;
                this.myModal.showSuccessModal("Worker Deleted!");
            }, onError => {
                this.loading = false;
                this.myModal.showErrorModal("Failed to Delete Worker");
            });
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


