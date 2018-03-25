import { Component, Input } from "@angular/core";

import { NavMenuItemState } from "./nav-menu-item-state";

export abstract class NavMenuItemBase {
    /* Properties *************************************************************/
    public navMenuItemState = NavMenuItemState; // For use in templating

    protected abstract get iconName(): string;

    protected abstract get title(): string;

    protected abstract get state(): NavMenuItemState;

    protected get stateText(): string {
        return "";
    }
}
