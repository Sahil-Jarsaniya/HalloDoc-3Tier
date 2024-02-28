

//active nav links
var navLinks = document.querySelectorAll('nav a');

navLinks.forEach(link => {
    link.addEventListener('click', function () {
        navLinks.forEach(link => link.classList.remove('active'));
        
        this.classList.add('active');
    });
});


const patientPhone = document.querySelector(".patientPhone");
var phoneInput = window.intlTelInput(patientPhone,
    {
        utilsScript:
            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
    });
