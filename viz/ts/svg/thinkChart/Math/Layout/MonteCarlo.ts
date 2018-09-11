namespace Layout {

    /**
     * 极性退火布局的蒙特卡洛计算模块
    */
    export class MonteCarlo {

        public labeler: Labeler;

        public max_move: number = 5.0;
        public max_angle: number = 0.5;
        public acc: number = 0;
        public rej: number = 0;

        private w: number;
        private h: number;

        public constructor(width: number, height: number, labeler: Labeler) {
            this.w = width;
            this.h = height;
            this.labeler = labeler;
        }

        /**
         * Monte Carlo translation move
        */
        public Move(currT: number) {
            var lab = this.labeler.lab;
            var anc = this.labeler.anc;

            // select a random label
            var i = Math.floor(Math.random() * lab.length);

            // save old coordinates
            var x_old = lab[i].left;
            var y_old = lab[i].top;

            // old energy
            var old_energy = this.labeler.energy_function(i, lab, anc);

            // random translation
            lab[i].left += (Math.random() - 0.5) * this.max_move;
            lab[i].top += (Math.random() - 0.5) * this.max_move;

            // hard wall boundaries
            if (lab[i].left > this.w) lab[i].left = x_old;
            if (lab[i].left < 0) lab[i].left = x_old;
            if (lab[i].top > this.h) lab[i].top = y_old;
            if (lab[i].top < 0) lab[i].top = y_old;

            // new energy
            var new_energy = this.labeler.energy_function(i, lab, anc);

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
        public Rotate(currT: number) {
            var lab = this.labeler.lab;
            var anc = this.labeler.anc;

            // select a random label
            var i = Math.floor(Math.random() * lab.length);

            // save old coordinates
            var x_old = lab[i].left;
            var y_old = lab[i].top;

            // old energy
            var old_energy = this.labeler.energy_function(i, lab, anc);

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
            var new_energy = this.labeler.energy_function(i, lab, anc)

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
    }
}