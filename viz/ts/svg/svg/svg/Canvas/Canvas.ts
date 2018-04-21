/// <reference path="../Utils.ts"/>

interface ICSSStyle {
    Styling(node: SVGElement): SVGElement;
    CSSStyle(): string;
}

/**
 * The object location data model 
*/
class Point {

    x: number;
    y: number;

    constructor(x: number, y: number) {
        this.x = x;
        this.y = y;
    }
}

class Size {

    width: number;
    height: number;

    constructor(width: number, height: number) {
        this.width = width;
        this.height = height;
    }
}

class Rectangle {

    left: number;
    top: number;
    width: number;
    height: number;

    constructor(x: number, y: number, width: number, height: number) {
        this.left = x;
        this.top = y;
        this.width = width;
        this.height = height;
    }

    Location(): Point {
        return new Point(this.left, this.top);
    }

    Size(): Size {
        return new Size(this.width, this.height);
    }
}

