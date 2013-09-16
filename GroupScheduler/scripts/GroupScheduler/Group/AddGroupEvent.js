$(document).ready(function() {
    AddGroupEvent.Init();
});

var AddGroupEvent = new function() {
    this.Init = function () {
        $("#EventDateTime").val('').datepicker();
        $("#EventDurration").val('').timepicker();
        $("#EventEndDateTime").val('').datepicker();
    };
};