(function () {

    var showContentTimer;

    $(document).ready(function () {
        sendGetExhibitsRequest(0);

        var resizeTimer;
        $(window).resize(function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(onResizeTimerElapsed, 100);
        });

        $("input[name=mode]").change(changeModeHandler);

        $("#modeBtn").click(showModePopupHandler);
        $("#logInBtn").click(showLogInPopupHandler);
        $("#signUpBtn").click(showSignUpPopupHandler);

        if ($("#auth").html() === "True") {
            $("#logOutForm").show();
        }

        $("#page-cover").click(closeTask);

        $(document).on("mouseenter", ".commentContent", showFullContentHandler);
        $(document).on("mouseleave", ".commentContent", hideFullContentHandler);
    });

    function showFullContentHandler(event) {
        var $element = $(event.target);
        var $c = $element
                   .clone()
                   .css({ display: "inline", width: "auto", visibility: "hidden" })
                   .appendTo("body");

        if ($c.width() > $element.width()) {
            // text was truncated. 
            clearTimeout(showContentTimer);
            showContentTimer = setTimeout(function () {
                $("#commentContentHelper").html($element.html());
                var height = $("#commentContentHelper").outerHeight(true);
                var position = { top: ($element.offset().top - height) };

                $("#commentContentHelper").show();
                $("#commentContentHelper").offset(position);
            }, 1000);
        }

        $c.remove();
    }

    function hideFullContentHandler(event) {
        clearTimeout(showContentTimer);
        $("#commentContentHelper").hide();
    }

    function onAddCommentSuccess(response) {
        if (response.indexOf("validation-summary-errors") > -1)
            return;

        $.ajax({
            url: "Forum/GetComments",
            type: "Post",
            data: {
                pictureIndex: $("input[name=PictureIndex]").val()
            },
            success: function (response) {
                $("#forumPlaceholder").html(response);
            }
        });
    }

    function onGetCommentsSuccess() {
        $("#page-cover").fadeIn(300);
        $("#forumPlaceholder").show();
    }

    function closeTask() {
        $("#page-cover").fadeOut(300);
        $("#forumPlaceholder").hide();
    }

    function onLogInSuccess(response) {
        if (response.indexOf("validation-summary-errors") > -1)
            return;

        $("#logInForm").hide();

        $(document).off("click", hideLogInPopupHandler);

        $("#logInBtn").hide();
        $("#signUpBtn").hide();

        $("#logOutForm").show();

        var pageNumber = $("input[name='currentPageNumber']").val();

        if ($("input[name=mode]:checked").val() === "Json") {
            $.getJSON("Home/GetExhibitsJson", { pageNumber: pageNumber, pageSize: getPageSize() }, onFirstJsonReceived);
        } else {
            sendGetExhibitsRequest(pageNumber);
        }
    }

    function onSignUpSuccess(response) {
        if (response.indexOf("validation-summary-errors") > -1)
            return;

        $("#signUpForm").hide();

        $(document).off("click", hideSignUpPopupHandler);

        $("#logInBtn").hide();
        $("#signUpBtn").hide();

        $("#logOutForm").show();

        var pageNumber = $("input[name='currentPageNumber']").val();

        if ($("input[name=mode]:checked").val() === "Json") {
            $.getJSON("Home/GetExhibitsJson", { pageNumber: pageNumber, pageSize: getPageSize() }, onFirstJsonReceived);
        } else {
            sendGetExhibitsRequest(pageNumber);
        }
    }

    function showSignUpPopupHandler(event) {
        event.preventDefault();
        event.stopPropagation();

        hideModePopupHandler(event);
        hideLogInPopupHandler(event);

        $("#signUpForm").show();
        $(document).click(hideSignUpPopupHandler);
        $("#signUpBtn").off("click", showSignUpPopupHandler);
    }

    function hideSignUpPopupHandler(event) {
        if ($(event.target).closest("#signUpForm").length === 0) {
            $("#signUpForm").hide();
            $(document).off("click", hideSignUpPopupHandler);
            $("#signUpBtn").click(showSignUpPopupHandler);
        }
    }

    function showLogInPopupHandler(event) {
        event.preventDefault();
        event.stopPropagation();

        hideModePopupHandler(event);
        hideSignUpPopupHandler(event);

        $("#logInForm").show();
        $(document).click(hideLogInPopupHandler);
        $("#logInBtn").off("click", showLogInPopupHandler);
    }

    function hideLogInPopupHandler(event) {
        if ($(event.target).closest("#logInForm").length === 0) {
            $("#logInForm").hide();
            $(document).off("click", hideLogInPopupHandler);
            $("#logInBtn").click(showLogInPopupHandler);
        }
    }

    function showModePopupHandler(event) {
        event.stopPropagation();

        hideLogInPopupHandler(event);
        hideSignUpPopupHandler(event);

        $("#modeForm").show();
        $(document).click(hideModePopupHandler);
        $("#modeBtn").off("click", showModePopupHandler);
    }

    function hideModePopupHandler(event) {
        if ($(event.target).closest("#modeForm").length === 0) {
            $("#modeForm").hide();
            $(document).off("click", hideModePopupHandler);
            $("#modeBtn").click(showModePopupHandler);
        }
    }

    function changeModeHandler(event) {
        var pageNumber = $("input[name='currentPageNumber']").val();

        if ($("input[name=mode]:checked").val() === "Json") {
            $.getJSON("Home/GetExhibitsJson", { pageNumber: pageNumber, pageSize: getPageSize() }, onFirstJsonReceived);
        } else {
            sendGetExhibitsRequest(pageNumber);
        }
    }

    function showForumHandler(event) {
        event.preventDefault();

        $.ajax({
            url: "Forum/GetComments",
            type: "Post",
            data: {
                pictureIndex: $(event.target).siblings("input[name='pictureIndex']").val()
            },
            success: function (response) {
                $("#forumPlaceholder").html(response);
                onGetCommentsSuccess();
            }
        });
    }

    //-----------------------------------

    function onFirstJsonReceived(data) {
        onJsonReceived(data);
        $("#exhibitList").show();
        $("#pager").show();
    }

    function onPagingLinkClick(event) {
        event.preventDefault();
        var pageNumber = $("input[name='currentPageNumber']").val();
        if (event.target.id === "previous") {
            pageNumber--;
        } else {
            pageNumber++;
        }

        $.getJSON("Home/GetExhibitsJson", { pageNumber: pageNumber, pageSize: getPageSize() }, onJsonReceived);
    }

    function onJsonReceived(data) {
        var items = [];
        $.each(data.exhibits, function (key, val) {
            items.push("<li><input type='hidden' name='pictureIndex' value='" + (data.startIndex++) 
                + "'><div class='pictureContainer'><img class='picture' src='"
                + val.source + "'></div><div class='description'>" + val.description 
                + "</div>");
            if (data.isAuthorized === true) {
                items.push("<a href='#' class='forumLink'>Comments</a></li> ");
            } else {
                items.push("</li> ");
            }
        });

        $("#exhibitList").html(items.join(""));

        $(".forumLink").click(showForumHandler);

        var pagerContent = "";
        if (data.paging.pageNumber !== 0) {
            pagerContent += "<a href='Home/GetExhibits?pageNumber=" + (data.paging.pageNumber - 1) + "' class='pagingLink' id='previous'>previous</a>";
        }
        pagerContent += "<span id='pageDescription'>Page " + (data.paging.pageNumber + 1) + " of " + data.paging.pagesCount + "</span>";
        if (data.paging.nextPageExists === true) {
            pagerContent += "<a href='Home/GetExhibits?pageNumber=" + (data.paging.pageNumber + 1) + "' class='pagingLink' id='next'>next</a>";
        }

        $("#pager").html(pagerContent);

        $("#previous").click(onPagingLinkClick);
        $("#next").click(onPagingLinkClick);

        $("input[name='currentPageNumber']").val(data.paging.pageNumber);
    }

    //-----------------------------------

    function onResizeTimerElapsed() {
        var pageNumber = $("input[name='currentPageNumber']").val();

        if ($("input[name=mode]:checked").val() === "Partial") {
            sendGetExhibitsRequest(pageNumber);
        } else {
            $.getJSON("Home/GetExhibitsJson", { pageNumber: pageNumber, pageSize: getPageSize() }, onJsonReceived);
        }
    }

    function getPageSize() {
        var itemWidth = $("#exhibitList li").outerWidth(true);

        var pageWidth = $(window).width() - $("#leftMenu").outerWidth(true);
        var result = Math.floor(pageWidth / itemWidth);
        return result !== 0 ? result : 1;
    }

    function sendGetExhibitsRequest(pageNumber) {
        $.ajax({
            url: "Home/GetExhibits",
            type: "Post",
            data: {
                pageNumber: pageNumber,
                pageSize: getPageSize()
            },
            success: function (response) {
                $("#pageContent").html(response);
                onGetExhibitsSuccess(response);
            }
        });
    }

    function onGetExhibitsSuccess(response) {
        $("#exhibitList").show();
        $("#pager").show();
        $("input[name='pageSize']").val(getPageSize());
    }

    window.onGetExhibitsSuccess = onGetExhibitsSuccess;
    window.onLogInSuccess = onLogInSuccess;
    window.onSignUpSuccess = onSignUpSuccess;
    window.onGetCommentsSuccess = onGetCommentsSuccess;
    window.onAddCommentSuccess = onAddCommentSuccess;
})();