class Font implements ICSSStyle {

    size: string;
    family: string;
    bold: boolean;
    italic: boolean;

    constructor(family: string,
        size: any = "12px",
        bold: boolean = false,
        italic: boolean = false) {

        this.size = size;
        this.family = family;
        this.bold = bold;
        this.italic = italic;
    }

    Styling(node: SVGElement): SVGElement {
        var styles = [];

        if (this.bold) styles.push("bold");
        if (this.italic) styles.push("italic");

        node.style.fontFamily = this.family;
        node.style.fontSize = this.size;
        node.style.fontStyle = styles.join(" ");

        return node;
    }

    CSSStyle(): string {
        var styles = [];

        if (this.bold) styles.push("bold");
        if (this.italic) styles.push("italic");

        return `font: ${styles.join(" ")} ${this.size} "${this.family}"`;
    }
}