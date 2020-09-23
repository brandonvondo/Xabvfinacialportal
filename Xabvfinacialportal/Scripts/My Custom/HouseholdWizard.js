
(function ($) {
    'use strict';

    $(function () {
        /* Code for attribute data-custom-class for adding custom class to tooltip */
        if (typeof $.fn.tooltip.Constructor === 'undefined') {
            throw new Error('Bootstrap Tooltip must be included first!');
        }

        var Tooltip = $.fn.tooltip.Constructor;

        // add customClass option to Bootstrap Tooltip
        $.extend(Tooltip.Default, {
            customClass: ''
        });

        var _show = Tooltip.prototype.show;

        Tooltip.prototype.show = function () {

            // invoke parent method
            _show.apply(this, Array.prototype.slice.apply(arguments));

            if (this.config.customClass) {
                var tip = this.getTipElement();
                $(tip).addClass(this.config.customClass);
            }

        };
        $('[data-toggle="tooltip"]').tooltip();

    });
})(jQuery); //tooltip setup

(function ($) {
    'use strict';
    $(function () {
        $.fn.editable.defaults.mode = 'inline';
        $.fn.editableform.buttons =
            '<button type="submit" class="btn btn-primary editable-submit">' +
            '<i class="mdi mdi-check"></i>' +
            '</button>' +
            '<button type="button" class="btn btn-default editable-cancel">' +
            '<i class=" mdi mdi-close"></i>' +
            '</button>';
        $('.budgetName').editable({
            validate: function (value) {
                if ($.trim(value) === '') return ' This field is required';
            }
        });
    });
})(jQuery); //setting up my inline name change

var form = $("#householdwizard"); //intermediate var for our wizard

form.children("div").steps({
    headerTag: "h3",
    bodyTag: "section",
    transitionEffect: "slideLeft",
    onStepChanging: function (event, currentIndex, newIndex) {
        let isValid = true;

        if (currentIndex === 0) {
            isValid = baValid();
        }
        if (currentIndex === 1 && newIndex > currentIndex) {
            isValid = budgetValid();
        }
        if (currentIndex === 1 && newIndex === 2) {
            budgetArray.forEach(arrayNames);
        }
        if (newIndex === 3) {
            let ba = baArray.length.toString();
            let b = budgetArray.length.toString();
            let i = itemArray.length.toString();
            $("#bankCountSpan").text(ba);
            $("#budgetCountSpan").text(b);
            $("#budgetitemCountSpan").text(i);

        }

        return isValid; //form.valid()
    },
    onFinishing: function (event, currentIndex) {
        let amValid = baValid();
        if (!amValid) {
            return false;
        }
        return amValid;
    },
    onFinished: function (event, currentIndex) {
        swal({
            title: 'Are you sure?',
            text: "You won't be able to come back to this data",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3f51b5',
            cancelButtonColor: '#ff4081',
            confirmButtonText: 'Great ',
            buttons: {
                cancel: {
                    text: "Cancel",
                    value: null,
                    visible: true,
                    className: "btn btn-danger",
                    closeModal: true,
                },
                confirm: {
                    text: "OK",
                    value: true,
                    visible: true,
                    className: "btn btn-primary",
                    closeModal: true
                }
            }
        }).then((result) => {
            if (result) {
                var finalBudgets = packageBudgets();
                var finalBankAccounts = packageBankAccs();
                var houseSetup = { BankAccounts: finalBankAccounts, Budgets: finalBudgets };
                $.ajax({
                    type: "POST",
                    url: "/Households/ConfigureHouse",
                    data: JSON.stringify(houseSetup),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (APP_ROOT_URL) {
                        window.location.assign(APP_ROOT_URL);
                    }
                });
            }
        })
    }
}); //setting up our wizard

function arrayNames(value) {
    let text = $(`#${value}name`).text();
    $(`#pills-${value}-tab`).text(text);
} //looping on arrival to page 3 to update our names

