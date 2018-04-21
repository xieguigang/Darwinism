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

    size(width: number, height: number): Graphics {
        this.svg.setAttribute("width", width.toString())
        this.svg.setAttribute("height", height.toString());
        return this;
    }

    drawLine(pen: Pen, a: Point, b: Point): Graphics {

        return this;
    }

    drawRectangle(rect: Rectangle, border: Pen = new Pen(Color.Black(), 1), fill: Color = null, id: string = null, className: string = null): Graphics {
        var node = document.createElement("rect");

        node.id = id;
        node.className = className;

        node.setAttribute("x", rect.left.toString());
        node.setAttribute("y", rect.top.toString());
        node.setAttribute("width", rect.width.toString());
        node.setAttribute("height", rect.height.toString());

        if (fill) {
            node.setAttribute("fill", fill.ToHtmlColor());
        }
        if (border) {
            node.style.stroke = border.color.ToHtmlColor();
            node.style.strokeWidth = border.width.toString();
        }

        this.svg.appendChild(node);

        return this;
    }
}

class Pen {

    color: Color;
    width: number;

    constructor(color: Color, width: number = 1) {
        this.color = color;
        this.width = width;
    }

    CSSStyle(): string {
        return `stroke-width:${this.width};stroke:${this.color.ToHtmlColor()};`;
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

    /**
     * https://stackoverflow.com/questions/5623838/rgb-to-hex-and-hex-to-rgb
    */
    static FromHtmlColor(htmlColor: string): Color {
        // Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")
        var hex = htmlColor;
        var shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;

        hex = hex.replace(shorthandRegex, function (m, r, g, b) {
            return r + r + g + g + b + b;
        });

        var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);

        return result ? new Color(
            parseInt(result[1], 16),
            parseInt(result[2], 16),
            parseInt(result[3], 16)
        ) : null;
    }

    static Black(): Color {
        return new Color(0, 0, 0);
    }

    static White(): Color {
        return new Color(255, 255, 255);
    }

    static Red(): Color {
        return new Color(255, 0, 0);
    }

    static Green(): Color {
        return new Color(0, 255, 0);
    }

    static Blue(): Color {
        return new Color(0, 0, 255);
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