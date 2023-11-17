let instance;

export function listenForResize(dotnetRef, options) {
    instance =  = new ResizeListener();
    instance.listenForResize(dotnetRef, options);
}

export function cancelListener() {
    instance.cancelListener();
}

export function matchMedia(query) {
    return instance.matchMedia(query);
}

export function getViewportSize() {
    return instance.getViewportSize();
}

export class ViewportSize {
    constructor(height, width) {
        this.height = height;
        this.width = width;
    }
}

export class ResizeOptions {
    constructor() {
        this.reportRate = 100;
        this.enableLogging = false;
        this.suppressFirstEvent = true;
        this.notifyOnBreakpointOnly = true;
        this.breakpoints = {};
    }
}

export class ResizeListener {
    constructor() {
        this.logger = (message) => { };
        this.options = new ResizeOptions();
        this.throttleResizeHandlerId = -1;
        this.dotnet = null;
    }

    resizeHandler = () => {
        this.dotnet.invokeMethodAsync(
            'RaiseOnResized', {
            height: window.innerHeight,
            width: window.innerWidth
        });
        this.logger("[MediaQuery] RaiseOnResized invoked");
    }

    throttleResizeHandler = () => {
        clearTimeout(this.throttleResizeHandlerId);
        this.throttleResizeHandlerId = window.setTimeout(this.resizeHandler, this.options.reportRate);
    }

    listenForResize(dotnetRef, options) {
        console.log('listenForResize ', dotnetRef);
        this.options = options;
        this.dotnet = dotnetRef;
        this.logger = this.options.enableLogging ? console.log : (message) => { };
        this.logger(`[MediaQuery] Reporting resize events at rate of: ${this.options.reportRate}ms`);
        window.addEventListener("resize", this.throttleResizeHandler);
        if (!this.options.suppressInitEvent) {
            this.resizeHandler();
        }
    }

    cancelListener() {
        window.removeEventListener("resize", this.throttleResizeHandler);
    }

    matchMedia(query) {
        let m = window.matchMedia(query).matches;
        this.logger(`[MediaQuery] matchMedia "${query}": ${m}`);
        return m;
    }

    getViewportSize() {
        return {
            height: window.innerHeight,
            width: window.innerWidth
        };
    }
}
