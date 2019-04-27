import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Article } from '../../models/article';
import { RssFeed } from '../../models/rssFeed';
import { FormControl } from '@angular/forms';
import "rxjs/add/operator/debounceTime";


@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.css']
})
export class TimelineComponent implements OnInit {

  word: string;
  titleWord: string;
  base: string = document.getElementsByTagName('base')[0].href;
  articles: Article[];
  rssFeeds: RssFeed[];
  articleTypes = [1, 2, 3];
  politicalSpectrum = [5];
  filterLetters: FormControl;
  currentLetters: string;
  pageSize = 20;
  showSpinner = true;

  constructor(private activatedRoute: ActivatedRoute, private http: HttpClient) { }

  ngOnInit() {
    this.filterLetters = new FormControl();
    this.activatedRoute.params.subscribe(params => {
      this.word = params['word'];

      this.titleWord = this.word.replace(/-/g, ' ');
      this.titleWord = this.toTitleCase(this.titleWord);

      this.filterLetters.valueChanges.debounceTime(400).subscribe(x => {
        this.currentLetters = x;
        this.GetTimelineContent();
      });

      this.GetTimelineContent();

      this.ClearTwitter();
    });
  }

  toTitleCase(str) {
    return str.replace(/\w\S*/g, function (txt) {
      return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
    });
  }

  GetTimelineContent() {
    this.showSpinner = true;
    let path = `api/Articles/timeline/${this.word}?pageSize=${this.pageSize}`;

    if (this.articleTypes.length > 0) {
      path += `&articleTypes=`;
      for (const type in this.articleTypes) {
        path += `${this.articleTypes[type]}|`
      }
      path = path.substring(0, path.length - 1);
    }

    if (this.politicalSpectrum.length > 0) {
      path += `&politicalSpectrum=`;
      for (const spectrumRank in this.politicalSpectrum) {
        path += `${this.politicalSpectrum[spectrumRank]}|`
      }
      path = path.substring(0, path.length - 1);
    }

    if (this.currentLetters) {
      path += `&letterFilter=${this.currentLetters}`;
    }

    this.http.get<Article[]>(this.base + path).subscribe(result => {
      this.articles = result;
      this.showSpinner = false;
    }, error => console.error(error));

    this.ClearTwitter();
  }

  private ClearTwitter() {
    if (document.getElementById("twitter-wjs")) {
      document.getElementById("twitter-wjs").outerHTML = "";
    }
  }

  toggleType(articleType: number) {
    let doesContain = this.articleTypes.indexOf(articleType);

    if (doesContain === -1) {
      this.articleTypes.push(articleType);
    } else {
      this.articleTypes.splice(doesContain, 1);
    }

    this.GetTimelineContent();
  }

  toggleSpectrum(politicalSpectrumRank: number) {
    const doesContain = this.politicalSpectrum.indexOf(politicalSpectrumRank);

    if (doesContain === -1) {
      this.politicalSpectrum = [politicalSpectrumRank];
    }

    this.GetTimelineContent();
  }

  getNextPage() {
    this.pageSize += 20;
    this.GetTimelineContent();
  }
}

