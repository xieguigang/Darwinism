var threeApp = /** @class */ (function () {
    function threeApp(containerId) {
        if (containerId === void 0) { containerId = null; }
        this.mouseX = 0;
        this.mouseY = 0;
        this.windowHalfX = window.innerWidth / 2;
        this.windowHalfY = window.innerHeight / 2;
        var app = this;
        this.container = document.createElement('div');
        document.addEventListener('mousemove', function (event) {
            app.onDocumentMouseMove(event);
        }, false);
        window.addEventListener('resize', function () {
            app.onWindowResize();
        }, false);
        if (containerId) {
            document.getElementById(containerId).appendChild(this.container);
        }
        else {
            document.body.appendChild(this.container);
        }
        this.init();
        this.animate();
    }
    threeApp.prototype.init = function () {
        this.camera = new THREE.PerspectiveCamera(60, window.innerWidth / window.innerHeight, 1, 10000);
        this.camera.position.z = 500;
        this.scene = new THREE.Scene();
        this.scene.background = new THREE.Color(0xffffff);
        var geometry = new THREE.BoxBufferGeometry(100, 100, 100);
        var material = new THREE.MeshNormalMaterial({ overdraw: 0.5 });
        this.group = new THREE.Group();
        for (var i = 0; i < 200; i++) {
            var mesh = new THREE.Mesh(geometry, material);
            mesh.position.x = Math.random() * 2000 - 1000;
            mesh.position.y = Math.random() * 2000 - 1000;
            mesh.position.z = Math.random() * 2000 - 1000;
            mesh.rotation.x = Math.random() * 2 * Math.PI;
            mesh.rotation.y = Math.random() * 2 * Math.PI;
            mesh.matrixAutoUpdate = false;
            mesh.updateMatrix();
            this.group.add(mesh);
        }
        this.scene.add(this.group);
        this.renderer = new THREE.CanvasRenderer();
        this.renderer.setPixelRatio(window.devicePixelRatio);
        this.renderer.setSize(window.innerWidth, window.innerHeight);
        this.container.appendChild(this.renderer.domElement);
    };
    threeApp.prototype.onWindowResize = function () {
        this.windowHalfX = window.innerWidth / 2;
        this.windowHalfY = window.innerHeight / 2;
        this.camera.aspect = window.innerWidth / window.innerHeight;
        this.camera.updateProjectionMatrix();
        this.renderer.setSize(window.innerWidth, window.innerHeight);
    };
    threeApp.prototype.onDocumentMouseMove = function (event) {
        this.mouseX = (event.clientX - this.windowHalfX) * 10;
        this.mouseY = (event.clientY - this.windowHalfY) * 10;
    };
    threeApp.prototype.animate = function () {
        var app = this;
        requestAnimationFrame(function () {
            app.animate();
        });
        this.render();
    };
    threeApp.prototype.render = function () {
        this.camera.position.x += (this.mouseX - this.camera.position.x) * .05;
        this.camera.position.y += (-this.mouseY - this.camera.position.y) * .05;
        this.camera.lookAt(this.scene.position);
        var currentSeconds = Date.now();
        this.group.rotation.x = Math.sin(currentSeconds * 0.0007) * 0.5;
        this.group.rotation.y = Math.sin(currentSeconds * 0.0003) * 0.5;
        this.group.rotation.z = Math.sin(currentSeconds * 0.0002) * 0.5;
        this.renderer.render(this.scene, this.camera);
    };
    return threeApp;
}());
//# sourceMappingURL=3DApp.js.map