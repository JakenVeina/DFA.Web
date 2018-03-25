import { Component } from "@angular/core";

import { NavMenuItemBase } from "./nav-menu-item-base";
import { NavMenuItemState } from "./nav-menu-item-state";

@Component({
    selector: "nav-menu-item-inactive",
    templateUrl: "./nav-menu-item.component.html",
    styleUrls: ["./nav-menu-item.component.css"]
})
export class NavMenuItemInactiveComponent extends NavMenuItemBase {
    /* NavMenuItemBase Members ************************************************/
    protected get iconName(): string {
        return "check_box_outline_blank";
    }
    protected get title(): string {
        return "Inactive";
    }
    protected get state(): NavMenuItemState {
        return NavMenuItemState.inactive;
    }
    protected get stateText(): string {
        return "3";
    }
}
