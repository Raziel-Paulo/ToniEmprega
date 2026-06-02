// wwwroot/js/site.js
function toggleMenu() {
    const menu = document.getElementById('navMenu');

    if (menu) {
        menu.classList.toggle('active');
    }
}

document.addEventListener('click', function (e) {
    const menu = document.getElementById('navMenu');
    const toggle = document.querySelector('.nav-toggle');

    if (!menu || !toggle) {
        return;
    }

    if (!toggle.contains(e.target) && !menu.contains(e.target)) {
        menu.classList.remove('active');
    }
});