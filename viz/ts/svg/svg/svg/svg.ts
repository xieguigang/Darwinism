/// <reference path="Utils.ts"/>

/**
 * 提供类似于VB.NET之中的Graphics对象的模拟
 * 
*/
class Graphics {

    svg: HTMLElement;
    container: HTMLElement;

    /**
     * 创建一个SVG画布对象
     * 
     * @param div div id
    */ 
    constructor(div: string) {
        this.svg = document.createElement("svg");
        this.container = document.getElementById(div);
        this.container.appendChild(this.svg);
    }

    drawLine(pen: Pen, a: Point, b: Point) {

    }

    drawRectangle(rect: Rectangle, border: Pen, fill: Color = null) {

    }
}

class Pen {

    color: Color;
    width: number;

    constructor(color: Color, width: number = 1) {
        this.color = color;
        this.width = width;
    }
}

class Color {

    r: number;
    g: number;
    b: number;

    constructor(r: number, g: number, b: number) {
        this.r = r;
        this.g = g;
        this.b = b;
    }

    ToHtmlColor(): string {
        return "#" + componentToHex(this.r) + componentToHex(this.g) + componentToHex(this.b);
    }

    ToRGBColor(): string {
        return `rgb(${this.r},${this.g},${this.b})`;
    }
}

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