function packageBankAccs() {
    var b = new Array(baArray.length);
    for (var i = 0; i < b.length; i++) {
        var ba = baArray[i];
        var name = $(`#${ba}name`).val();
        var type = $(`#${ba}type`).val();
        var startPre = $(`#${ba}startbal`).val();
        var startFin = inputMaskToNum(startPre);
        var warnPre = $(`#${ba}warningbal`).val();
        var warnFin = inputMaskToNum(warnPre);

        b[i] = { Name: name, Type: type, StartingBalance: startFin, WarningBalance: warnFin};
    }
    return b;
}

function packageBudgets() {
    var b = new Array(budgetArray.length);
    for (var i = 0; i < b.length; i++) {
        var budget = budgetArray[i]
        let name = $(`#${budget}name`).text();
        var items = new Array();
        var bc = 0;
        for (var l = 0; l < itemArray.length; l++) {
            let str = itemArray[l] //budget1-item1
            if (str.includes(budget)) {
                let strA = str.split("-");
                let iN = strA[1]; //item1
                let name = $(`#${iN}name`).text();
                let valPre = $(`#${iN}target`).val();
                let valFin = inputMaskToNum(valPre);
                items[bc] = { Name: name, TargetValue: valFin };
                bc++;
            }
        }
        b[i] = { Name: name, Items: items}
    }
    return b;
}

function removeA(arr) {
    var what, a = arguments, L = a.length, ax;
    while (L > 1 && arr.length) {
        what = a[--L];
        while ((ax = arr.indexOf(what)) !== -1) {
            arr.splice(ax, 1);
        }
    }
    return arr;
} // removing a specific account from our array upon deletion // Setting up everything


var baArray = ["bankAccount1"]; //starting our array to hold bank account IDS
var baCount = 1; //count for bank account unique creation

function baValid() {
    let noError = true;
    for (loop = 0; loop < baArray.length; loop++) {
        let ba = baArray[loop];
        let name = $(`#${ba}name`);
        var start = $(`#${ba}startbal`);
        var warn = $(`#${ba}warningbal`);
        let sVal = start.val();
        let wVal = warn.val();
        var s = inputMaskToNum(sVal);
        var w = inputMaskToNum(wVal);
        if (s > w) {
            warn.removeClass("errorname");
            warn.tooltip('dispose');
        }
        else {
            noError = false;
            warn.addClass("errorname");
            warn.data("placement", "right");
            warn.data("custom-class", "tooltip-danger");
            warn.attr("title", "Warning Balance should be lower than Starting Balance.");
        }
        let content = name.val();
        if (content === "") {
            noError = false;
            name.addClass("errorname");
            name.data("placement", "right");
            name.data("custom-class", "tooltip-danger");
            name.attr("title", "A Name is Required");
        }
        else {
            name.removeClass("errorname");
            name.tooltip('dispose');
        }
    }
    $('.errorname').tooltip();
    return noError;
}

function BAC() {
    let num = baArray.length;
    $("#baCount").text(`Accounts being created: ${num}`)
} // tracking our nmber of accounts in our account array

