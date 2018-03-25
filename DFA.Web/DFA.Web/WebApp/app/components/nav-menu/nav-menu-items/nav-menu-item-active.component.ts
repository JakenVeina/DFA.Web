import { Component } from "@angular/core";

import { NavMenuItemBase } from "./nav-menu-item-base";
import { NavMenuItemState } from "./nav-menu-item-state";

@Component({
    selector: "nav-menu-item-active",
    templateUrl: "./nav-menu-item.component.html",
    styleUrls: ["./nav-menu-item.component.css"]
})
export class NavMenuItemActiveComponent extends NavMenuItemBase {
    /* NavMenuItemBase Members ************************************************/
    protected get iconName(): string {
        return "check_box";
    }
    protected get title(): string {
        return "Active";
    }
    protected get state(): NavMenuItemState {
        return NavMenuItemState.active;
    }
    protected get stateText(): string {
        return "1";
    }
}
