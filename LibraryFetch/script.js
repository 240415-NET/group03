document.addEventListener('DOMContentLoaded', () => {

    const port = "5185"; // port number for localhost

    const loginDiv = document.getElementById('login-div');
    const userOptionsDiv = document.getElementsByClassName('user-options-div');

    const signInForm = document.getElementById('sign-in-form');
    const createUserForm = document.getElementById('create-form');

    signInForm.addEventListener('submit', async (event) => {
        // const username = document.getElementById('username').value;
        event.preventDefault();

        loginDiv.style.display = 'none';

        for(i = 0; i < userOptionsDiv.length; i++) {
            userOptionsDiv[i].style.display = 'block';
        }
    });

    createUserForm.addEventListener('submit', async (event) => {
        
    });

}); // End DOMContentLoaded

function Options(option) {
    if (option != null) {
        let allOptions = document.getElementsByClassName("options");
        for (i = 0; i < allOptions.length; i++) {
            allOptions[i].style.display = "none";
        }
        document.getElementById(option).style.display = "block";
    }
}

// Function that displays/hides the create user form
function toggleLoginElementsDisplay(event) {

    event.preventDefault();

    const elements = document.getElementsByClassName('login-elements');
    const headingText = document.getElementById('login-header');
    const toggleText = document.getElementById('toggle-link');

    for (i = 0; i < elements.length; i++) {
        if (window.getComputedStyle(elements[i]).display == 'none') {
            elements[i].style.display = 'block';
        } else {
            elements[i].style.display = 'none';
        }
    }

    if (toggleText.textContent == "Create New User") {
        toggleText.textContent = "Back";
    } else {
        toggleText.textContent = "Create New User";
    }

    if (headingText.textContent == "Sign In to Ask Yenny!") {
        headingText.textContent = "Create New User";
    } else {
        headingText.textContent = "Sign In to Ask Yenny!";
    }
}

