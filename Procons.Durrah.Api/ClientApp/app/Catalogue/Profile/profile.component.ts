import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
@Component({
    selector: '[app-profile]',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class profileComponent implements OnInit {
   public items: MenuItem[];
   
  constructor() { }
  public Book() {
    console.log('Attempting to Book profileComponent');
  }

  ngOnInit() {
  }

}
