﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChatDefault.aspx.vb" Inherits="IntranetPortal.ChatDefault" %>

<!DOCTYPE html>
<html>
<head>
    <title>SignalR Simple Chat</title>
    <link href='http://fonts.googleapis.com/css?family=Source+Sans+Pro:200,300,400,600,700,900' rel='stylesheet' type='text/css' />
    <style type="text/css">
        html, body {
            margin: 0;
            padding: 0;
            font-family: Source Sans Pro,Tahoma;
        }

        .container {
            background-color: #99CCFF;
            border: thick solid #808080;
            padding: 20px;
        }

        .chatRoom {
            width: 550px;
            margin-left: auto;
            margin-right: auto;
            border: solid 1px gray;
        }

            .chatRoom .title {
                font-size: 16px;
                font-weight: bold;
                padding: 8px;
                background-color: #d7e5e4;
                border-bottom: solid 1px #5f9482;
            }

            .chatRoom .content {
                height: 350px;
                clear: both;
            }

        .leftwindow {
            float: left;
            width: 409px;
            height: 350px;
            border-right: solid 1px #5f9482;
        }

        .chatRoom .content .chatWindow {
            float: left;
            width: 409px;
            height: 300px;
            border-right: solid 1px #5f9482;
            border-bottom: solid 1px #5f9482;
            overflow-y: scroll;
        }

            .chatRoom .content .chatWindow .message {
                padding: 4px;
            }

                .chatRoom .content .chatWindow .message .userName {
                    font-weight: bold;
                }

        .chatRoom .content .users {
            float: right;
            width: 140px;
            height: 300px;
        }

            .chatRoom .content .users .user {
                display: block;
                cursor: pointer;
                padding: 4px;
                background-color: #f9f9f9;
                border-bottom: solid 1px #5f9482;
                transition:background-color 0.5s ease;
            }

            .chatRoom .content .users .loginUser {
                display: block;
                padding: 4px;
                color: gray;
                border-bottom: solid 1px #5f9482;
            }

            .chatRoom .content .users .user:hover {
                background-color: #e1e1e1;
            }

        .chatRoom .messageBar {
            border-top: solid 1px gray;
            padding: 4px;
        }

            .chatRoom .messageBar .textbox {
                width: 300px;
            }

            .newMessage{
                 background:lightgreen;
            }
    </style>
    <!--Script references. -->

<%--    <script src="/bower_components/jquery/dist/jquery.min.js"></script>
    <script src="/bower_components/signalr/jquery.signalR.min.js"></script>--%>
    <script src="/Scripts/vendor.js"></script>
    <script src="/signalr/hubs"></script>

