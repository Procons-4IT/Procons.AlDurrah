"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const worker_managment_component_1 = require("../worker-managment.component");
const language_convert_pipe_1 = require("../../language-convert.pipe");
let ViewAgentWorkersComponent = class ViewAgentWorkersComponent {
    constructor() {
        this.transitionEmitter = new core_1.EventEmitter();
        this.loading = false;
        //Temp Show the Add Profile Modal
        this.state = {
            ShowWorkerManagment: true,
            ShowProfiles: false,
            ShowAddProfile: false,
            searchCriteriaParams: null,
            completeListOfWorkers: null,
            workers: null,
            workersServerData: null,
            selectedWorker: null
        };
    }
    ngOnInit() {
        console.log("I was initialized");
        this.$workers.subscribe(viewAndServerworkers => {
            this.state.workers = viewAndServerworkers[0];
            this.state.completeListOfWorkers = Object.assign([], viewAndServerworkers[0]);
            this.state.workersServerData = viewAndServerworkers[1];
            console.log('got workers', this.state.workersServerData);
        });
    }
    ShowProfiles() {
        this.NavigateTo(worker_managment_component_1.WorkerManagmentTransition.ViewProfiles);
    }
    ShowAddWorker() {
        this.NavigateTo(worker_managment_component_1.WorkerManagmentTransition.AddProfile);
    }
    ShowEditWorker(worker, index) {
        this.NavigateTo(worker_managment_component_1.WorkerManagmentTransition.EditProfile, [worker, index]);
    }
    Delete(worker, i) {
    }
    NavigateTo(action, value = null) {
        this.transitionEmitter.emit({ action: action, value: value });
    }
    filterByWorkerName(workerName) {
        if (workerName) {
            this.state.workers = this.state.completeListOfWorkers.filter(worker => { return worker["workerName"].toLowerCase().includes(workerName.toLowerCase()); });
        }
        else {
            this.state.workers = this.state.completeListOfWorkers;
        }
    }
};
__decorate([
    core_1.Input()
], ViewAgentWorkersComponent.prototype, "$workers", void 0);
__decorate([
    core_1.Output()
], ViewAgentWorkersComponent.prototype, "transitionEmitter", void 0);
ViewAgentWorkersComponent = __decorate([
    core_1.Component({
        selector: "view-agent-workers",
        templateUrl: "./view-agent-workers.component.html",
        styleUrls: ["./view-agent-workers.component.css"],
        providers: [language_convert_pipe_1.LanguageConvertPipe]
    })
], ViewAgentWorkersComponent);
exports.ViewAgentWorkersComponent = ViewAgentWorkersComponent;
//# sourceMappingURL=view-agent-workers.component.js.map