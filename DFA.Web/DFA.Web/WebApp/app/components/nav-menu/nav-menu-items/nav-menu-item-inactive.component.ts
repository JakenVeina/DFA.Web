import { Component } from '@angular/core';

import { NavMenuItemBase } from './NavMenuItemBase';
import { NavMenuItemState } from './NavMenuItemState';

@Component({
    selector: 'nav-menu-item-inactive',
    templateUrl: './nav-menu-item.component.html',
    styleUrls: ['./nav-menu-item.component.css']
})
export class NavMenuItemInactiveComponent extends NavMenuItemBase {
    /* NavMenuItemBase Members ************************************************/
    protected get IconName(): string {
        return "check_box_outline_blank";
    }
    protected get Title(): string {
        return "Inactive";
    }
    protected get State(): NavMenuItemState {
        return NavMenuItemState.Inactive;
    }
    protected get StateText(): string {
        return "3";
    }
}
