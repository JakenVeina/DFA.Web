import { Component } from "@angular/core";

import { HubConnection } from "@aspnet/signalr";


@Component({
    selector: "nav-menu",
    templateUrl: "./nav-menu.component.html",
    styleUrls: ["./nav-menu.component.css"]
})
export class NavMenuComponent {

    private _hubConnection: HubConnection;

    constructor() {

        this._hubConnection = new HubConnection("/api/events?token=ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

        this._hubConnection
            .onclose((error) => alert("Hub Connection Closed: " + error));

        this._hubConnection
            .on("raiseEvent", (eventName, eventData) => alert("raiseEvent " + eventName + " " + eventData));

        this._hubConnection
            .start()
            .catch(reason => alert("Hub Connection Failed: \"" + reason + "\""));

    }

    test() {

    }

}
