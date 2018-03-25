import { UserViewModel } from "../user/user-view-model";

export interface NewsPostEventViewModel {
    /* Properties *************************************************************/
    id: number;
    created: Date;
    createdBy: UserViewModel;
    message: string;
}