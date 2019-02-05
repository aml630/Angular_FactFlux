import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormControl } from '@angular/forms';
import { Word } from '../words';
import "rxjs/add/operator/debounceTime";

@Component({
  selector: 'app-words',
  templateUrl: './words.component.html',
  styleUrls: ['./words.component.css']
})
export class WordsComponent implements OnInit {

  base: string = document.getElementsByTagName('base')[0].href;
  wordList: wordShow[];
  newPhrase: string;
  wordTyping: FormControl;
  selectedFile: File;
  selectedWord: Word;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.wordTyping = new FormControl();
    this.wordTyping.valueChanges.debounceTime(400).subscribe(x => {
      if (x == "") {
        this.GetWords();
      } else {
        this.GetMatchingWords(x);
      }
    })
    this.GetWords();
  }

  SetShowTrue(word: wordShow) {
    if (word.show) {
      word.show = false;
    } else {
      word.show = true;
    }
  }

  GetMatchingWords(typedStuff: string) {
    this.http.get<wordShow[]>(this.base + `api/Words/GetMatching/${typedStuff}`)
      .subscribe(result => {
        let basicWordList = result;

        let listWithShow: wordShow[] = [];

        basicWordList.forEach(function (value) {
          var newww: wordShow = {
            show: false,
            wordId: value.wordId,
            word: value.word,
            daily: value.daily,
            weekly: value.weekly,
            yearly: value.yearly,
            type: value.type
          }
          listWithShow.push(newww)
        })

        this.wordList = listWithShow;

      }, error => console.error(error));
  }

  GetWords() {
    this.http.get<wordShow[]>(this.base + 'api/Words').subscribe(result => {
      this.wordList = result;
    }, error => console.error(error));
  }

  CreateWord() {
    let newWord: Word = {
      word: this.newPhrase,
      wordId: 0
    };
    this.http.post<wordShow>(this.base + 'api/Words', newWord).subscribe(result => {
      this.wordList.push(result);
    }, error => console.error(error));
  }

  UpdateWord(word: Word) {
    word.main = true;
    this.http.put<Word[]>(this.base + 'api/Words/' + word.wordId, word).subscribe(result => {
    }, error => console.error(error));
    this.GetWords();
  }

  DeleteWord(wordId: number) {
    this.http.delete<Word[]>(this.base + 'api/Words/' + wordId).subscribe(result => {
      this.GetWords();
    }, error => console.error(error));
  }

  onFileChanged(event) {
    this.selectedFile = event.target.files[0]
  }

  onUpload(theWord:string, theWordId: number) {
    const uploadData = new FormData();
    uploadData.append('myFile', this.selectedFile, this.selectedFile.name);

    let myHeaders = new HttpHeaders();
    myHeaders = myHeaders.set('enctype', 'multipart/form-data');

    this.http.post(this.base + `api/Words/AddImage/${theWord}/${theWordId}`, uploadData, {
      headers: myHeaders
    })
      .subscribe(res => {
        debugger;
        console.log(res);
      });
  }
}

interface wordShow extends Word {
  show: boolean;
}