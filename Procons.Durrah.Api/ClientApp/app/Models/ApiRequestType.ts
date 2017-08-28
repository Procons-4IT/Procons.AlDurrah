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