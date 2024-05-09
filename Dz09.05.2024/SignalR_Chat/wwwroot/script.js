$(function () {
    $('#chatBody').hide();
    $('#loginBlock').show();
    const hubConnection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
    hubConnection.on("AddMessage", function (name, message) {
        $('#chatroom').append('<p><b>' + htmlEncode(name) + '</b>: <i>' + htmlEncode(message) + '</i></p>');
    });
    // Функция, вызываемая сервером при подключении нового пользователя
    hubConnection.on("Connected", function (id, userName, allUsers, messagesData) {
        $('#loginBlock').hide();
        $('#chatBody').show();
        $('#hdId').val(id);
        $('#username').val(userName);
        $('#header').html('<h3>Добро пожаловать, ' + userName + '</h3>');

        // Добавляем всех пользователей в чат
        for (let i = 0; i < allUsers.length; i++) {
            AddUser(allUsers[i].connectionId, allUsers[i].name);
        }

        // Добавляем сообщения в чат
        if (messagesData) {
            messagesData.forEach(function (msg) {
                $('#chatroom').append('<p><b>' + htmlEncode(msg.UserName) + '</b>: <i>' + htmlEncode(msg.Content) + '</i></p>');
            });
        }
    });
    // Функция, вызываемая сервером для добавления нового пользователя
    hubConnection.on("NewUserConnected", function (id, name) {
        AddUser(id, name);
    });
    // Функция, вызываемая сервером для удаления пользователя
    hubConnection.on("UserDisconnected", function (id, userName) {
        $('#' + id).remove();
    });
    hubConnection.start().then(function () {
        $('#sendmessage').click(function () {
            hubConnection.invoke("Send", $('#username').val(), $('#message').val()).catch(function (err) {
                return console.error(err.toString());
            });
            $('#message').val('');
        });
        $("#btnLogin").click(function () {
            let name = $("#txtUserName").val();
            if (name.length > 0) {
                hubConnection.invoke("Connect", name).catch(function (err) {
                    return console.error(err.toString());
                });
            } else alert("Введите имя!");
        });
    }).catch(function (err) {
        return console.error(err.toString());
    });
});
function htmlEncode(value) { // Кодирование тегов
    let encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
function AddUser(id, name) { //Добавление нового пользователя
    $("#chatusers").append('<li id="' + id + '"><b>' + name + '</b></li>');
}