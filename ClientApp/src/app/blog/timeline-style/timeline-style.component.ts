import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-timeline-style',
  templateUrl: './timeline-style.component.html',
  styleUrls: ['./timeline-style.component.css']
})
export class TimelineStyleComponent implements OnInit {

  constructor() { }
  data: any;
  ngOnInit() {
    this.data = {
      labels: ['January', 'February', 'March'],
      datasets: [
        {
          data: [300, 50, 100],
          backgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56"
          ],
          hoverBackgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56"
          ]
        }]
    };
  }
}
