/**********************************************************************
* Description:  Handles dynamic styling of ASP.NET calendar on the client
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/16/2009
*
* $Workfile: baptismCalendar.js $
* $Revision: 2 $
* $Header: /trunk/Arena/UserControls/Custom/Cccev/BaptismScheduler/js/baptismCalendar.js   2   2009-12-23 15:06:57-07:00   JasonO $
*
* $Log: /trunk/Arena/UserControls/Custom/Cccev/BaptismScheduler/js/baptismCalendar.js $
*  
*  Revision: 2   Date: 2009-12-23 22:06:57Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-23 20:53:17Z   User: JasonO 
**********************************************************************/

$(document).ready(function()
{
    initBaptismEvents();
    initBaptismCalendar();
});

function initBaptismEvents()
{
    $("input[id$='lbFindPerson']").click(function()
    {
        $("input[id$='ihSearchType']").val("person");
    });

    $("input[id$='lbFindApprover']").click(function()
    {
        $("input[id$='ihSearchType']").val("approver");
    });

    $("input[id$='lbFindBaptizer']").click(function()
    {
        $("input[id$='ihSearchType']").val("baptizer");
    });

    $("input.delete").click(function()
    {
        if (confirm("Are you sure you want to delete this item?"))
        {
            return true;
        }

        return false;
    });

    $("input.removePerson").click(function()
    {
        var person = $(this).parent().siblings("span.person:first");

        if (confirm("Are you sure you want to remove " + person.html() + "?"))
        {
            $(this).parent().siblings("input:hidden").val('');

            person.fadeOut("slow", function()
            {
                $(this).html("");
            });

            $(this).fadeOut("slow");
        }

        return false;
    });

    $("li.baptizer input:image").click(function()
    {
        if (confirm("Are you sure you want to remove " + $(this).siblings("span.formItem").html() + "?"))
        {
            return true;
        }

        return false;
    });
}

function initBaptismCalendar()
{
    if ($("td.calendar-selected").length <= 0)
    {
        $("td.calendar-today").removeClass().addClass("calendar-selected")
            .css("background-color", "silver").children("a").css("color", "#ffffff");
    }
    
    var selectedWeek = $("td.calendar-selected").siblings();

    selectedWeek.each(function()
    {
        if ($(this).css("background-color") == "transparent")
        {
            $(this).css("background-color", "#e9e9e9");
        }
    });
}