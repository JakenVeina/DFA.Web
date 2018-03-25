import { Component } from "@angular/core";

import { NavMenuItemBase } from "./nav-menu-item-base";
import { NavMenuItemState } from "./nav-menu-item-state";

@Component({
    selector: "nav-menu-item-profile",
    templateUrl: "./nav-menu-item.component.html",
    styleUrls: ["./nav-menu-item.component.css"]
})
export class NavMenuItemProfileComponent extends NavMenuItemBase {
    /* NavMenuItemBase Members ************************************************/
    protected get iconName(): string {
        return "account_box";
    }
    protected get title(): string {
        return "Profile";
    }
    protected get state(): NavMenuItemState {
        return NavMenuItemState.active;
    }
}
