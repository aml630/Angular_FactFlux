import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/components/common/menuitem';

@Component({
  selector: 'app-admin-header',
  templateUrl: './admin-header.component.html',
  styleUrls: ['./admin-header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor() { }
  private items: MenuItem[];

  ngOnInit() {
    this.items = [
      { label: 'Home', icon: 'fa fa-fw fa-bar-chart', url: "/"},
      { label: 'Stories', icon: 'fa fa-fw fa-calendar', url: "/stories" },
      { label: 'Feeds', icon: 'fa fa-fw fa-book', url: "/rss-feeds" },
      { label: 'Words', icon: 'fa fa-fw fa-support', url: "/words" },
      { label: 'Parent Words', icon: 'fa fa-fw fa-twitter', url: "/word-parents" }
    ];
  }

  closeItem(event, index) {
    this.items = this.items.filter((item, i) => i !== index);
    event.preventDefault();
}

}
