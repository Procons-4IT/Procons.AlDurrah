import { Component } from '@angular/core';
import { Worker } from "../../Models/Worker";

@Component({
    selector: "view-profiles",
    templateUrl: "./view-profiles.component.html",
    styles: ["./view-profiles.component.css"]
})
export class ViewProfilesComponent {
    state = {
        workers: Array(7)
    }
    constructor() {
        console.log('render the page!');
    }
    
    back(){
        console.log('click me baby one more time!');
    }
}