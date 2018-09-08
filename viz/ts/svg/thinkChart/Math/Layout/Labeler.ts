namespace Layout {

    export class Labeler {

        private lab: Label[] = [];
        private anc: Anchor[] = [];
        private w = 1; // box width
        private h = 1; // box width

        private max_move = 5.0;
        private max_angle = 0.5;
        private acc = 0;
        private rej = 0;

        //#region ""
        private weights: Weights;
        private energy_function: (index: number, lab: Label[], anc: Anchor[]) => number;
        private schedule_function: (currT: number, initialT: number, nsweeps: number) => number;
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
        energy(index: number): number {
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
            if (dx > 0 && dy > 0) { ener += 0 * w.orient; }
            else if (dx < 0 && dy > 0) { ener += 1 * w.orient; }
            else if (dx < 0 && dy < 0) { ener += 2 * w.orient; }
            else { ener += 3 * w.orient; }

            var x21 = lab[index].left,
                y21 = lab[index].top - lab[index].height + 2.0,
                x22 = lab[index].left + lab[index].width,
                y22 = lab[index].top + 2.0;
            var x11, x12, y11, y12, x_overlap, y_overlap, overlap_area;

            for (var i = 0; i < m; i++) {
                if (i != index) {

                    // penalty for intersection of leader lines
                    overlap = Math2D.Geometry.intersect(anc[index].x, lab[index].left, anc[i].x, lab[i].left,
                        anc[index].y, lab[index].top, anc[i].y, lab[i].top);
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

        /**
         * Monte Carlo translation move
        */
        mcmove(currT: number) {
            var lab = this.lab;
            var anc = this.anc;

            // select a random label
            var i = Math.floor(Math.random() * lab.length);

            // save old coordinates
            var x_old = lab[i].left;
            var y_old = lab[i].top;

            // old energy
            var old_energy = this.energy_function(i, lab, anc);

            // random translation
            lab[i].left += (Math.random() - 0.5) * this.max_move;
            lab[i].top += (Math.random() - 0.5) * this.max_move;

            // hard wall boundaries
            if (lab[i].left > this.w) lab[i].left = x_old;
            if (lab[i].left < 0) lab[i].left = x_old;
            if (lab[i].top > this.h) lab[i].top = y_old;
            if (lab[i].top < 0) lab[i].top = y_old;

            // new energy
            var new_energy = this.energy_function(i, lab, anc);

            // delta E
            var delta_energy = new_energy - old_energy;

            if (Math.random() < Math.exp(-delta_energy / currT)) {
                this.acc += 1;
            } else {
                // move back to old coordinates
                lab[i].left = x_old;
                lab[i].top = y_old;
                this.rej += 1;
            }
        }

        /**
         * Monte Carlo rotation move
        */
        mcrotate(currT: number) {
            var lab = this.lab;
            var anc = this.anc;

            // select a random label
            var i = Math.floor(Math.random() * lab.length);

            // save old coordinates
            var x_old = lab[i].left;
            var y_old = lab[i].top;

            // old energy
            var old_energy = this.energy_function(i, lab, anc);

            // random angle
            var angle = (Math.random() - 0.5) * this.max_angle;

            var s = Math.sin(angle);
            var c = Math.cos(angle);

            // translate label (relative to anchor at origin):
            lab[i].left -= anc[i].x
            lab[i].top -= anc[i].y

            // rotate label
            var x_new = lab[i].left * c - lab[i].top * s,
                y_new = lab[i].left * s + lab[i].top * c;

            // translate label back
            lab[i].left = x_new + anc[i].x
            lab[i].top = y_new + anc[i].y

            // hard wall boundaries
            if (lab[i].left > this.w) lab[i].left = x_old;
            if (lab[i].left < 0) lab[i].left = x_old;
            if (lab[i].top > this.h) lab[i].top = y_old;
            if (lab[i].top < 0) lab[i].top = y_old;

            // new energy
            var new_energy = this.energy_function(i, lab, anc)

            // delta E
            var delta_energy = new_energy - old_energy;

            if (Math.random() < Math.exp(-delta_energy / currT)) {
                this.acc += 1;
            } else {
                // move back to old coordinates
                lab[i].left = x_old;
                lab[i].top = y_old;
                this.rej += 1;
            }

        }

        /**
         * main simulated annealing function
        */
        public start(nsweeps: number): Labeler {
            var m = this.lab.length,
                currT = 1.0,
                initialT = 1.0;

            for (var i = 0; i < nsweeps; i++) {
                for (var j = 0; j < m; j++) {
                    if (Math.random() < 0.5) { this.mcmove(currT); }
                    else { this.mcrotate(currT); }
                }
                currT = this.schedule_function(currT, initialT, nsweeps);
            }

            return this;
        }

        width(x) {
            // users insert graph width
            if (!arguments.length) return w;
            w = x;
            return labeler;
        }

        height(x) {
            // users insert graph height
            if (!arguments.length) return h;
            h = x;
            return labeler;
        }

        label(x) {
            // users insert label positions
            if (!arguments.length) return lab;
            lab = x;
            return labeler;
        }

        anchor(x) {
            // users insert anchor positions
            if (!arguments.length) return anc;
            anc = x;
            return labeler;
        }
    }
}