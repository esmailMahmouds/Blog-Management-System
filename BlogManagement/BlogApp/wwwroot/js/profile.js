function formatDate(myDate) {
    // reformat date to be "yyyy-mm-dd"
    const [month, day, year] = myDate.split('/');
    const formattedDate = `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;

    const dateInput = document.getElementById('DateOfBirth');
    dateInput.value = formattedDate; 
}

function handleToggleInput() {
    const userInfoForm = document.getElementById('user-info-form');
    const myInputs = userInfoForm.querySelectorAll('.form-control');
    const editButton = userInfoForm.querySelector('#edit-button');
    const updateButton = userInfoForm.querySelector('#update-button');
    const chooseImageButton = document.getElementById('choose-image-button');

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
    const userInfoForm = document.getElementById('user-info-form');
    if (!userInfoForm) return;

    const chooseImageButton = userInfoForm.querySelector('#choose-image-button');
    const profileImageInput = userInfoForm.querySelector('#profile-image-input');
    const profileImagePreview = userInfoForm.querySelector('#profile-image-preview');

    if (!chooseImageButton || !profileImageInput || !profileImagePreview) return;

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

function handleResetPasswordButtons() {
    const resetPasswordForm = document.getElementById('reset-password-form');
    if (!resetPasswordForm) return;

    const oldPasswordInput = resetPasswordForm.querySelector('#OldPassword');
    const newPasswordBox = resetPasswordForm.querySelector('#new-password-box');
    const newPasswordInput = resetPasswordForm.querySelector('#NewPassword');
    const changePasswordButton = resetPasswordForm.querySelector('#change-password-button');
    const updatePasswordButton = resetPasswordForm.querySelector('#update-password-button');

    oldPasswordInput.setAttribute('disabled', '');
    newPasswordBox.classList.add('invisible');
    updatePasswordButton.setAttribute('hidden', '');
    changePasswordButton.removeAttribute('hidden');

    changePasswordButton.addEventListener('click', () => {
        // Enable old password
        oldPasswordInput.removeAttribute('disabled');
        // Show new password box
        newPasswordBox.classList.remove('invisible');
        // Hide change password button
        changePasswordButton.setAttribute('hidden', '');
        // Show update password button
        updatePasswordButton.removeAttribute('hidden');
        // Focus on new password input
        newPasswordInput.focus();
    });

    //updatePasswordButton.addEventListener('click', (e) => {
    //    // Hide update password button
    //    updatePasswordButton.setAttribute('hidden', '');

    //    // Show change password button
    //    changePasswordButton.removeAttribute('hidden');

    //    // Hide new password box
    //    newPasswordBox.classList.add('invisible');

    //    // Disable and clear old password input
    //    oldPasswordInput.setAttribute('disabled', '');
    //    oldPasswordInput.value = '';

    //    // Clear new password input
    //    newPasswordInput.value = '';
    //});
}