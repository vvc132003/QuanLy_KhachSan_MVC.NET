"use strict";
var chatInitialized = false;
function chat() {
    if (!chatInitialized) {
        var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
        document.getElementById("sendButton").disabled = true;
        connection.on("ReceiveMessage", function (cuochoithoaiid, nhanvienguiid, noidung) {
            var cuochoithoaiid = parseInt(document.getElementById("cuochoithoaiidinput").value);
            displayMessages(cuochoithoaiid);
            document.getElementById('messageInput').value = '';
            callChatBot();
        });
        connection.start()
            .then(function () {
                document.getElementById("sendButton").disabled = false;
            })
            .catch(function (err) {
                return console.error(err.toString());
            });
        document.getElementById("sendButton").addEventListener("click", function (event) {
            var cuochoithoaiid = parseInt(document.getElementById("cuochoithoaiidinput").value);
            var nhanvienguiid = parseInt(document.getElementById("nhanvienguiidinput").value);
            var noidung = document.getElementById("messageInput").value;
            const likeButton = document.getElementById('likeButton');
            const sendButton = document.getElementById('sendButton');
            likeButton.style.display = 'inline-block';
            sendButton.style.display = 'none';
            connection.invoke("SendMessage", cuochoithoaiid, nhanvienguiid, noidung)
                .catch(function (err) {
                    return console.error(err.toString());
                });
            event.preventDefault();
        });
        chatInitialized = true;
    }
}
var chatInitializeds = false;
function chats() {
    if (!chatInitializeds) {
        var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
        document.getElementById("likeButton").disabled = true;
        connection.on("ReceiveMessages", function (cuochoithoaiid, nhanvienguiid, noidung) {
            var cuochoithoaiid = parseInt(document.getElementById("cuochoithoaiidinput").value);
            displayMessages(cuochoithoaiid);
            document.getElementById('messageInput').value = '';
            callChatBot();
        });
        connection.start()
            .then(function () {
                document.getElementById("likeButton").disabled = false;
            })
            .catch(function (err) {
                return console.error(err.toString());
            });
        document.getElementById("likeButton").addEventListener("click", function (event) {
            var cuochoithoaiid = parseInt(document.getElementById("cuochoithoaiidinput").value);
            var nhanvienguiid = parseInt(document.getElementById("nhanvienguiidinput").value);
            var noidung = document.getElementById("messageInput").value;
            const likeButton = document.getElementById('likeButton');
            const sendButton = document.getElementById('sendButton');
            likeButton.style.display = 'inline-block';
            sendButton.style.display = 'none';
            connection.invoke("SendMessages", cuochoithoaiid, nhanvienguiid, noidung)
                .catch(function (err) {
                    return console.error(err.toString());
                });
            event.preventDefault();
        });
        chatInitializeds = true;
    }
}
function handleAjax(url, data, successCallback) {
    $.ajax({
        type: 'POST',
        url: url,
        data: data,
        success: successCallback,
        error: function () {
            alert('Đã xảy ra lỗi khi lấy tin nhắn.');
        }
    });
}

function displayMessages(cuochoithoaiid) {
    handleAjax('/Chat/HienThiThongTinHoiThoai', { cuochoithoaiid: cuochoithoaiid }, function (data) {
        $('.input').html(data).scrollTop($('.input')[0].scrollHeight);
    });

    handleAjax('/Chat/TinNhanBuycuochoithoaiid', { cuochoithoaiid: cuochoithoaiid }, function (data) {
        $('.message-chat').html(data).scrollTop($('.message-chat')[0].scrollHeight);
    });

    handleAjax('/Chat/HienThiNhanVienNhan', { cuochoithoaiid: cuochoithoaiid }, function (data) {
        $('.user-profile').html(data).scrollTop($('.user-profile')[0].scrollHeight);
    });
}

function callChatBot() {
    handleAjax('/Chat/DanhSachCuocTroChuyen', {}, function (data) {
        $('.conversation-lists').html(data);
    });
    chat();
    chats();
}
