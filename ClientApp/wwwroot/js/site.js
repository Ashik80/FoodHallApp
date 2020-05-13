function openCity(evt, foodcat) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(foodcat).style.display = "block";
    evt.currentTarget.className += " active";
}
document.getElementById("defaultOpen").click();

function urau(id) {
    var lekho = "#lekho" + id.toString();
    $.ajax({
        url: "/Menus/Create",
        method: 'post',
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        dataType: 'text',
        data: {
            pid: id
        },
        success: function (data) {
            $(lekho).text("Added to cart").stop(true, true).show().delay(1000).fadeOut();
            countdekhau();
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function barau(id, quaf, prif, realprice) {
    var qua = "#" + quaf;
    var pri = "#" + prif;
    $.ajax({
        url: "/Menus/IncreaseQuantity",
        method: 'post',
        data: {
            id: id
        },
        success: function (data) {
            $(qua).text(
                parseFloat($(qua).text()) + 1
            );
            $(pri).text(function () {
                var pr = parseFloat($(pri).text()) + parseFloat(realprice);
                return pr.toFixed(2);
            });
            $("#totalhoise").text(function () {
                var tot = parseFloat($("#totalhoise").text()) + parseFloat(realprice);
                return tot.toFixed(2);
            });
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function komau(id, quaf, prif, realprice, row) {
    var rowamar = "#" + row;
    var qua = "#" + quaf;
    var pri = "#" + prif;
    $.ajax({
        url: "/Menus/DecreaseQuantity",
        method: 'post',
        data: {
            id: id
        },
        success: function (data) {
            if (parseFloat($(qua).text()) > 0) {
                $(qua).text(
                    parseFloat($(qua).text()) - 1
                );
            }
            if (parseFloat($(qua).text()) <= 0) {
                $(rowamar).text("");
                countdekhau();
            }    
            $(pri).text(function () {
                var pr = parseFloat($(pri).text()) - parseFloat(realprice);
                return pr.toFixed(2);
            });
            $("#totalhoise").text(function () {
                var tot = parseFloat($("#totalhoise").text()) - parseFloat(realprice);
                return tot.toFixed(2);
            });
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function loaded() {
    $.ajax({
        url: "/Menus/Cartamar",
        method: 'post',
        dataType: 'html',      
        success: function (data) {
            $("#abaro").html(data);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function countdekhau() {
    $.ajax({
        url: "/Menus/TotalCount",
        method: "post",
        dataType: "text",
        success: function (data) {
            $('#countdekhau').text(data);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

countdekhau();


