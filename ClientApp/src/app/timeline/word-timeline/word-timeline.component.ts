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

  getImageForArticle(article: Article) {
    if (article.articleType === 1 || article.articleType === 2) {
      let feed = this.rssFeeds.filter(x => x.feedId == article.feedId)[0];
      return feed.feedImage
    }else {
      return "https://aquaprosprinklers.com/wp-content/uploads/2018/02/TWITTER.png";
    }
  }

  GetFeeds() {
    this.http.get<RssFeed[]>(this.base + 'api/RssFeeds').subscribe(result => {
      this.rssFeeds = result;
    }, error => console.error(error));
  }

}

