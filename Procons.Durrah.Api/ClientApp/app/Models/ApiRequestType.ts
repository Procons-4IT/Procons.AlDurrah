export interface KnetPayment {
    PaymentID: string,
    Postdate: any,
    Result: string,
    TranID: string,
    Auth: string,
    Ref: string,
    TrackID: string
}
export interface PaymentRedirectParams {
    SerialNumber: string,
    CardCode: string,
    Amount: string,
    Code: string
}
export interface SearchCriteriaParams {
    languages: NameValuePair[],
    age: NameValuePair[],
    nationality: NameValuePair[],
    gender: NameValuePair[],
    maritalStatus: NameValuePair[],
    workerTypes: NameValuePair[],
    education: NameValuePair[],
    religion: NameValuePair[],
    location: string,
    hobbies: string,
    isNew: string,
    period: number,
    country: NameValuePair[],
    yearsOfExperience: number
}

export interface NameValuePair {
    name: string,
    value: string
}
export interface ResetPasswordParams {
    EmailAddress: string,
    ValidationId: string,
    Password: string
}
export interface WorkerFilterParams {
    languages: NameValuePair[],
    nationality: NameValuePair[],
    gender: NameValuePair[],
    maritalStatus: NameValuePair[],
    workerTypes: NameValuePair[]
}
export interface CreateNewUserParams {
    FirstName: string,
    LastName: string,
    UserName: string,
    CivilId: string,
    Password: string,
    Email: string,
    Mobile: string,
    "g-recaptcha-response"?: string
}

export interface ConfirmEmailParams {
    ValidationId: string,
    Email: string
}

export interface WorkerTypeParam {
    workerType: string
}