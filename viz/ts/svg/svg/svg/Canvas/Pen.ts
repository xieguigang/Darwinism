/**
 * The css border style
*/
class Pen implements ICSSStyle {

    color: Color;
    width: number;

    /**
     * Create a new css border style for svg rectangle, line, etc.
     * 
     * @param color The border color
     * @param width The border width
    */
    constructor(color: Color, width: number = 1) {
        this.color = color;
        this.width = width;
    }

    Styling(node: SVGElement): SVGElement {
        node.style.stroke = this.color.ToHtmlColor();
        node.style.strokeWidth = this.width.toString();

        return node;
    }

    CSSStyle(): string {
        return `stroke-width:${this.width};stroke:${this.color.ToHtmlColor()};`;
    }
}