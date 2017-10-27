import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { Worker, WorkerManagementData } from "../../Models/Worker";
import { NavigateAction, WorkerManagmentTransition } from "../worker-managment.component";


@Component({
    selector: "view-agent-workers",
    templateUrl: "./view-agent-workers.component.html"
})
export class ViewAgentWorkersComponent implements OnInit {
    @Input() $workers: Observable<Worker[]>;
    @Output() transitionEmitter = new EventEmitter<NavigateAction>();

    ngOnInit(): void {
        console.log("I was initialized");
        this.$workers.subscribe(workers => {
            this.state.workers = workers;
        })
    }

    loading = false;
    //Temp Show the Add Profile Modal
    state = {
        ShowWorkerManagment: true,
        ShowProfiles: false,
        ShowAddProfile: false,
        searchCriteriaParams: null,
        workers: null,
        workersServerData: null,
        selectedWorker: null
    }


    constructor() { }

    ShowProfiles() {
        this.NavigateTo(WorkerManagmentTransition.ViewProfiles);
    }

    ShowAddWorker() {
        this.NavigateTo(WorkerManagmentTransition.AddProfile);
    }

    ShowEditWorker(worker: Worker, index: number) {
        this.NavigateTo(WorkerManagmentTransition.EditProfile, [worker, index]);
    }
    Delete(worker, i) {

    }

    NavigateTo(action: WorkerManagmentTransition, value: any = null) {
        this.transitionEmitter.emit({ action: action, value: value });
    }

}