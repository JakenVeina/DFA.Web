import { Component } from '@angular/core';

import { NavMenuItemBase } from './NavMenuItemBase';
import { NavMenuItemState } from './NavMenuItemState';

@Component({
    selector: 'nav-menu-item-offline',
    templateUrl: './nav-menu-item.component.html',
    styleUrls: ['./nav-menu-item.component.css']
})
export class NavMenuItemOfflineComponent extends NavMenuItemBase {
    /* NavMenuItemBase Members ************************************************/
    protected get IconName(): string {
        return "indeterminate_check_box";
    }
    protected get Title(): string {
        return "Offline";
    }
    protected get State(): NavMenuItemState {
        return NavMenuItemState.Offline;
    }
    protected get StateText(): string {
        return "2";
    }
}
