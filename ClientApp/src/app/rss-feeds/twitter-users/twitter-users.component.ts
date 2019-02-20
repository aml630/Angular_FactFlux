import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TwitterUser } from '../../twitterUser';

@Component({
  selector: 'app-twitter-users',
  templateUrl: './twitter-users.component.html',
  styleUrls: ['./twitter-users.component.css']
})
export class TwitterUsersComponent implements OnInit {

  foundTwitterUsers: TwitterUser[];
  form: FormGroup;
  base: string = document.getElementsByTagName('base')[0].href;

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
      debugger;
    }, error => console.error(error));
  }

  UpdateTwitterUser(TwitterUser: TwitterUser) {
    this.http.put<TwitterUser>(this.base + 'api/Twitter/' + TwitterUser.twitterUserId, TwitterUser).subscribe(result => {
    }, error => console.error(error));

    this.GetTwitterUsers();
  }

  DeleteTwitterUser(TwitterUserId: number) {
    this.http.delete<TwitterUser>(this.base + 'api/Twitter/' + TwitterUserId).subscribe(result => {
      this.GetTwitterUsers();
    }, error => console.error(error));
  }
}
