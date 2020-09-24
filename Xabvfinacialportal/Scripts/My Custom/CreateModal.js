function createModal(purpose) {
    var nUrl = `/CreateModal/CreateMultiModal/${purpose}`

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
            $('#Amount').inputmask();
        }, function errorCallback(reponse) {
            swal('Something went wrong!');
        });
}