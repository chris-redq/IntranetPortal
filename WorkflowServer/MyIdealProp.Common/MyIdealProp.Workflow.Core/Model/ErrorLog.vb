Namespace MyIdealProp.Workflow.Core.Model
    Public Class ErrorLog
        Public Property LogId As Integer
        Public Property ProcInstId As Integer
        Public Property ActInstId As Integer
        Public Property EventInstId As Integer
        Public Property ProcessInstance As ProcessInstance
        Public Property Source As String
        Public Property ErrorMsg As String
        Public Property CreateTime As DateTime

        Public Shared Function Create(procInst As ProcessInstance, errorMsg As Exception) As ErrorLog

            Dim actInstId = Nothing
            Dim eventInstId = Nothing
            If procInst.ActivityInstanceLog IsNot Nothing AndAlso procInst.ActivityInstanceLog.Count > 0 Then
                Dim actInst = procInst.ActivityInstanceLog.Last
                If actInst IsNot Nothing Then
                    actInstId = actInst.ActInstId

                    If actInst.EventInstanceLog IsNot Nothing AndAlso actInst.EventInstanceLog.Count > 0 Then
                        eventInstId = actInst.EventInstanceLog.Last.Id
                    End If
                End If
            End If

            Return New ErrorLog With
                   {
                       .ProcInstId = procInst.Id,
                       .ActInstId = actInstId,
                       .EventInstId = eventInstId,
                       .Source = errorMsg.Source,
                        .ErrorMsg = errorMsg.Message
                       }
        End Function
    End Class
End Namespace