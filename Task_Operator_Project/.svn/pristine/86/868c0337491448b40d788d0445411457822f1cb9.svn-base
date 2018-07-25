(function () {
    $(document).ready(function () {
        $(".taskName").click(dimBackground);
    });

    function dimBackground() {
        $("#page-cover").css("opacity", 0.6).fadeIn(300, function () {
            //Attempting to set/make #red appear upon the dimmed DIV
            $("#taskPlaceholder").css("zIndex", 10000);
        });
    }

    window.dimBackground = dimBackground;
})();