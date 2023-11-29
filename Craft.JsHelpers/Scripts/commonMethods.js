export function copyToClipboard(text) {
    navigator.clipboard.writeText(text);
}

export function changeCssById(elementId, css) {
    var element = document.getElementById(elementId);
    if (element) {
        element.className = css;
    }
}

export function changeCssBySelector(selector, css) {
    var elements = document.querySelectorAll(selector);
    if (elements) {
        elements.forEach(element => {
            element.className = css;
        });
    }
}

export function updateStyleProperty(elementId, propertyName, value) {
    const element = document.getElementById(elementId);
    if (element) {
        element.style.setProperty(propertyName, value);
    }
}

export function updateStylePropertyBySelector(selector, propertyName, value) {
    const elements = document.querySelectorAll(selector);
    if (elements) {
        elements.forEach(element => {
            element.style.setProperty(propertyName, value);
        });
    }
}

export function changeGlobalCssVariable(name, newValue) {
    document.documentElement.style.setProperty(name, newValue);
}

export function isDarkMode() {
    return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
}

export function scrollToTop() {
    window.scrollTo(0, 0);
}

export function isTouchSupported() {
    return 'ontouchstart' in window || navigator.maxTouchPoints;
}