import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Stories } from '../stories';
import { DomSanitizer } from '@angular/platform-browser';


@Component({
  selector: 'app-stories',
  templateUrl: './stories.component.html',
  styleUrls: ['./stories.component.css']
})
export class StoriesComponent implements OnInit {

  base: string = document.getElementsByTagName('base')[0].href;

  constructor(private http: HttpClient, public sanitizer: DomSanitizer) { }

mainStories: Stories[];
wordTypes: number [];
  ngOnInit() {
    this.GetStories();
    this.wordTypes = [1,2,3,0]
  }

  GetStories() {
    this.http.get<Stories[]>(this.base + 'api/Stories').subscribe(result => {
      this.mainStories = result;
      console.log(this.mainStories);
    }, error => console.error(error));
  }

  ToggleType(type: number){
     if(this.wordTypes.indexOf(type) == -1)
     {
       this.wordTypes.push(type)
     }else {
       this.wordTypes.splice(this.wordTypes.indexOf(type), 1)
     }
     console.log(this.wordTypes);
  }

  photoURL(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl(url);
  }
}
