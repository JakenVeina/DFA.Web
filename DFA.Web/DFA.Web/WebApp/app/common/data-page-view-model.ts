export interface DataPageViewModel<T> {
    /* Properties *************************************************************/
    totalRowCount: number;
    filteredRowCount: number;
    rows: T[];
}