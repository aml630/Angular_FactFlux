import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Tweet } from '../tweet';
import { TwitterUser } from '../twitterUser';


@Component({
  selector: 'app-twitter-users',
  templateUrl: './twitter-users.component.html',
  styleUrls: ['./twitter-users.component.css']
})
export class TwitterUsersComponent implements OnInit {

  foundTwitterUsers: TwitterUser[];
  form: FormGroup;
  base: string = document.getElementsByTagName('base')[0].href;
  foundTweets: Tweet[];
  
  constructor(private formBuilder: FormBuilder, private http: HttpClient) { }

  ngOnInit() {
    this.GetTwitterUsers();

    this.form = this.formBuilder.group({
      twitterUsername: ['', Validators.required]
    });
  }

  GetTwitterUsers() {
    this.http.get<TwitterUser[]>(this.base + 'api/Twitter').subscribe(result => {
      this.foundTwitterUsers = result;
    }, error => console.error(error));
  }

  CreateTwitterUser(twitterForm: TwitterUser) {   
    this.http.post<TwitterUser>(this.base + 'api/Twitter', twitterForm).subscribe(result => {
      this.GetTwitterUsers();
    }, error => console.error(error));
  }

  UpdateTwitterUser(twitterUser: TwitterUser) {
    this.http.put<TwitterUser>(this.base + `api/Twitter/${twitterUser.twitterUserId}`, twitterUser).subscribe(result => {
      this.GetTwitterUsers();
    }, error => console.error(error));
  }

  GetTweetsForUser(twitterUser: TwitterUser){
    this.http.post<Tweet[]>(this.base + `api/Twitter/AddTweetsForUser/${twitterUser.twitterUsername}`, 
    null).subscribe(result => {
      this.foundTweets = result;
    }, error => console.error(error));

  }

  DeleteTwitterUser(TwitterUserId: number) {
    this.http.delete<TwitterUser>(this.base + 'api/Twitter/' + TwitterUserId).subscribe(result => {
      this.GetTwitterUsers();
    }, error => console.error(error));
  }
}
