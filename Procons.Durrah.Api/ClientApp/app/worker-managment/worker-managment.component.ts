//Consider Changing the Navigation Logic to a router-outlet with Child Components and a Feature Module (issue: Fix the NavBar Navigations)
import { Component } from '@angular/core';
import { Worker } from "../Models/Worker";

@Component({
    selector: "worker-managment",
    templateUrl: "./worker-managment.component.html",
    styles: ["./worker-managment.component.css"]
})
export class WorkerMangmentComponent {

    state = {
        showProfiles: false,
        showWorkerManagment: true,
        showAddProfile: false,
        workers: Array.from({ length: 7 }, (_, idx) => { return { name: idx++ } }),
        selectedWorker: null
    }

    constructor() {
        console.log('render the page!');
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
        this.ShowAddWorker();
        this.state.selectedWorker = worker;
    }

    Delete(worker: Worker, index: number) {
        console.log('deleting Worker ', worker, ' ', index);
        this.state.workers.splice(index, 1);
        console.log('#TO DO IMPLEMENT DELETE API');
    }
}