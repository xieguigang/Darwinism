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
        function chart() {
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