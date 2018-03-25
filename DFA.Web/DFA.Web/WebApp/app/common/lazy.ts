export class Lazy<T>{
    /* Construction ***********************************************************/
    constructor(constructor: () => T) {
        this._constructor = constructor;
    }

    /* Public Methods *********************************************************/
    public get(): T {
        if (this._instance == null)
            this._instance = this._constructor();
        return this._instance;
    }
    public exists(): boolean {
        return (this._instance != null);
    }

    /* Private Fields *********************************************************/
    private _constructor: () => T;
    private _instance: T;
}