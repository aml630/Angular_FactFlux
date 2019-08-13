import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Word } from '../../models/words';

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

  UpdateWord(word: Word, main: boolean) {
    word.main = main;
    if (!word.description) {
      word.description = '';
    }
    this.http.put<Word[]>(this.base + 'api/Words/' + word.wordId, word).subscribe(result => {
    }, error => console.error(error));

    this.GetMainWords();
  }

  UploadHotLink(word: Word) {
    this.http.post(this.base + `api/Words/AddImage/Word/${word.wordId}?hotLink=${word.image}`, {
    }).subscribe(res => {
        console.log(res);
      });
  }
}
