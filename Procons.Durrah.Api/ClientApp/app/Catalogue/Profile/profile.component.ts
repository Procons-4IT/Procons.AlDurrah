import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { Worker,Experience } from '../../Models/Worker';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class profileComponent implements OnInit {

  @Input() worker: Worker
  @Output() onBook = new EventEmitter<Boolean>();
  @Output() onBack = new EventEmitter<any>();

  constructor() { }

  ngOnInit() {
    
  }

  GoBack() {
    this.onBack.emit();
  }

  Book() {
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
