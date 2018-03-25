import { Component } from "@angular/core";

import { NavMenuItemBase } from "./nav-menu-item-base";
import { NavMenuItemState } from "./nav-menu-item-state";

@Component({
    selector: "nav-menu-item-offline",
    templateUrl: "./nav-menu-item.component.html",
    styleUrls: ["./nav-menu-item.component.css"]
})
export class NavMenuItemOfflineComponent extends NavMenuItemBase {
    /* NavMenuItemBase Members ************************************************/
    protected get iconName(): string {
        return "indeterminate_check_box";
    }
    protected get title(): string {
        return "Offline";
    }
    protected get state(): NavMenuItemState {
        return NavMenuItemState.offline;
    }
    protected get stateText(): string {
        return "2";
    }
}
