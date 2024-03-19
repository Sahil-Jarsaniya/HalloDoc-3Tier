const conciergePhone = document.querySelector(".conciergePhone");
var phoneInput = window.intlTelInput(conciergePhone,
    {
        utilsScript:
            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
        preferredCountries: ["in"],
        separateDialCode: true,
        initialCountry: "in"
    });


    const patientPhone = document.querySelector(".patientPhone");
    var phoneInput = window.intlTelInput(patientPhone,
        {
            utilsScript:
                "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
            preferredCountries: ["in"],
            separateDialCode: true,
            initialCountry: "in"
        });
    
const Phone = document.getElementById("Phone");
var phoneInput = window.intlTelInput(Phone,
    {
        utilsScript:
            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
        preferredCountries: ["in"],
        separateDialCode: true,
        initialCountry: "in"
    });
