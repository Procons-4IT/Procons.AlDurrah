import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'languageConvert'
})
export class LanguageConvertPipe implements PipeTransform {

  transform(value: any, args?: any): any {
        if (value && value.length) {
            return value.map(x => x.name).reduce((acc, cv) => {return  acc + ', ' + cv });
        } else {
            return '';
        }

  }

}
