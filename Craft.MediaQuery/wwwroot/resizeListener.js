let mapping = {};
let logger = function (message) { };

export function resizeListener(dotnetReference, options, id) {
    logger = (options && options.enableLogging) ? console.log : (message) => { };

    logger('dotnetReference: ', dotnetReference);
    //console.log('dotnetReference: ', dotnetReference);
    var map = mapping;

    if (map[id]) {
        logger('Resize listener already added');
        //console.log('Resize listener already added');
        return;
    }

    var listener = new ResizeListener(id);
    listener.addResizeListener(dotnetReference, options);
    map[id] = listener;
}

export function cancelListener(id) {
    logger('Canceling listener with id: ', id);
    //console.log('Canceling listener with id: ', id);

    var map = mapping;

    if (map[id]) {
        map[id].cancelListener();
        delete map[id];
    }
}

export function cancelAllListeners() {
    logger('Canceling all listeners');
    //console.log('Canceling all listeners');

    var map = mapping;

    for (var id in map) {
        map[id].cancelListener();
        delete map[id];
    }
}

export function matchMediaQuery(query, id) {
    logger('Matching matchMediaQuery: ', query);
    //console.log('Matching matchMediaQuery: ', query)

    var map = mapping;

    if (map[id]) {
        return map[id].matchMedia(query);
    }

    return false;
}

export function getViewportSize(id) {
    logger('Getting browser window size');
    //console.log('Getting browser window size');

    var map = mapping;

    if (map[id]) {
        return map[id].getBrowserWindowSize();
    }

    return {
        height: 0,
        width: 0
    };
}

export function getBreakpoint(id) {
    logger('Getting browser Breakpoint');
    //console.log('Getting browser Breakpoint');

    var map = mapping;

    if (map[id]) {
        return map[id].getCurrentBreakpoint();
    }

    return 14;  // Breakpoint.None
}

class ResizeListener {
    constructor(id) {
        this.options = {};
        this.dotnetResizeService = undefined;
        this.breakpoint = -1;
        this.id = id;
        this.reportRate = 100;
        this.throttleResizeHandlerId = -1;
        this.handleResize = this.throttleResizeHandler.bind(this);
    }

    addResizeListener(dotnetReference, options) {
        logger('Adding resize listener, Options: ', options);

        if (this.dotnetResizeService) {
            logger('Resize listener already added');
            // console.log('Resize listener already added');
            this.options = options;
            return;
        }

        this.dotnetResizeService = dotnetReference;
        this.options = options;

        this.reportRate = (options || {}).reportRate || 100;

        logger(`Reporting resize events at rate of: ${this.reportRate}`);
        // console.log(`Reporting resize events at rate of: ${this.reportRate}`);

        window.addEventListener('resize', this.handleResize, false);

        // Make The First Call
        if (!this.options.suppressFirstEvent)
            this.resizeHandler();

        // Get The Current Breakpoint
        this.breakpoint = this.getCurrentBreakpoint();
    }

    throttleResizeHandler() {
        clearTimeout(this.throttleResizeHandlerId);
        this.throttleResizeHandlerId = window.setTimeout(this.resizeHandler.bind(this), ((this.options || {}).reportRate || 100));
    }

    resizeHandler() {
        logger('Resize event triggered');
        // console.log('Resize event triggered');

        if (this.dotnetResizeService) {
            const width = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
            const height = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;

            let newBreakpoint = this.getBreakpoint(width);

            logger("Width: ", width, "Height: ", height);
            //console.log("Width: ", width, "Height: ", height);
            logger("Breakpoints [Old]: ", this.breakpoint, "[New]: ", newBreakpoint);
            // console.log("Breakpoints [Old]: ", this.breakpoint, "[New]: ", newBreakpoint);

            if (this.options.notifyOnBreakpointOnly) {
                if (this.breakpoint == newBreakpoint) {
                    logger("Breakpoint has not changed, skipping resize event", this.breakpoint, newBreakpoint);
                    // console.log("Breakpoint has not changed, skipping resize event", this.breakpoint, newBreakpoint);
                    return;
                }

                this.breakpoint = newBreakpoint;

                logger("Breakpoint changed to ", this.breakpoint);
                // console.log("Breakpoint changed to ", this.breakpoint);
            }

            try {
                logger('Invoking RaiseOnResized');
                // console.log('Invoking RaiseOnResized');
                this.dotnetResizeService.invokeMethodAsync('RaiseOnResized',
                    {
                        height: height,
                        width: width
                    },
                    newBreakpoint);
            } catch (error) {
                logger(`Error invoking resize event: ${error}`);
                // console.log(`Error invoking resize event: ${error}`);
            }
        }
    }

    getBreakpoint(width) {
        logger(`Getting breakpoint for width: ${width}`);

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

    getCurrentBreakpoint() {
        const width = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;

        return this.getBreakpoint(width);
    }

    cancelListener() {
        // console.log('Canceling resize listener');
        logger('Canceling resize listener');

        this.dotnetResizeService = undefined;
        window.removeEventListener("resize", this.handleResize);
    }

    matchMedia(query) {
        let m = window.matchMedia(query).matches;

        logger(`[BlazorSize] matchMedia "${query}": ${m}`);

        return m;
    }

    getBrowserWindowSize() {
        return {
            height: window.innerHeight,
            width: window.innerWidth
        };
    }
}