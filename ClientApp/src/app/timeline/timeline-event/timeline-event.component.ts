import { Component, OnInit, Input } from '@angular/core';
import { Article } from '../../article';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-timeline-event',
  templateUrl: './timeline-event.component.html',
  styleUrls: ['./timeline-event.component.css']
})
export class TimelineEventComponent implements OnInit {

  @Input() article: Article
  @Input() index: number
  @Input() feedImage: string
  
  constructor(private sanitizer: DomSanitizer) {
  }

  ngOnInit() {
  }

  vidUrl(url: string) {
    let urlString = 'https://www.youtube.com/embed/'+url;
    return this.sanitizer.bypassSecurityTrustUrl(urlString);
  }

}
