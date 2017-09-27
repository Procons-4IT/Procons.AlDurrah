import { Component } from '@angular/core';
import { Worker } from "../Models/Worker";

@Component({
    selector: "worker-managment",
    templateUrl: "./worker-managment.component.html",
    styles: ["./worker-managment.component.css"]
})
export class WorkerMangmentComponent {
    workers: any[] = Array(7);
    constructor() {
        console.log('render the page!');
    }
}