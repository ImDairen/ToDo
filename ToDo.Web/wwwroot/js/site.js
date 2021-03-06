﻿var toDoItems = document.getElementsByClassName("toDoItemTitle")
Array.from(toDoItems).forEach(toDo => toDo.addEventListener("click", async function ajaxDescription() {
    jsonInput = toDo.parentElement.querySelector(".toDoItemId").getAttribute("value")
    $.ajax({
        type: "GET",
        url: "Do/GetDescription",
        dataType: "html",
        data: {
            jsonInput
        },
        success: function (data) {
            $('.column.middle').empty()
            $('.column.middle').append(data)
        }
    })
}))

var toDoArrows = document.getElementsByClassName("toDoArrow")
Array.from(toDoArrows).forEach(arrow => arrow.addEventListener("click", async function GetSubTasks() {
    arrow.parentElement.querySelector(".nested").classList.toggle("active")
    if (arrow.classList.contains("expand")) {
        arrow.classList.remove("expand")
    }
    else {
        arrow.classList.add("expand")
    }
}))