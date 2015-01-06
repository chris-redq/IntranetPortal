﻿Imports System.ServiceModel

' NOTE: You can use the "Rename" command on the context menu to change the interface name "ICommonService" in both code and config file together.
<ServiceContract()>
Public Interface ICommonService

    <OperationContract()>
    Sub SendMessage(userName As String, title As String, msg As String, bble As String, notifyTime As DateTime, createBy As String)

    <OperationContract()>
    Sub SendEmail(userName As String, subject As String, body As String)

End Interface