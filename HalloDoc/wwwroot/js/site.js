
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

if (localStorage.getItem("PageTheme") == "light") {
    document.documentElement.setAttribute('data-bs-theme', 'light')
}
else {
    document.documentElement.setAttribute('data-bs-theme', 'dark')
}




// file upload
const actualBtn = document.getElementById('actualFileBtn');

const fileChosen = document.getElementById('fileChoosen');

actualBtn.addEventListener('change', function () {
    fileChosen.textContent = this.files[0].name
})

// passowrd show and hide 

function showHidePassword() {
    var floatingPassword = document.getElementById('floatingPassword');
    var passImg = document.getElementById('passImg');
    if (floatingPassword.type === 'password') {
        floatingPassword.type = 'text';
        passImg.src = "/images/password_visibility_off.svg";
    } else {
        floatingPassword.type = 'password';
        passImg.src = "/images/password_visibility_on.svg";
    }
}

//showHideConfirmPassword
function showHideConfirmPassword() {
    var floatingPassword2 = document.getElementById('floatingPassword2');
    var passImg2 = document.getElementById('passImg2');
    if (floatingPassword2.type === 'password') {
        floatingPassword2.type = 'text';
        passImg2.src = "/images/password_visibility_off.svg";
    } else {
        floatingPassword2.type = 'password';
        passImg2.src = "/images/password_visibility_on.svg";
    }
}


//$(document).ready(function () {


//    const patientPhoneInputElement = document.getElementById("patientPhone");
//    let patientIntlInput = window.intlTelInput(patientPhoneInputElement, {
//        utilsScript:
//            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
//        preferredCountries: ["in"],
//        separateDialCode: true,
//        initialCountry: "in"
//    });

//    const otherPhoneInputElement = document.getElementById("otherPhone");
//    let otherIntlInput;

//    if (otherPhoneInputElement != null) {
//        otherIntlInput = window.intlTelInput(otherPhoneInputElement, {
//            utilsScript:
//                "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
//            preferredCountries: ["in"],
//            separateDialCode: true,
//            initialCountry: "in"
//        });
//    }
//    var div = document.getElementById("phoneDiv");
//    $("#patientPhone").on("input", function () {
//        console.log(patientIntlInput.isValidNumber());
//        if (!patientIntlInput.isValidNumber()) {
//            div.nextElementSibling.textContent = "Please Enter Valid Number";
//            this.parentNode.classList.add("invalidInput");
//        } else {
//            div.nextElementSibling.textContent = "";
//            this.parentNode.classList.remove("invalidInput");
//        }
//    })
//    $("#patientPhone").blur(function () {
//        console.log(patientIntlInput.isValidNumber());
//        if (!patientIntlInput.isValidNumber()) {
//            div.nextElementSibling.textContent = "Please Enter Valid Number";
//            this.parentNode.classList.add("invalidInput");
//        } else {
//            div.nextElementSibling.textContent = "";
//            this.parentNode.classList.remove("invalidInput");
//        }
//    })
//    if (otherPhoneInputElement != null) {
//        var div2 = document.getElementById("phoneDiv2");
//        $("#otherPhone").on("input", function () {
//            console.log(otherIntlInput.isValidNumber());
//            if (!otherIntlInput.isValidNumber()) {
//                div2.nextElementSibling.textContent = "Please Enter Valid Number";
//                this.parentNode.classList.add("invalidInput");
//            } else {
//                div2.nextElementSibling.textContent = "";
//                this.parentNode.parentNode.classList.remove("invalidInput");
//            }
//        })
//        $("#otherPhone").blur(function () {
//            console.log(otherIntlInput.isValidNumber());
//            if (!otherIntlInput.isValidNumber()) {
//                this.parentNode.parentNode.nextElementSibling.textContent = "Please Enter Valid Number";
//                this.parentNode.parentNode.classList.add("invalidInput");
//            } else {
//                this.parentNode.parentNode.nextElementSibling.textContent = "";
//                this.parentNode.parentNode.classList.remove("invalidInput");
//            }
//        })
//    }
//});
// alert box 
$(window).on('load', function () {
    $('#myModal').modal('show');
});
function closeModal() {
    $('#myModal').modal('hide')
}

// Edit Details buttons

function editDetails() {
    var cancelbtn = document.getElementById("cancelBtn");
    var submitBtn = document.getElementById("submitBtn");
    var editBtn = document.getElementById("editBtn");
    var field = document.getElementsByClassName("disableInput");
    cancelbtn.classList.remove("d-none");
    submitBtn.classList.remove("d-none");
    editBtn.classList.remove("d-block");

    cancelbtn.classList.add("d-block");
    submitBtn.classList.add("d-block");
    editBtn.classList.add("d-none");

    for (var i = 0; i < field.length; i++) {
        field[i].disabled = false;
    }
}

function cancelEdit() {
    var cancelbtn = document.getElementById("cancelBtn");
    var submitBtn = document.getElementById("submitBtn");
    var editBtn = document.getElementById("editBtn");
    var field = document.getElementsByClassName("disableInput")
    cancelbtn.classList.remove("d-block");
    submitBtn.classList.remove("d-block");
    editBtn.classList.remove("d-none");

    cancelbtn.classList.add("d-none");
    submitBtn.classList.add("d-none");
    editBtn.classList.add("d-block");
    for (var i = 0; i < field.length; i++) {
        field[i].disabled = true;
    }
}