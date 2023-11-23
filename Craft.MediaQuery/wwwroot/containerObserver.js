let mapping = {};
let logger = function (message) { };

export function containerObserver(dotnetReference, options, elementId, id) {
    //logger = (options || {}).enableLogging ? console.log : (message) => { };

    console.log('dotnetReference: ', dotnetReference);
    var map = mapping;

    const element = document.getElementById(elementId);

    if (!element) {
        console.log('Element with ', elementId, ' not found');
        return false;
    }

    if (map[id]) {
        //logger('Resize listener already added');
        console.log('Container Observer already added for Id: ', id);
        return true;
    }

    var observer = new CustomObserver(id);
    observer.addResizeObserver(dotnetReference, options, elementId);
    map[id] = observer;

    return true;
}

export function removeContainerObserver(id) {
}

export function removeObservers(ids) {
}

export function getContainerBreakpoint(id) {
    return 14;  // Breakpoint.None
}

export function getContainerSize(id) {
    return {
        height: 0,
        width: 0
    };
}

export function matchContainerQuery(query, id) {
    return false;
}

class CustomObserver {
    constructor(id) {
        this.id = id;
        this.options = {};
        this.elementId = '';
        this.element = undefined;
        this.logger = function (message) { };
        this.dotnetObserver = undefined;
        this.breakpoint = -1;
        this.reportRate = 250;
        this.throttleObserverHandlerId = -1;
        this.handleObserver = this.throttleObserverHandler.bind(this);
    }

    addResizeObserver(dotnetReference, options, elementId) {
        this.dotnetObserver = dotnetReference;
        this.options = options;
        this.elementId = elementId;

        this.element = document.getElementById(this.elementId);

        this.reportRate = (this.options || {}).reportRate || 250;

        this.logger = (this.options || {}).enableLogging ? console.log : (message) => { };

        this.resizeObserver = new ResizeObserver(this.handleObserver);
        this.resizeObserver.observe(this.element, { box: "border-box"});
    }

    throttleObserverHandler() {
        clearTimeout(this.throttleObserverHandlerId);
        this.throttleObserverHandlerId = window.setTimeout(this.resizeHandler.bind(this), ((this.options || {}).reportRate || 250));
    }

    resizeHandler(entries) {
        console.log('Resize Observer called');

        //entires.ForEach(entry => {
        //    console.log('Entry: ', entry);
        //})
    }
}