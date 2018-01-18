//TODO: Issue with DateFormat server returning mm/dd/yyyy
import { Component, ElementRef, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from "@angular/forms";
import { DatePipe } from '@angular/common';
import { Subject } from 'rxjs/Subject';
import { ApiService } from '../../Services/ApiService';
import { ProconsModalSerivce } from '../../Services/ProconsModalService';
import { Worker, Experience } from "../../Models/Worker";
import { WorkerTypeParam } from '../../Models/ApiRequestType';


import { SearchCriteriaParams, NameValuePair } from '../../Models/ApiRequestType'
import { MomentDatePipe } from '../../moment-date.pipe';
import * as moment from 'moment';

@Component({
    selector: "add-profile",
    templateUrl: "./add-profile.component.html",
    styleUrls: ["./add-profile.component.css"],
    providers: [DatePipe, MomentDatePipe]
})
//mobile # [0-9]{8}
export class AddProfileComponent implements OnInit {
    @Input() worker;
    @Input() searchCriterias: SearchCriteriaParams;
    @Output() onBack = new EventEmitter<any>();
    @ViewChild("photoUpload") photoUpload;
    @ViewChild("licenseUpload") licenseUpload;
    @ViewChild("passportUpload") passportUpload;
    @ViewChild("workerForm") workerForm;

    itemList = [];
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
    constructor(private formBuilder: FormBuilder, public myApi: ApiService, public myModal: ProconsModalSerivce, public momentDatePipe: MomentDatePipe) { }


    addWorkerForm: FormGroup;
    isAddMode: Boolean = true;
    ngOnInitNewDepricated(): void {
        this.state.isAddMode = true;

        this.createEmptyWorkerState();
        this.createForm();


        if (!this.state.worker.languages) {
            this.state.worker.languages = [];
        } else {
            this.state.worker.languages = this.state.worker.languages.map(nameValuePair => nameValuePair.value) as any;
        }
        if (this.searchCriterias.languages && Array.isArray(this.searchCriterias.languages)) {
            this.searchCriterias.languages = this.searchCriterias.languages.map(nameValuePair => { return { "label": nameValuePair.name, "value": nameValuePair.value } }) as any;
        }
    }

    //TODO: Remove state.worker since only uploads are using it.
    ngOnInit(): void {
        //TODO: DELETE ME 
        this.tempData();


        console.log('AddProfile DATA: ', JSON.stringify(this.worker));
        this.createForm();

        if (this.worker) {
            //EditMode
            this.state.isAddMode = false;
            this.state.title = "تعديل الملف"
            this.state.worker = Object.assign({}, this.worker);

            this.photoServerMode = FileUploadMode.Edit;
            this.passportServerMode = FileUploadMode.Edit;
            this.licenseServerMode = FileUploadMode.Edit;
            this.photoFileName = this.getFileImageName(this.state.worker.photo);
            this.licenseFileName = this.getFileImageName(this.state.worker.license);
            this.passportFileName = this.getFileImageName(this.state.worker.passport);

            this.fillFormWithWorkerData(this.worker);
            //Consider Case WorkerType and ItemType is set. How to prepopulate workerTypes? 
            if (this.state.worker.workerType) {
                this.getItemLookupsByType(this.state.worker.workerType);
            }
            console.log('worker ', this.state.worker);

        } else {
            this.createEmptyWorkerState();
            this.createForm();
            this.state.isAddMode = true;

        }
        console.log('searchCriteria Params ', JSON.stringify(this.searchCriterias));

        if (!this.state.worker.languages) {
            this.state.worker.languages = [];
        } else {
            this.state.worker.languages = this.state.worker.languages.map(nameValuePair => nameValuePair.value) as any;
        }
        if (this.searchCriterias.languages && Array.isArray(this.searchCriterias.languages)) {
            this.searchCriterias.languages = this.searchCriterias.languages.map(nameValuePair => { return { "label": nameValuePair.name, "value": nameValuePair.value } }) as any;
        }
    }
    fillFormWithWorkerData(existingWorker: Worker) {
        //this.state.worker.birthDate = moment(this.state.worker.birthDate, 'MM-DD-YYYY').toDate() as any;
        let momentToDate = str => moment(str, 'MM-DD-YYYY').toDate() as any;
        //Convert from Server Data Object to FormData Array or Array to Array.
        let handleXP = xp => {
            let perXP = xp => {
                return { workerID: xp.workerID, title: xp.title, description: xp.description, companyName: xp.companyName, startDate: momentToDate(xp.startDate), endDate: momentToDate(xp.endDate) }
            }
            if (xp) {
                if (Array.isArray(xp)) {
                    return xp.map(p => perXP(p));
                } else {
                    return [perXP(xp)];
                }
            } else {
                return [new Experience()];
            }
        };
        const workerData =
            {
                workerName: existingWorker.workerName,
                birthDate: momentToDate(existingWorker.birthDate),
                gender: existingWorker.gender,
                nationality: existingWorker.nationality,
                religion: existingWorker.religion,
                maritalStatus: existingWorker.maritalStatus,
                languages: existingWorker.languages.map(lang => lang.value), //?null
                salary: existingWorker.salary,
                price: existingWorker.price,
                mobile: existingWorker.mobile,
                weight: existingWorker.weight,
                height: existingWorker.height,
                education: existingWorker.education,
                workerType: existingWorker.workerType,
                code: existingWorker.code, //not required but not letting me proceed without it
                video: existingWorker.video,
                passportNumber: existingWorker.passportNumber,
                passportIssDate: momentToDate(existingWorker.passportIssDate),
                passportPOIssue: existingWorker.passportPoIssue,
                passportExpDate: momentToDate(existingWorker.passportExpDate),
                civilId: existingWorker.civilId,
                hobbies: existingWorker.hobbies,
                location: existingWorker.location,
                experience: handleXP(existingWorker.experiences)
                // experience: this.formBuilder.array([]), existingWorker.experiences.map(xp => { return { title: xp.title, description: xp.description, companyName: xp.companyName, startDate: momentToDate(xp.startDate), endDate: momentToDate(xp.endDate) } }),
            };
        this.addWorkerForm.patchValue(workerData);
    }
    createForm() {
        const workerFields =
            {
                workerName: ['', Validators.required],
                birthDate: ['', Validators.required],
                gender: ['', Validators.required],
                nationality: ['', Validators.required],
                religion: ['', Validators.required],
                maritalStatus: ['', Validators.required],
                languages: [[]],
                salary: ['', Validators.required],
                price: ['', Validators.required],
                mobile: ['', Validators.pattern('[0-9]{8}')],
                weight: ['', Validators.required],
                height: ['', Validators.required],
                education: ['', Validators.required],
                workerType: [''],
                code: [''], //not required but not letting me proceed without it
                video: [''],
                passportNumber: [''],
                passportIssDate: ['', Validators.required],
                passportPOIssue: ['', Validators.required],
                passportExpDate: ['', Validators.required],
                civilId: ['', Validators.required],
                hobbies: [''],
                location: [''],
                experience: this.formBuilder.array([]),
            };
        this.addWorkerForm = this.formBuilder.group(workerFields);
        this.addXP();
    }

    get experience(): FormArray {
        return this.addWorkerForm.get('experience') as FormArray;
    }
    addXP(): void {
        this.experience.push(this.formBuilder.group(new Experience()))
    }
    removeXP(): void {
        if (this.experience.length !== 1) {
            this.experience.removeAt(this.experience.length - 1);
        }
    }


    onWorkerTypeSelected(workerType) {
        this.getItemLookupsByType(workerType);
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
            // this.getFormData(formData);
            this.appendFormDataToForm(formData);
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


        this.appendFormDataToForm(formData);
        // this.DepricatedgetFormDataD(formData);
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

    appendFormDataToForm(formData: FormData) {
        const formValues = this.addWorkerForm.value;
        console.log('User inpued form values ', formValues);

        formData.append('WorkerName', formValues.workerName);
        formData.append('WorkerCode', formValues.passportNumber);
        formData.append('workerType', formValues.workerType);
        formData.append('Mobile', formValues.mobile);
        formData.append('Salary', formValues.salary.toString());
        formData.append('Price', formValues.price.toString());
        formData.append('BirthDate', this.formatDate(formValues.birthDate.toString()));
        formData.append('Gender', formValues.gender);
        formData.append('Nationality', formValues.nationality);
        formData.append('Religion', formValues.religion);
        formData.append('MaritalStatus', formValues.maritalStatus);
        formData.append('Languages', JSON.stringify(formValues.languages));
        formData.append('Weight', formValues.weight);
        formData.append('Height', formValues.height);
        formData.append('Education', formValues.education);
        formData.append('Video', formValues.video);
        formData.append('PassportNumber', formValues.passportNumber);
        formData.append('PassportIssDate', this.formatDate(formValues.passportIssDate.toString()));
        formData.append("PassportPoIssue", formValues.passportPOIssue);
        formData.append('PassportExpDate', this.formatDate(formValues.passportExpDate.toString()));
        formData.append('CivilId', formValues.civilId);
        let itemName: any[] = this.itemList.filter(nameValuePair => nameValuePair.value === formValues.code);
        formData.append('Name', itemName[0].name);
        formData.append('Code', formValues.code);
        formData.append('Hobbies', formValues.hobbies);
        formData.append('Location', formValues.location);
        formData.append('Experiences', JSON.stringify(formValues.experience));
    }
    DepricatedgetFormDataD(formData: FormData) {
        formData.append('WorkerName', this.state.worker.workerName);
        formData.append('WorkerCode', this.state.worker.passportNumber);
        formData.append('workerType', this.state.worker.workerType);
        formData.append('Mobile', this.state.worker.mobile);
        formData.append('Salary', this.state.worker.salary.toString());
        formData.append('Price', this.state.worker.price.toString());
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
        let itemName: any[] = this.itemList.filter(nameValuePair => nameValuePair.value === this.state.worker.code);
        formData.append('Name', itemName[0].name);
        formData.append('Code', this.state.worker.code);
    }


    formatDate(dateValue): string {
        return this.momentDatePipe.transform(dateValue);
    }

    createEmptyWorkerState() {
        let workerParams: any = Array.from({ length: 30 }, x => { return '' }) as any;
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

    getItemLookupsByType(workerType) {

        let workerTypeParam: WorkerTypeParam = { workerType: workerType };
        this.myApi.getItemsByWorkerType(workerTypeParam).subscribe(x => {
            this.itemList = x;
        });
    }

    //DELETE ME PLEASE !!!
    tempData() {
        this.worker = { "workerCode": "500901123", "workerName": "Test123", "serialNumber": "123456789106", "agent": "SA004", "mobile": "215554685", "age": 27, "name": "Maid - SriLanka", "code": "50090112", "birthDate": "5/6/1991", "gender": "M", "nationality": "3", "religion": "2", "maritalStatus": "1", "language": null, "photo": "https://dev.procons-scloud.com:443/api/Workers/Image?path=ben.d6d4e55c-8306-45fa-9e28-48e6ad3ef657.jpg", "license": "https://dev.procons-scloud.com:443/api/Workers/Image?path=ben.4c26c13b-d310-4ae4-a764-6682e8ffca14.jpg", "price": 550, "salary": 100, "weight": "60", "height": "190", "education": "3", "passport": "https://dev.procons-scloud.com:443/api/Workers/Image?path=ben.ef7ca69a-7f77-4780-89bb-c50067ab42ec.jpg", "video": "https://www.youtube.com/watch?v=H7DGigU3V2I", "passportNumber": "500901123", "passportIssDate": "5/6/1991 12:00:00 AM", "passportExpDate": "5/6/1991", "passportPoIssue": "Germany", "civilId": "123456789106", "status": "1", "workerType": "Maid", "languages": [{ "name": "الانكليزية", "value": "1" }], "hobbies": "", "location": "", "isNew": "N", "period": 0, "experiences": { "workerID": "50090112", "startDate": "5/6/2001 12:00:00 AM", "endDate": "5/6/2002 12:00:00 AM", "title": "Maid", "description": "Job Description", "companyName": "Procons" } };
    }

}

enum FileUploadMode {
    Add, Edit, Delete
}
