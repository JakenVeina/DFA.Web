import { CommonModule } from "@angular/common";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import {
    FormsModule,
    ReactiveFormsModule
} from "@angular/forms";
import { HttpModule } from "@angular/http";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import {
    MatCardModule,
    MatDividerModule,
    MatExpansionModule,
    MatIconModule,
} from "@angular/material";


import { NewsPostsService } from "../news/news-posts-service";

import { NavMenuComponent } from "./nav-menu.component";
import { NavMenuItemNewsComponent } from "./nav-menu-items/nav-menu-item-news.component";
import { NavMenuItemProfileComponent } from "./nav-menu-items/nav-menu-item-profile.component";
import { NavMenuItemActiveComponent } from "./nav-menu-items/nav-menu-item-active.component";
import { NavMenuItemInactiveComponent } from "./nav-menu-items/nav-menu-item-inactive.component";
import { NavMenuItemOfflineComponent } from "./nav-menu-items/nav-menu-item-offline.component";


@NgModule({
    declarations: [
        NavMenuComponent,
        NavMenuItemNewsComponent,
        NavMenuItemProfileComponent,
        NavMenuItemActiveComponent,
        NavMenuItemInactiveComponent,
        NavMenuItemOfflineComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        CommonModule,
        FormsModule,
        HttpModule,
        HttpClientModule,
        MatCardModule,
        MatDividerModule,
        MatExpansionModule,
        MatIconModule,
        ReactiveFormsModule,
        RouterModule,
    ],
    providers: [
        NewsPostsService,
    ],
    exports: [
        NavMenuComponent,
        NavMenuItemNewsComponent,
        NavMenuItemProfileComponent,
        NavMenuItemActiveComponent,
        NavMenuItemInactiveComponent,
        NavMenuItemOfflineComponent,
    ],
})
export class NavMenuModule { }