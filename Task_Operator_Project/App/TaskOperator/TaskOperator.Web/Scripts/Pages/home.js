(function() {
    $(document).ready(function() {
        $("#logInForm").hide();
        $("#signUpForm").hide();

        $("#popupLogInBtn").click(showPopupHandler);
        $("#popupSignUpBtn").click(showPopupHandler);

        $("#page-cover").click(closeTask);

        $.ajaxSetup({
            beforeSend: function () {
                $('#loading').show();
            },
            complete: function () {
                $('#loading').hide();
            }
        });

        if ($("#auth").html() === "1") {
            $("#logOutForm").show();
            sendGetTasksRequest(0);
        }

        $("#signUpForm").children("input[name='Username']").change(onUsernameChanged);
    });

    function onUsernameChanged(event) {
        $.ajax({
            url: "UsernameValidationHandler.validate",
            type: "Post",
            data: {
                username: event.target.value
            },
            success: function (response) {
                if (response === "1") {
                    $("#invalidUsername").hide();
                    $("#validUsername").show();
                } else {
                    $("#validUsername").hide();
                    $("#invalidUsername").show();
                }
            }
        });
    }

    function showPopupHandler(event) {
        event.stopPropagation();
        $(document).click(hidePopupHandler);

        $("#validUsername").hide();
        $("#invalidUsername").hide();

        if (event.target.id === "popupLogInBtn") {
            if ($("#popupSignUpBtn").is(":visible")) {
                $("#signUpForm").hide();
            }
            $("#logInForm").show();
            $("#popupLogInBtn").off("click", showPopupHandler);
        } else {
            // event.target.id === "#popupSignUpBtn"
            if ($("#popupLogInBtn").is(":visible")) {
                $("#logInForm").hide();
            }         
            $("#signUpForm").show();
            $("#popupSignUpBtn").off("click", showPopupHandler);
        }
    }

    function hidePopupHandler(event) {

        if ($(event.target).closest("#authPlaceholder").length === 0) {
            // hide
            $("#logInForm").hide();
            $("#signUpForm").hide();

            $(document).off("click", hidePopupHandler);

            $("#popupLogInBtn").click(showPopupHandler);
            $("#popupSignUpBtn").click(showPopupHandler);
        }
    }

    function onLogInSuccess(response) {
        if (response.indexOf("validation-summary-errors") > -1)
            return;

        $("#logInForm").hide();
        $(document).off("click", hidePopupHandler);
        $("#popupLogInBtn").hide();
        $("#popupSignUpBtn").hide();

        $("#logOutForm").show();

        sendGetTasksRequest(0);
    }

    function onSignUpSuccess(response) {
        if (response.indexOf("validation-summary-errors") > -1)
            return;

        $("#signUpForm").hide();
        $(document).off("click", hidePopupHandler);
        $("#popupLogInBtn").hide();
        $("#popupSignUpBtn").hide();

        $("#logOutForm").show();

        sendGetTasksRequest(0);
    }

    function onAddTaskSuccess(response) {
        if (response.indexOf("validation-summary-errors") > -1)
            return;

        var pageNumber = $("input[name='pageNumber']").val();
        sendGetTasksRequest(pageNumber);
    }

    function onSaveTaskSuccess(response) {
        if (response.indexOf("validation-summary-errors") > -1)
            return;

        var pageNumber = $("input[name='pageNumber']").val();
        sendGetTasksRequest(pageNumber);
    }

    function sendGetTasksRequest(pageNumber) {
        $.ajax({
            url: "Tasks/GetTasks",
            type: "Post",
            data: {
                pageNumber: pageNumber
            },
            success: function (response) {
                $("#pageContent").html(response);
            }
        });
    }

    function dimBackground() {
        $("#page-cover").fadeIn(300);
        $("#taskPlaceholder").show();
    }

    function closeTask() {
        $("#page-cover").fadeOut(300);
        $("#taskPlaceholder").hide();
    }

    window.onLogInSuccess = onLogInSuccess;
    window.onSignUpSuccess = onSignUpSuccess;
    window.onAddTaskSuccess = onAddTaskSuccess;
    window.dimBackground = dimBackground;
    window.closeTask = closeTask;
    window.onSaveTaskSuccess = onSaveTaskSuccess;
})();