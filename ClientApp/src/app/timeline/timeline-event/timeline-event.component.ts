import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { Article } from '../../article';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-timeline-event',
  templateUrl: './timeline-event.component.html',
  styleUrls: ['./timeline-event.component.css']
})
export class TimelineEventComponent implements OnInit {

  @Input() article: Article
  @Input() index: number
  @Input() feedImage: string
  @ViewChild("vid", {read: ElementRef}) vid: ElementRef;


  constructor(private sanitizer: DomSanitizer) {
  }

  ngOnInit() {



    // this.getVideos(this.video);

  }

  ngAfterViewInit(): void {
    if(this.vid)
    {
      this.getVideos(this.vid.nativeElement)
    }

}

  getVideos(el) {
    var img = document.createElement("img");
    // Get images
    img.setAttribute('src', 'http://i.ytimg.com/vi/' + el.id + '/hqdefault.jpg');
    // Add class to img
    img.setAttribute('class', 'thumb');
    img.setAttribute('width', '100%');
    img.setAttribute('height', '250px');

    // Make div to embed videos
    var video = document.createElement("div");
    // Remove this if you like
    video.setAttribute("class", "video_here");
    // Insert tags
    el.appendChild(img);
    el.appendChild(video);
    // On click get video
    el.addEventListener('click', function () {
      var iframe = document.createElement("iframe");
      iframe.setAttribute('class', 'youtube_video');
      iframe.setAttribute('width', '100%');
      iframe.setAttribute('height', '250px');

      iframe.setAttribute('src', 'https://www.youtube.com/embed/' +
        this.id + '?autoplay=1&autohide=1&border=0&wmode=opaque&enablejsapi=1');
      // Remplace img for video
      this.parentNode.replaceChild(iframe, this);
    }, false);
  }



  // vidUrl(url: string) {
  //   let urlString = 'https://www.youtube.com/embed/'+url;
  //   return this.sanitizer.bypassSecurityTrustUrl(urlString);
  // }



}