</head>
<body>
    <div id="divChat" class="chatRoom">
        <div class="title">
            <span id="spanChatTitle">Welcome to Chat Room</span> [<span id='spanUser'></span>]
        </div>
        <div class="content">
            <div class="leftwindow" id="chatContent">
                <div style="width:100%;height:100%;align-content:center;line-height:300px; padding-left:80px">
                    <span style="font-size:20px">Double Click User Name to Chat</span>
                </div>
                <div id="ctrUser" style="display:none">
                    <span id="spanChatWith">Select User to Chat...</span>
                    <div id="divChatWindow" class="chatWindow">
                    </div>
                    <div class="messageBar">
                        <input class="textbox" type="text" id="txtMessage" />
                        <input id="btnSendMsg" type="button" value="Send" class="submitButton" />
                    </div>
                </div>
            </div>
            <div id="divusers" class="users">
            </div>
        </div>

        <input id="hdId" type="hidden" />
        <input id="hdUserName" type="hidden" />
        <input id="hdChatToUser" type="hidden" />
    </div>

    <script type="text/javascript">
        $(function () {
            // Declare a proxy to reference the hub.
            var chat = $.connection.chatHub;
            registerClientMethods(chat);

            // Start the connection.
            $.connection.hub.start().done(function () {
                registerEvents(chat);
                chat.server.connect();
            });
        });

        function AddMessage(userName, message) {
            $('#divChatWindow').append('<div class="message"><span class="userName">' + userName + '</span>: ' + message + '</div>');

            var height = $('#divChatWindow')[0].scrollHeight;
            $('#divChatWindow').scrollTop(height);
        }

        function registerEvents(chatHub) {

            $('#btnSendMsg').click(function () {

                var msg = $("#txtMessage").val();
                if (msg.length > 0) {

                    var userName = $('#hdUserName').val();
                    chatHub.server.sendMessageToAll(userName, msg);
                    $("#txtMessage").val('');
                }
            });

            $("#txtMessage").keypress(function (e) {
                if (e.which == 13) {
                    $('#btnSendMsg').click();
                }
            });
        }

        function registerClientMethods(chatHub) {

            // Calls when user successfully logged in
            chatHub.client.onConnected = function (id, userName, allUsers, messages) {

                $('#hdId').val(id);
                $('#hdUserName').val(userName);
                $('#spanUser').html(userName);

                // Add All Users
                for (i = 0; i < allUsers.length; i++) {

                    AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName);
                }

                // Add Existing Messages
                for (i = 0; i < messages.length; i++) {

                    AddMessage(messages[i].UserName, messages[i].Message);
                }
            }

            // On New User Connected
            chatHub.client.onNewUserConnected = function (id, name) {

                AddUser(chatHub, id, name);
            }
            
            // On User Disconnected
            chatHub.client.onUserDisconnected = function (id, userName) {

                $('#' + id).remove();

                RemoveChatWin(id, userName);

                var disc = $('<div class="disconnect">"' + userName + '" logged off.</div>');

                $(disc).hide();
                $('#divusers').prepend(disc);
                $(disc).fadeIn(200).delay(2000).fadeOut(200);
            }

            chatHub.client.messageReceived = function (userName, message) {
                AddMessage(userName, message);
            }

            chatHub.client.sendPrivateMessage = function (name, id, fromUserName, message) {

                var chatWin = GetChatWin(name, id, chatHub);
                chatWin.addMessage(fromUserName, message);
            }
        }

        var PrivateChat = function (userName, userid, chatHub) {
            this.Name = userName;
            this.chatHub = chatHub;
            this.ChatDiv = null;
            this.UserId = userid;
            this.CtrId = 'divPrivateChat_' + userid;
            this.ChatDivTemplate = '<div id="' + this.CtrId + '">' +
                                '<span id="spanChatWith"> Chat with ' + userName + '</span>' +
                                    '<div id="divMessage" class="chatWindow">' +
                                    '</div>' +
                                    '<div class="messageBar">' +
                                        '<input class="textbox" type="text" id="txtPrivateMessage" />' +
                                        '<input id="btnSendMessage" type="button" value="Send" class="submitButton" /></div></div>';

            this.createChatWindow = function () {
                this.ChatDiv = $(this.ChatDivTemplate);
                this.addClickEvent();
            };

            this.addClickEvent = function () {
                var chatWin = this;
                this.ChatDiv.find("#btnSendMessage").click(function () {
                    $textBox = chatWin.ChatDiv.find("#txtPrivateMessage");
                    var msg = $textBox.val();
                    if (msg.length > 0) {
                        chatWin.chatHub.server.sendPrivateMessage(chatWin.Name, msg);
                        $textBox.val('');
                    }
                });

                // Text Box event
                this.ChatDiv.find("#txtPrivateMessage").keypress(function (e) {
                    if (e.which == 13) {
                        chatWin.ChatDiv.find("#btnSendMessage").click();
                    }
                });
            }

            this.showWindow = function () {
                $('#chatContent').html('');                
                $('#chatContent').prepend(this.ChatDiv);
                this.setScrollbar();
            };

            this.addMessage = function (fromUserName, message) {                
                var divMsg = this.ChatDiv.find('#divMessage');
                divMsg.append('<div class="message"><span class="userName">' + fromUserName + '</span>: ' + message + '</div>');                
              
                this.setScrollbar();

                if(!this.ChatDiv.is(':visible'))
                {
                    //$('#' + this.UserId).animate({backgroundColor: "#8bed7e"}, 500).animate({
                    //    backgroundColor: "red"
                    //}, 500);                   
                    $('#' + this.UserId).css("background", "lightgreen");
                }
            };

            this.setScrollbar = function () {
                // set scrollbar
                var height = this.ChatDiv.find('#divMessage')[0].scrollHeight;
                this.ChatDiv.find('#divMessage').scrollTop(height);
            };
        }
        
        function AddUser(chatHub, id, name) {

            var userId = $('#hdId').val();

            var code = "";

            if (userId == id) {

                code = $('<div class="loginUser">' + name + "</div>");

            }
            else {

                code = $('<a id="' + id + '" class="user" >' + name + '<a>');

                $(code).dblclick(function () {

                    var id = $(this).attr('id');

                    if (userId != id)
                        ShowChatWindow(chatHub, id, name);

                    $(this).css("background", "");
                });
            }

            $("#divusers").append(code);
        }

        var chatWindows = [];
        var currentUser = "";
        function ShowChatWindow(chatHub, id, name) {
            var chatWin = GetChatWin(name, id, chatHub);      
            chatWin.showWindow();
        }

        function RemoveChatWin(id, name)
        {            
            for (var i = 0; i < chatWindows.length; i++) {
                var chatWin = chatWindows[i];
                if (chatWin.Name == name) {
                    chatWindows.splice(i, 1);
                }
            }
        }

        function GetChatWin(userName, id, chatHub) {            
            for (var i = 0; i < chatWindows.length; i++) {
                var chatWin = chatWindows[i];
                if (chatWin.Name == userName) {
                    chatWin.addClickEvent();
                    return chatWin;
                }
            }

            return CreateNewChatWin(userName, id, chatHub);
        }

        function CreateNewChatWin(name, id, chatHub) {
            var chatWin = new PrivateChat(name, id, chatHub);
            chatWin.createChatWindow();

            chatWindows.push(chatWin);
            return chatWin;
        }

        function PrivateChat(chatHub, id, name) {
            $("#spanChatTitle").html("Chat with " + name);
            $("#hdChatToUser").val(name);
        }

    </script>
</body>
</html>
