/// <reference path="svg.ts"/>

$(function () {

    var svg = new Graphics("test-svg")
        .size(1000, 1000)
        .drawRectangle(new Rectangle(10, 20, 200, 300))
        .drawLine(new Pen(Color.Red(), 5), new Point(100, 200), new Point(500, 600));
});


