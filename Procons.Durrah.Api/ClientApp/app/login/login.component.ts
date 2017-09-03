import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ApiService } from '../Services/ApiService';
import { ProconsModalSerivce } from '../Services/ProconsModalService';

declare var $;
@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']

})
export class LoginComponent implements OnInit {

    public loading: boolean = false;
    public isLoggedIn: boolean = false;
    public forgotPassModalLoading: boolean = false;
    public forgotPassModalError: string = "";
    public resetPassModalLoading: boolean = false;
    public showForgotPasswordModal = false;

    constructor(private myApiService: ApiService, public myModal: ProconsModalSerivce) { }
    ngOnInit() { }

    Login(userName: string, password: string) {
        console.log('Logging in with userName: ', userName, ' password: ', '####');
        this.loading = true;

        this.myApiService.login(userName, password).subscribe(isLoggedIn => {
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
        this.myApiService.forgotPassword(email).subscribe(isSuccesful => {
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
        this.resetPassModalLoading = true;
        console.log('Reset Password Unimplemented Called!', password, ' ', confirmPassword);
        setTimeout(() => { this.resetPassModalLoading = false }, 500);
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
