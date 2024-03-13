
document.addEventListener("DOMContentLoaded", function () {
    // Get all the anchor tags inside the nav
    var navLinks = document.querySelectorAll('#adminNav .nav-link');

    // Function to remove 'active' class from all links
    function removeAllActive() {
        navLinks.forEach(function (link) {
            link.classList.remove('active');
        });
    }

    // Function to set the 'active' class to the clicked link and store it in localStorage
    function setActiveLink(link) {
        link.classList.add('active');
        localStorage.setItem('activeLink', link.getAttribute('href'));
    }

    // Check if there's an active link stored in localStorage
    var activeLink = localStorage.getItem('activeLink');

    // If there's an active link, set it as active
    if (activeLink) {
        navLinks.forEach(function (link) {
            if (link.getAttribute('href') === activeLink) {
                setActiveLink(link);
            }
        });
    } else {
        document.getElementById("DashboardLink").classList.add('active');
    }

    // Add click event listeners to all links
    navLinks.forEach(function (link) {
        link.addEventListener('click', function () {
            removeAllActive(); // Remove 'active' class from all links
            setActiveLink(this); // Set 'active' class to the clicked link
        });
    });
});


const patientPhone = document.querySelector(".patientPhone");
var phoneInput = window.intlTelInput(patientPhone,
    {
        utilsScript:
            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
    });
