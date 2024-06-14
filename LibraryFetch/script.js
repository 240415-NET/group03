document.addEventListener('DOMContentLoaded', () => {

    const port = "http://localhost:5185"; // port number for localhost

    const loginDiv = document.getElementById('login-div');
    const userOptionsDiv = document.getElementsByClassName('user-options-div');

    const signInForm = document.getElementById('sign-in-form');
    const createUserForm = document.getElementById('create-form');

    const loginMessage = document.getElementById('login-message');

    const storedUser = JSON.parse(localStorage.getItem('user'));

    if (storedUser) {
        // Handle a stored user
    }

    // Login Event Listener
    signInForm.addEventListener('submit', async (event) => {
        event.preventDefault();
        const username = document.getElementById('login-username').value;

        if (username) {
            try {
                const user = await GetUser(username);
                if (user) {
                    // const userJson = await user.json();
                    localStorage.setItem('user', JSON.stringify(user));
                    HandledLoggedInUser();
                } else {
                    // Handle user not found
                    loginMessage.style.dispay = 'block';
                    loginMessage.textContent = 'That user could not be found.';

                    // Call all books function
                }
            } catch (error) {
                console.error('How did we get here? What is the meaning of life?' + error);
            }
        }
    });

    createUserForm.addEventListener('submit', async (event) => {
        event.preventDefault();

        const username = document.getElementById('create-username').value;
        const url = `${port}/Users/${username}`;

        if (username) {
            try {
                const userExists = await GetUser(username);

                if (!userExists) {
                    // User does not exist
                    const response = await fetch(url, {
                        method: 'POST',
                    });
                    if (response.ok) {
                        // user created so log in
                    } else {
                        console.error('Could not create user' + error);
                    }
                } else {
                    loginMessage.style.display = 'bock';
                    loginMessage.textContent = 'User may already exist.';
                }
            } catch (error) {
                console.error('Could not create user' + error);
            }
        }
    });

    function HandledLoggedInUser(username) {
        loginDiv.style.display = 'none';

        for (i = 0; i < userOptionsDiv.length; i++) {
            userOptionsDiv[i].style.display = 'block';
        }
    }

    async function GetUser(username) {
        const url = `${port}/Users/${username}`;

        try {
            const response = await fetch(url);

            if (response.status === 404) {
                return null;
            }

            if (!response.ok) {
                console.error('Unable to get user: ' + response.status);
                return null;
            }

            const json = await response.json();
            return json;
        } catch (error) {
            console.error(error);
            return null;
        }
    }


}); // End DOMContentLoaded

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

function Options(option) {
    if (option != null) {
        let allOptions = document.getElementsByClassName("options");
        for (i = 0; i < allOptions.length; i++) {
            allOptions[i].style.display = "none";
        }
        document.getElementById(option).style.display = "block";
    }

    if (option == 'user') {
        GetUsersBooks();
    }
}

async function GetUsersBooks()  {

}

// Changes the Ask Yenny logo to Oi Yenny
function YennyUK() {
    const image = document.getElementById('logo-image');
    const current = "images/logo.png";
    const newUrl = "images/yenny-uk.png";

    if (image.src.endsWith(current)) {
        image.src = newUrl;
    } else {
        image.src = current;
    }
}