$('#addBankbtn').click(function (e) {
    e.preventDefault();
    baCount++;
    baArray.push(`bankAccount${baCount}`)
    let large = `<div id="bankAccount${baCount}card" class="card card-inverse-success wizardcard-context new-item">
<div class="card-header mt-1">
<div class="row">
<div class="col-4">
<button onclick="minBa(this.name)" name="bankAccount${baCount}" class="btn btn-rounded btn-primary btn-icon" type="button"><i class="mdi mdi-window-minimize"></i></button>
<button onclick="remBa(this.name)" name="bankAccount${baCount}" class="btn btn-rounded btn-primary btn-icon ml-1" type="button"><i class="mdi mdi-window-close "></i></button>
</div>
<div class="col-8">
<div class="card-title batitle">Bank Account: <span id="bankAccount${baCount}ns">Sample Account Name</span></div>
</div>
</div>
</div>
<div id="bankAccount${baCount}body" class="card-body bankcardbody">
<div class="container bankAccount${baCount} mt-3">
<div class="row">
<div class="col-4">
<div class="form-group">
<label>Account Name</label>
<input id="bankAccount${baCount}name" onkeyup="nameUpdate(this.id)" class="form-control accname required" placeholder="Sample Account Name">
</div>
</div>
<div class="col-2">
<div class="form-group">
<label>Account Type</label>
<select class="form-control" data-val="true" data-val-required="The Account Type field is required." id="bankAccount${baCount}type" name="AccountType">
<option selected="selected" value="0">Checking</option>
<option value="1">Savings</option>
</select>
</div>
</div>
<div class="col-4">
<div class="card-title">
<h6>Suggested Warning Balance Amounts:</h6>
</div>
<div class="buttonbox">
<div onclick="percBtn(this.id)" id="bankAccount${baCount}-66" class="btn btn-dark btn-rounded percbtn">66%</div>
<div onclick="percBtn(this.id)" id="bankAccount${baCount}-50" class="btn btn-dark btn-rounded percbtn">50%</div>
<div onclick="percBtn(this.id)" id="bankAccount${baCount}-33" class="btn btn-dark btn-rounded percbtn">33%</div>
<div onclick="percBtn(this.id)" id="bankAccount${baCount}-20" class="btn btn-dark btn-rounded percbtn">20%</div>
</div>
</div>
</div>
<div class="row">
<div class="col-4">
<div class="form-group">
<label>Starting Balance</label>
<input id="bankAccount${baCount}startbal" name="bankAccount${baCount}" onkeyup="babalUpdate(this.name)" class="form-control" data-inputmask="'alias': 'currency'" value="1"/>
</div>
</div>
<div class="col-4 offset-2">
<div class="form-group">
<label>Warning Balance</label>
<input id="bankAccount${baCount}warningbal" name="bankAccount${baCount}" onkeyup="babalUpdate(this.name)" class="form-control" data-inputmask="'alias': 'currency'" value="0"/>
</div>
</div>
</div>
</div>
</div>
</div>`;
    $("#baContainer").append(large);
    $('#baContainer').animate({ scrollTop: $('#baContainer').prop("scrollHeight") }, 500);
    $(":input").inputmask();
    BAC();
    $('#addBankbtn').blur();
}); //adding a new bank account

function minBa(baId) {
    let bodyId = `#${baId}body`;
    $(bodyId).toggle(400);
    $(`[name='${baId}']`).children('.mdi-window-minimize, .mdi-window-maximize ').toggleClass("mdi-window-minimize mdi-window-maximize");
} // minimize bank account tabs

$("#minAllBa").on("click", function () {
    $(".bankcardbody").hide(400);
    $(".btn").children('.mdi-window-minimize').toggleClass("mdi-window-minimize mdi-window-maximize");
}); // toggle bank account tabs

function nameUpdate(baId) {
    let name = $(`#${baId}`);
    let inpVal = name.val();
    if (inpVal === "") {
        name.addClass("errorname");
        name.data("placement", "right");
        name.data("custom-class", "tooltip-danger");
        name.attr("title", "A Name is Required");
        $('.errorname').tooltip();
    }
    else {
        name.removeClass("errorname");
        name.tooltip('dispose');
    }
    let Id = baId.slice(0, -4);
    let tspan = `#${Id}ns`;

    $(tspan).text(`${inpVal}`);
    if (inpVal == "") {
        $(tspan).text("Sample Account Name");
    }
} //as we type the bank account name we update our card name span

function babalUpdate(baId) {
    var start = $(`#${baId}startbal`);
    var warn = $(`#${baId}warningbal`);
    let sVal = start.val();
    let wVal = warn.val();
    var s = inputMaskToNum(sVal);
    var w = inputMaskToNum(wVal);
    if (s > w) {
        warn.removeClass("errorname");
        warn.tooltip('dispose');
    }
    else {
        noError = false;
        warn.addClass("errorname");
        warn.data("placement", "right");
        warn.data("custom-class", "tooltip-danger");
        warn.attr("title", "Warning Balance should be lower than Starting Balance.");
        $('.errorname').tooltip();
    }
} //as we type balance we validate them

nameUpdate("bankAccount1name") // running the function once on page load

function inputMaskToNum(num) {
    let a = num;
    var number = Number(a.replace(/[^0-9.-]+/g, ""));
    return number;
}

