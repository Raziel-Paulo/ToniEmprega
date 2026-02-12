// wwwroot/js/site.js
function toggleMenu() {
    document.getElementById('navMenu').classList.toggle('active');
}

document.addEventListener('click', function (e) {
    const menu = document.getElementById('navMenu');
    const toggle = document.querySelector('.nav-toggle');
    if (!toggle.contains(e.target) && !menu.contains(e.target)) {
        menu.classList.remove('active');
    }
});