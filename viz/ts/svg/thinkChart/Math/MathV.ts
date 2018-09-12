namespace MathV {

    export function Add(x: number | IEnumerator<number>, y: number | IEnumerator<number>): Vector {
        return mathInternal(x, y, (x, y) => x + y);
    }

    export function Subtract(x: number | IEnumerator<number>, y: number | IEnumerator<number>): Vector {
        return mathInternal(x, y, (x, y) => x - y);
    }

    function mathInternal(x: number | IEnumerator<number>, y: number | IEnumerator<number>, op: (x: number, y: number) => number): Vector {
        var typeofX = TypeInfo.typeof(x);
        var typeofY = TypeInfo.typeof(y);

        if (typeofX.typeOf == "number") {
            x = <number>x;

            if (typeofY.typeOf == "number") {
                return new Vector([op(x, <number>y)]);
            } else if (typeofY.IsEnumerator || typeofY.class == "Vector") {
                return XNumberYVector(x, <IEnumerator<number>>y, op);
            } else {
                throw `typeof y = '${typeofX.class}' is not supported!`;
            }

        } else if (typeofX.IsEnumerator) {
            return XVectorMath(<IEnumerator<number>>x, y, op);
        } else {
            throw `typeof x = '${typeofX.class}' is not supported!`;
        }
    }

    function XNumberYVector(x: number, y: IEnumerator<number>, op: (x: number, y: number) => number): Vector {
        var vy: number[] = y.ToArray();

        for (var i: number = 0; i < y.Count; i++) {
            vy[i] = op(x, vy[i]);
        }

        return new Vector(vy);
    }

    /**
     * 向量运算实现的通用过程: ``this op x``
     * 
     * @param x 任意实数或者一个与当前向量等长的实数序列
     * 
     * @returns 一个新的向量对象
    */
    export function XVectorMath(
        x: IEnumerator<number>,
        y: number | IEnumerator<number>,
        op: (x: number, y: number) => number): Vector {

        var type = TypeInfo.typeof(y);
        var out: number[] = [];
        var vx: number[] = x.ToArray();

        if (type.typeOf == "number") {
            y = <number>y;

            for (var i: number = 0; i < this.Count; i++) {
                out.push(op(vx[i], y));
            }
        } else if (type.IsEnumerator || type.class == "Vector") {
            var vy = (<IEnumerator<number>>y).ToArray();

            if (x.Count != vy.length) {
                throw `Two sequence length not agree [x=${x.Count}, y=${vy.length}]!`;
            }

            for (var i: number = 0; i < this.Count; i++) {
                out.push(op(vx[i], vy[i]));
            }
        } else {
            throw `Unsupported type: '${type.class}'!`;
        }

        return new Vector(out);
    }
}