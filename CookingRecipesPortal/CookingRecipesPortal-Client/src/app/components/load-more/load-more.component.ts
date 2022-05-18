import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-load-more',
  templateUrl: './load-more.component.html',
  styleUrls: ['./load-more.component.css']
})
export class LoadMoreComponent {

  @Output()
  loadMore = new EventEmitter<number>();

  @Input()
  pageNumber!: number;

  @Input()
  totalPages!: number;

  loadMoreElements(): void {
    this.loadMore.emit(++this.pageNumber);
  }
}
