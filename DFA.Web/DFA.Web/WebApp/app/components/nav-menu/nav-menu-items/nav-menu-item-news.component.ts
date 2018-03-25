import { Component } from "@angular/core";

import { NewsPostsService } from "../../news/news-posts-service";

import { NavMenuItemBase } from "./nav-menu-item-base";
import { NavMenuItemState } from "./nav-menu-item-state";


@Component({
    selector: "nav-menu-item-news",
    templateUrl: "./nav-menu-item.component.html",
    styleUrls: ["./nav-menu-item.component.css"]
})
export class NavMenuItemNewsComponent extends NavMenuItemBase {
    /* Construction ***********************************************************/
    public constructor(
        private _newsPostsService: NewsPostsService) {
        super();

        // TODO
    }

    /* NavMenuItemBase Members ************************************************/
    protected get iconName(): string {
        return "subject";
    }
    protected get title(): string {
        return "News";
    }
    protected get state(): NavMenuItemState {
        return (this._unreadNewsPosts > 0) ? NavMenuItemState.active : NavMenuItemState.inactive;
    }
    protected get stateText(): string {
        return this._unreadNewsPosts.toString();
    }

    /* Private Fields *********************************************************/
    private _unreadNewsPosts = 0;
}
