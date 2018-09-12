/// <reference path="../../../build/linq.d.ts"/>

namespace Math2D {

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
            return this.mathInternal(0, (x, y) => x - y);
        }

        public Add(x: number | IEnumerator<number> | Vector): Vector {
            return this.mathInternal(x, (x, y) => x + y);
        }

        public Multiply(x: number | IEnumerator<number> | Vector): Vector {
            return this.mathInternal(x, (x, y) => x * y);
        }

        public Subtract(x: number | IEnumerator<number> | Vector): Vector {
            return this.mathInternal(x, (x, y) => x - y);
        }

        public Divide(x: number | IEnumerator<number> | Vector): Vector {
            return this.mathInternal(x, (x, y) => x / y);
        }

        /**
         * 向量运算实现的通用过程: ``this op x``
         * 
         * @param x 任意实数或者一个与当前向量等长的实数序列
         * 
         * @returns 一个新的向量对象
        */
        private mathInternal(
            x: number | IEnumerator<number> | Vector,
            op: (x: number, y: number) => number): Vector {

            var type = TypeInfo.typeof(x);
            var out: number[] = [];

            if (type.typeOf == "number") {
                x = <number>x;

                for (var i: number = 0; i < this.Count; i++) {
                    out.push(op(this.sequence[i], x));
                }
            } else if (type.IsEnumerator || type.class == "Vector") {
                x = <IEnumerator<number>>x;

                if (x.Count != this.Count) {
                    throw `Two sequence length not agree!`;
                }

                for (var i: number = 0; i < this.Count; i++) {
                    out.push(op(this.sequence[i], x.ElementAt(i)));
                }
            } else {
                throw `Unsupported type: ${type.class}!`;
            }

            return new Vector(out);
        }
    }
}