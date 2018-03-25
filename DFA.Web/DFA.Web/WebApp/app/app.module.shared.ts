import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { MatSidenavModule } from "@angular/material";

import { NavMenuModule } from "./components/nav-menu/nav-menu.module";
import { AppComponent } from "./app.component";


@NgModule({
    declarations: [
        AppComponent,
    ],
    imports: [
        RouterModule.forRoot([
            { path: "", redirectTo: "news", pathMatch: "full" },
            //{ path: "news", loadChildren: "./components/news/news.module#NewsModule?chunkName=news" },
            { path: "**", redirectTo: "news" }
        ]),

        MatSidenavModule,

        NavMenuModule,
    ],
})
export class AppModuleShared {
}
