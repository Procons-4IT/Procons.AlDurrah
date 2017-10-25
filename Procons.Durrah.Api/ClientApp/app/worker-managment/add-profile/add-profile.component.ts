import { Component, ElementRef, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';

import { ApiService } from '../../Services/ApiService';
import { Worker } from "../../Models/Worker";
import { SearchCriteriaParams } from '../../Models/ApiRequestType'

@Component({
    selector: "add-profile",
    templateUrl: "./add-profile.component.html",
    styleUrls: ["./add-profile.component.css"],
    providers: [DatePipe]
})
export class AddProfileComponent implements OnInit {
    @Input() worker;
    @Input() searchCriterias: SearchCriteriaParams;
    @Output() onBack = new EventEmitter<any>();
    print: any;

    photoFileText = "Select Photo File";
    passportFileText = "Select Passport File";

    state: { isAddMode: boolean, title: string, worker: Worker } = {
        isAddMode: true,
        title: "Add Profile",
        worker: null
    }
    constructor(public myApi: ApiService) {
    }

    ngOnInit(): void {
        if (this.worker) {
            this.state.isAddMode = false;
            this.state.title = "Edit Profile"
            this.state.worker = Object.assign({}, this.worker);
            console.log('searchCriteria Params ', this.searchCriterias);
        } else {
            // let workerParams: any = Array.from({ length: 22 }, x => { return 'a' }) as any;
            // this.state.worker = new (<any>Worker)(...workerParams);
            this.state.worker = {
                "workerCode": null,
                "serialNumber": "A123456",
                "agent": "SA004",
                "age": 25,
                "name": null,
                "code": "DW00003",
                "birthDate": "9/1/1990 12:00:00 AM",
                "gender": "M",
                "nationality": "1",
                "religion": "1",
                "maritalStatus": "1",
                "language": "1",
                "photo": "C:\\Program Files (x86)\\sap\\SAP Business One\\Attachments\\test.jpg",
                "price": 200.0,
                "weight": "76",
                "height": "180",
                "education": "1",
                "passport": "A123456",
                "video": "https://www.youtube.com/embed/HYNpSAR2yd4",
                "passportNumber": "BK22456",
                "passportIssDate": "9/1/2017",
                "passportExpDate": "9/1/2018",
                "passportPoIssue": "01.09.2017",
                "civilId": "224456123456",
                "status": "2"
            } as any;
        }
    }

    back() {
        this.onBack.emit();
    }
    addWorker(photoInput: any, passInput: any) {
debugger;
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
            formData.append('Video', this.state.worker.video);
            formData.append('PassportNumber', this.state.worker.passportNumber);
            formData.append('PassportIssDate', this.state.worker.passportIssDate);
            formData.append('PassportExpDate', this.state.worker.passportExpDate);
            formData.append('CivilId', this.state.worker.civilId);
            formData.append('Code', this.state.worker.code);



            this.myApi.uploadFile(formData).subscribe(x => {
                console.log('Somethign Happend ! ', formData)
            }, onError => {
                console.log('someting terrible happened!');
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
    myUploader(event) {
        //event.files == files to upload
        console.log('I should upload the files!');
    }
}