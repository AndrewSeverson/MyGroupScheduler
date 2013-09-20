$(document).ready(function () {
    HomePage.Init();
});

var HomePage = new function () {
    // list of groups currently being searched on
    var globalGroupsList = {};
    this.Init = function () {
        
        // initialize the calendar
        $('#HomeCalendar').fullCalendar({
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

        // autocomplete initialization for typehead, this is for searching for public groups
        $("#SearchPublicGroup").typeahead({
            items: 8,
            minLength: 3,
            source: function (query, process) {
                //return process(getPublicGroups(query));
                return $.ajax({
                    url: GroupScheduler.GetLongURL() + "Home/GetAllPublicGroups",
                    type: 'post',
                    data: { search: query },
                    dataType: 'json',
                    success: function (result) {

                        var resultList = result.map(function (item) {
                            var aItem = { id: item.id, name: item.name };
                            return JSON.stringify(aItem);
                        });

                        return process(resultList);

                    }
                });
            },
            matcher: function (obj) {
                var item = JSON.parse(obj);
                return item.name;
            },

            sorter: function (items) {          
                var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
                while (aItem = items.shift()) {
                    var item = JSON.parse(aItem);
                    if (!item.name.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                    else if (~item.name.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                    else caseInsensitive.push(JSON.stringify(item));
                }

                return beginswith.concat(caseSensitive, caseInsensitive);

            },


            highlighter: function (obj) {
                var item = JSON.parse(obj);
                var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
                return item.name.replace(new RegExp('(' + query + ')', 'ig'), function($1, match) {
                    return '<strong>' + match + '</strong>';
                });
            },
            updater: function (obj) {
                var item = JSON.parse(obj);
                window.location.href = GroupScheduler.GetLongURL() + "Group/ViewGroup?groupId=" + item.id;
                return item.name;
            }
        });
    };

    var getPublicGroups = function (search) {
        GroupScheduler.Ajax({
            url: "Home/GetAllPublicGroups",
            data: { search: search },
            success: function (result) {
                globalGroupsList = result;
                var list = [];
                for (var i = 0; i < result.length; i++) {
                    list.push(result[i].name);
                }
                return list;
            },
            error: function () {
                globalGroupsList = {};
                return [];
            }
        });
    };

};

    