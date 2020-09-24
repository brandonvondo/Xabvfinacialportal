function leaveHOH() {
    var nUrl = `/Households/LeaveAsyncHOH`

    $('#loader').show();
    $('#ajax-shell').html('');
    $('#transactionModal').modal('show');

    $.ajax({
        url: nUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
        .then(function successCallback(response) {
            $('#loader').hide();
            $('#ajax-shell').html(response);
        }, function errorCallback(reponse) {
            swal('Something went wrong!');
        });
}