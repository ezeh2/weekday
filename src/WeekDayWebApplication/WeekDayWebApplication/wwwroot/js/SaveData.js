$(document).ready(function () {

    $('#send').on("click", function (event) {
        Send();
    });

    function Send() {
        var data = $('#data').val();

        $.ajax({
            'url': '/Home/SaveData',
            'data': {
                'data': data
            },
            'method': 'GET',
            'cache': false,
            'success': function (data, textStatus, jqXHR) {
                console.log('success');
            },
            'error': function (jqXHR, textStatus, errorThrown) {
                console.log('error');
            }
        });
    }
});

