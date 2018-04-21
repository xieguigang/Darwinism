/// <reference path="Utils.ts"/>
/// <reference path="Canvas.ts"/>

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

        console.log(div);
        console.log(this.svg);
        console.log(this.container);

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