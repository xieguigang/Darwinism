var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../ts/build/linq.d.ts" />
/// <reference path="../ts/build/svg.d.ts" />
var viz;
(function (viz) {
    var chart = /** @class */ (function () {
        function chart(displayId, size, margin) {
            if (size === void 0) { size = [900, 600]; }
            if (margin === void 0) { margin = new Canvas.Margin(20, 20, 30, 30); }
            this.displayId = displayId;
            this.size = size;
            this.margin = margin;
            if (Array.isArray(size)) {
                this.size = new Canvas.Size(size[0], size[1]);
            }
            else {
                this.size = size;
            }
        }
        return chart;
    }());
    viz.chart = chart;
})(viz || (viz = {}));
/// <reference path="./D3Chart.ts" />
var viz;
(function (viz) {
    var areaChart = /** @class */ (function (_super) {
        __extends(areaChart, _super);
        function areaChart() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        return areaChart;
    }(viz.chart));
    viz.areaChart = areaChart;
})(viz || (viz = {}));
var viz;
(function (viz) {
    var lineChart = /** @class */ (function () {
        function lineChart() {
        }
        return lineChart;
    }());
    viz.lineChart = lineChart;
})(viz || (viz = {}));
//# sourceMappingURL=ANN_d3viz.js.map