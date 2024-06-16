var pageNumber = 1;
var pageSize = 9;
$(document).ready(function () {
    
    $('#searchButton').click(function () {
        var categoryRMSelect = document.getElementById("categoryRMSelect");
        var categoryName = categoryRMSelect.options[categoryRMSelect.selectedIndex].text;

        var subCategoryRMSelect = document.getElementById("subCategoryRMSelect");
        var subCategoryName = subCategoryRMSelect.options[subCategoryRMSelect.selectedIndex].text;

        var category = categoryName;
        var subCategory = subCategoryName;
        $.ajax({
            url: '/Admin/GetRequestedImages',
            type: 'POST',
            data: {
                categoryName: category,
                subCategoryName: subCategory
            },
            success: function (data) {
                renderImages(data, pageNumber, pageSize); 
                renderPagination(data.length);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

    fetchCategoriesAndPopulateSubcategories();

    // Function to fetch categories and populate the category select box
    function fetchCategoriesAndPopulateSubcategories() {
        fetch("/Admin/GetCategories")
            .then(response => response.json())
            .then(data => {
                var categoryRMSelect = document.getElementById("categoryRMSelect");
                categoryRMSelect.innerHTML = ''; // Clear existing options

                // Add "None" option for categories
                var noneCategoryOption = document.createElement("option");
                noneCategoryOption.text = "None";
                noneCategoryOption.value = "0";
                noneCategoryOption.selected = true; // Set "None" option as selected by default
                categoryRMSelect.add(noneCategoryOption);

                data.forEach(category => {
                    var option = document.createElement("option");
                    option.text = category.name;
                    option.value = category.id;
                    categoryRMSelect.add(option);
                });

                // Add event listener to category select box
                categoryRMSelect.addEventListener('change', function () {
                    var categoryId = this.value; // Get selected category ID
                    fetchSubcategoriesAndPopulateSelect(categoryId);
                });

                // Check if category is selected (not empty), then fetch subcategories
                var categoryId = $('#categoryRMSelect').val();
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
                var subCategoryRMSelect = document.getElementById("subCategoryRMSelect");
                subCategoryRMSelect.innerHTML = ''; // Clear existing options

                // Add "None" option for subcategories
              
                if (data.length === 0) { // Check if no data is returned
                    var noneOption = document.createElement("option");
                    noneOption.text = "None";
                    noneOption.value = "0";
                    noneOption.selected = true; // Set "None" option as selected by default
                    subCategoryRMSelect.add(noneOption);
                }
                else {
                    data.forEach(subCategory => {
                        var option = document.createElement("option");
                        option.text = subCategory.name;
                        option.value = subCategory.id;
                        subCategoryRMSelect.add(option);
                    });
                }
            });
    }


    function renderImages(imageUrls, pageNumber, pageSize) {
        var $photoSection = $('#section-photos');
        $photoSection.empty();

        if (imageUrls && imageUrls.length > 0) {
            var startIndex = (pageNumber - 1) * pageSize;
            var endIndex = Math.min(startIndex + pageSize, imageUrls.length);

            for (var i = startIndex; i < endIndex; i++) {
                // Extracting the part of the path after "UploadedFiles"
                var compressedImageUrl = imageUrls[i].compressed.substring(imageUrls[i].compressed.indexOf('UploadedFiles'));

                // Create the container div for the image
                var $container = $('<div>').addClass('image-container');

                // Create and append the image element within the container
                var $image = $('<img>').attr('src', '/' + compressedImageUrl).attr('alt', 'Image').addClass('img-fluid mb-0');
                $container.append($image);

                // Create and append the delete button container
                var $deleteContainer = $('<div>').addClass('delete-container');
                var $deleteIcon = $('<span>').addClass('delete-icon').html('&times;');

                // Attach click event handler to delete button
                $deleteIcon.click((function (index) {
                    return function (event) {
                        event.stopPropagation(); // Stop event propagation to prevent the container from being triggered

                        // Show the loader immediately when the delete button is clicked
                        $('.custom-loader, .overlay').show();

                        // Get the compressed image URL
                        var compressedImageUrl = imageUrls[index].compressed;

                        // Make an AJAX request to call the delete image endpoint
                        $.ajax({
                            url: '/Admin/DeleteImage',
                            type: 'POST',
                            data: { imagePath: compressedImageUrl },
                            success: function (response) {
                                var categoryRMSelect = document.getElementById("categoryRMSelect");
                                var categoryName = categoryRMSelect.options[categoryRMSelect.selectedIndex].text;

                                var subCategoryRMSelect = document.getElementById("subCategoryRMSelect");
                                var subCategoryName = subCategoryRMSelect.options[subCategoryRMSelect.selectedIndex].text;

                                var category = categoryName;
                                var subCategory = subCategoryName;

                                // Fetch the updated images based on selected category and subcategory
                                $.ajax({
                                    url: '/Admin/GetRequestedImages',
                                    type: 'POST',
                                    data: {
                                        categoryName: category,
                                        subCategoryName: subCategory
                                    },
                                    success: function (data) {
                                        var newTotalPages = Math.ceil(data.length / pageSize);

                                        // Check if the current page is affected by the deletion
                                        if (pageNumber > newTotalPages) {
                                            // If the current page is affected, move to the previous page
                                            pageNumber = Math.max(1, newTotalPages); // Ensure pageNumber doesn't go below 1
                                        }

                                        renderImages(data, pageNumber, pageSize);
                                        renderPagination(data.length);
                                        $('.custom-loader, .overlay').hide();
                                    },
                                    error: function (xhr, status, error) {
                                        console.error(error);
                                        $('.custom-loader, .overlay').hide();
                                    }
                                });
                            },
                            error: function (xhr, status, error) {
                                console.error(xhr.responseText); // Log error message or handle as needed
                                $('.custom-loader, .overlay').hide();
                            }
                        });
                    };
                })(i)); // Pass the current value of i to the click event handler

                $deleteContainer.append($deleteIcon);
                $container.append($deleteContainer);

                // Create and append the photo-text-more div within the container
                var $photoTextMore = $('<div>').addClass('photo-text-more');
                $container.append($photoTextMore);

                // Create and append the col element with data-aos attribute
                var $col = $('<div>').addClass('col-6 col-md-6 col-lg-4').attr('data-aos', 'fade-up');
                $col.append($container);

                // Append the col element to the photo section
                $photoSection.append($col);
            }
        } else {
            $photoSection.append($('<div>').addClass('no-images-container').append($('<div>').addClass('no-images-text').text('No images found.')));
        }
    }


    function renderPagination(totalImages) {
        var totalPages = Math.ceil(totalImages / pageSize);
        var $paginationContainer = $('.pagination-container');
        $paginationContainer.empty();

        if (totalPages > 1) {
            var $nav = $('<nav>').attr('aria-label', 'Page navigation');
            var $ul = $('<ul>').addClass('pagination justify-content-center');

            for (var i = 1; i <= totalPages; i++) {
                var $li = $('<li>').addClass('page-item');
                var $a = $('<a>').addClass('page-link').attr('href', 'javascript:void(0)').text(i);
                if (i === pageNumber) {
                    $li.addClass('active');
                }
                $li.append($a);
                $ul.append($li);
            }

            $nav.append($ul);
            $paginationContainer.append($nav);

            $('.pagination-container .page-link').click(function () {
                pageNumber = parseInt($(this).text());
                $('#searchButton').trigger('click');
            });
        }
    }


});