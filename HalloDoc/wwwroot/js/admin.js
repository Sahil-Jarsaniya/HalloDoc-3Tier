//active nav links
var navLinks = document.querySelectorAll('nav a');

navLinks.forEach(link => {
    link.addEventListener('click', function () {
        navLinks.forEach(link => link.classList.remove('active'));

        this.classList.add('active');
    });
});


//dark mode

document.getElementById('light-dark-btn').addEventListener('click', () => {
    if (document.documentElement.getAttribute('data-bs-theme') == 'dark') {
        localStorage.setItem("PageTheme", "light");
        document.documentElement.setAttribute('data-bs-theme', 'light')
    }
    else {
        localStorage.setItem("PageTheme", "dark");
        document.documentElement.setAttribute('data-bs-theme', 'dark')
    }
})

if (localStorage.getItem("PageTheme") === "light") {
    document.documentElement.setAttribute('data-bs-theme', 'light')
}
else {
    document.documentElement.setAttribute('data-bs-theme', 'dark')
}
