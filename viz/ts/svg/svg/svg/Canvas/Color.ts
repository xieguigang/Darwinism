namespace Canvas {

    /**
     * RGB color data model
    */
    export class Color {

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
}