import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Article } from '../../article';
import { RssFeed } from '../../rssFeed';

@Component({
  selector: 'app-word-timeline',
  templateUrl: './word-timeline.component.html',
  styleUrls: ['./word-timeline.component.css']
})
export class WordTimelineComponent implements OnInit {

  word: string;
  base: string = document.getElementsByTagName('base')[0].href;
  articles: Article[];
  rssFeeds: RssFeed[];


  constructor(private activatedRoute: ActivatedRoute, private http: HttpClient) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      this.word = params['word'];

      this.http.get<Article[]>(this.base + `api/Articles/timeline/${this.word}`).subscribe(result => {
        this.articles = result;
        console.log(this.articles);
      }, error => console.error(error));

      this.GetFeeds();

    });
  }

  getImageForArticle(id: number) {
    let feed = this.rssFeeds.filter(x => x.feedId == id)[0];
    return feed.feedImage
  }

  GetFeeds() {
    this.http.get<RssFeed[]>(this.base + 'api/RssFeeds').subscribe(result => {
      this.rssFeeds = result;
    }, error => console.error(error));
  }

}

