import { Component, OnInit } from '@angular/core';
import { Word } from '../words';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-main-words',
  templateUrl: './main-words.component.html',
  styleUrls: ['./main-words.component.css']
})
export class MainWordsComponent implements OnInit {
  base: string = document.getElementsByTagName('base')[0].href;
  mainWords: Word[];
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.GetMainWords();
  }

  GetMainWords() {
    this.http.get<Word[]>(this.base + `api/Words/GetMain`)
      .subscribe(result => {
        this.mainWords = result;
      }, error => console.error(error));
  }
}
