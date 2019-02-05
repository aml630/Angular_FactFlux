import { Component, OnInit, Input } from '@angular/core';
import { Article } from '../../article';

@Component({
  selector: 'app-timeline-event',
  templateUrl: './timeline-event.component.html',
  styleUrls: ['./timeline-event.component.css']
})
export class TimelineEventComponent implements OnInit {

  @Input() article: Article
  @Input() index: number
  @Input() feedImage: string
  
  constructor() {
  }

  ngOnInit() {
  }

}
