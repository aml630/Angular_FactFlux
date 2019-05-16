import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { TimelineEventComponent } from './timeline/timeline-event/timeline-event.component';
import { StoriesComponent } from './stories/stories.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { WordParentsComponent } from './admin-components/word-parents/word-parents.component';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { AccordionModule } from 'primeng/accordion';
import { TabMenuModule } from 'primeng/tabmenu';
import { MenuModule } from 'primeng/menu';
import { HeaderComponent } from './admin-components/admin-header/admin-header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { TwitterUsersComponent } from './admin-components/twitter-users/twitter-users.component';
import { OAuthModule } from 'angular-oauth2-oidc';
import { AuthGuard } from './AuthGuard';
import { StoriesTitleComponent } from './stories/stories-title/stories-title.component';
import { FooterComponent } from './footer/footer.component';
import { AboutComponent } from './about/about.component';
import { PrivacyComponent } from './privacy/privacy.component';
import { UserHeaderComponent } from './user-header/user-header.component';
import { AdminLayoutComponent } from './admin-components/admin-layout/admin-layout.component';
import { MainWordsComponent } from './admin-components/main-words/main-words.component';
import { RssFeedsComponent } from './admin-components/rss-feeds/rss-feeds.component';
import { WordsComponent } from './admin-components/words/words.component';
import { FactoidsComponent } from './factoids/factoids.component';
import { Graph1Component } from './factoids/graph1/graph1.component';
import { SpinnerComponent } from './spinner/spinner.component';
import { TimelineComponent } from './timeline/timeline/timeline.component';
import { Post1Component } from './factoids/post1/post1.component';
import { Fact1Component } from './factoids/fact1/fact1.component';
import { PostHeaderComponent } from './factoids/post-header/post-header.component';
import { Post2Component } from './factoids/post2/post2.component';
import { PostEventComponent } from './factoids/post2/post-event/post-event.component';
import { NewFrontComponent } from './template/new-front/new-front.component';
import { NavbarComponent } from './template/navbar/navbar.component';
import { FooterComponent2 } from './template/footer2/footer2.component';
import { Post3Component } from './template/post3/post3.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PostTimelineComponent } from './template/post-timeline/post-timeline.component';

@NgModule({
  declarations: [
    AppComponent,
    RssFeedsComponent,
    WordsComponent,
    TimelineComponent,
    TimelineEventComponent,
    StoriesComponent,
    WordParentsComponent,
    HeaderComponent,
    AdminLayoutComponent,
    SidebarComponent,
    UserHeaderComponent,
    TwitterUsersComponent,
    StoriesTitleComponent,
    FooterComponent,
    AboutComponent,
    PrivacyComponent,
    MainWordsComponent,
    FactoidsComponent,
    Graph1Component,
    SpinnerComponent,
    Post1Component,
    Fact1Component,
    PostHeaderComponent,
    Post2Component,
    PostEventComponent,
    NewFrontComponent,
    NavbarComponent,
    FooterComponent2,
    Post3Component,
    PostTimelineComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonModule,
    BrowserAnimationsModule, AccordionModule, TabMenuModule,
    CardModule, MenuModule,
    OAuthModule.forRoot(), NgbModule.forRoot(),
    RouterModule.forRoot([
      { path: '', component: StoriesComponent },
      { path: 'stories', component: StoriesComponent },
      { path: 'about', component: AboutComponent },
      { path: 'timeline/:word', component: TimelineComponent },
      { path: 'privacy', component: PrivacyComponent },
      { path: 'post1', component: Post1Component },
      { path: 'post2', component: Post2Component },
      { path: 'factoids', component: FactoidsComponent },
      { path: 'front', component: NewFrontComponent },
      { path: 'post3', component: Post3Component },
      { path: 'post4', component: PostTimelineComponent },
      {
        path: '',
        component: AdminLayoutComponent,
        canActivate: [AuthGuard],
        children: [
          { path: 'rss-feeds', component: RssFeedsComponent },
          { path: 'words', component: WordsComponent },
          { path: 'word-parents', component: WordParentsComponent },
          { path: 'twitter-users', component: TwitterUsersComponent },
          { path: 'main-words', component: MainWordsComponent }
        ]
      }
    ])
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
