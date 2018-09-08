/// <reference path="../../../../build/svg.d.ts"/>

namespace Layout {

    export class Label extends Canvas.Rectangle {

        public text: string;

        public toString() {
            return this.text;
        }
    }
}