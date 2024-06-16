$(document).ready(function () {
    // Fetch categories and populate the category select box
    fetchCategoriesAndPopulateSubcategories();

    // Prevent default form submission
    document.getElementById("uploadForm").addEventListener("submit", function (event) {
        event.preventDefault();
        $('.custom-loader, .overlay').show();
        var formData = new FormData(); // Create FormData object to send files

        var files = document.getElementById("fileUpload").files;
        for (var i = 0; i < files.length; i++) {
            formData.append("files", files[i]);
        }

        // Add category and subcategory data to FormData
        var categorySelect = document.getElementById("categorySelect");
        var categoryName = categorySelect.options[categorySelect.selectedIndex].text;

        var subCategorySelect = document.getElementById("subCategorySelect");
        var subCategoryName = subCategorySelect.options[subCategorySelect.selectedIndex].text;

        formData.append("categoryName", categoryName);
        formData.append("subCategoryName", subCategoryName);

        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/Admin/HandleUpload?categoryName=" + encodeURIComponent(categoryName) + "&subCategoryName=" + encodeURIComponent(subCategoryName));
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    $('.custom-loader, .overlay').hide();
                    alert("Files uploaded successfully!");
                } else {
                    $('.custom-loader, .overlay').hide();
                    alert("Error uploading files: " + xhr.responseText);
                }
            }
        };
        xhr.send(formData); // Send FormData with files and additional data
    });

    // Fetch subcategories only when a category is selected
    $('#categorySelect').change(function () {
        var categoryId = $(this).val(); // Get the selected category ID
        if (categoryId) { // Check if category ID is selected (not empty)
            fetchSubcategoriesAndPopulateSelect(categoryId);
        }
    });

    // Function to fetch categories and populate the category select box
    function fetchCategoriesAndPopulateSubcategories() {
        fetch("/Admin/GetCategories")
            .then(response => response.json())
            .then(data => {
                var categorySelect = document.getElementById("categorySelect");
                data.forEach(category => {
                    var option = document.createElement("option");
                    option.text = category.name;
                    option.value = category.id;
                    categorySelect.add(option);
                });

                // Check if category is selected (not empty), then fetch subcategories
                var categoryId = $('#categorySelect').val();
                if (categoryId) {
                    fetchSubcategoriesAndPopulateSelect(categoryId);
                }
            });
    }

    // Function to fetch subcategories based on category ID and populate the subcategory select box
    function fetchSubcategoriesAndPopulateSelect(categoryId) {
        fetch("/Admin/GetSubCategories?categoryId=" + categoryId)
            .then(response => response.json())
            .then(data => {
                var subCategorySelect = document.getElementById("subCategorySelect");
                subCategorySelect.innerHTML = ''; // Clear existing options

                if (data.length === 0) { // Check if no data is returned
                    var noneOption = document.createElement("option");
                    noneOption.text = "None";
                    noneOption.value = "0";
                    noneOption.selected = true; // Set "None" option as selected by default
                    subCategorySelect.add(noneOption);
                } else {
                    data.forEach(subCategory => {
                        var option = document.createElement("option");
                        option.text = subCategory.name;
                        option.value = subCategory.id;
                        subCategorySelect.add(option);
                    });
                }
            });
        }

});
