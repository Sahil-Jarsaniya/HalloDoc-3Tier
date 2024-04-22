
$(document).ready(function () {


    const patientPhoneInputElement = document.getElementById("patientPhone");
    let patientIntlInput = window.intlTelInput(patientPhoneInputElement, {
        utilsScript:
            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
        preferredCountries: ["in"],
        separateDialCode: true,
        initialCountry: "in"
    });

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