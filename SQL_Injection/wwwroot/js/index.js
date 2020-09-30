var mode = "";
var homeStep = document.getElementById("home-step");
var injectStep = document.getElementById("inject-step");
var btnEasyMode = document.getElementById("btnEasyMode");
var btnHardMode = document.getElementById("btnHardMode");
var btnExpertMode = document.getElementById("btnExpertMode");
var btnBack = document.getElementById("btnBack");
var btnSearch = document.getElementById("btnSearch");

// Mode will be set depending on which button we click,
btnEasyMode.onclick = function(){
    ShowInjectStep("easy");
};

btnHardMode.onclick = function () {
    ShowInjectStep("hard");
}

btnExpertMode.onclick = function () {
    ShowInjectStep("expert");
}

// Go back to home step
btnBack.onclick = function () {
    document.getElementsByClassName("animation-overlay")[0].classList.toggle("show");
    setTimeout(function () {
        homeStep.style.display = "block";
        injectStep.style.display = "none";
        btnBack.style.display = "none";
        $("#search").val(""); 
    }, 500)
}

// Closing and clearing console
document.getElementById("btnCloseConsole").onclick = function () {
    $(".rowData").remove();
    $(".center-console").remove();
    $("#console").hide();
}

btnSearch.onclick = function () {
    $("#showData").show();
    $.ajax(
        {
            url: "/Home/SetMode",
            type: "GET",
            dataType: "html",
            data: {
                mode: mode,
                data : $("#search").val()
            },
            success: function (result) {
                Success(result);
            },
            error: function () {
                console.log("Cannot load data");
            }
        }
    );
}


// Functions section

function Success(result) {

    if (result === "noDataErrorMessage") {
        $("#console").show();
        $(".rowData").remove();
        $(".center-console").remove();
        $("#ShowDataTable").hide();
        let message = "Entry does not match any data. Try different word!";
        $("#showData").append("<h4 class='center center-console'>" + message + "</h4>");
    }
    else if (result === "internalErrorMessage")
    {
        $("#console").hide();
        $(".rowData").remove();
        $(".center-console").remove();
        $("#ShowDataTable").hide();
        $("#internalErrorMessage").fadeOut();
        $("#internalErrorMessage").remove();
        $("#btnBack").after
        (
           "<div id='internalErrorMessage'><strong>Internal error message:</strong> There is some error in server! You shoud be aware what you are typing.</div>"
        );
        setTimeout(function ()
        {
            $("#internalErrorMessage").fadeOut();
            $("#internalErrorMessage").remove();
            },
            9000);
        }
    else {
        $("#console").show();
        $(".rowData").remove();
        $(".center-console").remove();
        $("#ShowDataTable").show();
        $("#ShowDataTable tbody").show().append(result);
    }
    
  
}

function ShowInjectStep(modeParam) {
    document.getElementsByClassName("animation-overlay")[0].classList.toggle("show");
    setTimeout(function () {
        homeStep.style.display = "none";
        injectStep.style.display = "block";
        btnBack.style.display = "block";
        mode = modeParam;
    }, 500)
}
