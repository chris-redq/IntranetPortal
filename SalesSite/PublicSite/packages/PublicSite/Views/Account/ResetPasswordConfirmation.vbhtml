﻿@Code
    ViewBag.Title = "Reset password confirmation"
End Code

<hgroup class="title">
    <h1>@ViewBag.Title.</h1>
</hgroup>
<div>
    <p>
        Your password has been reset. Please @Html.ActionLink("click here to log in", "Login", "Account", routeValues:=Nothing, htmlAttributes:=New With {Key .id = "loginLink"})
    </p>
</div>
