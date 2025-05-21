function performSearch () {
    var keyword = $('#searchBox').val();
    if (keyword.trim().length === 0) return;

    $.ajax({
        url: '/Product/Search',
        type: 'GET',
        data: { query: keyword },
        success: function (result) {
            $('#searchResult').html(result);
        },
        error: function (xhr, status, error) {
            console.error('Lỗi AJAX:', error);
        }
    });

}

$(document).ready(function () {
    console.log('jQuery is loaded:', typeof $);
    // Use event delegation for dynamic elements
    $(document).on('click', '#searchButton', function () {
        performSearch();
    });

    // Optional: Allow search on Enter key
    $('#searchBox').on('keypress', function (e) {
        if (e.which === 13) { // Enter key
            performSearch();
        }
    });
});