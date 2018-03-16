import { Injectable } from "@angular/core";

import { Observable } from "rxjs/Observable";
import { map } from "rxjs/operators";
import { HttpClient } from "@angular/common/http";

import { EntityCount } from "../../common/EntityCount";

@Injectable()
export class NewsPostsService {
    /* Construction ***********************************************************/
    public constructor(
        private _httpClient: HttpClient) { }

    /* Public Methods *********************************************************/
    public GetUnreadCount(): Observable<number> {
        return this._httpClient
            .get<EntityCount>("api/newsposts/unread/count")
            .pipe(map(x => x.count));
    }

}