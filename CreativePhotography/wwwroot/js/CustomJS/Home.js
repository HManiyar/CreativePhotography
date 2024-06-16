$(document).ready(function () {
    var containerFluid = document.querySelector(".container-fluid.site-section");

    // Check if the container-fluid element exists
    if (containerFluid) {
        // Clear the class attribute
        containerFluid.removeAttribute("class");
    }
    // Event listener for form submission
    $('form').submit(function (event) {
        // Prevent default form submission
        event.preventDefault();

        // Show the loader
        $('.custom-loader, .overlay').show();

        // Collect form data
        var firstName = $('#fname').val();
        var lastName = $('#lname').val();
        var contact = $('#contact').val();
        var email = $('#email').val();
        var subject = $('#subject').val();
        var message = $('#message').val();

        // Validate input fields
        if (!firstName.trim() || !lastName.trim() || !contact.trim() || !email.trim() || !subject.trim() || !message.trim()) {
            // Hide the loader
            $('.custom-loader, .overlay').hide();
            // Show validation message
            alert("Please fill in all fields");
            return; // Exit the function if any field is empty
        }

        // Validate contact number
        if (contact.length !== 10 || isNaN(contact)) {
            // Hide the loader
            $('.custom-loader, .overlay').hide();
            // Show validation message
            alert("Contact number must be exactly 10 digits");
            return; // Exit the function if contact number is invalid
        }

        // Send AJAX request to sendmail endpoint
        $.ajax({
            url: '/Home/SendMail', // Change the URL to match your endpoint
            type: 'POST',
            data: {
                FirstName: firstName,
                LastName: lastName,
                Contact: contact,
                Email: email,
                Subject: subject,
                Message: message
            },
            success: function (response) {
                // Hide the loader on success
                $('.custom-loader, .overlay').hide();

                // Handle success response, e.g., show a success message
                alert(response);

                // Optionally, you can clear the form fields after successful submission
                $('form')[0].reset();
            },
            error: function (xhr, status, error) {
                // Hide the loader on error
                $('.custom-loader, .overlay').hide();

                // Handle error response, e.g., show an error message
                console.error(error);
                alert('Failed to send email. Please try again later.');
            }
        });
    });
});
