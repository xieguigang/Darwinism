/// <reference path="svg.ts"/>

$(function () {

    var svg = new Graphics("test-svg")
        .size(1000, 1000)
        .drawRectangle(new Canvas.Rectangle(10, 20, 200, 300))
        .drawLine(new Canvas.Pen(Canvas.Color.Red(), 5), new Canvas.Point(100, 200), new Canvas.Point(500, 600));
});


