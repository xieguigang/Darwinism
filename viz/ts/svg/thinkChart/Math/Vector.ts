/// <reference path="../../../build/linq.d.ts"/>

namespace MathV {

    export class Vector extends IEnumerator<number> {

        public get Dim(): number {
            return this.sequence.length;
        }

        public constructor(x: number[] | IEnumerator<number>) {
            super(x);
        }

        public static Zero(dim: number): Vector {
            return this.New(0, dim);
        }

        public static One(dim: number): Vector {
            return this.Zero(dim).Add(1);
        }

        public static New(n: number, dim: number): Vector {
            var x: number[] = [];

            for (var i: number = 0; i < dim; i++) {
                x.push(n);
            }

            return new Vector(x);
        }

        /**
         * 符号取反
        */
        public Negative(): Vector {
            // 0 - x
            // 零是在左边的，所以在这里是 y - x
            return XVectorMath(this, 0, (y, x) => x - y);
        }

        public Add(x: number | IEnumerator<number> | Vector): Vector {
            return XVectorMath(this, x, (x, y) => x + y);
        }

        public Multiply(x: number | IEnumerator<number> | Vector): Vector {
            return XVectorMath(this, x, (x, y) => x * y);
        }

        public Subtract(x: number | IEnumerator<number> | Vector): Vector {
            return XVectorMath(this, x, (x, y) => x - y);
        }

        public Divide(x: number | IEnumerator<number> | Vector): Vector {
            return XVectorMath(this, x, (x, y) => x / y);
        }
    }
}