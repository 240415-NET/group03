document.addEventListener('DOMContentLoaded', () => {

    const signInForm = document.getElementById('sign-in-form');
    const createUserForm = document.getElementById('create-form');
    const checkInput = document.getElementById('check-input');
    const checkButton = document.getElementById('check-button');
    let storedUser;
    let checkedOut = false; // Variable that's set dynamically to determine whether clicking the check-button element
    let bookAvailable = false;

    // #############################################

    // Sets StoredUser on refresh if user is still logged in
    if (JSON.parse(localStorage.getItem('user'))) {
        SetStoredUser();
    }

    // Does login procedures if user stored locally
    if (storedUser) {
        // Handle a stored user
        HandleLogInOut();
    }

    // Sets StoredUser from localStorage
    function SetStoredUser(user) {

        if (user) {
            localStorage.setItem('user', JSON.stringify(user));
            storedUser = JSON.parse(localStorage.getItem('user'));
        } else {
            storedUser = JSON.parse(localStorage.getItem('user'));
        }
    }

    // #############################################
    //mark Yenny01
    // ##### CHECK IN-OUT CLICK EVENT LISTENER ##### <-----------------
    checkButton.addEventListener('click', async () => {
        HandleCheckErrorMessage();  //call to make it not display
        let barcode = checkInput.value;
        SetStoredUser();

        if (checkedOut) {
            //do the check in call via the fetch
            // User does not exist
            const response = fetch(`http://localhost:5185/Checkout/Checkin?barcode=${barcode}`, {
                method: 'PATCH',
            });
        } else {
            const allAvailableBooksResponse = await fetch(`http://localhost:5185/Checkout/Books`);
            const allBooks = await allAvailableBooksResponse.json();

            if (allBooks.find(o => o.barcode === parseInt(barcode))) {

                let checkoutObj = {
                    checkoutId: "", //constructor will give it a guid
                    status: "OUT",
                    dueDate: "", //constructor will set this date to current date + 14days
                    userId: storedUser.userId, //this does not change to user 
                    bookBarcode: barcode
                };

                const checkoutPostResponse = fetch(`http://localhost:5185/Checkout`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(checkoutObj)
                });
            }
            else {
                //in case a book is checked out and the user attempts to check it out again
                //show a message "This book is already checked-out"
                HandleCheckErrorMessage("This book is already checked-out");
            }
        }
        checkInput.value = '';
        SetCheckButtonText();
    });

    // ##### CREATE USER SUBMIT EVENT LISTENER ##### <-----------------
    createUserForm.addEventListener('submit', async (event) => {
        event.preventDefault();
        HandleLoginErrorMessage();

        const username = document.getElementById('create-input').value;
        const url = `http://localhost:5185/Users/${username}`;

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
                        const newUserToLogin = await GetUser(username); // Gets json for newly created user
                        SetStoredUser(newUserToLogin); // sets user to local data
                        toggleLoginElementsDisplay(event); // Resets login div to default landing
                        HandleLogInOut(); // turns on the logged in display
                        GetAllAvailableBooks();
                    } else {
                        console.error('Could not create user' + error);
                    }
                } else {
                    HandleLoginErrorMessage('User may already exist.');
                }
            } catch (error) {
                console.error('Could not create user' + error);
            }
        }
        document.getElementById('create-input').value = '';
    });

    // ###### INPUT EVENT LISTENER ##### <-----------------
    // Event listener makes changes when check-in/out input is updated
    checkInput.addEventListener('input', async () => {
        HandleCheckErrorMessage();
        // Each time the check-input field is changed, run a check on the input against
        // the user's books. If it's checked out, make changes, otherwise... don't.
        SetStoredUser();
        let usersCheckedOutBooksResponse = await fetch(`http://localhost:5185/Checkout/${storedUser.userId}`);
        let usersCheckedOutBooks = await usersCheckedOutBooksResponse.json();
        let barcode = checkInput.value;

        //in the event a book is available, rename the button Ask to CheckOut
        const allAvailableBooksResponse = await fetch(`http://localhost:5185/Checkout/Books`);
        const allBooks = await allAvailableBooksResponse.json();

        checkedOut = usersCheckedOutBooks.some(book =>
            book.checkoutBook.barcode == barcode &&
            book.status === "OUT"
        );

        bookAvailable = allBooks.some(x => x.barcode == barcode);

        if (checkedOut) {
            // Change button text for checkout to check in
            SetCheckButtonText('Check In');
        } else if (!checkedOut && checkButton.textContent == 'Check In') {
            SetCheckButtonText();
        }

        if (bookAvailable) {
            // Change button text for ask to checkout 
            SetCheckButtonText('Check Out');
        } else if (!bookAvailable && checkButton.textContent == 'Check Out') {
            SetCheckButtonText();
        }
    });

    // ##### LOGIN EVENT LISTENER ##### <-----------------
    signInForm.addEventListener('submit', async (event) => {
        event.preventDefault();
        HandleLoginErrorMessage();
        const username = document.getElementById('login-input').value;

        if (username) {
            try {
                const user = await GetUser(username);
                if (user) {
                    // const userJson = await user.json();
                    // localStorage.setItem('user', JSON.stringify(user));
                    SetStoredUser(user);
                    SetUsersBooksText(username);
                    HandleLogInOut();
                    // Call all books function
                    GetAllAvailableBooks();
                } else {
                    // Handle user not found
                    HandleLoginErrorMessage('That user could not be found.');
                }
            } catch (error) {
                console.error('How did we get here? What is the meaning of life?' + error);
            }
        }
        document.getElementById('login-input').value = '';
    });

}); // ##### END DOMCONTENTLOADED #####

