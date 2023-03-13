<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchList.aspx.cs" Inherits="WebApplication1.SearchList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Security-Policy: frame-ancestors 'self'" />
    <%--<meta name="author" content="colorlib.com">--%>
    <link href="https://fonts.googleapis.com/css?family=Poppins:400,800" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript" src="Content/js/ellips.js"></script>
    <link href="Content/css/main.css" rel="stylesheet" />
    <script type="text/javascript" src="Content/js/main.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.2.6/jquery.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.2.6/jquery.min.js"></script>
    <script type="text/javascript" src="/js/jquery-ui-personalized-1.5.2.packed.js"></script>
    <script type="text/javascript" src="/js/sprinkle.js"></script>
    <link rel="stylesheet" href="Content/css/paginationstyle.css" />

    <style>
        /*.search {
            margin-right: 200px;
            background: white !important;
            color: black;
            border-radius: 30px;
            border: 1px black solid !important;
        }*/
        .para {
            box-shadow: 0px 0px 10px 1px #bbbbbb;
            padding: 2%;
            margin-bottom: 2%;
        }

        .flightDetailBox {
            margin-left: 5%;
        }

        .link {
            color: #551a8b;
            background: none;
            border: none;
            font-size: larger;
            font-weight: bold;
            text-decoration: underline;
            font-family: 'Poppins';
            cursor: pointer;
            text-align: justify;
        }

        .s004 {
            background: none !important;
            /*background-color: bisque !important;*/
            display: block !important;
        }

        .bodyclass {
            background: none !important;
            /*background-color: bisque !important;*/
        }

        .s004 form {
            max-width: 100%;
        }

        #txtsearch {
            border-radius: 25px;
            border: 2px solid black;
        }

        .input-field .btn-search {
            background: white !important;
            color: black;
            border-radius: 30px;
            border: 2px black solid !important;
        }

        #myData button {
            padding: 0px;
        }

        .loading {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            background: #fff;
        }

        .loader {
            left: 50%;
            margin-left: -4em;
            font-size: 10px;
            border: .8em solid rgba(218, 219, 223, 1);
            border-left: .8em solid rgba(58, 166, 165, 1);
            animation: spin 1.1s infinite linear;
        }

            .loader, .loader:after {
                border-radius: 50%;
                width: 8em;
                height: 8em;
                display: block;
                position: absolute;
                top: 50%;
                margin-top: -4.05em;
            }

        @keyframes spin {
            0% {
                transform: rotate(360deg);
            }

            100% {
                transform: rotate(0deg);
            }
        }

        .divimg {
            float: left;
        }

            .divimg img {
                width: 100%;
                margin-top: 17%;
            }

        .divlegend {
            float: right;
        }

        .s004 form legend {
            margin-bottom: 20px;
        }

        @media screen and (min-width: 350px) and (max-width: 768px) {
            #txtsearch {
                margin-left: 5%;
                width: 70%;
            }

            .input-field .btn-search {
                margin-right: 5%;
            }

            .pagination ul {
                padding: 0px;
            }

                .pagination ul li.numb {
                    margin: 0px 0px;
                }

            .resp {
                margin-bottom: 10px;
            }

            #myBtn {
                margin-right: 2px;
            }
        }



        #PDF .diviframe iframe {
            height: 100%;
            width: 100%;
        }

        .para {
            box-shadow: 0px 0px 10px 1px #bbbbbb;
            padding: 2%;
            margin-bottom: 2%;
        }

        .ellipsis {
            overflow: hidden;
            text-overflow: ellipsis;
            display: -webkit-box;
            -webkit-line-clamp: 2;
            line-clamp: 2;
            -webkit-box-orient: vertical;
        }

            .ellipsis.multiline {
                overflow: hidden;
                text-overflow: ellipsis;
                display: -webkit-box;
                -webkit-line-clamp: 2;
                line-clamp: 2;
                -webkit-box-orient: vertical;
            }

        .maindiv {
            margin-bottom: 5%;
        }

            .maindiv p {
                margin-bottom: unset;
            }

            .maindiv button {
                font-weight: 500;
                margin-bottom: 2px;
            }

        #myDataButton button img {
            height: 20px;
        }

        #myDataButton input {
            width: 20%;
            display: inline-block;
            padding-left: 0px;
            padding-right: 0px;
            text-align: center;
        }

        .dropdown {
            width: 75%;
            float: right;
        }

        .drpdwnlbl {
            margin-top: 2%;
        }

        .search {
        }

        /* Style the tab */
        .tab {
            overflow: hidden;
            border: 2px solid #000;
            background-color: #f1f1f1;
            border-radius: 30px;
        }

            /* Style the buttons inside the tab */
            .tab button {
                background-color: inherit;
                float: left;
                border: none;
                outline: none;
                cursor: pointer;
                padding: 14px 16px;
                transition: 0.3s;
                font-size: 17px;
            }

                /* Change background color of buttons on hover */
                .tab button:hover {
                    background-color: #ddd;
                }

                /* Create an active/current tablink class */
                .tab button.active {
                    background-color: #ccc;
                    box-shadow: 0px 0px 6px 1px #685b5b;
                }

        /* Style the tab content */
        .tabcontent {
            display: none;
            padding: 6px 12px;
            /*border: 1px solid #ccc;*/
            border-top: none;
        }

        #mypdfBtn {
            width: 10%;
            height: 35px;
        }
    </style>

    <script type="text/javascript">
        function validate() {
            if (document.getElementById("txtsearch").value.trim() == "") {
                alert("Please enter some word first !");

            }
            else {

                //var e = document.getElementById("ddlshort");
                //var value = e.value;
                var value = $('#ddlshort :selected').val();
                $("#hidsort").val(value);
                //var sort = e.options[e.selectedIndex].text;


                //var p = document.getElementById("ddlpage");
                var valuep = $('#ddlpage :selected').val();
                //var retmax = p.options[e.selectedIndex].text;

                btnSubmitfunc('', 0, value, valuep);
            }

        }

        function validatepdf() {
            if (document.getElementById("txtsearch").value.trim() == "") {
                alert("Please enter main search term !");

            } else {
                if (document.getElementById("txtpdfsearch").value.trim() == "") {
                    alert("Please enter pdf search term !");
                }
                else {
                    btnpdfSubmitfunc('', 1);
                }
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divh").hide()
        })
    </script>
    <script>
        $(document).ready(function () {
            //document.getElementById("tabdiv").style.display = "none";

            var input = document.getElementById("txtsearch");
            input.addEventListener("keypress", function (event) {
                if (event.key === "Enter") {
                    event.preventDefault();
                    document.getElementById("myBtn").click();
                }
            });

            var input1 = document.getElementById("txtpdfsearch");
            input1.addEventListener("keypress", function (event) {
                if (event.key === "Enter") {
                    event.preventDefault();
                    document.getElementById("mypdfBtn").click();
                }
            });
        });
    </script>
