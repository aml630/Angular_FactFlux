import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-post-event',
  templateUrl: './post-event.component.html',
  styleUrls: ['./post-event.component.css']
})
export class PostEventComponent implements OnInit {
  @Input() left: boolean;
  @Input() text: string;
  @Input() date: string;

  constructor() { }

  ngOnInit() {
    console.log(this.text);
  }

}
