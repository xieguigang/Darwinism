var fs = require('fs');
var compressor = require('yuicompressor');
var src = "CanvasRenderer.js";
var out = "CanvasRenderer.min.js";

compressor.compress(src, {
    charset: 'utf8'
}, function (err, data, extra) {
    fs.writeFile(out, data, function(){
        console.log('yuicompressor success!');
    });
});