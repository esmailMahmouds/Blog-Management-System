function formatDate(myDate) {
    // reformat date to be "yyyy-mm-dd"
    const [month, day, year] = myDate.split('/');
    const formattedDate = `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;

    const dateInput = document.getElementById('DateOfBirth');
    dateInput.value = formattedDate; 
}

function handleToggleInput() {
    const editButton = document.getElementById('edit-button');
    const updateButton = document.getElementById('update-button');
    const myInputs = document.getElementsByClassName('form-control');
    const chooseImageButton = document.getElementById('choose-image-button');
    const profileImageInput = document.getElementById('profile-image-input');
    const profileImagePreview = document.getElementById('profile-image-preview');

    editButton.addEventListener('click', () => {
        for (const input of myInputs) {
            if (input.hasAttribute('disabled')) {
                console.log("enable input");
                input.removeAttribute('disabled');
                updateButton.removeAttribute('hidden');
                editButton.setAttribute('hidden', "")
                chooseImageButton.removeAttribute('disabled');
            }
            else {
                console.log("disable input");
                input.setAttribute('disabled', "");
                updateButton.setAttribute('hidden', "")
                editButton.removeAttribute('hidden');
                chooseImageButton.setAttribute('disabled', "");
            }
        }
    });
}

function handleImageUpload() {
    const chooseImageButton = document.getElementById('choose-image-button');
    const profileImageInput = document.getElementById('profile-image-input');
    const profileImagePreview = document.getElementById('profile-image-preview');

    chooseImageButton.addEventListener('click', () => {
        profileImageInput.click();
    });

    profileImageInput.addEventListener('change', function (event) {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                profileImagePreview.src = e.target.result;
            };
            reader.readAsDataURL(file);
        }
    });
}