import { OnInit } from '@angular/core';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-image-carousel',
  templateUrl: './image-carousel.component.html',
  styleUrls: ['./image-carousel.component.css']
})
export class ImageCarouselComponent implements OnInit {

  @Input()
  images!: any[];

  base64Images: string[] = [];

  shouldHideArrows = false;

  ngOnInit(): void {
    if (!this.images.length || this.images.length == 1) {
      this.shouldHideArrows = true;
    }

    this.images.forEach(x => {
      this.base64Images.push("data:image/png;base64," + x);
    });
  }
}
