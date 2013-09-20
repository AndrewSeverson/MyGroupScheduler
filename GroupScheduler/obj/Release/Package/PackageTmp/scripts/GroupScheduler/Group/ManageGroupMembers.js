$(document).ready(function() {
    ManageGroupMembers.Init();
});

var ManageGroupMembers = new function () {
    // the current row being edited
    var currentRow = null;
    this.Init = function() {
        // button click event for updating a member to an admin
        $(".DemoteAdmin, .PromoteMember").click(function () {
            currentRow = $(this).parent("td").parent("tr");
            updateMemberRole();
            $(this).removeClass("btn-danger").removeClass("btn-success");

        });

        // button click to remove a member from a group
        $(".RemoveMember").click(function() {
            currentRow = $(this).parent("td").parent("tr");
            removeMember();
        });
    };

    // removes a members permissions from a group
    var removeMember = function() {
        GroupScheduler.Ajax({
            url: "Group/RemoveMemberFromGroup",
            data: { "userId": $(currentRow).find(".hiddenMemberUserId").html(), "groupId": $("#HiddenGroupId").text() },
            success: function (result) {
                if (result) {
                    // member deleted successfully, change the row accordingly
                    $(currentRow).remove();

                } else {
                    // error removing member
                    alert("There was an error removing this member. Please refresh the page and try agian.", "Error");
                }
            },
            error: function () {
                // error removing memeber
                alert("There was an error removing this member. Please refresh the page and try agian.", "Error");
            }
        });
    };

    // method to update a group members membership
    var updateMemberRole = function () {
        var updatedMember = buildGroupMemberObject();
        GroupScheduler.Ajax({
            url: "Group/UpdateMemberRole",
            data: { "updatedMember": updatedMember, "groupId": $("#HiddenGroupId").text() },
            success: function (result) {
                if (result) {
                    // role updated successfully, change the row accordingly
                    if (updatedMember.IsAdmin) {
                        $(currentRow).removeClass("success").addClass("error");
                        $(currentRow).find(".memberRoleCell").html("Admin");
                        $(currentRow).find(".DemoteAdmin, .PromoteMember").html("Demote to Member").addClass("btn-danger");
                    } else {
                        $(currentRow).removeClass("error").addClass("success");
                        $(currentRow).find(".memberRoleCell").html("Member");
                        $(currentRow).find(".DemoteAdmin, .PromoteMember").html("Promote to Admin").addClass("btn-success");
                    }
                    
                } else {
                    // error updated role
                    alert("There was an error updating this role. Please refresh the page and try agian.", "Error");
                }
            },
            error: function () {
                // error updated role
                alert("There was an error updating this role. Please refresh the page and try agian.", "Error");
            }
        });
    };
    // method to build the GroupMember object 
    var buildGroupMemberObject = function() {
        var groupMember = {};
        groupMember.UserId = $(currentRow).find(".hiddenMemberUserId").html();
        groupMember.IsAdmin = $(currentRow).find(".memberRoleCell").html() == "Admin" ? 0 : 1;
        return groupMember;
    };
};