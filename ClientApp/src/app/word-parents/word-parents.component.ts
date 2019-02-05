import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Word } from '../words';
import { Subscription } from 'rxjs/Subscription';
import { FormControl } from '@angular/forms';
import "rxjs/add/operator/debounceTime";


@Component({
  selector: 'app-word-parents',
  templateUrl: './word-parents.component.html',
  styleUrls: ['./word-parents.component.css']
})
export class WordParentsComponent implements OnInit {
  base: string = document.getElementsByTagName('base')[0].href;
  wordParents: WordParent[];
  parentWordList: Word[];
  childWordList: Word[];
  parentLetters: string;
  childLetters: string;
  searches: string[] = [];
  parentTyping: FormControl;
  childTyping: FormControl;

  parentWord: string;
  childWord: string;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.parentTyping = new FormControl();
    this.parentTyping.valueChanges.debounceTime(400).subscribe(x => {
      this.parentLetters = x;
      this.parentWordList = [];
      this.GetWords(this.parentLetters, 1);
    })

    this.childTyping = new FormControl();
    this.childTyping.valueChanges.debounceTime(400).subscribe(x => {
      this.childLetters = x;
      this.childWordList = [];
      this.GetWords(this.childLetters, 2);
    })

    this.GetParents()
  }

  GetWords(typedStuff: string, type: number) {
    this.http.get<Word[]>(this.base + `api/Words/GetMatching/${typedStuff}`).subscribe(result => {
      if (type === 1) {
        this.parentWordList = result;
      } else {
        this.childWordList = result;
      }
    }, error => console.error(error));
  }

  SetChild(childWordButton: Word) {
    this.childWord = childWordButton.word;
  }

  SetParent(parentWordButton: Word) {
    this.parentWord = parentWordButton.word;
  }

  CreateParent() {
    this.http.post<WordParent>(this.base + `api/ParentWords/${this.parentWord}/${this.childWord}`, null).subscribe(result => {
      this.GetParents()
    }, error => console.error(error));
  }

  DeleteChild(id: number) {
    this.http.delete(this.base + `api/ParentWords/${id}`).subscribe(result => {
      this.GetParents()
    }, error => console.error(error));
  }

  GetParents() {
    this.http.get<WordParent[]>(this.base + `api/ParentWords`).subscribe(result => {
      this.wordParents = result;
      for(let wordParent of this.wordParents)
      {
        this.http.get<Word>(this.base + `api/Words/${wordParent.parentWordId}`).subscribe(result => {
          let thisWord: Word = result;
          wordParent.parentWord= thisWord;
        }, error => console.error(error));

        this.http.get<Word>(this.base + `api/Words/${wordParent.childWordId}`).subscribe(result => {
          let thisWord: Word = result;
          wordParent.childWord= thisWord;
        }, error => console.error(error));
      }
    }, error => console.error(error));
  }
}

interface WordParent {
  wordJoinId? : number,
  parentWordId: number,
  childWordId: number
  childWord: Word,
  parentWord: Word
}
