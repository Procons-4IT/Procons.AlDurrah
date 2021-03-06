import { SearchCriteriaParams, NameValuePair } from './ApiRequestType';


export class Worker {
    constructor(public workerName: string, public name: string,
        public birthDate: string,
        public gender: string,
        public nationality: string,
        public religion: string,
        public maritalStatus: string,
        public language: string,
        public languages: NameValuePair[],
        public photo: string,
        public weight: string,
        public height: string,
        public education: string,
        public video: string,
        public passportNumber: string,
        public passportIssDate: string,
        public passportPoIssue: string,
        public passportExpDate: string,
        public civilId: string,
        public yearsOfExperience:number,

        public serialNumber: string,
        public agent: string,
        public code: string,
        public workerCode: string,
        public passport: string,
        public license: string,
        public status: string,
        public passportCopy: any,

        public salary: number,
        public price: number,
        public workerType: any,
        public mobile: string,

        public hobbies: string,
        public location: string,
        public isNew: string,
        public period: boolean,
        public experiences?: Experience[]
    ) { }
}

export class WorkerManagementData {
    searchCriteria: SearchCriteriaParams;
    workerServerData: Worker[];
    workerDisplayData: Worker[];
    constructor() { }
}

export class Experience {
    title = ''
    description = ''
    companyName = ''
    country = ''
    location = ''
    startDate = ''
    endDate = ''
}

//Problem is that some views expect worker keys and othe expect worker values ? 
// a- create a function that can  convert from one state to the other
// b- have a new interface that contains all the different logic
// c- consider how much code needs to be changed ...