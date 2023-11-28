let mapping = {};

export function containerObserver(dotnetReference, options, elementId) {
    const logger = (options && options.enableLogging) ? console.log : () => { };

    console.log('dotnetReference: ', dotnetReference);
    console.log('ElementId: ', elementId);

    logger('dotnetReference: ', dotnetReference);
    logger('ElementId: ', elementId);

    const element = document.getElementById(elementId);

    if (!element) {
        console.log('Element with ', elementId, ' not found');
        return false;
    }

    if (mapping[elementId]) {
        //logger('Resize listener already added');
        console.log('Container Observer already added for Id: ', elementId);
        return true;
    }

    const observer = new CustomObserver(elementId);
    observer.addResizeObserver(elementId, dotnetReference, options);
    mapping[elementId] = observer;

    return true;
}

export function removeContainerObserver(elementId) {
}

export function removeObservers(elementIds) {
}

export function getContainerBreakpoint(elementId) {
    return 14;  // Breakpoint.None
}

export function getContainerSize(elementId) {
    return {
        height: 0,
        width: 0
    };
}

export function matchContainerQuery(query, elementId) {
    return false;
}

class CustomObserver {
    constructor(elementId) {
        console.log('Custom Observer created for Id: ', elementId);

        this.elementId = elementId;
        this.dotnetObserver = undefined;
        this.options = {};
        this.resizeTimers = {};
        this.reportRate = 250;
        this.logger = function (message) { };
        this.resizeObserver = new ResizeObserver(this.resizeHandler);
        this.resizeHandler = this.resizeHandler.bind(this);
    }

    addResizeObserver(elementId, dotnetObserver, options) {
        console.log('Adding Resize Observer for Id: ', this.elementId);

        this.elementId = elementId;
        this.dotnetObserver = dotnetObserver;
        this.options = options || {};
        this.element = document.getElementById(elementId);
        this.reportRate = this.options.reportRate || 250;
        this.logger = this.options.enableLogging ? console.log : () => { };

        this.resizeObserver.observe(this.element, { box: "border-box" });
    }

    resizeHandler(entries) {
        var elementId = entries[0].target.id;
        console.log('Resize Observer called');
        console.log('Target: ', entries[0].target.id);
        console.log('Content Rect: ', entries[0].contentRect);
        console.log('Dotnet Observer: ', this.dotnetObserver);

        //if (this.resizeTimers[elementId])
        //    clearTimeout(this.resizeTimers[elementId]);

    //    this.resizeTimers[elementId] = setTimeout(() => {
    //        this.dotnetObserver.invokeMethodAsync('RaiseOnResized', {
    //            height: entries[0].target.clientHeight,
    //            width: entries[0].target.clientWidth
    //        }, 1, elementId);
    //    }, this.reportRate);
    }

    throttleObserverHandler() {
        clearTimeout(this.resizeTimers[this.elementId]);
        this.resizeTimers[this.elementId] = setTimeout(() => {
            this.dotnetObserver.invokeMethodAsync('RaiseOnResized', {
                height: this.element.clientHeight,
                width: this.element.clientWidth
            }, 1, this.elementId);
        }, this.reportRate);
    }
}
