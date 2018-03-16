import { Component } from '@angular/core';

import { NavMenuItemBase } from './NavMenuItemBase';
import { NavMenuItemState } from './NavMenuItemState';

@Component({
    selector: 'nav-menu-item-active',
    templateUrl: './nav-menu-item.component.html',
    styleUrls: ['./nav-menu-item.component.css']
})
export class NavMenuItemActiveComponent extends NavMenuItemBase {
    /* NavMenuItemBase Members ************************************************/
    protected get IconName(): string {
        return "check_box";
    }
    protected get Title(): string {
        return "Active";
    }
    protected get State(): NavMenuItemState {
        return NavMenuItemState.Active;
    }
    protected get StateText(): string {
        return "1";
    }
}
