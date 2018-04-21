/**
 * https://stackoverflow.com/questions/5623838/rgb-to-hex-and-hex-to-rgb
*/
function componentToHex(c) {
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}

var $ = (fn: any) => {

    // Sanity check
    if (typeof fn !== 'function') return;

    // If document is already loaded, run method
    if (document.readyState === 'complete') {
        return fn();
    } else {
        // Otherwise, wait until document is loaded
        document.addEventListener('DOMContentLoaded', fn, false);
    }
};