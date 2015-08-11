﻿Public Class ActivityManageBase

    Public Property LogCategory As LeadsActivityLog.LogCategory
    Public Property TaskActionList() As String()

    Public Overridable Function LogDataSource(bble As String) As List(Of LeadsActivityLog)
        Return LeadsActivityLog.GetLeadsActivityLogs(bble, {LogCategory.ToString})
    End Function

End Class
