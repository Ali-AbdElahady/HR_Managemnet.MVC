// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#EmployeeSearch').on('keyup', function () {
    let searchTerm = $(this).val();
    console.log("SearchEmployees")
    if (searchTerm.length > 0) {
        $.ajax({
            url: 'SearchEmployees', // API URL for patient search
            type: 'GET',
            data: { searchTerm: searchTerm },
            success: function (data) {
                // Clear previous results
                $('#employeeResults').empty().show();

                if (data.length > 0) {
                    // Populate the dropdown list with search results
                    data.forEach(function (employee) {
                        $('#employeeResults').append(`<li class="list-group-item" data-id="${employee.id}">${employee.name}</li>`);
                    });
                } else {
                    $('#employeeResults').append('<li class="list-group-item disabled">No results found</li>');
                }
            }
        });
    } else {
        $('#employeeResults').empty().hide(); // Hide dropdown if input is empty
    }
});

// Handle click on a result item
$(document).on('click', '#employeeResults li', function () {
    let selectedEmployeeName = $(this).text();
    let selectedEmployeetId = $(this).data('id');

    // Set the input field value to the selected patient's name
    $('#EmployeeSearch').val(selectedEmployeeName);

    // Optionally, store the selected patient ID somewhere (e.g., hidden field)
    $('#selectedEmployeeId').val(selectedEmployeetId);

    // Hide the results dropdown
    $('#employeeResults').hide();
});

// Hide dropdown if clicking outside of input or result list
$(document).on('click', function (event) {
    if (!$(event.target).closest('#EmployeeSearch, #employeeResults').length) {
        $('#employeeResults').hide();
    }
});