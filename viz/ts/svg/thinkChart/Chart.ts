/// <reference path="../../build/svg.d.ts"/>

namespace Chart {

    /**
     * 得到浏览器的窗口大小
    */
    export function windowSize(): Canvas.Size {
        return new Canvas.Size(
            window.innerWidth,
            window.innerHeight
        );
    }
}