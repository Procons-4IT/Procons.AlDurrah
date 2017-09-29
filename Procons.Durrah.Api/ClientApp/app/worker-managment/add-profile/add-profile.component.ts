import { Component } from '@angular/core';
import { Worker } from "../../Models/Worker";

@Component({
    selector: "add-profile",
    templateUrl: "./add-profile.component.html",
    styles: ["./add-profile.component.css"]
})
export class AddProfileComponent {
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