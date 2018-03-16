import { Component } from '@angular/core';

import { NavMenuItemBase } from './NavMenuItemBase';
import { NavMenuItemState } from './NavMenuItemState';

@Component({
    selector: 'nav-menu-item-profile',
    templateUrl: './nav-menu-item.component.html',
    styleUrls: ['./nav-menu-item.component.css']
})
export class NavMenuItemProfileComponent extends NavMenuItemBase {
    /* NavMenuItemBase Members ************************************************/
    protected get IconName(): string {
        return "account_box";
    }
    protected get Title(): string {
        return "Profile";
    }
    protected get State(): NavMenuItemState {
        return NavMenuItemState.Active;
    }
}