function percBtn(fullperc) {
    let percarray = fullperc.split("-");
    let bankacc = percarray[0];
    let percent = percarray[1];
    percent = parseInt(percent, 10);
    let startid = `#${bankacc}startbal`;
    let warnid = `#${bankacc}warningbal`;
    let currency = $(startid).val();
    var number = inputMaskToNum(currency);
    let rnumber = Math.round(number);
    let warnval = (percent / 100) * rnumber;
    warnval = Math.round(warnval);
    $(warnid).val(warnval);
}; // getting our warnbal suggestion buttons to work // Bank Account Code

var budgetArray = ["budget1"]; //starting our array to hold bank account IDS
var budgetCount = 1; //count for bank account unique creation

$('#budgetInfo').click(function () {
    $('#budgetInfo').blur();
});

function budgetValid() {
    let noError = true;
    for (loop = 0; loop < budgetArray.length; loop++) {
        let name = $(`#${budgetArray[loop]}name`);
        let content = name.text();
        if (content === "") {
            noError = false;
            name.addClass("errorname");
            name.data("placement", "top");
            name.data("custom-class", "tooltip-danger");
            name.attr("title", "A Name is Required");
        }
        else {
            name.removeClass("errorname");
            name.tooltip('dispose');
        }
    }
    $('.errorname').tooltip();
    return noError;
}

function bnameUpdate(budgetId) {
    let name = $(`#${budgetId}`);
    let Id = $(`#pills-${budgetId}-tab`)
    let inpVal = name.val();
    Id.text(`${inpVal}`);
} //as we type the budget name we update our budget item name

$('.budgetName').on('hidden', function (e, reason) {
    if (reason === 'save' || reason === 'cancel') {
        bnameUpdate(e.name);
    }
});

function BUC() {
    let num = budgetArray.length;
    $("#budgetCount").text(`Budgets being created: ${num}`)
} // tracking our nmber of categories in our budget array     BUdget array Count = BUC



var itemArray = ["budget1-item1"];
var itemCount = 1;

$('#addBudgetBtn').click(function (e) {
    e.preventDefault();
    budgetCount++;
    budgetArray.push(`budget${budgetCount}`)
    itemCount++;
    itemArray.push(`budget${budgetCount}-item${itemCount}`)
    let large =
        `<div id="budget${budgetCount}card" class="card card-inverse-success wizardcard-context budgetcard mt-4 mb-4 new-item">
<div class="card-header">
<div class="row">
<div class="col-2">
<button onclick="remBa(this.name)" name="budget${budgetCount}" class="btn btn-rounded btn-primary btn-icon ml-1" type="button"><i class="mdi mdi-window-close "></i></button>
</div>
<div class="col-8 text-center">
<p class="budgetNamecontainer">Budget:</p> <br />
<a href="#" id="budget${budgetCount}name" name="budget${budgetCount}" onchange="bnameUpdate(this.name)" data-type="text" class="budgetName">Sample Budget Name</a>
</div>
</div>
</div>
</div>`;
    $("#budgetContainer").append(large);
    $('#budgetContainer').animate({ scrollTop: $('#budgetContainer').prop("scrollHeight") }, 500);

    let large2 =
        `<li class="nav-item" id="budget${budgetCount}list">
<a class="nav-link" id="pills-budget${budgetCount}-tab" data-toggle="pill" href="#pills-budget${budgetCount}" role="tab" aria-controls="pills-budget${budgetCount}" aria-selected="false">Sample Budget Name</a>
</li>`
    $('#bitabs ul').append(large2);
    let large3 =
`<div class="tab-pane fade" id="pills-budget${budgetCount}" role="tabpanel" aria-labelledby="pills-budget${budgetCount}-tab">
<div class="bahead mb-2">
<div class="row">
<h2>Budget Item Creation <button onclick="biCreate(this.name, 'budget${budgetCount}')" name="#budget${budgetCount}itemContainer" type="button" class="btn btn-rounded btn-primary btn-icon itemAdd" data-toggle="tooltip" data-placement="right" title="Add an Additional Item" data-custom-class="tooltip-primary"><i class="mdi mdi-plus"></i></button></h2>
</div>
</div>

<div id="budget${budgetCount}itemContainer">
<div id="item${itemCount}card" name="item1" class="card card-inverse-success wizardcard-context">
<div class="card-header bihead mt-1">
<div class="row">
<div class="col-1">
<button id="itemInfo" type="button" class="btn btn-rounded btn-primary btn-icon" data-toggle="tooltip" data-placement="top" title="A budget item is a category within a budget e.g. budget 'Household Expenses' with budget item 'Food'" data-custom-class="tooltip-primary"><i class=" mdi mdi-information-variant"></i></button>
</div>
<div class="col-3">
<div class="card-title batitle text-right">
<a href="#" data-type="text" id="item${itemCount}name" class="budgetName budgetNamecontainer">Sample Item</a>
</div>
</div>
<div class="offset-1 col-3">
Target Amount:
</div>
<div class="col-4">
<input id="item${itemCount}target" class="budgetitemtotal text-center" data-inputmask="'alias': 'currency'" />
</div>
</div>
</div>
</div>
</div>

</div>`;
    $('#pills-tabContent').append(large3);
    BUC();
    $(":input").inputmask();
    $('.budgetName').editable({
        validate: function (value) {
            if ($.trim(value) === '') return ' This field is required';
        }
    });
    $('#addBudgetbtn').blur();
}); //adding a new budget account // Budget Code


