namespace Math2D.Geometry {

    /**
     * returns true if two lines intersect, else false
     * from http://paulbourke.net/geometry/lineline2d/
    */
    export function intersect(x1: number, x2: number, x3: number, x4: number,
        y1: number, y2: number, y3: number, y4: number): boolean {

        var mua, mub;
        var denom, numera, numerb;

        denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
        numera = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
        numerb = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);

        /* Is the intersection along the the segments */
        mua = numera / denom;
        mub = numerb / denom;
        if (!(mua < 0 || mua > 1 || mub < 0 || mub > 1)) {
            return true;
        } else {
            return false;
        }
    }
}