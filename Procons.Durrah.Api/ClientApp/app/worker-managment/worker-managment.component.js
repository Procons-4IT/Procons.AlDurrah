"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
//Consider Changing the Navigation Logic to a router-outlet with Child Components and a Feature Module (issue: Fix the NavBar Navigations)
const core_1 = require("@angular/core");
let WorkerMangmentComponent = class WorkerMangmentComponent {
    constructor() {
        //Temp Show the Add Profile Modal
        this.state = {
            showWorkerManagment: true,
            showProfiles: false,
            showAddProfile: false,
            workers: Array.from({ length: 7 }, (_, idx) => { return { name: idx++ }; }),
            selectedWorker: null
        };
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
    Delete(worker, index) {
        console.log('deleting Worker ', worker, ' ', index);
        this.state.workers.splice(index, 1);
        console.log('#TO DO IMPLEMENT DELETE API');
    }
};
WorkerMangmentComponent = __decorate([
    core_1.Component({
        selector: "worker-managment",
        templateUrl: "./worker-managment.component.html",
        styles: ["./worker-managment.component.css"]
    })
], WorkerMangmentComponent);
exports.WorkerMangmentComponent = WorkerMangmentComponent;
//# sourceMappingURL=worker-managment.component.js.map