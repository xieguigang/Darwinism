/// <reference path="../../../../build/svg.d.ts"/>

namespace Layout {

    export class Label extends Canvas.Rectangle {

        public text: string;

        public get x(): number {
            return this.left;
        }

        public get y(): number {
            return this.top;
        }

        public constructor(geo2D: Canvas.Rectangle, label: string) {
            super(geo2D.left, geo2D.top, geo2D.width, geo2D.height);

            this.text = label;
        }

        public toString() {
            return this.text;
        }
    }
}