import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Article } from '../../article';
import { RssFeed } from '../../rssFeed';
import { FormControl } from '@angular/forms';


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
  articleTypes = [1, 2, 3];
  filterLetters: FormControl;


  constructor(private activatedRoute: ActivatedRoute, private http: HttpClient) { }

  ngOnInit() {
    this.filterLetters = new FormControl();
    this.activatedRoute.params.subscribe(params => {
      this.word = params['word'];

      this.filterLetters.valueChanges.debounceTime(400).subscribe(x => {
        this.GetArticlesVidsTweets(x);
      })

      this.GetArticlesVidsTweets(null);
      this.GetFeeds();
    });
  }

  GetArticlesVidsTweets(typedStuff: string) {

    let path = `api/Articles/timeline/${this.word}?`;

    for (let type in this.articleTypes) {
      path += `articleTypes=${this.articleTypes[type]}&`
    }
    path = path.substring(0, path.length - 1);

    if (typedStuff !== null) {
      path += `&letterFilter=${typedStuff}`
    }

    this.http.get<Article[]>(this.base + path).subscribe(result => {
      this.articles = result;
    }, error => console.error(error));
  }


  toggleType(articleType: number) {
    let doesContain = this.articleTypes.indexOf(articleType);

    if (doesContain == -1) {
      this.articleTypes.push(articleType);
    } else {
      this.articleTypes.splice(doesContain, 1);
    }

    this.GetArticlesVidsTweets(null);
  }

  getImageForArticle(article: Article) {
    if (article.articleType === 1 || article.articleType === 2) {
      let feed = this.rssFeeds.filter(x => x.feedId == article.feedId)[0];
      return feed.feedImage
    } else {
      return "https://aquaprosprinklers.com/wp-content/uploads/2018/02/TWITTER.png";
    }
  }

  GetFeeds() {
    this.http.get<RssFeed[]>(this.base + 'api/RssFeeds').subscribe(result => {
      this.rssFeeds = result;
    }, error => console.error(error));
  }

}

