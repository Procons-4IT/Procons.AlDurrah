import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
@Component({
    selector: '[app-SearchResults]',
  templateUrl: './SearchResults.component.html',
  styleUrls: ['./SearchResults.component.css']
})
export class SearchResultsComponent implements OnInit {
   public items: MenuItem[];
   
  constructor() { }
  public Book() {

      console.log('SearchResultsComponent Book!');
      // window.location.href = "http://knet.testyourprojects.co.in/";
  }

  ngOnInit() {
  }

}
