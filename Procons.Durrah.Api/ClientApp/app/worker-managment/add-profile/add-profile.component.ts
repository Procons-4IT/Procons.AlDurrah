import { Component, ElementRef, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { DatePipe } from '@angular/common';

import { ApiService } from '../../Services/ApiService';
import { ProconsModalSerivce } from '../../Services/ProconsModalService';
import { Worker } from "../../Models/Worker";
import { SearchCriteriaParams, NameValuePair } from '../../Models/ApiRequestType'
import { MomentDatePipe } from '../../moment-date.pipe';
import * as moment from 'moment';

@Component({
    selector: "add-profile",
    templateUrl: "./add-profile.component.html",
    styleUrls: ["./add-profile.component.css"],
    providers: [DatePipe, MomentDatePipe]
})
export class AddProfileComponent implements OnInit {
    @Input() worker;
    @Input() filteredItems: NameValuePair[];
    @Input() searchCriterias: SearchCriteriaParams;
    @Output() onBack = new EventEmitter<any>();
    @Output() onWorkerTypeChange = new EventEmitter<any>();
    @ViewChild("photoUpload") photoUpload;
    @ViewChild("licenseUpload") licenseUpload;
    @ViewChild("passportUpload") passportUpload;
    @ViewChild("workerForm") workerForm;

    itemCode = "";
    passportFileName = "";
    photoFileName = "";
    licenseFileName = "";
    photoFileText = "Select Photo File";
    passportFileText = "Select Passport File";

    loading = false;
    state: { isAddMode: boolean, title: string, worker: Worker, selectOptionText: string } = {
        isAddMode: true,
        title: "اضافة ملف",
        worker: null,
        selectOptionText: "-- اختيار --"
    }
    photoServerMode: FileUploadMode = FileUploadMode.Add;
    passportServerMode: FileUploadMode = FileUploadMode.Add;
    licenseServerMode: FileUploadMode = FileUploadMode.Add;
    constructor(public myApi: ApiService, public myModal: ProconsModalSerivce, public momentDatePipe: MomentDatePipe) {
    }

    ngOnInit(): void {
        if (this.worker) {
            //EditMode
            this.state.isAddMode = false;
            this.state.title = "Edit Profile"
            this.state.worker = Object.assign({}, this.worker);
            // this.tempData();
            this.photoServerMode = FileUploadMode.Edit;
            this.passportServerMode = FileUploadMode.Edit;
            this.licenseServerMode = FileUploadMode.Edit;
            this.photoFileName = this.getFileImageName(this.state.worker.photo);
            this.licenseFileName = this.getFileImageName(this.state.worker.license);
            this.passportFileName = this.getFileImageName(this.state.worker.passport);

            this.state.worker.birthDate = moment(this.state.worker.birthDate, 'DD-MM-YYYY').toDate() as any;
            this.state.worker.passportExpDate = moment(this.state.worker.passportExpDate, 'DD-MM-YYYY').toDate() as any;
            this.state.worker.passportIssDate = moment(this.state.worker.passportIssDate, 'DD-MM-YYYY').toDate() as any;

            console.log('worker ', this.state.worker);
            console.log('searchCriteria Params ', this.searchCriterias);

        } else {
            this.createEmptyWorkerState();
            this.state.isAddMode = true;
        }
        if (!this.state.worker.languages) {
            this.state.worker.languages = [];
        } else {
            this.state.worker.languages = this.state.worker.languages.map(nameValuePair => nameValuePair.value) as any;
        }
        if (this.searchCriterias.languages && Array.isArray(this.searchCriterias.languages)) {
            this.searchCriterias.languages = this.searchCriterias.languages.map(nameValuePair => { return { "label": nameValuePair.name, "value": nameValuePair.value } }) as any;
        }
    }

    onChange(workerType) {

        this.onWorkerTypeChange.emit(workerType);
    }

    back() {
        this.onBack.emit();
    }
    submitWorker(a, b, c) {
        if (this.state.isAddMode) {
            this.addWorker(a, b, c);
        } else {
            this.editWorker(a, b, c);
        }
    }
    addWorker(photoInput: any, passInput: any, licenseInput: any) {
        let photoFile = photoInput.files;
        let passportFile = passInput.files;
        let LicenseFile = licenseInput.files;

        if (photoFile && photoFile[0] && passportFile && passportFile[0] && LicenseFile && LicenseFile[0]) {
            const formData = new FormData();
            formData.append("Photo", photoFile[0], photoFile[0].name);
            formData.append("Passport", passportFile[0], passportFile[0].name);
            formData.append("License", LicenseFile[0], LicenseFile[0].name);
            this.getFormData(formData);
            this.loading = true;
            this.myApi.addWorker(formData).subscribe(x => {
                this.onSucessfullUpdate(x, true);
            }, onError => {
                this.onErrorUpdate(onError);
            });

        } else {
            alert('TEMP Error Message: Photo or Passport Files are missing!');
        }

    }

    editWorker(photoInput: any, passInput: any, licenseInput: any) {
        //entering editMode! 
        let photoFile = photoInput.files;
        let passportFile = passInput.files;
        let licenseFile = licenseInput.files;

        const formData = new FormData();

        this.appendOneFile(formData, "Photo", photoFile, this.photoServerMode);
        this.appendOneFile(formData, "Passport", passportFile, this.passportServerMode);
        this.appendOneFile(formData, "License", licenseFile, this.licenseServerMode);


        this.getFormData(formData);
        this.loading = true;
        this.myApi.updateWorker(formData).subscribe(x => {
            this.onSucessfullUpdate(x, false);
        }, onError => {
            this.onErrorUpdate(onError);
        });
    }
    appendOneFile(formData, property, file, fileMode: FileUploadMode) {
        // formData.append("Photo", photoFile[0], photoFile[0].name);
        if (file && file[0]) {
            formData.append(property, file[0], file[0].name);
        } else {
            formData.append(property, fileMode === FileUploadMode.Delete ? "0" : "1");
        }
    }

    onSucessfullUpdate(onSuccessMessage, resetForm: boolean = false) {
        this.loading = false;
        this.myModal.showSuccessModal(onSuccessMessage, false);
        console.log('state', this.state);
        if (resetForm) {
            this.clearForm();
        }
    }
    clearForm() {
        console.log('clearing the form');
        this.createEmptyWorkerState();
        this.photoUpload.clear();
        this.licenseUpload.clear();
        this.passportUpload.clear();
    }
    onErrorUpdate(errorResponse) {
        console.log(errorResponse);
        let errorObject = errorResponse.json();
        let onErrorMessage = errorObject.exceptionMessage;
        console.log(onErrorMessage);
        this.loading = false;
        this.myModal.showErrorModal(onErrorMessage, false);

    }

    getFormData(formData: FormData) {
        formData.append('WorkerName', this.state.worker.workerName);
        formData.append('WorkerCode', this.state.worker.passportNumber);
        formData.append('BirthDate', this.formatDate(this.state.worker.birthDate.toString()));
        formData.append('Gender', this.state.worker.gender);
        formData.append('Nationality', this.state.worker.nationality);
        formData.append('Religion', this.state.worker.religion);
        formData.append('MaritalStatus', this.state.worker.maritalStatus);
        formData.append('Languages', JSON.stringify(this.state.worker.languages));
        formData.append('Weight', this.state.worker.weight);
        formData.append('Height', this.state.worker.height);
        formData.append('Education', this.state.worker.education);
        formData.append('Video', this.state.worker.video);
        formData.append('PassportNumber', this.state.worker.passportNumber);
        formData.append('PassportIssDate', this.formatDate(this.state.worker.passportIssDate.toString()));
        formData.append("PassportPoIssue", this.state.worker.passportPoIssue);
        formData.append('PassportExpDate', this.formatDate(this.state.worker.passportExpDate.toString()));
        formData.append('CivilId', this.state.worker.civilId);
        let itemName: any[] = this.searchCriterias.workerTypes.filter(nameValuePair => nameValuePair.value === this.state.worker.code);
        formData.append('Name', itemName[0].name);
        formData.append('Code', this.state.worker.code);
    }


    formatDate(dateValue): string {
        return this.momentDatePipe.transform(dateValue);
    }

    createEmptyWorkerState() {
        let workerParams: any = Array.from({ length: 27 }, x => { return '' }) as any;
        let tempWorker = new (<any>Worker)(...workerParams);
        tempWorker.languages = [];
        this.state.worker = tempWorker;
    }
    isFilesAttached(): boolean {

        let photoFile = this.photoUpload.files;
        let passportFile = this.passportUpload.files;
        let licenseFile = this.licenseUpload.files;

        return (photoFile && photoFile[0] && passportFile && passportFile[0] && licenseFile && licenseFile[0]);
    }

    getFileImageName(fileUrl): string {
        if (typeof fileUrl === "string") {
            return (fileUrl as string).match(/\?path=(.*)/)[1];
        }
        return;

    }


    deleteServerFile(fileType: string) {
        if (fileType == 'PHOTO') {
            this.photoServerMode = FileUploadMode.Delete;
        } else if (fileType == 'PASSPORT') {
            this.passportServerMode = FileUploadMode.Delete;
        } else {
            this.licenseServerMode = FileUploadMode.Delete;
        }
    }
    FUM = FileUploadMode;

    tempData() {
        this.state.worker = {
            "workerCode": null,
            "serialNumber": "AB23102017",
            "agent": "SA004",
            "age": 47,
            "name": "Maid Srilanka Sample",
            "code": "DW00003",
            "birthDate": "1/1/1970 12:00:00 AM",
            "gender": "F",
            "nationality": "2",
            "religion": "2",
            "maritalStatus": "1",
            "language": "2",
            "photo": "http://127.0.0.1:1357/minion2.jpeg",
            "price": 237.0,
            "weight": "65",
            "height": "150",
            "education": "1",
            "passport": "http://127.0.0.1:1357/minion2.jpeg",
            "video": null,
            "passportNumber": "AB23102017",
            "passportIssDate": "1/1/2016",
            "passportExpDate": "12/31/2026",
            "passportPoIssue": "Bandaranaiyak",
            "civilId": null,
            "status": "2"
        } as any;
        this.searchCriterias = { "languages": [{ "name": "English", "value": "1" }, { "name": "Arabic", "value": "2" }, { "name": "Hindi", "value": "3" }], "education": [{ "name": "University", "value": "1" }, { "name": "High School", "value": "2" }, { "name": "None", "value": "3" }, { "name": "Technical", "value": "4" }], "religion": [{ "name": "Christian", "value": "1" }, { "name": "Muslim", "value": "2" }, { "name": "Hindu", "value": "3" }], "nationality": [{ "name": "India", "value": "1" }, { "name": "Bengladish", "value": "2" }, { "name": "Philipin", "value": "3" }, { "name": "Ethioipia", "value": "4" }, { "name": "Srilanka", "value": "5" }, { "name": "Egypt", "value": "6" }], "gender": [{ "name": "Male", "value": "M" }, { "name": "Female", "value": "F" }], "maritalStatus": [{ "name": "Single", "value": "1" }, { "name": "Married", "value": "2" }, { "name": "Divorced", "value": "3" }], "workerTypes": [{ "name": "Domestic worker", "value": "DW00001" }, { "name": "Maid - Ethiopia", "value": "DW00002" }, { "name": "Maid - SriLanka", "value": "DW00003" }, { "name": "Driver Indian Fresh", "value": "DW00004" }, { "name": "Laptop Lenovo Thinkpad", "value": "FA00001" }, { "name": "Decoretion for main office", "value": "FA00002" }, { "name": "Laptop", "value": "FA00003" }] };

    }
}

enum FileUploadMode {
    Add, Edit, Delete
}