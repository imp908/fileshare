function Watch()
{
    var date = new Date();

    var hours = date.getHours();
    var minutes = date.getMinutes();
    var seconds = date.getSeconds();
    if (hours < 10) hours = "0" + hours;
    if (minutes < 10) minutes = "0" + minutes;
    if (seconds < 10) seconds = "0" + seconds;
    document.getElementById("watch").innerHTML = hours + ":" + minutes + ":" + seconds;

    setTimeout("Watch()", 1000);
}

function DateNow()
{
    var date = new Date();

    var weekday = new Array(7);
    weekday[1] = "Понедельник";
    weekday[2] = "Вторник";
    weekday[3] = "Среда";
    weekday[4] = "Четверг";
    weekday[5] = "Пятница";
    weekday[6] = "Суббота";
    weekday[0] = "Воскресенье";


    var month = date.getMonth() + 1;
    var day = date.getDate();

    if (day < 10) day = "0" + day;
    if (month < 10) month = "0" + month;

    document.getElementById("dateNow").innerHTML = weekday[date.getDay()] + ' ' + day + '/' + month + '/' + date.getFullYear();

    setTimeout("DateNow()", 100000);
}