/**
 * https://stackoverflow.com/questions/5623838/rgb-to-hex-and-hex-to-rgb
*/
function componentToHex(c) {
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}
