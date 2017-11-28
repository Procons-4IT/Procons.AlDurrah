"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const Worker_1 = require("../../Models/Worker");
let AddProfileComponent = class AddProfileComponent {
    constructor(myApi) {
        this.myApi = myApi;
        this.onBack = new core_1.EventEmitter();
        this.state = {
            isAddMode: true,
            title: "Add Profile",
            worker: null
        };
        this.print = JSON.stringify;
    }
    ngOnInit() {
        if (this.worker) {
            this.state.isAddMode = false;
            this.state.title = "تعديل الملف";
            this.state.worker = Object.assign({}, this.worker);
        }
        else {
            let workerParams = Array.from({ length: 22 }, x => { return ''; });
            this.state.worker = new Worker_1.Worker(...workerParams);
        }
    }
    back() {
        this.onBack.emit();
    }
    addWorker(photoInput, passInput) {
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
                console.log('Somethign Happend ! ', formData);
            }, onError => {
            });
        }
        else {
            alert('TEMP Error Message: Photo or Passport Files are missing!');
        }
    }
    originalUploadFile(elFiles) {
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
                console.log('Somethign Happend ! ', formData);
            }, onError => {
            });
        }
    }
};
__decorate([
    core_1.Input()
], AddProfileComponent.prototype, "worker", void 0);
__decorate([
    core_1.Output()
], AddProfileComponent.prototype, "onBack", void 0);
AddProfileComponent = __decorate([
    core_1.Component({
        selector: "add-profile",
        templateUrl: "./add-profile.component.html",
        styles: ["./add-profile.component.css"]
    })
], AddProfileComponent);
exports.AddProfileComponent = AddProfileComponent;
//# sourceMappingURL=add-profile.component.js.map