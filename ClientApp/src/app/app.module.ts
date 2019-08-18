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
import { ChartModule } from 'primeng/chart';
import { HeaderComponent } from './admin-components/admin-header/admin-header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { TwitterUsersComponent } from './admin-components/twitter-users/twitter-users.component';
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
import { TimelineStyleComponent } from './blog/timeline-style/timeline-style.component';
import { TimelineHeaderComponent } from './blog/timeline-style/timeline-header/timeline-header.component';

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
    TimelineStyleComponent,
    TimelineHeaderComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonModule,
    BrowserAnimationsModule, AccordionModule, TabMenuModule,
    CardModule, MenuModule, ChartModule,
    // OAuthModule.forRoot(),
    RouterModule.forRoot([
      { path: '', component: StoriesComponent },
      { path: 'stories', component: StoriesComponent },
      { path: 'about', component: AboutComponent },
      { path: 'timeline/:word', component: TimelineComponent },
      { path: 'privacy', component: PrivacyComponent },
      { path: 'post1', component: TimelineStyleComponent},
      { path: 'factoids', component: FactoidsComponent},
      {
        path: '',
        component: AdminLayoutComponent,
        canActivate: [AuthGuard],
        children: [
          { path: 'rss-feeds', component: RssFeedsComponent},
          { path: 'words', component: WordsComponent},
          { path: 'word-parents', component: WordParentsComponent},
          { path: 'twitter-users', component: TwitterUsersComponent},
          { path: 'main-words', component: MainWordsComponent}
        ]
      }
    ])
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
