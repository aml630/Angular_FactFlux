import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { RssFeedsComponent } from './rss-feeds/rss-feeds.component';
import { WordsComponent } from './words/words.component';
import { WordTimelineComponent } from './timeline/word-timeline/word-timeline.component';
import { TimelineEventComponent } from './timeline/timeline-event/timeline-event.component';
import { StoriesComponent } from './stories/stories.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { WordParentsComponent } from './word-parents/word-parents.component';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { AccordionModule } from 'primeng/accordion';     //accordion and accordion tab
import { TabMenuModule } from 'primeng/tabmenu';
import { MenuModule } from 'primeng/menu';
import { HeaderComponent } from './header/header.component';
import { FrontPageComponent } from './front-page/front-page.component';
import { ShellComponent } from './shell/shell.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { CustomHeaderComponent } from './custom-header/custom-header.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    RssFeedsComponent,
    WordsComponent,
    WordTimelineComponent,
    TimelineEventComponent,
    StoriesComponent,
    WordParentsComponent,
    HeaderComponent,
    FrontPageComponent,
    ShellComponent,
    SidebarComponent,
    CustomHeaderComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,    
    ReactiveFormsModule,
    ButtonModule,
    BrowserAnimationsModule, AccordionModule, TabMenuModule,
    CardModule, MenuModule,
    RouterModule.forRoot([
      {
        path: '',
        component: ShellComponent,
        children: [
          { path: '', component: WordsComponent, pathMatch: 'full' },
          { path: 'rss-feeds', component: RssFeedsComponent },
          { path: 'words', component: WordsComponent },
          { path: 'word-parents', component: WordParentsComponent },
          // { path: 'time/:word', component: WordTimelineComponent },
          { path: 'stories1', component: StoriesComponent }
        ]
      },
      { path: 'front-page', component: FrontPageComponent },
      { path: 'stories', component: StoriesComponent },
      { path: 'time/:word', component: WordTimelineComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
