import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  title = 'app';

  tweetUrl: string;
  fbUrl: string;

  constructor() { }

  ngOnInit() {
    (function () {

      var shareButtons = document.querySelectorAll(".social-icon-link");

      if (shareButtons) {
        [].forEach.call(shareButtons, function (button) {
          button.addEventListener("click", function (event) {
            const width = 650,
              height = 450;

            event.preventDefault();

            window.open(this.href, 'Share Dialog', 'menubar=no,toolbar=no,resizable=yes,scrollbars=yes,width=' 
            + width + ',height=' + height + ',top=' + (screen.height / 2 - height / 2) + ',left=' + (screen.width / 2 - width / 2));
          });
        });
      }

    })();

    this.tweetUrl = "https://twitter.com/intent/tweet/?url=" + document.URL

    this.fbUrl = "https://www.facebook.com/sharer/sharer.php?u=" + document.URL



  }
}