function biCreate(container, budgetNum) {
    itemCount++;
    itemArray.push(`budget${budgetNum}-item${itemCount}`);
    let large =
        `<div id="item${itemCount}card" name="item${itemCount}" class="card card-inverse-success wizardcard-context new-item">
                <div class="card-header bihead mt-1">
                <div class="row">
                <div class="col-1">
                <button onclick="remBi(this.name , 'budget${budgetNum}')" name="item${itemCount}" class="btn btn-rounded btn-primary btn-icon ml-1" type="button"><i class="mdi mdi-window-close "></i></button>
                </div>
                <div class="col-3">
                <div class="card-title batitle text-right">
                <a href="#" data-type="text" id="item${itemCount}name" class="budgetName budgetNamecontainer">Sample Item</a>
                </div>
                </div>
                <div class="offset-1 col-3">
                Target Amount:
                </div>
                <div class="col-4">
                <input id="item${itemCount}target" class="budgetitemtotal text-center" data-inputmask="'alias': 'currency'" />
                </div>
                </div>
                </div>
                </div>`;
    $(container).append(large);
    $(":input").inputmask();
    $('.budgetName').editable({
        validate: function (value) {
            if ($.trim(value) === '') return ' This field is required';
        }
    });
    $('.itemBtn').blur();
}

function remBa(Id) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, it can't be recovered.",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                let cardId = `${Id}card`;
                var card = document.getElementById(cardId);
                let type = Id.charAt(1);
                switch (type) {
                    case "a":
                        baArray = removeA(baArray, Id);
                        BAC();
                        $(`#${cardId}`).addClass('removed-item').one('webkitAnimationEnd oanimationend msAnimationEnd animationend', function (e) {
                            $(`#${cardId}`).css({ "visibility": "hidden" }).slideUp(500, function () {
                                card.remove();
                            });
                        });
                        swal("Your Bank Account entry has been deleted!", {
                            icon: "success",
                        });
                        break;
                    case "u":
                        budgetArray = removeA(budgetArray, Id);
                        BUC();
                        $(`#${Id}list`).remove();
                        $(`#pills-${Id}`).remove();
                        $(`#${cardId}`).addClass('removed-item').one('webkitAnimationEnd oanimationend msAnimationEnd animationend', function (e) {
                            card.remove();
                        });
                        swal("Your Budget entry has been deleted!", {
                            icon: "success",
                        });
                        break;
                }
            } 
        });
}

function remBi(name, bicname) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, it can't be recovered.",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                let cardId = `${name}card`;
                var card = document.getElementById(cardId);
                let fullname = `${bicname}-${name}`;
                itemArray = removeA(itemArray, fullname);
                $(`#${cardId}`).addClass('removed-item').one('webkitAnimationEnd oanimationend msAnimationEnd animationend', function (e) {
                    card.remove();
                });
                swal("Your Budget Item entry has been deleted!", {
                    icon: "success",
                });
            }
        });
};