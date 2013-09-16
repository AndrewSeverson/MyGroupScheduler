$(document).ready(function() {
    ViewGroupCalendar.Init();
});

var ViewGroupCalendar = new function() {
    this.Init = function() {
        $('#ViewGroupCalendar').fullCalendar({
            editable: true,
            header: {
                left: 'prev,next today',
                center: ' title',
                right: 'month,basicWeek,basicDay'
            },
            events: {
                // grab basic US holidays to throw in the calander
                url: 'https://www.google.com/calendar/feeds/en.usa%23holiday%40group.v.calendar.google.com/public/basic',
                className: 'UsHolidays'
            }
        });
    };
};