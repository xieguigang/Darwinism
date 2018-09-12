/// <reference path="../../../build/linq.d.ts"/>

namespace Math2D {

    export class Vector extends IEnumerator<number> {

        public constructor(x: number[] | IEnumerator<number>) {
            super(x);
        }

        public Add(x: number | IEnumerator<number> | Vector): Vector {
            var type = TypeInfo.typeof(x);
            var out: number[] = [];

            if (type.typeOf == "number") {
                x = <number>x;

                for (var i: number = 0; i < this.Count; i++) {
                    out.push(x + this.sequence[i]);
                }
            } else if (type.IsEnumerator || type.class == "Vector") {
                x = <IEnumerator<number>>x;

                if (x.Count != this.Count) {
                    throw `Two sequence length not agree!`;
                }

                for (var i: number = 0; i < this.Count; i++) {
                    out.push(x.ElementAt(i) + this.sequence[i]);
                }
            } else {
                throw `Unsupported type: ${type.class}!`;
            }

            return new Vector(out);
        }
    }
}