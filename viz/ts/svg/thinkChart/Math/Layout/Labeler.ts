namespace Layout {

    export class Labeler {

        public lab: Label[] = [];
        public anc: Anchor[] = [];

        //#region "box size"

        /**
         * box width
        */
        private w: number = 1;
        /**
         * box height
        */
        private h: number = 1;

        //#endregion

        //#region ""
        private weights: Weights;

        public readonly energy_function: (index: number, lab: Label[], anc: Anchor[]) => number;
        public readonly schedule_function: (currT: number, initialT: number, nsweeps: number) => number;
        //#endregion

        public constructor(
            weights: Weights = new Weights(),
            energy: (index: number, lab: Label[], anc: Anchor[]) => number = null,
            schedule: (currT: number, initialT: number, nsweeps: number) => number = Labeler.cooling_schedule) {

            this.energy_function = energy == null ? this.energy : energy;
            this.schedule_function = schedule;
            this.weights = weights;
        }

        /**
         * linear cooling
        */
        private static cooling_schedule(currT: number, initialT: number, nsweeps: number): number {
            return (currT - (initialT / nsweeps));
        }

        /**
         * energy function, tailored for label placement
        */
        private energy(index: number): number {
            var lab = this.lab;
            var anc = this.anc;
            var w = this.weights;
            var m = lab.length,
                ener = 0,
                dx = lab[index].left - anc[index].x,
                dy = anc[index].y - lab[index].top,
                dist = Math.sqrt(dx * dx + dy * dy),
                overlap = true,
                amount = 0;
            var theta = 0;

            // penalty for length of leader line
            if (dist > 0) ener += dist * w.len;

            // label orientation bias
            dx /= dist;
            dy /= dist;

            if (dx > 0 && dy > 0) {
                ener += 0 * w.orient;
            } else if (dx < 0 && dy > 0) {
                ener += 1 * w.orient;
            } else if (dx < 0 && dy < 0) {
                ener += 2 * w.orient;
            } else {
                ener += 3 * w.orient;
            }

            var x21 = lab[index].left,
                y21 = lab[index].top - lab[index].height + 2.0,
                x22 = lab[index].left + lab[index].width,
                y22 = lab[index].top + 2.0;
            var x11, x12, y11, y12, x_overlap, y_overlap, overlap_area;

            for (var i = 0; i < m; i++) {
                if (i != index) {

                    // penalty for intersection of leader lines
                    overlap = Math2D.Geometry.intersect(
                        anc[index].x, lab[index].left, anc[i].x, lab[i].left,
                        anc[index].y, lab[index].top, anc[i].y, lab[i].top
                    );
                    if (overlap) ener += w.inter;

                    // penalty for label-label overlap
                    x11 = lab[i].left;
                    y11 = lab[i].top - lab[i].height + 2.0;
                    x12 = lab[i].left + lab[i].width;
                    y12 = lab[i].top + 2.0;
                    x_overlap = Math.max(0, Math.min(x12, x22) - Math.max(x11, x21));
                    y_overlap = Math.max(0, Math.min(y12, y22) - Math.max(y11, y21));
                    overlap_area = x_overlap * y_overlap;
                    ener += (overlap_area * w.lab2);
                }

                // penalty for label-anchor overlap
                x11 = anc[i].x - anc[i].r;
                y11 = anc[i].y - anc[i].r;
                x12 = anc[i].x + anc[i].r;
                y12 = anc[i].y + anc[i].r;
                x_overlap = Math.max(0, Math.min(x12, x22) - Math.max(x11, x21));
                y_overlap = Math.max(0, Math.min(y12, y22) - Math.max(y11, y21));
                overlap_area = x_overlap * y_overlap;
                ener += (overlap_area * w.lab_anc);
            }

            return ener;
        }

        //#region "public interface"

        /**
         * main simulated annealing function
        */
        public start(nsweeps: number): Labeler {
            var m: number = this.lab.length,
                currT = 1.0,
                initialT = 1.0;
            var monteCarlo = new MonteCarlo(this.w, this.h, this);

            for (var i = 0; i < nsweeps; i++) {
                for (var j = 0; j < m; j++) {
                    if (Math.random() < 0.5) {
                        monteCarlo.Move(currT);
                    } else {
                        monteCarlo.Rotate(currT);
                    }
                }
                currT = this.schedule_function(currT, initialT, nsweeps);
            }

            return this;
        }

        /**
         * users insert graph width
        */
        public width(x: number): Labeler {
            this.w = x;
            return this;
        }

        /**
         * users insert graph height
        */
        public height(x: number): Labeler {
            this.h = x;
            return this;
        }

        /**
         * users insert label positions
        */
        public label(x: Label[]): Labeler {
            this.lab = x;
            return this;
        }

        /**
         * users insert anchor positions
        */
        public anchor(x: Anchor[]): Labeler {
            this.anc = x;
            return this;
        }

        //#endregion
    }
}