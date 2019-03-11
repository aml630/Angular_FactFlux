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
import { ShellComponent } from './shell/shell.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { CustomHeaderComponent } from './custom-header/custom-header.component';
import { TwitterUsersComponent } from './rss-feeds/twitter-users/twitter-users.component';
import { OAuthModule } from 'angular-oauth2-oidc';
import { AuthGuard } from './AuthGuard';
import { StoriesTitleComponent } from './stories-title/stories-title.component';
import { FooterComponent } from './footer/footer.component';
import { AboutComponent } from './about/about.component';
import { PrivacyComponent } from './privacy/privacy.component';

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
    ShellComponent,
    SidebarComponent,
    CustomHeaderComponent,
    TwitterUsersComponent,
    StoriesTitleComponent,
    FooterComponent,
    AboutComponent,
    PrivacyComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonModule,
    BrowserAnimationsModule, AccordionModule, TabMenuModule,
    CardModule, MenuModule,
    OAuthModule.forRoot(),
    RouterModule.forRoot([
      { path: '', component: StoriesComponent },
      { path: 'stories', component: StoriesComponent },
      { path: 'about', component: AboutComponent },
      { path: 'timeline/:word', component: WordTimelineComponent },
      { path: 'privacy', component: PrivacyComponent },
      {
        path: '',
        component: ShellComponent,
        canActivate: [AuthGuard],
        children: [
          { path: 'rss-feeds', component: RssFeedsComponent },
          { path: 'words', component: WordsComponent },
          { path: 'word-parents', component: WordParentsComponent },
        ]
      }
    ])
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
