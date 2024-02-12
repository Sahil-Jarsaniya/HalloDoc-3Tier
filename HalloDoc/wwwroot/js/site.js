
//dark mode

document.getElementById('light-dark-btn').addEventListener('click',()=>{
    if (document.documentElement.getAttribute('data-bs-theme') == 'dark') {
        document.documentElement.setAttribute('data-bs-theme','light')
    }
    else {
        document.documentElement.setAttribute('data-bs-theme','dark')
    }
})



// file upload
const actualBtn = document.getElementById('actualFileBtn');

const fileChosen = document.getElementById('fileChoosen');

actualBtn.addEventListener('change', function () {
    fileChosen.textContent = this.files[0].name
})

// passowrd show and hide 

function showHidePassword() {
    var floatingPassword = document.getElementById('floatingPassword');
    var floatingPassword2 = document.getElementById('floatingPassword2');
    var passImg = document.getElementsByClassName('passImg');
    var passImg = document.getElementsByClassName('passImg2');
    var passImg = document.getElementsByClassName('passImg3');
    if (floatingPassword.type === 'password') {
        floatingPassword.type = 'text';
        floatingPassword2.type = 'text';
        passImg.src = "/images/password_visibility_off.svg";
        passImg2.src = "/images/password_visibility_off.svg";
        passImg3.src = "/images/password_visibility_off.svg";
    } else {
        floatingPassword.type = 'password';
        floatingPassword2.type = 'password';
        passImg.src = "/images/password_visibility_on.svg";
        passImg2.src = "/images/password_visibility_on.svg";
        passImg3.src = "/images/password_visibility_on.svg";
    }
}

//contry code for phone number
const patientPhone = document.querySelector(".patientPhone");
var phoneInput = window.intlTelInput(patientPhone,
    {
        utilsScript:
            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
    });

const friendPhone = document.querySelector(".friendPhone");
var phoneInput = window.intlTelInput(friendPhone,
    {
        utilsScript:
            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
    });


// alert box 
$(window).on('load', function () {
    $('#myModal').modal('show');
});
function closeModal() {
    $('#myModal').modal('hide')
}