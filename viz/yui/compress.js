var fs = require('fs');
var compressor = require('yuicompressor');
var src = "Projector.js";
var out = "Projector.min.js";

compressor.compress(src, {
    charset: 'utf8'
}, function (err, data, extra) {
    fs.writeFile(out, data, function(){
        console.log('yuicompressor success!');
    });
});