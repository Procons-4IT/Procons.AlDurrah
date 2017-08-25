export class ComponentBase<T>  {

    public header: Array<any> = new Array<any>();
    public displayDialog: boolean;
    baseObject: T = null;
    selectedBaseObject: T;
    newBaseObject: boolean;
    baseObjects: T[] = null;

    NewBaseObject(value?: boolean): boolean {
        if (value === null) {
            this.newBaseObject = value;
        }
        return this.newBaseObject;
    }

    SelectedBaseObject(value?: T): T {
        if (value === null) {
            this.selectedBaseObject = value;
        }
        return this.selectedBaseObject;
    }

    DisplayDialog(value?: boolean): boolean {
        if (value === null) {
            this.displayDialog = value;
        }
        return this.displayDialog;
    }

    BaseObject(value?: T): T {
        if (value === null) {
            this.baseObject = value;
        }
        return this.baseObject;
    }

    BaseObjects(value?: T[]): T[] {
        if (value === null) {
            this.baseObjects = value;
        }
        return this.baseObjects;
    }

    constructor() {
    }

    public addHeader(name: string, value: string): any {
        return this.header.push(name, value);
    }

    CloneObject<T>(par: T, c: { new (): T; }): T {
         
        let object: T = new c();
        for (let prop in par) {
            object[prop] = par[prop];
        }
        return object;
    }

    ShowDialogToAdd(isNewObject: boolean, objectInstance: T, displayDialog: boolean) {
         
        isNewObject = true;
        objectInstance = null;// new T();
        displayDialog = true;
    }



    FindSelectedIndex(baseInstances: T[], baseInstance: T): number {
        return baseInstances.indexOf(baseInstance);
    }

    DisplayErrorMessage(error: any): string {
        let errorMessage: string = "";
        for (var key in error.json().modelState) {
            if (error.json().modelState.hasOwnProperty(key)) {
                errorMessage = errorMessage + "<p>" + error.json().modelState[key] + "<p>\n";
            }
        }
        return errorMessage;
    }
}