</head>

<body class="bodyclass">



    <script type="text/javascript">

        function btnSubmitfunc(term, ret, sort, max) {
            $('#spinner').show();
            //document.getElementById("tabdiv").style.display = "none";
            var searchterm = '';
            if (term == '') {
                searchterm = document.getElementById("txtsearch").value;
            } else {
                searchterm = term;
            }

            BindGridView(searchterm, ret, sort, max);
        }

        function BindGridView(searchterm, ret, sort, max) {
            document.getElementById("txtpdfsearch").value = "";
            var obj = { Test: searchterm };
            $.ajax({
                type: "POST",
                url: "SearchList.aspx/GetSearchList",
                data: '{Test: "' + searchterm + '",start:"' + ret + '",sort:"' + sort + '",max:"' + max + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    $('#spinner').hide();
                    alert("Something went wrong");
                    return;
                },
                error: function (response) {
                    $('#spinner').hide();
                    alert("Something went wrong");
                    return;
                }
            });
        };

        function OnSuccess(response) {
            console.log(response.d);
            var data = JSON.parse(response.d);
            console.log(data.length);
            $("#divh").hide()
            var mainContainer = document.getElementById("myData");
            var FooterContainer = document.getElementById("myDataButton");
            //var mainContainerpdf = document.getElementById("myData1");
            $('#myData').empty();
            $('#myData1').empty();
            $('#myData2').empty();
            $('#myDataDetails').empty();
            $('#myDataButton').empty();
            for (var i = 0; i < data.length; i++) {
                //var pdff = (data[i].pdf);
                //if (pdff == "") {
                var div = document.createElement('div');
                div.id = 'div#' + data[i].PMID;
                div.className = "maindiv";
                div.innerHTML = '<button type=button id=' + data[i].PMID + ' class=link onclick=btnSubmit(' + data[i].PMID + ',"' + data[i].WebEnv + '",' + data[i].QryList + ')>' + data[i].ArticleTitle + '</button>' +

                    '<p>' + data[i].Author + '</p>' +

                    '<p >' + data[i].ISOAbbreviation + ' ' + data[i].PubDate + ';' + data[i].Volume + '(' + data[i].Issue + '):' + data[i].Pagination + ' doi: ' + data[i].DOI + ' EPub: ' + data[i].EPub + '</p>' +
                    '<p>PMID : ' + data[i].PMID + ' ' + data[i].PubType + '</p>' +
                    '<p class="ellipsis">' + data[i].Abstract + '</p>' +
                    '<p>Keywords : ' + data[i].Keyword + '</p>';

                mainContainer.appendChild(div);
                //}
                //else {
                //$("#divh").show()
                //var div1 = document.createElement('div');
                //div1.style.width = "12%";
                //div1.style.float = "left";
                //div1.style.paddingBottom = "10px";
                //div1.style.paddingTop = "10px";
                //div1.innerHTML = '<a href=' + data[i].pdf + ' target="_blank"><img src="Content/images/pdf.png" /></a>';// '<a href="' + data[i].pdf + '" target="_blank" ' + data[i].pdf + '><img src="Content/images/pdf.png" /></a>';
                //mainContainerpdf.appendChild(div1);
                //}
            }
            $("#hidterm").val(data[0].Term.replaceAll(" ", "%20"));
            //var div1 = document.createElement('div');
            //div1.innerHTML = '<button type=button id=btnprev class=link onclick=btnSubmitfunc("' + data[0].Term.replaceAll(" ", "%20") + '",' + data[0].Start + ',"' + data[0].sorting + '",' + data[0].max + ')><img src="Content/images/left-fast-arrow.png" /></button>&nbsp;&nbsp;<button type=button id=btnprev class=link onclick=btnSubmitfunc("' + data[0].Term.replaceAll(" ", "%20") + '", ' + data[0].RetPrev + ',"' + data[0].sorting + '", ' + data[0].max + ')> <img src="Content/images/left-arrow.png" /></button >&nbsp;&nbsp;<input class="form-control" type="text" placeholder="" value="' + data[0].Page + '" readonly />&nbsp;&nbsp; <button type=button id=btnnxt class=link onclick=btnSubmitfunc("' + data[0].Term.replaceAll(" ", "%20") + '", ' + data[0].RetNext + ',"' + data[0].sorting + '",' + data[0].max + ')> <img src="Content/images/right-arrow.png" /></button >&nbsp;&nbsp; <button type=button id=btnnxt class=link onclick=btnSubmitfunc("' + data[0].Term.replaceAll(" ", "%20") + '", ' + data[0].Last + ',"' + data[0].sorting + '",' + data[0].max + ')> <img src="Content/images/right-fast-arrow.png" /></button > ';
            //FooterContainer.appendChild(div1);

            if (parseInt(data[0].RetPrev) == 0) {
                createPagination(data[0].totalpage, data[0].max, 1, 0, 0);
            }
            $('#spinner').hide();
        }

    </script>

    <script type="text/javascript">
        function createPagination(totalPage, max, pages, start, flag) {

            let totalPages = totalPage;
            let page = pages;
            let starting = '';
            if (parseInt(start) > 0) {
                starting = parseInt(start);
            } else {
                starting = 0;
            }
            const element = document.querySelector(".pagination ul");
            let liTag = '';
            let active;
            let beforePage = page - 1;
            let afterPage = 10;
            if (parseInt(page) > 1) { //show the next button if the page value is greater than 1
                if (parseInt(start) > 0) {
                    starting = parseInt(start) - parseInt(max);
                }
                var prev = parseInt(page) - 1;
                liTag += `<li class="btn prev" onclick="createPagination(${parseInt(totalPages)},${parseInt(max)}, ${parseInt(prev)},${parseInt(starting)},1)"><span><i class="fas fa-angle-left"></i> Prev</span></li>`;
            }

            if (parseInt(page) > 2) { //if page value is less than 2 then add 1 after the previous button
                starting = parseInt(start) + parseInt(max);
                liTag += `<li class="first numb" onclick="createPagination(${parseInt(totalPages)},${parseInt(max)}, 1,0,1)"><span>1</span></li>`;
                if (parseInt(page) > 3) { //if page value is greater than 3 then add this (...) after the first li or page
                    liTag += `<li class="dots"><span>...</span></li>`;
                }
            }

            // how many pages or li show before the current li
            if (parseInt(page) == parseInt(totalPages)) {
                beforePage = beforePage - 2;
            } else if (parseInt(page) == totalPages - 1) {
                beforePage = beforePage - 1;
            }
            // how many pages or li show after the current li
            if (parseInt(page) == 1) {
                afterPage = afterPage + 2;
            } else if (parseInt(page) == 2) {
                afterPage = afterPage + 1;
            }

            for (var plength = beforePage; plength <= afterPage; plength++) {
                if (parseInt(plength) > parseInt(totalPages)) { //if plength is greater than totalPage length then continue
                    continue;
                }
                if (parseInt(plength) == 0) { //if plength is 0 than add +1 in plength value
                    plength = parseInt(plength) + 1;
                }
                if (parseInt(page) == parseInt(plength)) { //if page is equal to plength than assign active string in the active variable
                    active = "active";
                } else { //else leave empty to the active variable
                    active = "";
                }
                starting = (parseInt(plength) - 1) * parseInt(max);
                liTag += `<li class="numb ${active}" onclick="createPagination(${parseInt(totalPages)},${parseInt(max)}, ${parseInt(plength)},${starting},1)"><span>${plength}</span></li>`;
            }

            if (parseInt(page) < parseInt(totalPages) - 1) { //if page value is less than totalPage value by -1 then show the last li or page
                if (parseInt(page) < parseInt(totalPages) - 2) { //if page value is less than totalPage value by -2 then add this (...) before the last li or page
                    liTag += `<li class="dots"><span>...</span></li>`;
                }
                //starting = parseInt(totalPages) - parseInt(max);
                //liTag += `<li class="last numb" onclick="createPagination(${parseInt(totalPages)},${parseInt(max)}, ${parseInt(totalPages)},${starting},1)"><span>${totalPages}</span></li>`;
            }

            if (parseInt(page) < parseInt(totalPages)) { //show the next button if the page value is less than totalPage(20)
                starting = parseInt(start) + parseInt(max);
                liTag += `<li class="btn next" onclick="createPagination(${parseInt(totalPages)},${parseInt(max)}, ${parseInt(page) + 1},${parseInt(starting)},1)"><span>Next <i class="fas fa-angle-right"></i></span></li>`;
            }

            element.innerHTML = liTag; //add li tag inside ul tag
            if (flag == 1) {
                btnSubmitfunc($("#hidterm").val(), start, $("#hidsort").val(), max);
                flag = 0;
            }
            return liTag; //reurn the li tag

        }

    </script>

    <script type="text/javascript">
        function SendData(PMID, WebEnv, QryList) {
            window.location = "SearchDetails.aspx?PMID=" + PMID + "&WebEnv=" + WebEnv + "&QryList=" + QryList;
        }
    </script>

    <script type="text/javascript">

        function btnSubmit(PMID, WebEnv, QryList) {
            $('#spinner').show();
            //var a = 'div#' + PMID;;
            var elems = document.querySelectorAll(".maindiv");

            [].forEach.call(elems, function (el) {
                el.classList.remove("para");
            });
            var element = document.getElementById('div#' + PMID);
            element.classList.add("para");

            var obj = { PMID: PMID, WebEnv: WebEnv, QryList: QryList };
            $.ajax({
                type: "POST",
                url: "SearchList.aspx/GetDetails",
                data: '{PMID: "' + PMID + '",WebEnv:"' + WebEnv + '",QryList:"' + QryList + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessDetails,
                failure: function (response) {
                    //$('#spinner').hide();
                    alert("Something went wrong");
                    return;
                },
                error: function (response) {
                    //$('#spinner').hide();
                    alert("Something went wrong");
                    return;
                }
            });
        };

        function OnSuccessDetails(response) {
            console.log(response.d);
            var data = JSON.parse(response.d);
            console.log(data.length);
            var headContainer = document.getElementById("header");
            var mainContainer = document.getElementById("myDataDetails");
            var linkresult = document.getElementById("linkresult");
            var fulltext = document.getElementById("fulltext");


            var intro = "";
            var obj = "";
            var state = "";
            var conc = "";


            var Keyword = document.getElementById("Keyword");
            var Author = document.getElementById("Affiliation");
            var RefrenceURL = document.getElementById("RefrenceURL");
            var PDF = document.getElementById("PDF");


            $('#mainContainer').empty();
            $('#headContainer').empty();
            $('#myDataDetails').empty();
            $('#Keyword').empty();
            $('#Author').empty();
            $('#RefrenceURL').empty();
            $('#PDF').empty();

            document.getElementById("tabdiv").style.display = "block";

            var div = document.createElement('div');

            var sourcelink = '';
            var sourceintr = 'style="display: none;"';
            var sourceobj = 'style="display: none;"';
            var sourcestate = 'style="display: none;"';
            var sourceconc = 'style="display: none;"';

            var sourcepdf = 'style="display: inline-flex;"';
            if (data[0].link == '') {
                sourcelink = 'style="display: none;"';
            }


            if (data[0].Citiation == '') {
                sourcelink = 'style="display: none;"';
            } else {
                sourcelink = 'style="display:block;"';
            }

            console.log(data[0].link);
            //var data1 = JSON.parse(data[0].link);
            //div.className = '/*flightDetailBox*/';

            //div2.innerHTML = ;

            //headContainer.appendChild(div2);

            var link = "https://doi.org/" + data[0].DOI;

            //div3.innerHTML = '<p>FULL TEXT</p>' +
            //    '<a href="' + link + '" target="_blank"><img src="Content/images/pagination.png" /></a>';

            //fulltext.appendChild(div3);

            if (data[0].Name != null) {
                if (data[0].Name.length > 0) {
                    for (var j = 0; j < data[0].Name.length; j++) {
                        if (data[0].Name[j].INTRODUCTION != '') {
                            intro = data[0].Name[j].INTRODUCTION;
                            sourceintr = 'style="display:block;"';
                        }
                        if (data[0].Name[j].OBJECTIVE != '') {
                            obj = data[0].Name[j].OBJECTIVE;
                            sourceobj = 'style="display:block;"';
                        }
                        if (data[0].Name[j].STATEOFKNOWLEDGE != '') {
                            state = data[0].Name[j].STATEOFKNOWLEDGE;
                            sourcestate = 'style="display:block;"';
                        }
                        if (data[0].Name[j].CONCLUSIONS != '') {
                            conc = data[0].Name[j].CONCLUSIONS;
                            sourceconc = 'style="display:block;"';
                        }
                    }
                } else {

                }
            } else {

            }


            div.innerHTML = '<h2>' + data[0].ArticleTitle + '</h2>' +
                '<p >' + '<b>' + data[0].PubType + '</b>' + ' ' + data[0].ISOAbbreviation + ' ' + data[0].PubDate + ';' + data[0].Volume + '(' + data[0].Issue + '):' + data[0].Pagination + ' doi: ' + data[0].DOI + ' EPub: ' + data[0].EPub + '</p>' +
                '<p>' + data[0].Author + '</p>' +
                '<p> PMID : ' + data[0].PMID + ' DOI : ' + '<a href="' + link + '" target="_blank">' + data[0].DOI + '</a>' + '</p>' +
                '<h3>Abstract</h3>' +
                '<p ' + sourceintr + '> <b>Introduction -</b> ' + intro + '</p>' +
                '<p ' + sourceobj + '> <b>Objective -</b> ' + obj + '</p>' +
                '<p ' + sourcestate + '> <b>State of Knowledge -</b> ' + state + '</p>' +
                '<p ' + sourceconc + '> <b>Conclusions -</b> ' + conc + '</p>';
            //'<p > <b>Citiation Count -</b> ' + data[0].Citiation + '</p>';
            //'<p>Keywords : ' + data[0].Keyword + '</p>' +
            //'<a href="' + data[0].link + '" target="_blank"  ' + sourcelink + ' ><img src="Content/images/url.png" /></a>&nbsp;&nbsp;' +
            //'<a href="' + data[0].pdf + '" target="_blank" ' + sourcepdf + '><img src="Content/images/pdf.png" /></a>';

            mainContainer.appendChild(div);

            if (data[0].Affiliation != null) {
                if (data[0].Affiliation.length > 0) {
                    for (var j = 0; j < data[0].Affiliation.length; j++) {
                        var div1 = document.createElement('div');
                        div1.innerHTML = (j + 1) + '&nbsp;-&nbsp;' + data[0].Affiliation[j].Affiliation + '<br/><br/>';

                        Author.appendChild(div1);
                    }
                } else {
                    var div1 = document.createElement('div');
                    div1.innerHTML = 'No data found';

                    Author.appendChild(div1);
                }
            } else {
                var div1 = document.createElement('div');
                div1.innerHTML = 'No data found';

                Author.appendChild(div1);
            }

            if (data[0].Keyword != null) {
                if (data[0].Keyword.length > 0) {
                    for (var j = 0; j < data[0].Keyword.length; j++) {
                        var div1 = document.createElement('div');
                        div1.innerHTML = (j + 1) + '&nbsp;-&nbsp;' + data[0].Keyword[j].keyword + '<br/><br/>';

                        Keyword.appendChild(div1);
                    }
                } else {
                    var div1 = document.createElement('div');
                    div1.innerHTML = 'No data found';

                    Keyword.appendChild(div1);
                }
            } else {
                var div1 = document.createElement('div');
                div1.innerHTML = 'No data found';

                Keyword.appendChild(div1);
            }

            if (data[0].link != null) {
                if (data[0].link.length > 0) {



                    for (var j = 0; j < data[0].link.length; j++) {
                        sourcepdf = 'style="display: block; margin-left:5px;float: left;"';
                        if (data[0].link[j].pdf == '') {
                            sourcepdf = 'style="display: none;"';
                        }
                        if (data[0].link[j].link != '') {
                            var div1 = document.createElement('div');
                            div1.innerHTML = (j + 1) + '&nbsp;-&nbsp;<a href="' + data[0].link[j].link + '" target="_blank">' + data[0].link[j].linkdet + '</a><br/><br/>';

                            RefrenceURL.appendChild(div1);
                        }

                        if (data[0].link[j].pdf != '') {

                            var div1 = document.createElement('div');
                            div1.innerHTML = '<a href="' + data[0].link[j].pdf + '" target="_blank" ' + sourcepdf + '><img src="Content/images/pdf.png" /></a>';
                            PDF.appendChild(div1);
                        }
                    }
                } else {
                    var div1 = document.createElement('div');
                    div1.innerHTML = 'No data found';
                    RefrenceURL.appendChild(div1);
                    PDF.appendChild(div1);
                }
            } else {
                var div1 = document.createElement('div');
                div1.innerHTML = 'No data found';
                RefrenceURL.appendChild(div1);
                PDF.appendChild(div1);
            }


            //}
            $('#spinner').hide();
        }
    </script>

    <script>
        function openCity(evt, cityName) {
            var i, tabcontent, tablinks;
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }
            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }
            document.getElementById(cityName).style.display = "block";
            evt.currentTarget.className += " active";
        }
    </script>

    <script type="text/javascript">

        function btnpdfSubmitfunc(term, ret) {
            $('#spinner').show();
            //document.getElementById("tabdiv").style.display = "none";
            var searchterm = '';
            if (term == '') {
                searchterm = document.getElementById("txtpdfsearch").value;
            } else {
                searchterm = term;
            }
            // alert(searchterm)
            BindpdfGridView(searchterm, ret);
        }

        function BindpdfGridView(searchterm, ret) {

            var obj = { Test: searchterm };
            $.ajax({
                type: "POST",
                url: "SearchList.aspx/GetSearchListpdf",
                data: '{Test: "' + searchterm + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccesspdf,
                failure: function (response) {
                    $('#spinner').hide();
                    alert("Something went wrong");
                    return;
                },
                error: function (response) {
                    $('#spinner').hide();
                    alert("Something went wrong");
                    return;
                }
            });
        };

        function OnSuccesspdf(response) {
            //$("#linkresult").show();

            console.log(response.d);
            var data = JSON.parse(response.d);
            console.log(data.length);
            var mainContainer = document.getElementById("myData1");
            var mainContainer1 = document.getElementById("myData2");
            var FooterContainer = document.getElementById("myDataButton1");
            $('#myData1').empty();
            $('#myData2').empty();
            /* $('#myDataDetails').empty();*/
            $('#myDataButton1').empty();
            //  alert(data)

            //div.className = "maindiv";
            var div = document.createElement('div');
            var div1 = document.createElement('div');
            for (var i = 0; i < data.length; i++) {


                if (data[i].foundpdf != "") {

                    div.innerHTML += '<a href=' + data[i].foundpdf + ' target="_blank" style="padding-right:5px;"><img src="Content/images/pdf.png" /></a>';

                } else {
                    //div1.innerHTML = '<a href=' + data[i].pdf + ' target="_blank">' + data[i].notpdf + '</a>';

                    div1.innerHTML += '<a href=' + data[i].notpdf + ' target="_blank" style="padding-right:5px;"><img src="Content/images/pdf.png" /></a>';

                }
                //div.className = '/*flightDetailBox*/';
                /* div.id = 'div#' + data[i].PMID;*/


                //'<p>' + data[i].ArticleTitle + '</p>' +
                //'<p>PMID : ' + data[i].PMID + ' Abbreviation : ' + data[i].ISOAbbreviation + '</p>' +
                //'<p>Keywords : ' + data[i].Keyword + '</p>';



            }
            mainContainer.appendChild(div);
            mainContainer1.appendChild(div1);
            $("#divh").show()

            //var div1 = document.createElement('div');
            //div1.innerHTML = '<button type=button id=btnprev class=link onclick=btnSubmitfunc("' + data[0].Term.replaceAll(" ", "%20") + '",' + data[0].RetPrev + ')><i class="fa fa-arrow-left" aria-hidden="true"></i></button>&nbsp;&nbsp;<button type=button id=btnnxt class=link onclick=btnSubmitfunc("' + data[0].Term.replaceAll(" ", "%20") + '",' + data[0].RetNext + ')><i class="fa fa-arrow-right" aria-hidden="true"></i></button>';
            //FooterContainer.appendChild(div1);
            $('#spinner').hide();
        }

    </script>

    <script>
        var input = document.getElementById("txtsearch");
        input.addEventListener("keypress", function (event) {
            if (event.key === "Enter") {
                event.preventDefault();
                document.getElementById("myBtn").click();
            }
        });
    </script>

    <div id="spinner" class="loading" style="display: none">
        <div class="loader"></div>
    </div>

    <div class="container-fluid">
        <div class="s004">
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <form id="form1" runat="server">
                        <fieldset>
                            <input type="hidden" id="hidterm" />
                            <input type="hidden" id="hidsort" />
                            <legend>COST EFFECTIVENESS PUBLICATIONS</legend>
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="inner-form">
                                        <div class="input-field">
                                            <input class="form-control" id="txtsearch" type="text" placeholder=" Type to search..." />
                                            <button id="myBtn" class="btn-search search" type="button" onclick="javascript:return validate();">
                                                <i class="fa fa-search" aria-hidden="true"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-xs-12 resp">
                                    <label for="exampleInputEmail1" class="drpdwnlbl">Sort By:</label>
                                    <select id="ddlshort" class="form-control dropdown">
                                        <option value="relevance">Best Search</option>
                                        <option value="Author">Author</option>
                                        <option value="pub_date">Publication Date</option>
                                        <option value="JournalName">Journal</option>
                                    </select>


                                    <%--<asp:DropDownList ID="ddlshort" runat="server" class="form-control dropdown">
                                        <asp:ListItem Text="Best Search" Value="relevance" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Author" Value="Author"></asp:ListItem>
                                        <asp:ListItem Text="Publication Date" Value="pub_date"></asp:ListItem>
                                        <asp:ListItem Text="Journal" Value="JournalName"></asp:ListItem>
                                    </asp:DropDownList>--%>
                                </div>
                                <div class="col-md-3 col-xs-12 resp">
                                    <label for="exampleInputEmail1" class="drpdwnlbl">Per Page:</label>

                                    <select id="ddlpage" class="form-control dropdown">
                                        <option value="5">5</option>
                                        <option value="10">10</option>
                                        <option value="20">20</option>
                                        <option value="25">25</option>
                                    </select>

                                    <%--<asp:DropDownList ID="ddlpage" runat="server" class="form-control dropdown">
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                    </asp:DropDownList>--%>
                                </div>
                            </div>
                        </fieldset>
                    </form>
                </div>
                <div class="row">
                    <%--<div class="col-md-2 col-sm-2 col-xs-12">
                        Filters
                    </div>--%>


                    <div class="col-md-6 col-sm-6 col-xs-12" style="float: right">
                        <div id="mainsearch">

                            <div id="myData" class="flightDetailBox">
                            </div>



                        </div>

                        <div id="myDataButton">
                        </div>

                        <div id="mypagination" class="pagination">
                            <ul>
                                <!--pages or li are comes from javascript -->
                            </ul>
                        </div>
                    </div>

                    <div class="col-md-6 col-sm-6 col-xs-12" style="float: left">
                        <div id="pdfsearch">
                            <div class="inner-form">
                                <div class="input-field">
                                    <input class="form-control" id="txtpdfsearch" type="text" placeholder=" Type to search..." />
                                    <button id="mypdfBtn" class="btn-search search" type="button" onclick="javascript:return validatepdf();">
                                        <i class="fa fa-search" aria-hidden="true"></i>
                                    </button>
                                </div>

                            </div>
                        </div>

                        <div id="linkresult">
                            <div id="header">
                            </div>
                            <div id="myDataDetails">
                            </div>

                            <div id="tabdiv" style="display: none">
                                <div id="myDataTab">
                                    <div class="tab">
                                        <button class="tablinks" onclick="openCity(event, 'Keyword')">Keyword</button>
                                        <button class="tablinks" onclick="openCity(event, 'Affiliation')">Affiliation</button>
                                        <button class="tablinks" onclick="openCity(event, 'RefrenceURL')">Refrence Url</button>
                                        <button class="tablinks" onclick="openCity(event, 'PDF')">PDF</button>
                                    </div>
                                </div>

                                <div id="Keyword" class="tabcontent">
                                </div>
                                <div id="Affiliation" class="tabcontent">
                                </div>
                                <div id="RefrenceURL" class="tabcontent">
                                </div>
                                <div id="PDF" class="tabcontent">
                                </div>
                            </div>
                        </div>


                        <br />
                        <div id="myDataButton1" class="flightDetailBox">
                        </div>
                        <br />
                        <div id="divh" style="padding: 10px">

                            <div class="para">
                                <h4>All PDF for search terms</h4>

                                <div id="myData1">
                                </div>
                            </div>
                            <br />
                            <div class="para">
                                <h4>All PDF not for search terms</h4>

                                <div id="myData2">
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

            </div>


        </div>
    </div>

    <script src="js/extention/choices.js"></script>
    <script>
        var textPresetVal = new Choices('#choices-text-preset-values',
            {
                removeItemButton: true,
            });

    </script>
</body>
</html>
