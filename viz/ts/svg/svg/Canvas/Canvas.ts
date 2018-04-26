/// <reference path="../Utils.ts"/>

namespace Canvas {

    /**
     * CSS style object model
    */
    export interface ICSSStyle {

        /**
         * Apply CSS style to a given svg node element
         * 
         * @param node a given svg document node object
        */
        Styling(node: SVGElement): SVGElement;
        /**
         * Generate css style string value from this 
         * css style object model.
        */
        CSSStyle(): string;
    }

    /**
     * The object location data model 
    */
    export class Point {

        x: number;
        y: number;

        constructor(x: number, y: number) {
            this.x = x;
            this.y = y;
        }
    }

    export class Size {

        width: number;
        height: number;

        constructor(width: number, height: number) {
            this.width = width;
            this.height = height;
        }
    }

    export class Rectangle {

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
}