// #############################################
// #############################################

// ##### DELETE USER ##### <-----------------
async function DeleteUser() {
    const storedUser = JSON.parse(localStorage.getItem('user'));
    let usersCheckedOutBooksResponse = await fetch(`http://localhost:5185/Checkout/${storedUser.userId}`);
    let usersCheckedOutBooks = await usersCheckedOutBooksResponse.json();

    // Checks in any books user has checked out (so nothing gets stuck in limbo)
    usersCheckedOutBooks.forEach(async (book) => {
        await fetch(`http://localhost:5185/Checkout/Checkin?barcode=${book.checkoutBook.barcode}`, {
            method: 'PATCH'
        })
    });

    // Completes logout steps
    Options('all');
    localStorage.removeItem('user');
    HandleLogInOut();

    // Removes user from db
    await fetch(`http://localhost:5185/Users/Delete/${storedUser.userName}`, {
        method: 'DELETE'
    });
}

// ##### GET ALL AVAILABLE BOOKS ##### <-----------------
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
}

// ##### GET USER ##### <-----------------
async function GetUser(username) {
    // Returns user json is username argument matches in db
    const url = `http://localhost:5185/Users/${username}`;

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
//mark Mike01
// ##### GET USER'S CHECKED OUT BOOKS ##### <-----------------
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
}

// ##### HANDLE LOGIN ERROR MESSAGE ##### <-----------------
function HandleLoginErrorMessage(text) {
    const errorMessage = document.getElementById('login-message');
    if (text) {
        errorMessage.style.display = "block";
        errorMessage.textContent = text;
    } else {
        errorMessage.style.display = 'none';
    }
}

// ##### HANDLE CHECK ERROR MESSAGE ##### <-----------------
function HandleCheckErrorMessage(text) {
    const errorMessage = document.getElementById('check-error-message');
    if (text) {
        errorMessage.style.display = "block";
        errorMessage.textContent = text;
    }
    else {
        errorMessage.style.display = "none";
    }
}

// ##### HANDLE LOG IN-OUT ##### <-----------------
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
    if (JSON.parse(localStorage.getItem('user'))) {
        const username = JSON.parse(localStorage.getItem('user')).userName;
        SetUsersBooksText(username);
    }
}

// ##### LOGOUT ##### <-----------------
function Logout(event) {
    event.preventDefault();
    Options('all');
    localStorage.removeItem('user');
    HandleLogInOut();
}

// ##### OPTIONS ##### <-----------------
function Options(option) {

    HandleCheckErrorMessage();

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

// ##### RENDER ALL AVAILABLE BOOKS LIST <-----------------
function RenderAllAvailableBooksList(allAvailableBooks) {
    const allBooks = document.getElementById('all-books');
    const allBooksTableBody = document.getElementById("all-books-body");

    const headers = ["Barcode", "Title", "Author", "Genre"];

    allBooksTableBody.innerHTML = '';

    headers.forEach((header) => {
        let head = document.createElement('th');
        head.textContent = header;
        allBooksTableBody.appendChild(head);
    });
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
}
//mark Mike02
// ##### RENDER USER'S CHECKED OUT BOOKS LIST ##### <-----------------
function RenderUsersCheckedOutBooksList(usersCheckedOutBooks) {
    const userBooks = document.getElementById('user-books');
    const tblBody = document.getElementById("user-table-body");
    const headers = ["Barcode", "Title", "Author", "Genre", "Due Date"];
    tblBody.innerHTML = '';
    let counter = 0;

    headers.forEach((header) => {
        let head = document.createElement('th');
        head.textContent = header;
        tblBody.appendChild(head);
    });

    //itereate once for each row
    for (let i = 0; i < usersCheckedOutBooks.length; i++) {
        counter = counter + 1;
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

    if (counter == 0) {
        tblBody.innerHTML = 'No books currently checked out.';
    }
}

// ##### SET CHECK BUTTON TEXT ##### <-----------------
function SetCheckButtonText(text) {
    const button = document.getElementById('check-button');

    if (text) {
        button.textContent = text;
    } else {
        button.textContent = "Ask";
    }
}

// ##### SET USER'S BOOKS TEXT ##### <-----------------
function SetUsersBooksText(username) {
    const link = document.getElementById('link-users-books');
    link.textContent = username + "'S BOOKS";
    link.style.textTransform = 'uppercase';
}

// ##### TOGGLE LOGIN ELEMENTS DISPLAY ##### <-----------------
function toggleLoginElementsDisplay(event) {
    // Function that displays/hides the create user form
    event.preventDefault();
    HandleLoginErrorMessage();

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
    document.getElementById('login-input').value = '';
    document.getElementById('create-input').value = '';
}

// Changes the Ask Yenny logo to Oi Yenny
function YennyUK() {
    const link = document.getElementById('uk-link');
    const image = document.getElementById('logo-image');
    const current = "images/logo.png";
    const newUrl = "images/yenny-uk.png";

    if (image.src.endsWith(current)) {
        // Oi Yenny
        image.src = newUrl;
        link.textContent = 'Oi Yenny U.S.';
    } else {
        // Ask Yenny
        image.src = current;
        link.textContent = 'Ask Yenny U.K.';
    }
}