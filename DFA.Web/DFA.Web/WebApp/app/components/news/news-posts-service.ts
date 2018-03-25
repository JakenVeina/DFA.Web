import { Injectable } from "@angular/core";

import { Observable, Observer, Subscription } from "rxjs/Rx";
import { map } from "rxjs/operators";
import { HttpClient, HttpParams } from "@angular/common/http";

import { CountViewModel } from "../../common/count-view-model";
import { DataPageRequest } from "../../common/data-page-request";
import { DataPageViewModel } from "../../common/data-page-view-model";

import { ApiEventService } from "../../events/api-event-service";

import { NewsPostViewModel } from "./news-post-view-model";
import { NewsPostEventViewModel } from "./news-post-event-view-model";

@Injectable()
export class NewsPostsService {
    /* Construction ***********************************************************/
    public constructor(private _httpClient: HttpClient, apiEventService: ApiEventService) {
        this.created = apiEventService.getEvent("api/newsposts/created");
        this.modified = apiEventService.getEvent("api/newsposts/modified");
        this.deleted = apiEventService.getEvent("api/newsposts/deleted");
        this.unreadCount = this._createUnreadCountObservable();
    }

    /* Properties *************************************************************/
    public created: Observable<NewsPostEventViewModel>
    public modified: Observable<NewsPostEventViewModel>
    public deleted: Observable<NewsPostEventViewModel>
    public unreadCount: Observable<number>

    /* Methods ****************************************************************/
    public getPage(dataPageRequest: DataPageRequest): Observable<DataPageViewModel<NewsPostViewModel>> {

        let params = new HttpParams();
        if (dataPageRequest.firstRowIndex == null) {
            params.append("firstRowIndex", dataPageRequest.firstRowIndex!.toString());
        }
        if (dataPageRequest.lastRowIndex == null) {
            params.append("lastRowIndex", dataPageRequest.lastRowIndex!.toString());
        }

        return this._httpClient.get("api/newsposts", { params: params })
            .pipe(map(response => response as DataPageViewModel<NewsPostViewModel>));
    }

    /* Private Methods ********************************************************/
    private _createUnreadCountObservable(): Observable<number> {
        let unreadCount = 0;
        let observers: Observer<number>[] = [];
        let createdSubscription: Subscription;
        let deletedSubscription: Subscription;
        let updateObservers = () => {
            observers.forEach(observer => observer.next(unreadCount));
        };

        return Observable.create((observer: Observer<number>) => {
            if (observers.length === 0) {
                createdSubscription = this.created.subscribe(x => {
                    ++unreadCount;
                    updateObservers();
                });
                deletedSubscription = this.deleted.subscribe(x => {
                    --unreadCount;
                    updateObservers();
                });

                this._httpClient.get("api/newsposts/unreadcount")
                    .subscribe(response => {
                        unreadCount = (response as CountViewModel).count;
                        updateObservers();
                    });
            }

            let observerIndex = observers.length;
            observers.push(observer);

            return () => {
                observers.splice(observerIndex, 1);

                if (observers.length === 0) {
                    createdSubscription.unsubscribe();
                    deletedSubscription.unsubscribe();
                }
            };
        });
    }
}