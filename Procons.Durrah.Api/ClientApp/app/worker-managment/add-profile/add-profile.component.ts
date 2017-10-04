import { Component, ElementRef, EventEmitter, Input, Output, OnInit } from '@angular/core';

import { ApiService } from '../../Services/ApiService';
import { Worker } from "../../Models/Worker";

@Component({
    selector: "add-profile",
    templateUrl: "./add-profile.component.html",
    styles: ["./add-profile.component.css"]
})
export class AddProfileComponent implements OnInit {
    @Input() worker;
    @Output() onBack = new EventEmitter<any>();
    print:any;
    state: { isAddMode: boolean, title: string, worker: Worker } = {
        isAddMode: true,
        title: "Add Profile",
        worker: null
    }
    constructor(public myApi: ApiService) {
        this.print = JSON.stringify;
    }

    ngOnInit(): void {
        console.log('add-profile: ', this.worker);
        if (this.worker) {
            this.state.isAddMode = false;
            this.state.title = "Edit Profile"
            this.state.worker = Object.assign({}, this.worker);
        }else{
            let workerParams: any = Array.from({length:22},x=>{return ''}) as any;
            this.state.worker = new (<any>Worker)(...workerParams);
        }
    }

    back() {
        this.onBack.emit();
    }
    addWorker() {
        console.log('I am worker! ',this.state.worker);
    }

    originalUploadFile(elFiles: ElementRef) {
        console.log('click!');
        let files = elFiles.nativeElement.files;
        if (files && files[0]) {
            const formData = new FormData();
            for (var i = 0; i < files.length; i++) {
                formData.append("Photo", files[i], files[i].name);
                formData.append("Passport", files[i], files[i].name);
            }
            formData.append('Age', '43');
            formData.append('BirthDate', '01-01-2000');
            formData.append('CivilId', '124542154215');
            formData.append('Code', 'code');
            formData.append('Education', '1');
            formData.append('Gender', '1');
            formData.append('Height', '180');
            formData.append('Language', '1');
            formData.append('MaritalStatus', '1');
            formData.append('Nationality', '1');
            formData.append('PassportExpDate', '01-01-2000');
            formData.append('PassportIssDate', '01-01-2000');
            formData.append('PassportNumber', '01-01-2000');
            formData.append('Religion', '1');
            formData.append('Video', '01-01-2000');
            formData.append('BirthDate', '01-01-2000');
            formData.append('Weight', '80');

            console.log('sending FormData', formData);
            this.myApi.uploadFile(formData).subscribe(x => {
                console.log('Somethign Happend ! ', formData)
            }, onError => {
                console.log('oopsss! ', onError);
            })
        }
    }
}