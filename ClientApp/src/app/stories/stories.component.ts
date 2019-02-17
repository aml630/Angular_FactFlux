import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Stories } from '../stories';
import { DomSanitizer } from '@angular/platform-browser';
import { FormControl } from '@angular/forms';


@Component({
  selector: 'app-stories',
  templateUrl: './stories.component.html',
  styleUrls: ['./stories.component.css']
})
export class StoriesComponent implements OnInit {

  base: string = document.getElementsByTagName('base')[0].href;

  constructor(private http: HttpClient, public sanitizer: DomSanitizer) { }

  mainStories: Stories[] = [];
  pagedList: Stories[];
  wordTypes: number[];
  wordTyping: FormControl;
  storyPageNumber = 0;

  ngOnInit() {
    this.wordTyping = new FormControl();
    this.wordTyping.valueChanges.debounceTime(400).subscribe(x => {
      if (x == "") {
        this.RestoreStories();
      } else {
        this.GetMatchingStories(x);
      }
    })
    this.GetStories();
  }

  GetStories() {
    this.storyPageNumber++;
    let path = `api/Stories?pageNumber=${this.storyPageNumber}`;
    this.http.get<Stories[]>(this.base + path).subscribe(result => {
      var newBigArray = this.mainStories.concat(result);
      this.mainStories = newBigArray;
      this.pagedList = this.mainStories;
      console.log(this.mainStories);
    }, error => console.error(error));
  }

  RestoreStories() {
    this.mainStories = this.pagedList
  }

  GetMatchingStories(typedStuff: string) {
    this.http.get<Stories[]>(this.base + `api/Stories/GetMatching/${typedStuff}`)
      .subscribe(result => {
        this.mainStories = result;
      }, error => console.error(error));
  }

  ToggleType(type: number) {
    if (this.wordTypes.indexOf(type) == -1) {
      this.wordTypes.push(type)
    } else {
      this.wordTypes.splice(this.wordTypes.indexOf(type), 1)
    }
    console.log(this.wordTypes);
  }

  photoURL(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl(url);
  }
}
