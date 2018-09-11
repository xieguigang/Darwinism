namespace Layout {

    export class Anchor extends Canvas.Point {

        /**
         * Radius
        */
        public r: number;

        public constructor(x: number, y: number, radius: number) {
            super(x, y);

            this.r = radius;
        }
    }
}