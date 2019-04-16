import { Component, OnInit } from '@angular/core';
import { RssFeed } from '../models/rssFeed';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  base: string = document.getElementsByTagName('base')[0].href;
  foundFeeds: RssFeed[];
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.GetFeeds();
  }

  GetFeeds() {
    this.http.get<RssFeed[]>(this.base + 'api/RssFeeds').subscribe(result => {
      this.foundFeeds = result;
    }, error => console.error(error));
  }
}
