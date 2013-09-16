$(document).ready(function() {
    GroupScheduler.Init();
});

var GroupScheduler = new function() {
    this.Init = function () {
    };
    
    this.GetLongURL = function () {
        return FULL_URL;
    };
    
    this.Ajax = function (options) {
        if (options["url"] == undefined) {
            throw "URL cannot be null in a ajax call";
        }

        options["type"] = "POST";
        options["contentType"] = "application/json; charset=utf-8";
        options["dataType"] = "json";
        options["url"] = GroupScheduler.GetLongURL() + options["url"];

        //It turns out that 'failure' is not an ajax method anymore. This supports backwards compatibility.
        if (options["failure"] != undefined) {
            options["error"] = options["failure"];
        }

        if (options["data"] !== null) {
            options["data"] = JSON.stringify(options["data"]);
        }

        return $.ajax(options);
    };
};