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
    print: any;
    state: { isAddMode: boolean, title: string, worker: Worker } = {
        isAddMode: true,
        title: "Add Profile",
        worker: null
    }
    constructor(public myApi: ApiService) {
        this.print = JSON.stringify;
    }

    ngOnInit(): void {
        
        if (this.worker) {
            this.state.isAddMode = false;
            this.state.title = "Edit Profile"
            this.state.worker = Object.assign({}, this.worker);
        } else {
            let workerParams: any = Array.from({ length: 22 }, x => { return '' }) as any;
            this.state.worker = new (<any>Worker)(...workerParams);
        }
    }

    back() {
        this.onBack.emit();
    }
    addWorker(photoInput: any, passInput: any) {
        
        let photoFile = photoInput.files;
        let passportFile = passInput.files;

        if (photoFile && photoFile[0] && passportFile && passportFile[0]) {
            const formData = new FormData();
            formData.append("Photo", photoFile[0], photoFile[0].name);
            formData.append("Passport", passportFile[0], passportFile[0].name);
            formData.append('BirthDate', this.state.worker.birthDate);
            formData.append('Gender', this.state.worker.gender);
            formData.append('Nationality', this.state.worker.nationality);
            formData.append('Religion', this.state.worker.religion);
            formData.append('MaritalStatus', this.state.worker.maritalStatus);
            formData.append('Language', this.state.worker.language);
            //photo Missing
            formData.append('Weight', this.state.worker.weight);
            formData.append('Height', this.state.worker.height);
            formData.append('Education', this.state.worker.education);
            formData.append('Video', this.state.worker.videopublic);
            formData.append('PassportNumber', this.state.worker.passportNumber);
            formData.append('PassportIssDate', this.state.worker.passportIssDate);
            formData.append('PassportExpDate', this.state.worker.passportExpDate);
            formData.append('CivilId', this.state.worker.civilId);
            //type Missing ? is it code

            
            this.myApi.uploadFile(formData).subscribe(x => {
                console.log('Somethign Happend ! ', formData)
            }, onError => {
                
            });

        } else {
            alert('TEMP Error Message: Photo or Passport Files are missing!');
        }

    }

    originalUploadFile(elFiles: ElementRef) {
        
        let files = elFiles.nativeElement.files;
        if (files && files[0]) {
            const formData = new FormData();
            for (var i = 0; i < files.length; i++) {
                formData.append("Photo", files[i], files[i].name);
                formData.append("Passport", files[i], files[i].name);
            }
            formData.append('BirthDate', '01-01-2000');
            formData.append('Gender', '1');
            formData.append('Nationality', '1');
            formData.append('Religion', '1');
            formData.append('MaritalStatus', '1');
            formData.append('Language', '1');
            //photo Missing
            formData.append('Weight', '80');
            formData.append('Height', '180');
            formData.append('Education', '1');
            formData.append('Video', '01-01-2000');
            formData.append('PassportNumber', '01-01-2000');
            formData.append('PassportIssDate', '01-01-2000');
            formData.append('PassportExpDate', '01-01-2000');
            formData.append('CivilId', '124542154215');
            //type Missing ? is it code
            formData.append('Age', '43'); //This is not needed
            formData.append('Code', 'code');

            
            this.myApi.uploadFile(formData).subscribe(x => {
                console.log('Somethign Happend ! ', formData)
            }, onError => {
                
            });
        }
    }
}