import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { Worker, WorkerManagementData } from "../../Models/Worker";
import { NavigateAction, WorkerManagmentTransition } from "../worker-managment.component";
import { LanguageConvertPipe } from '../../language-convert.pipe';


@Component({
    selector: "view-agent-workers",
    templateUrl: "./view-agent-workers.component.html",
    styleUrls: ["./view-agent-workers.component.css"],
    providers: [LanguageConvertPipe]
})
export class ViewAgentWorkersComponent implements OnInit {
    @Input() $workers: Observable<[Worker[], Worker[]]>;
    @Output() transitionEmitter = new EventEmitter<NavigateAction>();

    ngOnInit(): void {
        console.log("I was initialized");
        this.$workers.subscribe(viewAndServerworkers => {
            this.state.workers = viewAndServerworkers[0];
            this.state.completeListOfWorkers = Object.assign([], viewAndServerworkers[0]);
            this.state.workersServerData = viewAndServerworkers[1];
            console.log('got workers', this.state.workersServerData);

        })
    }

    loading = false;
    //Temp Show the Add Profile Modal
    state = {
        ShowWorkerManagment: true,
        ShowProfiles: false,
        ShowAddProfile: false,
        searchCriteriaParams: null,
        completeListOfWorkers: null,
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
    Delete(worker: Worker, index: number) { 
        this.NavigateTo(WorkerManagmentTransition.DeleteWorker, [worker, index]);

    }

    NavigateTo(action: WorkerManagmentTransition, value: any = null) {
        this.transitionEmitter.emit({ action: action, value: value });
    }

    filterByWorkerName(workerName) {
        if (workerName) {
            this.state.workers = this.state.completeListOfWorkers.filter(worker => { return worker["workerName"].toLowerCase().includes(workerName.toLowerCase());});
        } else {
            this.state.workers = this.state.completeListOfWorkers;
        }

    }
}