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
  isbooked = false;
  constructor(private myModal: ProconsModalSerivce) { }

  ngOnInit() {
    
  }

  GoBack() {
    this.onBack.emit();
  }

  Book() {
    // this.myModal.showHTMLModal('<p><small>يرجى مراجعة المقر الرئيسي لشركة الدرة للعمالة&nbsp;لأختيار&nbsp;العامل / العاملة</small></p>'
    // +'<p><small>Please check the headquarters of Al-Durra for manpower Company to choose the worker / maid</small></p>');
    this.myModal.showHTMLModal('<p><small>يرجى الانتظار حتى اكتمال طلبك</small></p>'
    +'<p><small>Your order is in progress.please wait your order to be completed.</small></p>');
    this.isbooked = true;
    this.onBook.emit(true);
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
