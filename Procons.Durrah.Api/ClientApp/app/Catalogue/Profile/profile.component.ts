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
      window.location.href = "http://knet.testyourprojects.co.in/";
  }

  ngOnInit() {
  }

}
