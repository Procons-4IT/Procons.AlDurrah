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
    workerType: NameValuePair[]
}

export interface NameValuePair{
    name: string
    value: string
}