import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RssFeed } from '../rssFeed';

@Component({
  selector: 'app-rss-feeds',
  templateUrl: './rss-feeds.component.html',
  styleUrls: ['./rss-feeds.component.css']
})
export class RssFeedsComponent implements OnInit {

  foundFeeds: RssFeed[];
  foundArticles: Article[];
  form: FormGroup;
  showEdit: boolean = false;
  base: string = document.getElementsByTagName('base')[0].href;

  constructor(private formBuilder: FormBuilder, private http: HttpClient) { }

  ngOnInit() {

    this.GetFeeds()

    this.form = this.formBuilder.group({
      feedTitle: ['', Validators.required],
      feedLink: ['', Validators.required],
      feedImage: [],
      videoLink: [],
      feedId: []
    });
  }

  HideEdit() {
    this.showEdit = false;
  }

  FillEdit(feed: RssFeed) {
    this.showEdit = true;
    this.form.patchValue(feed);
  }

  GetFeeds() {
    this.http.get<RssFeed[]>(this.base + 'api/RssFeeds').subscribe(result => {
      this.foundFeeds = result;
    }, error => console.error(error));
  }

  CreateFeed(feed: RssFeed) {
    this.http.post<RssFeed>(this.base + 'api/RssFeeds', feed).subscribe(result => {
      this.foundFeeds.push(result);
    }, error => console.error(error));
  }

  UpdateFeed(feed: RssFeed) {
    this.http.put<RssFeed>(this.base + 'api/RssFeeds/' + feed.feedId, feed).subscribe(result => {
    }, error => console.error(error));

    this.GetFeeds();
  }

  DeleteFeed(feedId: number) {
    this.http.delete<RssFeed>(this.base + 'api/RssFeeds/' + feedId).subscribe(result => {
      this.GetFeeds();
    }, error => console.error(error));
  }

  GetArticles(feedId: number) {
    this.http.post<Article[]>(this.base + 'api/RssFeeds/' + feedId + '/GetArticles', null).subscribe(result => {
      this.foundArticles = result;
      console.log(this.foundArticles)
    }, error => console.error(error));
  }

  GetAllArticles() {
    this.http.post<Article[]>(this.base + 'api/RssFeeds/GetAllArticles', null).subscribe(result => {
      this.foundArticles = result;
      console.log(this.foundArticles)
    }, error => console.error(error));
  }
}

interface Article {
  ArticleTitle: string;
}
