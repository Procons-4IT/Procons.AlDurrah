import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Worker } from '../../Models/Worker';
import { ProconsModalSerivce } from '../../Services/ProconsModalService';
import { MenuItem } from 'primeng/primeng';

@Component({
  selector: 'search-result',
  templateUrl: './SearchResults.component.html',
  styleUrls: ['./SearchResults.component.css']
})
export class SearchResultsComponent implements OnInit {
  @Input() workers: Worker[];
  @Output() onSelectedWorker = new EventEmitter<Worker>();
  @Output() onBack = new EventEmitter<any>();

  public showVideoModal: boolean = false;
  showSearchForm = false;

  public videoUrl;
  public completeListOfWorkers;

  constructor(public sanitizer: DomSanitizer, public myModal: ProconsModalSerivce) { }

  ngOnInit() {
    this.completeListOfWorkers = Object.assign([], this.workers);

  }
  GoBack() {
    this.onBack.emit();
  }
  GoToProfile(selectedWorker: Worker) {

    this.onSelectedWorker.emit(selectedWorker);
  }
  GetAvailableCSS(worker: Worker) {
    var isAvaible = worker.status == "1" ? true : false;
    return {
      "glyphicon": true,
      "glyphicon-ok": isAvaible,
      "glyphicon-remove": !isAvaible
    };
  }
  public openRequestedPopup(url) {

    this.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    this.showVideoModal = true;
  }

  //If you want to add more than one searchField than traverse the entire inputs and reset if they are all empty
  //If one is empty of many then don't apply filter
  filterByWorkerName(workerName) {
    if (workerName) {
      this.workers = this.completeListOfWorkers.filter(worker => { return worker["workerName"].toLowerCase().includes(workerName.toLowerCase()); });
    } else {
      this.workers = this.completeListOfWorkers;
    }
  }

  toggleAdvanced() {
    this.showSearchForm = !this.showSearchForm;
  }

}
