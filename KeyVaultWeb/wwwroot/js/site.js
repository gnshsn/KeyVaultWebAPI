

function login()
{
        var loginData = {
            grant_type: 'password',
            username: document.getElementById("username").value,
            password: document.getElementById("password").value
        };
        $.ajax({
            type: 'POST',
            url: '/Token',
            data: loginData,
            contentType: 'application/json',
            success: function (response) {
                // Navigate to the user page
                //self.user(response.username);
                sessionStorage.setItem('accessToken', response.access_token);
                window.location.href = "/Keys/List";
            },
            error: function (jqHRX) {
                var err1 = JSON.parse(jqHRX.responseText);
                $('#ErrMsg').html('<span class = "text-danger"> ' + err1['error_description'] + '</span>');
            }
        });
}