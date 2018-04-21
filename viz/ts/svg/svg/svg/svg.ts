/// <reference path="Utils.ts"/>
/// <reference path="Canvas.ts"/>

/**
 * 提供类似于VB.NET之中的Graphics对象的模拟
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
        this.svg.setAttribute("version", "1.1");
        this.svg.setAttribute("xmlns", "http://www.w3.org/2000/svg");
        this.svg.setAttribute("xmlns:dc", "http://purl.org/dc/elements/1.1/");
        this.svg.setAttribute("xmlns:cc", "http://creativecommons.org/ns#");
        this.svg.setAttribute("xmlns:rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
        this.svg.setAttribute("xmlns:svg", "http://www.w3.org/2000/svg");
        this.svg.setAttribute("xmlns:xlink", "http://www.w3.org/1999/xlink");

        this.container = document.getElementById(div);
        this.container.appendChild(this.svg);

        console.log(div);
        console.log(this.svg);
        console.log(this.container);
    }

    /**
     * Set the size value of the svg canvas
    */
    size(width: number, height: number): Graphics {
        this.svg.setAttribute("width", width.toString() + "px");
        this.svg.setAttribute("height", height.toString() + "px");
        return this;
    }

    drawLine(pen: Pen, a: Point, b: Point, id: string = null, className: string = null): Graphics {

        return this;
    }

    drawRectangle(rect: Rectangle, border: Pen = new Pen(Color.Black(), 1), fill: Color = null, id: string = null, className: string = null): Graphics {
        var node = document.createElement("rect");

        if (id) node.id = id;
        if (className) node.className = className;

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