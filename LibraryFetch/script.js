document.addEventListener('DOMContentLoaded', () => {

    const port = "http://localhost:5185"; // port number for localhost

    const signInForm = document.getElementById('sign-in-form');
    const createUserForm = document.getElementById('create-form');
    const checkInput = document.getElementById('check-input');
    const loginMessage = document.getElementById('login-message');
    const checkButton = document.getElementById('check-button');

    const storedUser = JSON.parse(localStorage.getItem('user'));

    // Variable that's set dynamically to determine whether clicking the check-button element
    // results in a checkout call or a  check in call to the API
    let checkedOut = false;

    if (storedUser) {
        // Handle a stored user
        HandleLogInOut();
    }

    checkInput.addEventListener('input', async () => {
        // Each time the check-input field is changed, run a check on the input against
        // the user's books. If it's checked out, make changes, otherwise... don't.
        let usersCheckedOutBooksResponse = await fetch(`http://localhost:5185/Checkout/${storedUser.userId}`);
        let usersCheckedOutBooks = await usersCheckedOutBooksResponse.json();
        let barcode = checkInput.value;

        for (let i = 0; i < usersCheckedOutBooks.length; i++) {
            checkedOut = barcode == usersCheckedOutBooks[i].checkoutBook.barcode;
        }
        if (checkedOut) {
            // Change button text for checkout to check in
            checkButton.textContent = 'Check In';
        } else if (!checkedOut && checkButton.textContent == 'Check In') {
            checkButton.textContent = 'Ask';
        }
    });

    // Login Event Listener
    signInForm.addEventListener('submit', async (event) => {
        event.preventDefault();
        const username = document.getElementById('login-username').value;

        if (username) {
            try {
                const user = await GetUser(username, port);
                if (user) {
                    // const userJson = await user.json();
                    localStorage.setItem('user', JSON.stringify(user));
                    HandleLogInOut();
                    // Call all books function
                    GetAllAvailableBooks();
                } else {
                    // Handle user not found
                    loginMessage.style.dispay = 'block';
                    loginMessage.textContent = 'That user could not be found.';
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
                const userExists = await GetUser(username, port);

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
}); // End DOMContentLoaded

function HandleLogInOut() {
    const loginDiv = document.getElementsByClassName('login-div');
    const userOptionsDiv = document.getElementsByClassName('user-options-div');
    let var1 = loginDiv[0].style.display == 'none' ? 'block' : 'none';
    let var2 = loginDiv[0].style.display == 'none' ? 'none' : 'block';

    for (let i = 0; i < loginDiv.length; i++) {
        loginDiv[i].style.display = var1;
    }

    for (let i = 0; i < userOptionsDiv.length; i++) {
        userOptionsDiv[i].style.display = var2;
    }
}

function Logout(event) {
    event.preventDefault();
    localStorage.removeItem('user');
    HandleLogInOut();
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

function Options(option) {
    if (option != null) {
        let allOptions = document.getElementsByClassName("options");
        for (i = 0; i < allOptions.length; i++) {
            allOptions[i].style.display = "none";
        }
        document.getElementById(option).style.display = "block";
    }

    if (option == 'user') {
        const storedUser = JSON.parse(localStorage.getItem('user'));
        GetUsersCheckedOutBooks(storedUser.userId);
    } else if (option == 'all') {
        GetAllAvailableBooks();
    }
}


async function GetUser(username, port) {
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

async function GetAllAvailableBooks() {
    try {
        //calls the API to get all available books (those that are not checked out)
        const allAvailableBooksResponse = await fetch(`http://localhost:5185/Checkout/Books`);

        //takes the API's JSON result and puts it into an object
        const allAvailableBooks = await allAvailableBooksResponse.json();

        //takes the object and sends it to this function to place the resulting values into the table
        RenderAllAvailableBooksList(allAvailableBooks);

    }
    catch (error) {
        console.error("Error fetching all available books: ", error);
    }

}//end GetAllAvailableBooks


function RenderAllAvailableBooksList(allAvailableBooks) {
    const allBooks = document.getElementById('all-books');
    const allBooksTableBody = document.getElementById("all-books-body");

    //itereate once for each row
    for (let i = 0; i < allAvailableBooks.length; i++) {
        const row = document.createElement('tr');
        let cell = Array(4);
        let cellValue = Array(4);

        //iterate once for each column
        for (let c = 0; c < 4; c++) {
            //iterate for each column and create a cell
            cell[c] = document.createElement('td');

            //switch to determine what value will go into the cell based on the column iteration
            switch (c) {
                case 0:
                    cellValue[c] = document.createTextNode(allAvailableBooks[i].barcode);
                    break;
                case 1:
                    cellValue[c] = document.createTextNode(allAvailableBooks[i].title);
                    break;
                case 2:
                    cellValue[c] = document.createTextNode(allAvailableBooks[i].author);
                    break;
                case 3:
                    cellValue[c] = document.createTextNode(allAvailableBooks[i].genre);
                    break;
            }

            //put the value into the cell
            // console.log(cellValue[c]);
            cell[c].appendChild(cellValue[c]);
            //put the cell with its value into the row
            row.appendChild(cell[c]);
        }

        //put the completed row containing each cell and its value into the table body
        allBooksTableBody.appendChild(row);

    }

    //put the table body into the table
    allBooks.appendChild(allBooksTableBody);

    //display the table with the table body containing the rows which contain the cells and their values
    document.getElementById('all').appendChild(allBooks);

}//end RenderAllAvailableBooksList

async function GetUsersCheckedOutBooks(userId) {
    try {
        //calls the API to get the checked out books for the user designated by the logged in userId
        const usersCheckedOutBooksResponse = await fetch(`http://localhost:5185/Checkout/${userId}`);

        //takes the API's JSON result and puts it into an object
        const usersCheckedOutBooks = await usersCheckedOutBooksResponse.json();

        //takes the object and sends it to this function to place the resulting values into the table
        RenderUsersCheckedOutBooksList(usersCheckedOutBooks);

    }
    catch (error) {
        console.error("Error fetching User's checked out books: ", error);
    }

}//end GetUsersCheckedOutBooks

function RenderUsersCheckedOutBooksList(usersCheckedOutBooks) {
    const userBooks = document.getElementById('user-books');
    const tblBody = document.getElementById("user-table-body");

    //itereate once for each row
    for (let i = 0; i < usersCheckedOutBooks.length; i++) {
        const row = document.createElement('tr');
        let cell = Array(5);
        let cellValue = Array(5);

        //iterate once for each column
        for (let c = 0; c < 5; c++) {
            //iterate for each column and create a cell
            cell[c] = document.createElement('td');

            //switch to determine what value will go into the cell based on the column iteration
            switch (c) {
                case 0:
                    cellValue[c] = document.createTextNode(usersCheckedOutBooks[i].checkoutBook.barcode);
                    break;
                case 1:
                    cellValue[c] = document.createTextNode(usersCheckedOutBooks[i].checkoutBook.title);
                    break;
                case 2:
                    cellValue[c] = document.createTextNode(usersCheckedOutBooks[i].checkoutBook.author);
                    break;
                case 3:
                    cellValue[c] = document.createTextNode(usersCheckedOutBooks[i].checkoutBook.genre);
                    break;
                case 4:
                    cellValue[c] = document.createTextNode(usersCheckedOutBooks[i].dueDate);
                    break;
            }

            //put the value into the cell
            cell[c].appendChild(cellValue[c]);
            //put the cell with its value into the row
            row.appendChild(cell[c]);
        }

        //put the completed row containing each cell and its value into the table body
        tblBody.appendChild(row);

    }

    //put the table body into the table
    userBooks.appendChild(tblBody);

    //display the table with the table body containing the rows which contain the cells and their values
    document.getElementById('user').appendChild(userBooks);

}//end RenderUsersCheckedOutBooksList

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