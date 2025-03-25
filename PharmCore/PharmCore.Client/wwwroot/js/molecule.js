class Molecule {
    constructor(svg, container) {
        this.svg = svg;
        this.container = container;

        this.x = Math.random() * container.clientWidth;
        this.y = Math.random() * container.clientHeight;
        this.vx = (Math.random() - 0.5) * 1.5;
        this.vy = (Math.random() - 0.5) * 1.5;
        this.size = Math.random() * 20 + 10; // Random size between 10 and 30
        this.repulsionStrength = 80; // Base repulsion force

        this.createElement();
    }

    createElement() {
        this.element = document.createElementNS("http://www.w3.org/2000/svg", "circle");
        this.element.setAttribute("r", this.size);
        this.element.setAttribute("fill", `hsl(${Math.random() * 360}, 80%, 60%)`);
        this.svg.appendChild(this.element);
        this.updatePosition();
    }

    updatePosition() {
        this.element.setAttribute("cx", this.x);
        this.element.setAttribute("cy", this.y);
    }

    applyForce(fx, fy) {
        this.vx += fx;
        this.vy += fy;
    }

    update(molecules, mouse) {
        // Move naturally
        this.x += this.vx;
        this.y += this.vy;
        this.vx *= 0.98; // Friction
        this.vy *= 0.98;

        // Bounce off edges
        if (this.x - this.size < 0 || this.x + this.size > this.container.clientWidth) this.vx *= -1;
        if (this.y - this.size < 0 || this.y + this.size > this.container.clientHeight) this.vy *= -1;

        // Mouse repulsion effect
        if (mouse.active) {
            let dx = this.x - mouse.x;
            let dy = this.y - mouse.y;
            let dist = Math.sqrt(dx * dx + dy * dy);
            if (dist < 100) {
                let force = this.repulsionStrength / dist;
                this.applyForce((dx / dist) * force, (dy / dist) * force);
            }
        }

        // Repulsion between molecules
        for (let other of molecules) {
            if (other !== this) {
                let dx = this.x - other.x;
                let dy = this.y - other.y;
                let dist = Math.sqrt(dx * dx + dy * dy);
                if (dist < this.size * 2) {
                    let force = 2 / dist;
                    this.applyForce((dx / dist) * force, (dy / dist) * force);
                }
            }
        }

        this.updatePosition();
    }
}

// Main logic
document.addEventListener("DOMContentLoaded", () => {
    const container = document.body;
    const svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
    svg.setAttribute("width", "100%");
    svg.setAttribute("height", "100%");
    svg.style.position = "absolute";
    svg.style.top = "0";
    svg.style.left = "0";
    svg.style.zIndex = "-1"; // Behind content
    container.appendChild(svg);

    const molecules = [];
    const mouse = { x: 0, y: 0, active: false };

    for (let i = 0; i < 20; i++) {
        molecules.push(new Molecule(svg, container));
    }

    container.addEventListener("mousemove", (e) => {
        mouse.x = e.clientX;
        mouse.y = e.clientY;
        mouse.active = true;
    });

    container.addEventListener("mouseleave", () => {
        mouse.active = false;
    });

    function animate() {
        molecules.forEach(molecule => molecule.update(molecules, mouse));
        requestAnimationFrame(animate);
    }

    animate();
});
