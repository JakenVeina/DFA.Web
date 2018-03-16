import { Component } from "@angular/core";

import { NewsPostsService } from "../../news/NewsPostsService";

import { NavMenuItemBase } from "./NavMenuItemBase";
import { NavMenuItemState } from "./NavMenuItemState";


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

        _newsPostsService.GetUnreadCount()
            .subscribe(count => this._unreadNewsPosts = count);
    }

    /* NavMenuItemBase Members ************************************************/
    protected get IconName(): string {
        return "subject";
    }
    protected get Title(): string {
        return "News";
    }
    protected get State(): NavMenuItemState {
        return (this._unreadNewsPosts > 0) ? NavMenuItemState.Active : NavMenuItemState.Inactive;
    }
    protected get StateText(): string {
        return this._unreadNewsPosts.toString();
    }

    /* Private Fields *********************************************************/
    private _unreadNewsPosts = 0;
}
