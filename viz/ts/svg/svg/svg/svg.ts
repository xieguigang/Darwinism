/// <reference path="Utils.ts"/>
/// <reference path="Canvas.ts"/>

/**
 * 提供类似于VB.NET之中的Graphics对象的模拟
*/
class Graphics {

    svg: SVGElement;
    container: HTMLElement;

    /**
     * 创建一个SVG画布对象
     * 
     * @param div div id
    */
    constructor(div: string) {
        this.svg = svgNode("svg", { "version": "1.1" });
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

    /**
     * Draw a basic svg line shape
     * 
     * @param pen Defines the line border: color and line width
    */
    drawLine(pen: Pen, a: Point, b: Point,
        id: string = null,
        className: string = null): Graphics {

        var attrs = {
            "x1": a.x.toString(),
            "y1": a.y.toString(),
            "x2": b.x.toString(),
            "y2": b.y.toString(),
            "stroke": pen.color.ToHtmlColor()
        };

        if (id) attrs["id"] = id;
        if (className) attrs["class"] = className;

        var node = svgNode("line", attrs);

        node.style.strokeWidth = pen.width.toString();

        this.svg.appendChild(node);

        return this;
    }

    drawCircle(center: Point, radius: number,
        border: Pen = new Pen(Color.Black(), 1),
        fill: Color = null,
        id: string = null,
        className: string = null): Graphics {

        var attrs = {
            "cx": center.x.toString(),
            "cy": center.y.toString(),
            "r": radius
        };

        if (id) attrs["id"] = id;
        if (className) attrs["class"] = className;
        if (fill) attrs["fill"] = fill.ToHtmlColor();

        var node = svgNode("circle", attrs);

        if (border) {
            node.style.stroke = border.color.ToHtmlColor();
            node.style.strokeWidth = border.width.toString();
        }

        this.svg.appendChild(node);

        return this;
    }

    /**
     * Draw a basic svg rectangle shape
    */
    drawRectangle(rect: Rectangle,
        border: Pen = new Pen(Color.Black(), 1),
        fill: Color = null,
        id: string = null,
        className: string = null): Graphics {

        var attrs = {
            "x": rect.left.toString(),
            "y": rect.top.toString(),
            "width": rect.width.toString(),
            "height": rect.height.toString()
        };

        if (id) attrs["id"] = id;
        if (className) attrs["class"] = className;
        if (fill) attrs["fill"] = fill.ToHtmlColor();

        var node = svgNode("rect", attrs);

        if (border) {
            node.style.stroke = border.color.ToHtmlColor();
            node.style.strokeWidth = border.width.toString();
        }

        this.svg.appendChild(node);

        return this;
    }
}