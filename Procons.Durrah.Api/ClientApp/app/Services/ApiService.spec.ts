/* Warning! This makes actual HTTP Calls to the server! 
    Author: Ryan Zebian
*/
import { TestBed, inject, async } from '@angular/core/testing';
import { Http, HttpModule } from '@angular/http';
import { ApiService } from './ApiService'
import { ConfirmEmailParams, CreateNewUserParams, ResetPasswordParams, PaymentRedirectParams, KnetPayment } from '../Models/ApiRequestType';
import { WorkerManagementData } from '../Models/Worker';

describe('MyApiService', () => {
    let myApi: ApiService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpModule],
            providers: [ApiService]
        });
    });
    beforeEach(
        async(
            inject([ApiService], (service: ApiService) => {
                // service.language = 'en-US';
                this.myApi = service;
                let user = { userName: 'MainAgent', password: '12345' };
                service.login(user.userName, user.password).subscribe(isLoggedin => {
                    console.log('logged in with', user, isLoggedin);
                }, error => {
                    throw error;
                });
            })
        ), 10000);

    it('should ...', inject([ApiService], (service: ApiService) => {
        expect(service).toBeTruthy();
    }));

    it('get SearchCriteria ', async(inject([ApiService], (service: ApiService) => {
        service.getSearchCriteriaParameters().subscribe(criteriaParams => {

            expect(criteriaParams).toBeTruthy();
            expect(criteriaParams.languages.length).toBeGreaterThan(0);
        })
    })), 10000);

    //add getWorkers with filter Params
    it('get Workers ', async(inject([ApiService], (service: ApiService) => {
        service.getAllWorkers({}).subscribe(workers => {

            expect(workers).toBeTruthy();
            expect(workers.length).toBeGreaterThan(0);
        })
    })));

    it('call  knetPortal', async(inject([ApiService], (service: ApiService) => {
        let paymentInformation: PaymentRedirectParams = {
            Amount: "100",
            CardCode: "C100",
            Code: "Driver",
            SerialNumber: "322"
        };

        service.knetPaymentRedirectUrl(paymentInformation).subscribe(redirectUrl => {

            expect(redirectUrl).toBeTruthy();
            expect(typeof redirectUrl).toEqual(typeof '');
        });
    })));
    it('create  incomingPayment', async(inject([ApiService], (service: ApiService) => {
        let knetPayment: KnetPayment = {
            Auth: "SomeAuth",
            PaymentID: "someId",
            Postdate: "someDate",
            Result: "SomeResult",
            TranID: "ID",
            Ref: "SomeRef",
            TrackID: "SomeTrack"
        };

        service.createIncomingPayment(knetPayment).subscribe(url => {

            expect(url).toBeTruthy();
            expect(typeof url).toEqual(typeof '');
        }, onError => {
            throw onError;
            // 
            // fail();
        });
    })), 30000);
    it('login ', async(inject([ApiService], (service: ApiService) => {
        let userName = "Rami";
        let password = "1234";

        service.login(userName, password).subscribe(isLoggedIn => {

            expect(isLoggedIn).toEqual(true);
        });
    })));

    it('forgotPassword ', async(inject([ApiService], (service: ApiService) => {
        let email = "someEmail@gmail.com";
        service.forgotPassword(email).subscribe(emailResponse => {

            expect(emailResponse).toBeTruthy;
        },
            onError => {
                throw onError;
                // if (onError.status == '403') {
                //     

                // } else {
                //     fail();
                // }
            });
    })), 30000);

    it('resetPassword ', async(inject([ApiService], (service: ApiService) => {
        let resetParam: ResetPasswordParams = { "ValidationId": "3PGO8E9S6X0XCR3", "EmailAddress": "houssam.saghir@procons-4it.com", "Password": "12345" };
        service.resetPassword(resetParam).subscribe(response => {

            expect(response).toBeTruthy;
        },
            onError => {
                throw onError;
                // 
                // fail();
            });
    })), 30000);

    it('createNewUser ', async(inject([ApiService], (service: ApiService) => {
        let param: CreateNewUserParams = { "Email": "houssam.saghir1234@procons-4it.com", "UserName": "Rami1234", "Password": "1234", "FirstName": "Rami", "LastName": "Chalhoob", "CivilId": "1234", "Mobile": "12345678" };

        service.createNewUser(param).subscribe(response => {

            expect(response).toBeTruthy;
        },
            onError => {
                throw onError;
                // 
                // fail();
            });
    })), 30000);


    it('confirmEmail ', async(inject([ApiService], (service: ApiService) => {
        let param: ConfirmEmailParams = { "ValidationId": "F2C2HCOO677AM5T", "Email": "houssam.saghir@procons-4it.com" };

        service.confirmEmail(param).subscribe(response => {

            //
            expect(response).toBeTruthy;
        },
            onError => {
                throw onError;
                // 
                // fail();
            });
    })));

    fit('workerManagmentData ', done => {
        console.log('starting workerManagment Test');

        this.myApi.getWorkerManagmentData().subscribe(response => {
            console.log('allworkers', response);
            expect(response).toBeTruthy;
            done();
        },
            onError => {
                console.log('somethingElse')
                throw onError;
            });
    }, 100000);




});
