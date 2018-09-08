declare class threeApp {
    container: HTMLDivElement;
    camera: THREE.PerspectiveCamera;
    scene: THREE.Scene;
    renderer: THREE.CanvasRenderer;
    group: THREE.Group;
    mouseX: number;
    mouseY: number;
    windowHalfX: number;
    windowHalfY: number;
    constructor(containerId?: string);
    init(): void;
    onWindowResize(): void;
    onDocumentMouseMove(event: MouseEvent): void;
    animate(): void;
    render(): void;
}
