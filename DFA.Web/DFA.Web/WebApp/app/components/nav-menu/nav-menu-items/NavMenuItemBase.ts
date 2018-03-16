import { Component, Input } from '@angular/core';

import { NavMenuItemState } from './NavMenuItemState';

export abstract class NavMenuItemBase {
    /* Properties *************************************************************/
    public NavMenuItemState = NavMenuItemState; // For use in templating

    protected abstract get IconName(): string;

    protected abstract get Title(): string;

    protected abstract get State(): NavMenuItemState;

    protected get StateText(): string {
        return "";
    }
}
