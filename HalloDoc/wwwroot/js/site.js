
//dark mode

document.getElementById('light-dark-btn').addEventListener('click',()=>{
    if (document.documentElement.getAttribute('data-bs-theme') == 'dark') {
        localStorage.setItem("PageTheme", "light");
        document.documentElement.setAttribute('data-bs-theme','light')
    }
    else {
        localStorage.setItem("PageTheme", "dark");
        document.documentElement.setAttribute('data-bs-theme','dark')
    }
})

if (localStorage.getItem("PageTheme") === "light") {
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

showHideConfirmPassword
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

// Edit Details buttons
function editDetails() {
    var cancelbtn = document.getElementById("cancelBtn");
    var submitBtn = document.getElementById("submitBtn");
    var editBtn = document.getElementById("editBtn");
    var mapBtn = document.getElementById("mapBtn");
    var field = document.getElementById("field")
    cancelbtn.classList.remove("d-none");
    submitBtn.classList.remove("d-none");
    editBtn.classList.remove("d-block");

    cancelbtn.classList.add("d-block");
    submitBtn.classList.add("d-block");
    editBtn.classList.add("d-none");

    field.disabled = false;
    mapBtn.disabled = false;
}

function cancelEdit() {
    var cancelbtn = document.getElementById("cancelBtn");
    var submitBtn = document.getElementById("submitBtn");
    var editBtn = document.getElementById("editBtn");
    var mapBtn = document.getElementById("mapBtn");
    var field = document.getElementById("field")
    cancelbtn.classList.remove("d-block");
    submitBtn.classList.remove("d-block");
    editBtn.classList.remove("d-none");

    cancelbtn.classList.add("d-none");
    submitBtn.classList.add("d-none");
    editBtn.classList.add("d-block");
    field.disabled = true;
    mapBtn.disabled = false;
}