import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-recipe-item',
  templateUrl: './recipe-item.component.html',
  styleUrls: ['./recipe-item.component.css']
})
export class RecipeItemComponent implements OnInit {
  @ViewChild("testReference") testReference: ElementRef;

  constructor() {
    debugger;

  }
  CatchEvent(houssam: any) {
    debugger;
    // let $ = require('../ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js');
    // $.alert('ok');
    // let par = this.testReference.nativeElement.value;
  }
  ngOnInit() {
  }

}
