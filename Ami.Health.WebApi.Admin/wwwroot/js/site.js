/***
Name:           site.js
Description:    Customized jQuery code for application
Developer:      Htet Paing
Date:           2018-Feb-09
***/
$(document).ready(function () {

    //Lock user
    $('[id="lock"]').each(function () {
        $(this).on('click', function () {
            var conf = confirm("Are you sure you want to lock this account?");
            if (!conf) return false;
        });
    });

    //Deactivate user
    $('[id="delete"]').each(function () {
        $(this).on('click', function () {
            var conf = confirm("Are you sure you want to deactivate?");
            if (!conf) return false;
        });
    });
});