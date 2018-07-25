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
    });

    function showPopupHandler(event) {
        event.stopPropagation();
        $(document).click(hidePopupHandler);

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

        sendGetTasksRequest();
    }

    function onSignUpSuccess(response) {
        if (response.indexOf("validation-summary-errors") > -1)
            return;

        $("#signUpForm").hide();
        $(document).off("click", hidePopupHandler);
        $("#popupLogInBtn").hide();
        $("#popupSignUpBtn").hide();

        $("#logOutForm").show();

        sendGetTasksRequest();
    }

    function onLogOutSuccess(response) {
        $("#logOutForm").hide();

        $("#popupLogInBtn").show();
        $("#popupSignUpBtn").show();

        $("#popupLogInBtn").click(showPopupHandler);
        $("#popupSignUpBtn").click(showPopupHandler);

        $("#logInForm").find("input[type=text]").val("");
        $("#signUpForm").find("input[type=text]").val("");

        sendGetTasksRequest();
    }

    function onAddTaskSuccess(response) {
        if (response.indexOf("validation-summary-errors") > -1)
            return;

        sendGetTasksRequest();
    }

    function onSaveTaskSuccess(response) {
        if (response.indexOf("validation-summary-errors") > -1)
            return;

        sendGetTasksRequest();
    }

    function sendGetTasksRequest(parameters) {
        $.ajax({
            url: "/Tasks/GetTasks",
            type: "Post",
            success: function (response) {
                $("#pageContent").html(response);
            }
        });
    }

    function dimBackground() {
        $("#page-cover").fadeIn(300);
        $("#taskPlaceholder").show();
    }

    function closeTask(event) {
        $("#page-cover").fadeOut(300);
        $("#taskPlaceholder").hide();
    }

    window.onLogInSuccess = onLogInSuccess;
    window.onSignUpSuccess = onSignUpSuccess;
    window.onLogOutSuccess = onLogOutSuccess;
    window.onAddTaskSuccess = onAddTaskSuccess;
    window.dimBackground = dimBackground;
    window.closeTask = closeTask;
    window.onSaveTaskSuccess = onSaveTaskSuccess;
})();