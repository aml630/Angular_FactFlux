import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Article } from '../../models/article';
import { RssFeed } from '../../models/rssFeed';
import { FormControl } from '@angular/forms';
import "rxjs/add/operator/debounceTime";
import { Images } from '../../models/images';
import { DateCounts } from 'src/app/models/dateCounts';


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
  dateCounts: DateCounts[];
  dateCountOccuranceAverage: number;
  largestOccuranceCount: number;
  startDateFilter: Date;
  endDateFilter: Date;
  selectedDateFilter: DateCounts;
  rssFeeds: RssFeed[];
  imageLocation: string;
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
    });

    this.filterLetters.valueChanges.debounceTime(400).subscribe(x => {
      this.currentLetters = x;
      this.GetTimelineContent();
    });

    this.GetTimelineContent();
    this.GetImage();
    this.ClearTwitter();
    this.GetDateCounts();
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

      this.articleTypes.forEach((type, i) => {
        path += `${this.articleTypes[i]}|`;
      });
      path = path.substring(0, path.length - 1);
    }

    if (this.politicalSpectrum.length > 0) {
      path += `&politicalSpectrum=`;

      this.politicalSpectrum.forEach((obj, i) => {
        path += `${this.politicalSpectrum[i]}|`;
      });

      path = path.substring(0, path.length - 1);
    }

    if (this.currentLetters) {
      path += `&letterFilter=${this.currentLetters}`;
    }

    if (this.selectedDateFilter) {
      path += `&startDate=${this.selectedDateFilter.startDate}`;
      path += `&endDate=${this.selectedDateFilter.endDate}`;
    }

    this.http.get<Article[]>(this.base + path).subscribe(result => {
      this.articles = result;
      this.showSpinner = false;
    }, error => console.error(error));

    this.ClearTwitter();
  }

  GetImage() {
    this.showSpinner = true;
    const path = `api/Images/${this.word}`;
    this.http.get<Images[]>(this.base + path).subscribe(result => {
      if (result[0]) {
        this.imageLocation = result[0].imageLocation;
        console.log(this.imageLocation);
      }
    }, error => console.error(error));
  }

  GetDateCounts() {
    this.showSpinner = true;
    const path = `api/Articles/GetDateCount/${this.word}`;
    this.http.get<DateCounts[]>(this.base + path).subscribe(result => {
      this.dateCounts = result;
      this.dateCountOccuranceAverage = (result.reduce((a, b) => a + (b['occuranceCount'] || 0), 0)) / result.length;
      const resultHolder = result;
      this.largestOccuranceCount = resultHolder
        .sort((a, b) => a.occuranceCount < b.occuranceCount ? -1 : a.occuranceCount > b.occuranceCount ? 1 : 0)
        [resultHolder.length - 1].occuranceCount;

    }, error => console.error(error));
  }

  private ClearTwitter() {
    if (document.getElementById('twitter-wjs')) {
      document.getElementById('twitter-wjs').outerHTML = '';
    }
  }

  toggleType(articleType: number) {
    const doesContain = this.articleTypes.indexOf(articleType);

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

  toggleDate(dateCount: DateCounts) {

    if (this.selectedDateFilter === dateCount) {
      this.selectedDateFilter = undefined;
    } else {
      this.selectedDateFilter = dateCount;
    }

    this.GetTimelineContent();
  }

  getNextPage() {
    this.pageSize += 20;
    this.GetTimelineContent();
  }
}

