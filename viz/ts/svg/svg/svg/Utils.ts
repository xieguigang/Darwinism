/**
 * https://stackoverflow.com/questions/5623838/rgb-to-hex-and-hex-to-rgb
 * 
 * @param c The rgb color component numeric value
*/
function componentToHex(c): string {
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}

/**
 * jQuery equivalent on document ready handler.
 * 
 * @param fn function for execute when the html document is load completed.
*/
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

/**
 * https://stackoverflow.com/questions/20539196/creating-svg-elements-dynamically-with-javascript-inside-html
*/
function svgNode(n: string, v: any = null): SVGElement {
    var node = document.createElementNS("http://www.w3.org/2000/svg", n);
    var name = "";

    if (v) {
        for (var p in v) {
            name = p.replace(/[A-Z]/g, function (m, p, o, s) {
                return "-" + m.toLowerCase();
            });
            node.setAttributeNS(null, name, v[p]);
        }
    }

    return node;
}
