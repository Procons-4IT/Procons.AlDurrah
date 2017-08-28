"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ComponentBase = (function () {
    function ComponentBase() {
        this.header = new Array();
        this.baseObject = null;
        this.baseObjects = null;
    }
    ComponentBase.prototype.NewBaseObject = function (value) {
        if (value === null) {
            this.newBaseObject = value;
        }
        return this.newBaseObject;
    };
    ComponentBase.prototype.SelectedBaseObject = function (value) {
        if (value === null) {
            this.selectedBaseObject = value;
        }
        return this.selectedBaseObject;
    };
    ComponentBase.prototype.DisplayDialog = function (value) {
        if (value === null) {
            this.displayDialog = value;
        }
        return this.displayDialog;
    };
    ComponentBase.prototype.BaseObject = function (value) {
        if (value === null) {
            this.baseObject = value;
        }
        return this.baseObject;
    };
    ComponentBase.prototype.BaseObjects = function (value) {
        if (value === null) {
            this.baseObjects = value;
        }
        return this.baseObjects;
    };
    ComponentBase.prototype.addHeader = function (name, value) {
        return this.header.push(name, value);
    };
    ComponentBase.prototype.CloneObject = function (par, c) {
        var object = new c();
        for (var prop in par) {
            object[prop] = par[prop];
        }
        return object;
    };
    ComponentBase.prototype.ShowDialogToAdd = function (isNewObject, objectInstance, displayDialog) {
        isNewObject = true;
        objectInstance = null; // new T();
        displayDialog = true;
    };
    ComponentBase.prototype.FindSelectedIndex = function (baseInstances, baseInstance) {
        return baseInstances.indexOf(baseInstance);
    };
    ComponentBase.prototype.DisplayErrorMessage = function (error) {
        var errorMessage = "";
        for (var key in error.json().modelState) {
            if (error.json().modelState.hasOwnProperty(key)) {
                errorMessage = errorMessage + "<p>" + error.json().modelState[key] + "<p>\n";
            }
        }
        return errorMessage;
    };
    return ComponentBase;
}());
exports.ComponentBase = ComponentBase;
//# sourceMappingURL=app.ComponentBase.js.map