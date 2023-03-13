<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchDetails.aspx.cs" Inherits="WebApplication1.SearchDetails" %>

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

    <script>  
        $(document).ready(function () {
            var PMID = GetParameterValues('PMID');
            var WebEnv = GetParameterValues('WebEnv');
            var QryList = GetParameterValues('QryList');
            btnSubmit(PMID, WebEnv, QryList);
            function GetParameterValues(param) {
                var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                for (var i = 0; i < url.length; i++) {
                    var urlparam = url[i].split('=');
                    if (urlparam[0] == param) {
                        return urlparam[1];
                    }
                }
            }
        });
    </script>

    

    <style>
        .para {
            box-shadow: 0px 0px 10px 1px #bbbbbb;
            padding: 2%;
            margin-bottom: 2%;
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

        #header b {
            background: #dce4ef;
            color: #205493;
            padding: 5px;
        }
    </style>

</head>
<body>
    <div class="container">
        <div class="row">
            <a href="#">Search Results</a>
        </div>
        <br />
        <div class="row">
            

            <div class="col-md-6 col-sm-6 col-xs-12" style="float: right">
                <div id="fulltext">

                </div>
            </div>
        </div>
    </div>
</body>
</html>
