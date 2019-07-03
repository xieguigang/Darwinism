namespace viz {

    export abstract class chart {

        public constructor(
            public displayId: string,
            public size: Canvas.Size | [number, number] = [900, 600],
            public margin: Canvas.Margin = new Canvas.Margin(20, 20, 30, 30)) {

            if (Array.isArray(size)) {
                this.size = new Canvas.Size(size[0], size[1]);
            } else {
                this.size = size;
            }
        }

    }
}