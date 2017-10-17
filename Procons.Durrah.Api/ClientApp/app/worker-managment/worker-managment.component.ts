//Consider Changing the Navigation Logic to a router-outlet with Child Components and a Feature Module (issue: Fix the NavBar Navigations)
import { Component, OnInit } from '@angular/core';
import { Worker } from "../Models/Worker";
import { ApiService } from "../Services/ApiService";

@Component({
    selector: "worker-managment",
    templateUrl: "./worker-managment.component.html",
    styles: ["./worker-managment.component.css"]
})
export class WorkerMangmentComponent implements OnInit {

    ngOnInit(): void {
        console.log('view loading!');
        this.myApi.getAllWorkers({}).subscribe(workers => {
            console.log('i got a bunch of workers ', workers);
            this.state.workers = workers;
        })
    }

    loading= false;
    //Temp Show the Add Profile Modal
    state = {
        showWorkerManagment: true,
        showProfiles: false,
        showAddProfile: false,
        searchCriteriaParams: null,
        workers: null,
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
    ShowEditWorker(worker) {
        //Temp Fix
        this.loading = true;
        this.myApi.getSearchCriteriaParameters().subscribe(searchCriteriaParams => {
            this.loading = false;
            this.state.searchCriteriaParams = searchCriteriaParams;
            this.ShowAddWorker();
            this.state.selectedWorker = worker;

        },onError=>{
            console.error(onError);
            this.loading = false;
        });
    }

    Delete(worker: Worker, index: number) {
        console.log('deleting Worker ', worker, ' ', index);
        this.state.workers.splice(index, 1);
        console.log('#TO DO IMPLEMENT DELETE API');
    }
}