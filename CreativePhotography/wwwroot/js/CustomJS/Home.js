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
        var serviceType = $('#serviceType').val();
        var eventDate = $('#eventDate').val();
        var location = $('#location').val();
        var budgetRange = $('#budgetRange').val();
        var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        // Validate input fields
        if (!firstName.trim() || !lastName.trim() || !contact.trim() || !email.trim() || !subject.trim() || !message.trim()
            || !serviceType || !eventDate || !location.trim() || !budgetRange) {
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

        // Validate event date: no past dates, and no more than 6 months in the future
        var today = new Date();
        today.setHours(0, 0, 0, 0);
        var maxEventDate = new Date(today);
        maxEventDate.setMonth(maxEventDate.getMonth() + 6);
        var selectedEventDate = new Date(eventDate + 'T00:00:00');

        if (isNaN(selectedEventDate.getTime()) || selectedEventDate < today || selectedEventDate > maxEventDate) {
            // Hide the loader
            $('.custom-loader, .overlay').hide();
            // Show validation message
            alert("Event date must be today or a future date, within the next 6 months");
            return; // Exit the function if event date is out of range
        }

        // Validate email address
        if (!emailPattern.test(email)) {
            // Hide the loader
            $('.custom-loader, .overlay').hide();
            // Show validation message
            alert("Please enter a valid email address");
            return; // Exit the function if email is invalid
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
                Message: message,
                ServiceType: serviceType,
                EventDate: eventDate,
                Location: location,
                BudgetRange: budgetRange
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
