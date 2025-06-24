$("document").ready(
    function () {
        $(".city-item").click(
            function () {
                let uri = "http://localhost:5043/Catalog/GetHallList?";
                if (this.hasAttribute("data-cityName")) {
                    let ext = "city=" + this.getAttribute("data-cityName");
                    uri = uri + ext;
                }
                if (this.hasAttribute("data-minCap") && this.hasAttribute("data-maxCap")) {
                    let ext = "minCap=" + this.getAttribute("data-minCap") + "maxCap=" + this.getAttribute("data-maxCap");
                    uri = uri + ext;
                }
                if (this.hasAttribute("data-grade")) {
                    let ext = "grade=" + this.getAttribute("data-grade");
                    uri = uri + ext;
                }
                $.ajax({
                    url: uri,
                    method: "GET",
                    dataType: "html",
                    beforeSend: function () {
                        let loader = "<div class=loader>" +
                            "<img src='/imgs/loading.gif'/>" +
                            "</div>";
                        $("#main").html(loader);

                    },
                    error: function () {
                        // your code here;
                    },
                    success: function (data) {
                        setTimeout(function () {
                            $("#main").html(data);
                        }, 500);

                    },
                    complete: function () {
                        // your code here;
                    }
                });
            }
        );
    });