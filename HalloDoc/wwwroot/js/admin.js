
var navLinks = document.querySelectorAll('#adminNav .nav-link');
function removeAllActive() {
    navLinks.forEach(function (link) {
        link.classList.remove('active');
    });
}
function setActiveLink(link) {
    link.classList.add('active');
}
var path = window.location.pathname;
var abc = path.split("/");
var controller = abc[1];
var action = abc[2];

var ul = document.getElementById("togglebar").children;

switch (controller) {
    case "AdminDashboard":
        switch (action) {
            case "MyProfile":
                removeAllActive();
                setActiveLink(ul[2].children[0]);
                break;
            case "Provider":
                removeAllActive();
                setActiveLink(ul[3].children[0]);
                break;
            case "UserAccess":
                removeAllActive();
                setActiveLink(ul[5].children[0]);
                break;
            case "Access":
                removeAllActive();
                setActiveLink(ul[5].children[0]);
                break;
            case "EditRole":
                removeAllActive();
                setActiveLink(ul[5].children[0]);
                break;
            case "CreateRole":
                removeAllActive();
                setActiveLink(ul[5].children[0]);
                break;
            case "EditProvider":
                removeAllActive();
                setActiveLink(ul[3].children[0]);
                break;
            case "CreateProvider":
                removeAllActive();
                setActiveLink(ul[3].children[0]);
                break;
            default:
                removeAllActive();
                setActiveLink(ul[0].children[0]);
                break;
        }
        break;
    case "ProvidersMenu":
        switch (action) {
            case "ProviderLocation":
                removeAllActive();
                setActiveLink(ul[1].children[0]);
                break;
            default:
                removeAllActive();
                setActiveLink(ul[3].children[0]);
                break;
        }
        break;
    case "PartnersMenu":
        removeAllActive();
        setActiveLink(ul[4].children[0]);
        break;
    case "RecordsMenu":
        removeAllActive();
        setActiveLink(ul[6].children[0]);
        break;
    case "PhysicianDashboard":
        switch (action) {
            case "MyProfile":
                removeAllActive();
                setActiveLink(ul[1].children[0]);
                break;
            case "Scheduling":
                removeAllActive();
                setActiveLink(ul[2].children[0]);
                break;
            default:
                removeAllActive();
                setActiveLink(ul[0].children[0]);
                break;
        }
        break;
    case "Patient":
        switch (action) {
            case "MyProfile":
                removeAllActive();
                setActiveLink(ul[1].children[0]);
                break;
            default:
                removeAllActive();
                setActiveLink(ul[0].children[0]);
                break;
        }
}

$(document).ready(function () {


    const patientPhoneInputElement = document.getElementById("patientPhone");
    let patientIntlInput;
    if (patientPhoneInputElement != null) {

        patientIntlInput = window.intlTelInput(patientPhoneInputElement, {
            utilsScript:
                "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
            preferredCountries: ["in"],
            separateDialCode: true,
            initialCountry: "in"
        });

    }
    const otherPhoneInputElement = document.getElementById("otherPhone");
    let otherIntlInput;

    if (otherPhoneInputElement != null) {
        otherIntlInput = window.intlTelInput(otherPhoneInputElement, {
            utilsScript:
                "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
            preferredCountries: ["in"],
            separateDialCode: true,
            initialCountry: "in"
        });
    }
    if (patientPhoneInputElement != null) {
        var div = document.getElementById("phoneDiv");
        $("#patientPhone").on("input", function () {
            console.log(patientIntlInput.isValidNumber());
            if (!patientIntlInput.isValidNumber()) {
                div.nextElementSibling.textContent = "Please Enter Valid Number";
                this.parentNode.classList.add("invalidInput");
            } else {
                div.nextElementSibling.textContent = "";
                this.parentNode.classList.remove("invalidInput");
            }
        })
        $("#patientPhone").blur(function () {
            console.log(patientIntlInput.isValidNumber());
            if (!patientIntlInput.isValidNumber()) {
                div.nextElementSibling.textContent = "Please Enter Valid Number";
                this.parentNode.classList.add("invalidInput");
            } else {
                div.nextElementSibling.textContent = "";
                this.parentNode.classList.remove("invalidInput");
            }
        })
    }
    if (otherPhoneInputElement != null) {
        var div2 = document.getElementById("phoneDiv2");
        $("#otherPhone").on("input", function () {
            console.log(otherIntlInput.isValidNumber());
            if (!otherIntlInput.isValidNumber()) {
                div2.nextElementSibling.textContent = "Please Enter Valid Number";
                this.parentNode.classList.add("invalidInput");
            } else {
                div2.nextElementSibling.textContent = "";
                this.parentNode.parentNode.classList.remove("invalidInput");
            }
        })
        $("#otherPhone").blur(function () {
            console.log(otherIntlInput.isValidNumber());
            if (!otherIntlInput.isValidNumber()) {
                this.parentNode.parentNode.nextElementSibling.textContent = "Please Enter Valid Number";
                this.parentNode.parentNode.classList.add("invalidInput");
            } else {
                this.parentNode.parentNode.nextElementSibling.textContent = "";
                this.parentNode.parentNode.classList.remove("invalidInput");
            }
        })
    }
});



$(document).ready(function () {
    $('#loader').hide();

    $(document).ajaxSend(function () {
        $('#loader').show();
        $('#loader').fadeIn(240);
    });
    $(document).ajaxComplete(function () {
        $('#loader').hide();
        $('#loader').addClass('d-none').fadeOut(230);
    });

})