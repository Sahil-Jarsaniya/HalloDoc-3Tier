//First Name 
document.querySelectorAll(".firstnameInput").forEach(function (input) {
    input.addEventListener("change", function () {
        validateFirstName(this.value, this.parentNode.nextElementSibling, this.parentNode);
    });
    input.addEventListener("blur", function () {
        validateFirstName(this.value, this.parentNode.nextElementSibling, this.parentNode);
    });
});

function validateFirstName(firstName, errorSpan, div) {
    if (/^[a-zA-Z]+$/.test(firstName)) {
        errorSpan.textContent = "";
        div.classList.remove("invalidInput");
    } else {
        errorSpan.textContent = "Please enter a valid name";
        div.classList.add("invalidInput");
    }
}

//Date of Birth
document.querySelectorAll(".dobInput").forEach(function (input) {
    input.addEventListener("keyup", function () {
        validateDateOfBirth(this.value, this.parentNode.nextElementSibling, this.parentNode);
    });
    input.addEventListener("change", function () {
        validateDateOfBirth(this.value, this.parentNode.nextElementSibling, this.parentNode);
    });
});

function isValidDate(dateString) {
    // Check if the input is a valid date
    var regex = /^\d{4}-\d{2}-\d{2}$/;
    if (!regex.test(dateString)) return false;

    var parts = dateString.split("-");
    var year = parseInt(parts[0], 10);
    var month = parseInt(parts[1], 10);
    var day = parseInt(parts[2], 10);

    if (year < 1900 || year > new Date().getFullYear()) return false;
    if (month < 1 || month > 12) return false;
    if (day < 1 || day > 31) return false;

    return true;
}

function isPastDate(dateString) {
    // Check if the entered date is not greater than today's date
    var enteredDate = new Date(dateString);
    var currentDate = new Date();

    return enteredDate <= currentDate;
}

function validateDateOfBirth(dateOfBirth, errorSpan, div) {
    if (isValidDate(dateOfBirth) && isPastDate(dateOfBirth)) {
        errorSpan.textContent = "";
        div.classList.remove("invalidInput");
    }
    else {
        errorSpan.textContent = "Please enter a valid date of birth";
        div.classList.add("invalidInput");
    }
}


//Email
document.querySelectorAll(".emailInput").forEach(function (input) {
    input.addEventListener("keyup", function () {
        validateEmail(this.value, this.parentNode.nextElementSibling, this.parentNode);
    });
    input.addEventListener("change", function () {
        validateEmail(this.value, this.parentNode.nextElementSibling, this.parentNode);
    });
});
function validateEmail(email, errorSpan, div) {
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (emailRegex.test(email)) {
        errorSpan.textContent = "";
        div.classList.remove("invalidInput");
    } else {
        errorSpan.textContent = "Please enter a valid email address";
        div.classList.add("invalidInput");
    }
}




//Notes
document.querySelectorAll(".notesInput").forEach(function (input) {
    input.addEventListener("keyup", function () {
        validateNotes(this.value, this.parentNode.nextElementSibling, this);
    });

    input.addEventListener("blur", function () {
        validateNotes(this.value, this.parentNode.nextElementSibling, this);
    });
});

function validateNotes(notes, errorSpan, div){
    if (notes.length != 0) {
        errorSpan.textContent = "";
        div.classList.remove("invalidInput");
    } else {
        errorSpan.textContent = "Notes cant be empty!";
        div.classList.add("invalidInput");
    }
};

//Zip Code

document.querySelectorAll(".zipcode").forEach(function (input) {
    input.addEventListener("keyup", function () {
        validateZipCode(this.value, this.parentNode.nextElementSibling, this.parentNode);
    })
    input.addEventListener("blur", function () {
        validateZipCode(this.value, this.parentNode.nextElementSibling, this.parentNode);
    })
})
function validateZipCode(zipcode, errorSpan, dic) {
    var zipRegex = /^[1-9][0-9]{5}$/;
    if (zipRegex.test(zipcode)) {
        errorSpan.textContent = "";
        div.classList.remove("invalidInput");
    } else {
        errorSpan.textContent = "Please enter a valid Zip Code";
        div.classList.add("invalidInput");
    }
}



//Admin Dashboard

document.querySelectorAll(".regionDropdown").forEach(function (input) {
    input.addEventListener("change", function () {
        if (this.value == 0) {
            this.nextElementSibling.textContent = "Please Select Region";
        } else {
            this.nextElementSibling.textContent = "";
        }
    })
})

document.querySelectorAll(".physicianDropdown").forEach(function (input) {
    input.addEventListener("change", function () {
        if (this.value == 0) {
            this.nextElementSibling.textContent = "Please Select Physician";
        } else {
            this.nextElementSibling.textContent = "";
        }
    })
})

document.querySelectorAll(".CaseTag").forEach(function (input) {
    input.addEventListener("change", function () {
        if (this.value == 0) {
            this.nextElementSibling.textContent = "Please Select Reason";
        } else {
            this.nextElementSibling.textContent = "";
        }
    })
})


document.querySelectorAll(".adminNote").forEach(function (input) {
    input.addEventListener("input", function () {
        if (this.value == "") {
            this.nextElementSibling.nextElementSibling.textContent = "Enter Note";
        } else {
            this.nextElementSibling.nextElementSibling.textContent = "";
        }
    })
})


document.querySelectorAll(".faxInput").forEach(function (input) {
    input.addEventListener("keyup", function () {
        validateFaxNumber(this.value, this.parentNode.nextElementSibling, this.parentNode);
    });
});

function validateFaxNumber(faxNumber, errorSpan, div) {
    var faxRegex = /^\+?[0-9]{1,3}\.[0-9]{1,14}$/;

    if (faxRegex.test(faxNumber)) {
        errorSpan.textContent = "";
        div.classList.remove("invalidInput");
    } else {
        errorSpan.textContent = "Please enter a valid fax number";
        div.classList.add("invalidInput");
    }
}

document.querySelectorAll(".proffesionDD").forEach(function (input) {
    input.addEventListener("change", function () {
        if (this.value == 0) {
            this.parentNode.nextElementSibling.textContent = "Please Select Reason";
        } else {
            this.parentNode.nextElementSibling.textContent = "";
        }
    })
})

document.querySelectorAll(".OrderDetail").forEach(function (input) {
    input.addEventListener("input", function () {
        if (this.value == "") {
            this.parentNode.nextElementSibling.textContent = "Enter Prescription!!";
        } else {
            this.parentNode.nextElementSibling.textContent = "";
        }
    })
})

document.querySelectorAll(".businessDetail").forEach(function (input) {
    input.addEventListener("input", function () {
        if (this.value == "") {
            this.parentNode.nextElementSibling.textContent = "Enter Prescription!!";
        } else {
            this.parentNode.nextElementSibling.textContent = "";
        }
    })
})