import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'momentDate'
})
export class MomentDatePipe implements PipeTransform {

  transform(value: any, args?: any): any {
    // return moment(value).format("MM-DD-YYYY");
    return moment(value).format("DD-MM-YYYY");

  }

}
