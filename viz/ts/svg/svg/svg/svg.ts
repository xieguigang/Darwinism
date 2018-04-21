/// <reference path="Utils.ts"/>
/// <reference path="Canvas/Canvas.ts"/>
/// <reference path="Canvas/Pen.ts"/>

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
    drawLine(pen: Pen, a: Canvas.Point, b: Canvas.Point,
        id: string = null,
        className: string = null): Graphics {

        var attrs = {
            "x1": a.x.toString(),
            "y1": a.y.toString(),
            "x2": b.x.toString(),
            "y2": b.y.toString()
        };

        if (id) attrs["id"] = id;
        if (className) attrs["class"] = className;

        var node = pen.Styling(svgNode("line", attrs));
        this.svg.appendChild(node);

        return this;
    }

    drawCircle(center: Canvas.Point, radius: number,
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

        var node = border.Styling(svgNode("circle", attrs));
        this.svg.appendChild(node);

        return this;
    }

    /**
     * Draw a basic svg rectangle shape
    */
    drawRectangle(rect: Canvas.Rectangle,
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

        var node = border.Styling(svgNode("rect", attrs));
        this.svg.appendChild(node);

        return this;
    }
}