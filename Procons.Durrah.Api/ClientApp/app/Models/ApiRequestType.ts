export interface KnetPayment {
    PaymentID: string,
    Postdate: any,
    Result: string,
    TranID: string,
    Auth: string
}
export interface PaymentRedirectParams{
    SerialNumber: string,
    CardCode: string,
    Amount: string,
    Code: string
}
export interface SearchCriteriaParams{
    languages: NameValuePair[],
    nationality: NameValuePair[],
    gender: NameValuePair[],
    martialStatus: NameValuePair[],
    workerTypes: NameValuePair[]
}

export interface NameValuePair{
    name: string
    value: string
}
export interface ResetPasswordParams{
    EmailAddress: string,
    ValidationId: string,
    Password: string
}