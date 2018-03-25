import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http/src/client";

import { Observable, Observer } from "rxjs/Rx";

import { HubConnection } from "@aspnet/signalr";

import { ApiEventSubscriptionRequest } from "./api-event-subscription-request";

@Injectable()
export class ApiEventService {
    /* Construction ***********************************************************/
    constructor(private _httpClient: HttpClient) {
    }

    /* Public Methods *********************************************************/
    public getEvent<T>(eventName: string): Observable<T> {
        let name = this._normalizeEventName(eventName);

        if (this._eventsByName[name] == null) {
            this._eventsByName[name] = this._createEventObservable(name);
        }

        return this._eventsByName[name];
    }

    /* Private Methods ********************************************************/
    private _normalizeEventName(name: string): string {
        if (!name.startsWith("/"))
            name = "/" + name;
        if (!name.endsWith("/"))
            name += "/";

        return name;
    }
    private _createEventObservable<T>(name: string): Observable<T> {
        let observers: Observer<T>[] = [];
        let raiseEventHandler = (args: any[]) => {
            observers.forEach((observer) => {
                observer.next(args[0] as T);
            });
        };

        return Observable.create((observer: Observer<T>) => {
            if (observers.length == 0) {
                this._subscribe(name, raiseEventHandler);
            }

            let observerIndex = observers.length;
            observers.push(observer);

            return () => {
                observers.splice(observerIndex, 1);

                if (observers.length === 0) {
                    this._unsubscribe(name, raiseEventHandler);
                }
            };
        });
    }
    private _subscribe(eventName: string, eventHandler: (args: any[]) => void): void {
        this._httpClient.post(name + "subscribe", this._subscriptionRequest);

        if (this._subscriptionCount === 0) {
            this._subscriptionRequest = {
                subscriptionToken: ""
            };

            for (let i = 0; i < 32; ++i)
                this._subscriptionRequest.subscriptionToken += Math.floor(Math.random() * 35).toString(36);

            this._hubConnection = new HubConnection("/api/events?token=" + this._subscriptionRequest.subscriptionToken);
        }
        this._subscriptionCount += 1;

        this._hubConnection!.on("RaiseEvent", eventHandler);
    }
    private _unsubscribe(eventName: string, eventHandler: (args: any[]) => void): void {
        this._hubConnection!.off("RaiseEvent", eventHandler);

        this._subscriptionCount -= 1;
        if (this._subscriptionCount === 0) {
            this._hubConnection!.stop();
            this._hubConnection = null;
            this._subscriptionRequest = null;
        }

        this._httpClient.post(name + "unsubscribe", this._subscriptionRequest);
    }

    /* Private Fields *********************************************************/
    private _hubConnection: HubConnection | null;
    private _subscriptionCount: number = 0;
    private _eventsByName: { [name: string]: any } = {};
    private _subscriptionRequest: ApiEventSubscriptionRequest | null;
}