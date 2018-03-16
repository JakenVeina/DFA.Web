import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
    FormsModule,
    ReactiveFormsModule
} from '@angular/forms';
import { HttpModule } from '@angular/http';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import {
    MatCardModule,
    MatDividerModule,
    MatExpansionModule,
    MatIconModule,
} from '@angular/material';


import { NewsPageComponent } from './news-page.component';


@NgModule({
    declarations: [
        NewsPageComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        CommonModule,
        FormsModule,
        HttpModule,
        MatCardModule,
        MatDividerModule,
        MatExpansionModule,
        MatIconModule,
        ReactiveFormsModule,
        RouterModule,
    ],
    exports: [
        NewsPageComponent,
    ],
})
export class NavMenuModule { }