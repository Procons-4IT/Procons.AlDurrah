//https://juristr.com/blog/2016/11/ng2-template-driven-form-validators/
import { Directive, Input } from '@angular/core';
import { AbstractControl } from '@angular/forms'
import { NG_VALIDATORS } from '@angular/forms/src/validators';
import { Validator, MinLengthValidator } from '@angular/forms/src/directives/validators';


@Directive({
  selector: '[appMobileValidator]',
  providers: [{ provide: NG_VALIDATORS, useExisting: MinLengthValidator, multi: true }]
})
export class MobileValidatorDirective implements Validator {

  @Input() minLength: number;

  validate(control: AbstractControl): { [key: string]: any; } {
    return null;
  }
  constructor() { }

}
