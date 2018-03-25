import { NewsPostEventViewModel } from "./news-post-event-view-model";

export interface NewsPostViewModel extends NewsPostEventViewModel {
    /* Properties *************************************************************/
    isUnread: boolean;
}