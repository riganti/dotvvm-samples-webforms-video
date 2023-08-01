$(function () {
    // Apply jQuery UI
    $("nav a").button();
    $("article").addClass("ui-widget-content ui-corner-all");
    $("article header").addClass("ui-widget-header ui-corner-top");
    $(".paging a").button();
    $(".button").button();
    $(".aspNetDisabled").addClass("ui-state-disabled");
    $("a[disabled=true]").addClass("ui-state-disabled");

    // Click confirmation
    $("*[data-confirmprompt]").click(function () {
        return window.confirm($(this).data("confirmprompt"));
    });
});
