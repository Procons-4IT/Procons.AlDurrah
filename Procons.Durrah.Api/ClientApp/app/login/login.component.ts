import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { ResetPasswordParams, CreateNewUserParams } from '../Models/ApiRequestType';

import { ApiService } from '../Services/ApiService';
import { ProconsModalSerivce } from '../Services/ProconsModalService';
import { UtilityService } from '../Services/UtilityService';

declare var $;

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']

})
export class LoginComponent implements OnInit {

    public loading: boolean = false;
    public isLoggedIn: boolean = false;

    public showForgotPasswordModal = false;
    public forgotPassModalLoading: boolean = false;
    public forgotPassModalError: string = "";

    public resetPassModalLoading: boolean = false;
    public resetPassInputError: string = "";
    public resetPassModalText: string = "";


    public newUser: CreateNewUserParams = {
        firstName: "",
        lastName: "",
        userName: "",
        civilId: "",
        password: "",
        confirmPassword: "",
        email: ""
    };

    public resetParams: ResetPasswordParams = {
        EmailAddress: "",
        Password: "",
        ValidationId: ""
    };

    constructor(
        private myApi: ApiService
        , public myModal: ProconsModalSerivce
        , public activeRouter: ActivatedRoute
        , public utility: UtilityService) { }
    ngOnInit() {

        this.handlePasswordResetRoute();
    }

    handlePasswordResetRoute() {
        console.log('## Checking if PasswordResetRoute');

        this.activeRouter.data
            .filter((data, idx) => { return data.isPasswordReset; })
            .do(x => {
                $('#modalResetPass').modal('toggle');
            })
            .mergeMap(x => { return this.utility.getResetPasswordUrlProperties(this.activeRouter) })
            .subscribe(x => {
                console.log('Recieved ResetParams! ', x);
                this.resetParams.EmailAddress = x.Email;
                this.resetParams.ValidationId = x.ValidationId;
            }, onError => {
                this.resetPassModalText = 'Something Went Wrong!'
            });
    }


    Login(userName: string, password: string) {
        console.log('Logging in with userName: ', userName, ' password: ', '####');
        this.loading = true;

        this.myApi.login(userName, password).subscribe(isLoggedIn => {
            this.loading = false;
            this.OpenModal();
        }, (error) => {
            this.loading = false;
            this.myModal.showErrorModal();
        });

    }
    ForgotPassword(email: string) {
        console.log('### Sending ForgotPassword Request for email ', email);
        this.forgotPassModalError = "";
        this.forgotPassModalLoading = true;
        this.myApi.forgotPassword(email).subscribe(isSuccesful => {
            this.forgotPassModalLoading = false;
            console.log('I got a reply!');
            $('#myModal').modal('toggle')
            this.showForgotPasswordModal = false;


        }, onError => {
            this.forgotPassModalLoading = false;

            if (onError.status == '404') {
                this.forgotPassModalError = "Invalid Email";

            } else {
                this.myModal.showErrorModal();
            }
        })

    }
    ResetPassword(password: string, confirmPassword: string) {
        if (password !== confirmPassword) {
            this.resetPassInputError = "Error Passwords not matching";
        } else {
            this.resetPassModalLoading = true;
            console.log('Reset Password Unimplemented Called!', password, ' ', confirmPassword);
            this.resetParams.Password = password;
            console.log('Reset Password Unimplemented Called!', this.resetParams);

            this.myApi.resetPassword(this.resetParams)
                .subscribe(isReset => {
                    this.resetPassModalLoading = false;
                    if (isReset) {
                        console.log('## Password was Reset! ');
                    }
                }, onError => {
                    this.resetPassModalLoading = false;
                    this.resetPassModalText = 'Something Went Wrong!';
                });
        }
    }
    CreateUser() {
        console.log('### Create User Unimplemented Method! ', this.newUser);
        this.loading = true;
        this.myApi.createNewUser(this.newUser).subscribe(x => {
            this.loading = false;

        }, onError => {
            console.log(onError);
            this.loading = false;
            this.myModal.showErrorModal();
        });
    }
    OpenModal() {
        let html = `
            <div class="modal-body">
                <div class="modal-icon"><img src="/Assets/src/app/images/icon_lock.png" class="icon" /></div>
                <p><small>لقد سجلت الدخول</small></p>
                <h4>إنشاء كلمة المرور</h4>
                <a href="#tabNewPass" data-toggle="tab" data-dismiss="modal">واصل</a>
            </div> `

        this.myModal.showHTMLModal(html);
    }
}
