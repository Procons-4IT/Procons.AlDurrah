//Consider Changing the Navigation Logic to a router-outlet with Child Components and a Feature Module (issue: Fix the NavBar Navigations)
import { Component, OnInit } from '@angular/core';
import { Worker, WorkerManagementData } from "../Models/Worker";
import { ApiService } from "../Services/ApiService";

@Component({
    selector: "worker-managment",
    templateUrl: "./worker-managment.component.html",
    styles: ["./worker-managment.component.css"]
})
export class WorkerMangmentComponent implements OnInit {

    ngOnInit(): void {
        console.log('view loading!');
        this.myApi.getWorkerManagmentData().subscribe(allTheData => {
            console.log('i got a bunch of workers ', allTheData);
            this.state.workers = allTheData.workerDisplayData;
            this.state.workersServerData = allTheData.workerServerData;
            this.state.searchCriteriaParams = allTheData.searchCriteria;
            this.state.selectedWorker = this.state.workersServerData[0];
        });
    }

    loading = false;
    //Temp Show the Add Profile Modal
    state = {
        showWorkerManagment: true,
        showProfiles: false,
        showAddProfile: false,
        searchCriteriaParams: null,
        workers: null,
        workersServerData: null,
        selectedWorker: null
    }

    constructor(public myApi: ApiService) {
    }

    ShowProfiles() {
        this.state.showWorkerManagment = false;
        this.state.showAddProfile = false;
        this.state.showProfiles = true;
    }
    ShowWorkerManagment() {
        this.state.showAddProfile = false;
        this.state.showProfiles = false;
        this.state.showWorkerManagment = true;
    }

    ShowAddWorker() {
        this.state.showWorkerManagment = false;
        this.state.showProfiles = false;
        this.state.selectedWorker = null;
        this.state.showAddProfile = true;
    }
    ShowEditWorker(worker, i) {
        //Temp Fix
        this.ShowAddWorker();
        this.state.selectedWorker = this.state.workersServerData[i];
    }

    Delete(worker: Worker, index: number) {
        //fix delete!
        console.log('deleting Worker ', worker, ' ', index);
        // this.state.workers.splice(index, 1);
        console.log('#TO DO IMPLEMENT DELETE API');
    }
}