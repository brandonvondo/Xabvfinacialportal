
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
        if (currentIndex === 2) {

        }

        return isValid; //form.valid()
    },
    onFinishing: function (event, currentIndex) {
        form.validate().settings.ignore = ":disabled";
        return form.valid();
    },
    onFinished: function (event, currentIndex) {
        alert("Submitted!");
    }
}); //setting up our wizard

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
        let name = $(`#${baArray[loop]}name`);
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
<div class="col-4 offset-2">
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
<input id="bankAccount${baCount}startbal" class="form-control" data-inputmask="'alias': 'currency'" />
</div>
</div>
<div class="col-4 offset-2">
<div class="form-group">
<label>Warning Balance</label>
<input id="bankAccount${baCount}warningbal" class="form-control" data-inputmask="'alias': 'currency'" />
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

nameUpdate("bankAccount1name") // running the function once on page load

function percBtn(fullperc) {
    let percarray = fullperc.split("-");
    let bankacc = percarray[0];
    let percent = percarray[1];
    percent = parseInt(percent, 10);
    let startid = `#${bankacc}startbal`;
    let warnid = `#${bankacc}warningbal`;
    let currency = $(startid).val();
    var number = Number(currency.replace(/[^0-9.-]+/g, ""));
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

function binameUpdate(budgetId) {
    let name = $(`#${budgetId}`);
    let Id = $(`#pills-${budgetId}itemc-tab`)
    let inpVal = name.val();
    Id.text(`${inpVal}`);
} //as we type the budget name we update our budget item name

function BUC() {
    let num = budgetArray.length;
    $("#budgetCount").text(`Budgets being created: ${num}`)
} // tracking our nmber of categories in our budget array     BUdget array Count = BUC


$('#addBudgetBtn').click(function (e) {
    e.preventDefault();
    budgetCount++;
    budgetArray.push(`budget${budgetCount}`)
    let large =
        `<div id="budget${budgetCount}card" class="card card-inverse-success wizardcard-context budgetcard mt-4 mb-4">
<div class="card-header">
<div class="row">
<div class="col-2">
<button onclick="remBa(this.name)" name="budget${budgetCount}" class="btn btn-rounded btn-primary btn-icon ml-1" type="button"><i class="mdi mdi-window-close "></i></button>
</div>
<div class="col-8 text-center">
<p class="budgetNamecontainer">Budget:</p> <br />
<a href="#" id="budget${budgetCount}name" data-type="text" class="budgetName">Sample Budget Name</a>
</div>
</div>
</div>
</div>`;
    $("#budgetContainer").append(large);
    $('#budgetContainer').animate({ scrollTop: $('#budgetContainer').prop("scrollHeight") }, 500);
    $('.budgetName').editable({
        validate: function (value) {
            if ($.trim(value) === '') return ' This field is required';
        }
    });

    let large2 =
        `<li class="nav-item" id="budget${budgetCount}list">
<a class="nav-link active" id="pills-budget${budgetCount}itemc-tab" data-toggle="pill" href="#pills-budget${budgetCount}itemc" role="tab" aria-controls="pills-budget${budgetCount}itemc" aria-selected="false">Sample Budget Name</a>
</li>`
    $('#bitabs ul').append(large2);
    let large3 =
        `<div class="tab-pane fade show active" id="pills-budget${budgetCount}itemc" role="tabpanel" aria-labelledby="pills-budget${budgetCount}itemc-tab">

                                                        <div class="bahead mb-2">
                                                            <div class="row">
                                                                <div class="col-3">
                                                                    <h2>Budget Item Creation <button onclick="biCreate(this.name)" name="budget${budgetCount}item" data-budget="budget1itemc" type="button" class="btn btn-rounded btn-primary btn-icon" data-toggle="tooltip" data-placement="right" title="Add an Additional Item" data-custom-class="tooltip-primary"><i class="mdi mdi-plus"></i></button></h2>                                                                </div>
                                                                <div class="col-9">
                                                                    <div class="row">
                                                                        <div class="col-6">
                                                                            <h3 class="text-right">
                                                                                Budget Total: <input id="budget${budgetCount}itemctotal" class="budgettotal text-center" data-inputmask="'alias': 'currency'" readonly />
                                                                            </h3>
                                                                        </div>
                                                                        <div class="col-6">
                                                                            <h4 class="text-right" id="budget${budgetCount}itemcCount">Budget Items being created: 1</h4>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div id="budget${budgetCount}itemContainer">
                                                            <div id="budget${budgetCount}item1card" name="budget1item" class="card card-inverse-success wizardcard-context">
                                                                <div class="card-header bihead mt-1">
                                                                    <div class="row">
                                                                        <div class="col-1">
                                                                            <button onclick="remBi('budget${budgetCount}item1','budget1itemCount')" class="btn btn-rounded btn-primary btn-icon ml-1" type="button"><i class="mdi mdi-window-close "></i></button>
                                                                        </div>
                                                                        <div class="col-3">
                                                                            <div class="card-title batitle text-right">
                                                                                <a href="#" id="budget${budgetCount}item1name" onchange="binameUpdate(this.id)" data-type="text" class="budgetName budgetNamecontainer">Sample Item</a>
                                                                            </div>
                                                                        </div>
                                                                        <div class="offset-1 col-3">
                                                                            Target Amount:
                                                                        </div>
                                                                        <div class="col-4">
                                                                            <input id="budget${budgetCount}item1target" class="budgetitemtotal text-center" data-inputmask="'alias': 'currency'" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>`
    BUC();
    window[`budget${budgetCount}itemArray`] = ["budget1"];
    $('#addBudgetbtn').blur();
}); //adding a new budget account // Budget Code

function BIC(a, n) {
    let num = a.length;
    $(`#${n}`).text(`Budgets being created: ${num}`)
} // tracking our nmber of items in the budget array     Budget Item Count = BIC

function remBa(Id) {

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
            break;
        case "u":
            budgetArray = removeA(budgetArray, Id);
            BUC();
            $(`#${cardId}`).addClass('removed-item').one('webkitAnimationEnd oanimationend msAnimationEnd animationend', function (e) {
                card.remove();
            });
            break;
    }
}

function remBi(name, bicname) {

    let cardId = `${name}card`;
    var card = document.getElementById(cardId);
    let a = name.slice(0, -1);
    eval(a) = removeA(eval(a), name);
    BIC(aRay, bicname);
    $(`#${cardId}`).addClass('removed-item').one('webkitAnimationEnd oanimationend msAnimationEnd animationend', function (e) {
        card.remove();
    });
}