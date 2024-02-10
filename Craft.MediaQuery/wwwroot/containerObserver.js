let mapping = {};
var logger = function (message) { };

export function containerObserver(dotnetReference, options, elementId) {
    logger = (options && options.enableLogging) ? console.log : (message) => { };

    logger('dotnetReference: ', dotnetReference);
    logger('ElementId: ', elementId);

    const element = document.getElementById(elementId);

    if (!element) {
        logger('Element with ', elementId, ' not found');
        return false;
    }

    if (mapping[elementId]) {
        logger('Container Observer already added for Id: ', elementId);
        return true;
    }

    const observer = new CustomObserver(elementId);
    observer.addResizeObserver(elementId, dotnetReference, options);
    mapping[elementId] = observer;

    return true;
}

export function removeContainerObserver(elementId) {
    const observer = mapping[elementId];

    if (observer) {
        logger('Removing Container Observer for Id: ', elementId);

        observer.cancelObserver();
        delete mapping[elementId];
    }
}

export function removeAllObservers(elementIds) {
    elementIds.forEach(elementId => {
        removeContainerObserver(elementId);
    });
}

export function getContainerBreakpoint(elementId) {
    const observer = mapping[elementId];

    if (observer) {
        return observer.getContainerBreakpoint(elementId);
    }

    return 14;  // Breakpoint.None
}

export function getContainerSize(elementId) {
    const observer = mapping[elementId];

    if (observer) {
        return observer.getContainerSize(elementId);
    }

    return { height: 0, width: 0 };
}

class CustomObserver {
    constructor(elementId) {
        logger('Custom Observer created for Id: ', elementId);

        this.elementId = elementId;
        this.dotnetObserver = undefined;
        this.options = {};
        this.resizeTimers = {};
        this.reportRate = 250;
        this.breakpoint = -1;
        this.resizeObserver = new ResizeObserver(this.resizeHandler.bind(this));
        this.cancelObserver = this.cancelObserver.bind(this);
    }

    addResizeObserver(elementId, dotnetObserver, options) {
        logger('Adding Resize Observer for Id: ', this.elementId);

        this.elementId = elementId;
        this.dotnetObserver = dotnetObserver;
        this.options = options || {};
        this.element = document.getElementById(elementId);
        this.reportRate = this.options.reportRate || 250;

        this.resizeObserver.observe(this.element, { box: "border-box" });
        this.breakpoint = this.getContainerBreakpoint(elementId);

        // Make The First Call
        if (!this.options.suppressFirstEvent)
            this.resizeHandler();
    }

    resizeHandler(entries) {
        logger('Resize Observer called. Target: ', this.elementId);

        if (this.dotnetObserver) {
            const size = this.getContainerSize(this.elementId);

            let newBreakpoint = this.getBreakpoint(size.width);

            if (this.options.notifyOnBreakpointOnly) {
                if (this.breakpoint == newBreakpoint) {
                    logger("Breakpoint has not changed, skipping resize event", this.breakpoint, newBreakpoint);
                    return;
                }

                this.breakpoint = newBreakpoint;
            }

            try {
                if (this.resizeTimers[this.elementId]) {
                    clearTimeout(this.resizeTimers[this.elementId]);
                }

                this.resizeTimers[this.elementId] = setTimeout(() => {
                    this.dotnetObserver.invokeMethodAsync('RaiseOnResized', size, this.breakpoint, this.elementId);
                }, this.reportRate);
            }
            catch (error) {
                logger(`Error invoking resize event: ${error}`);
            }
        }
    }

    getContainerSize(elementId) {
        const element = document.getElementById(elementId);

        if (element)
            return {
                height: element.clientHeight,
                width: element.clientWidth
            };

        return { height: 0, width: 0 };
    }

    cancelObserver() {
        logger('Canceling Resize Observer:', this.elementId);

        this.dotnetObserver = undefined;

        if (this.resizeTimers[this.elementId])
            clearTimeout(this.resizeTimers[this.elementId]);

        this.resizeObserver.unobserve(this.element);
        this.resizeObserver.disconnect;
        delete this.resizeObserver;
    }

    getBreakpoint(width) {
        if (width >= this.options.breakpoints["FullHd"])
            return 5;
        else if (width >= this.options.breakpoints["Widescreen"])
            return 4;
        else if (width >= this.options.breakpoints["Desktop"])
            return 3;
        else if (width >= this.options.breakpoints["Tablet"])
            return 2;
        else if (width >= this.options.breakpoints["Mobile"])
            return 1;
        else
            return 0;
    }

    getContainerBreakpoint(elementId) {
        const size = this.getContainerSize(elementId);

        return this.getBreakpoint(size.width);
    }
}