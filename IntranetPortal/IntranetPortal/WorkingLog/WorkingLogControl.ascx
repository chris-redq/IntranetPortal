﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="WorkingLogControl.ascx.vb" Inherits="IntranetPortal.WorkingLogControl" %>
<%--<script src="/bower_components/signalr/jquery.signalR.min.js"></script>--%>
<script src="/signalr/hubs"></script>

<script type="text/javascript">

    var WorkingLogControl = {
        wLog: null,
        tryingToReconnect: null,
        initialProgress: null,
        inError: false,
        FileOpened: false,
        initalCallback: null,
        pageUrl: null,
        bble: null,
        category: null,
        initial: function () {
            if (this.initialProgress == null) {
                this.initialProgress = "InProcess";
                // Declare a proxy to reference the hub.
                this.wLog = $.connection.workingLogHub;
                this.registerClientMethods()
                var ctr = this;
                // Start the connection.
                $.connection.hub.start().done(function () {
                    ctr.initialProgress = "Done";
                    ctr.pageUrl = window.location.href;
                    ctr.registerEvents();
                    ctr.wLog.server.connect();

                    if (ctr.initalCallback)
                        ctr.initalCallback();
                });


                $.connection.hub.reconnecting(function () {
                    ctr.tryingToReconnect = true;
                });

                $.connection.hub.reconnected(function () {
                    ctr.tryingToReconnect = false;
                });

                $.connection.hub.disconnected(function () {
                    if (ctr.tryingToReconnect) {
                        ctr.notifyUserOfDisconnect();

                        // Restart connection after 5 seconds.
                        ctr.reConnectServer();
                    }
                });
            }
        },
        reConnectServer: function () {
            var ctr = this;
            $.connection.hub.start()
                .done(function () {
                    ctr.wLog.server.connect(ctr.bble, ctr.category, ctr.pageUrl);
                })
                .fail(function () {
                    console.log("Reconnect to server failed. Retry 5s later.")
                    setTimeout(function () {
                        ctr.reConnectServer();
                    }, 5000);
                });
        },
        notifyUserOfDisconnect: function () {
            //alert("Service was not connected.");
        },
        registerClientMethods: function () {
            this.wLog.client.hello = function (name) {
                //alert("hello, " + name);
            }
        },
        registerEvents: function () {

        },
        openFile: function (_bble, _category) {
            if (this.initialProgress != "Done") {
                var ctr = this;
                ctr.initial();
                ctr.initalCallback = function () {
                    ctr.openFile(_bble, _category);
                }

                return;
            }

            if (this.FileOpened) {
                this.closeFile();
            }

            this.bble = _bble;
            this.category = _category;

            this.wLog.server.openFile(this.bble, this.category, this.pageUrl);
            this.FileOpened = true;
        },
        closeFile: function () {
            if (this.initialProgress != "Done") {
                var ctr = this;
                ctr.initial();
                ctr.initalCallback = function () {
                    this.closeFile();
                }

                return;
            }

            this.wLog.server.closeFile(this.bble, this.category, this.pageUrl);
            this.FileOpened = false;
        },
        switchFile: function (bble, category) {
            this.closeFile();
            this.openFile(bble, category);
        }
    }

    $(function () {
        WorkingLogControl.initial();
    });

</script>
