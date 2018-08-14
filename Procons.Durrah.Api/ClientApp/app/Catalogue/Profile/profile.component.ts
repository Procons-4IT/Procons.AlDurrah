import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { Worker,Experience } from '../../Models/Worker';
import { ProconsModalSerivce } from '../../Services/ProconsModalService';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  providers: [ProconsModalSerivce]
})
export class profileComponent implements OnInit {

  @Input() worker: Worker
  @Output() onBook = new EventEmitter<Boolean>();
  @Output() onBack = new EventEmitter<any>();

  constructor(private myModal: ProconsModalSerivce) { }

  ngOnInit() {
    
  }

  GoBack() {
    this.onBack.emit();
  }

  Book() {
    this.myModal.showHTMLModal('<p><small>يرجى مراجعة المقر الرئيسي لشركة الدرة للعمالة&nbsp;لأختيار&nbsp;العامل / العاملة</small></p>'
    +'<p><small>Please check the headquarters of Al-Durra for manpower Company to choose the worker / maid</small></p>');
    //this.onBook.emit(true);
  }
  transformXptoArray(xp): Experience[] {
    if (xp) {
        if (Array.isArray(xp))
            return xp;
        else {
            return [xp];
        }
    } else {
        return xp;
    }
}
}
