/// <reference path="../viz/ts/build/linq.d.ts" />
/// <reference path="../viz/ts/build/svg.d.ts" />
declare namespace viz {
    abstract class chart {
        displayId: string;
        size: Canvas.Size | [number, number];
        margin: Canvas.Margin;
        constructor(displayId: string, size?: Canvas.Size | [number, number], margin?: Canvas.Margin);
    }
}
declare namespace viz {
    class areaChart extends chart {
    }
}
declare namespace viz {
    class lineChart {
    }
}
